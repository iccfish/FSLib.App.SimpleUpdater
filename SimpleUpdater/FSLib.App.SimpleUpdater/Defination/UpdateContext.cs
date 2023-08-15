namespace FSLib.App.SimpleUpdater.Defination
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Reflection;
    using System.Windows.Forms;

    using Dialogs;

    using Logs;

    using Wrapper;

    /// <summary> 表示当前更新的上下文环境 </summary>
    /// <remarks></remarks>
    public class UpdateContext
    {
        private static ILogger _logger = LogManager.Instance.GetLogger<UpdateContext>();

        private string _applicationDirectory;
        bool           _autoEndProcessesWithinAppDir;
        bool           _autoExitCurrentProcess;
        bool           _autoKillProcesses;

        private DialogStyle _dialogStyle;
        bool                _forceUpdate;
        bool                _hiddenUI;
        private string      _logFile;
        bool                _mustUpdate;

        string         _preserveFileListPath;
        bool           _promptUserBeforeAutomaticUpgrade;
        private object _updateAttribute;
        private string _updateDownloadUrl;
        private string _updateInfoFileName;

        string _updateInfoFilePath;

        string _updateNewFilePath;


        string _updatePackageListPath;


        string _updatePackagePath;

        string _updateRollbackPath;

        public UpdateContext()
        {
            InitializeCurrentVersion();

            AutoEndProcessesWithinAppDir = true;
            ExternalProcessID            = new List<int>();
            ExternalProcessName          = new List<string>();
            MultipleDownloadCount        = 3;
            MaxiumRetryDownloadCount     = 3;
            EnableEmbedDialog            = true;
            ComponentStatus              = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            AutoClosePreviousPopup       = true;

            //如果当前启动路径位于TEMP目录下，则处于临时路径模式
            // 已更新2021-05-19：往系统的temp目录下写东西，360太喜欢报毒。所以换用户数据目录下。
            // stupid 360.
            var temppath     = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FishSimpleUpdaterTemp");
            var assemblyPath = Assembly.GetExecutingAssembly().Location;

            _logger.LogInformation($"Current user temporary path was set to {temppath}");
            _logger.LogInformation($"Current assembly was located at '{assemblyPath}'");
#if NET5_0
            if (string.IsNullOrEmpty(assemblyPath))
            {
                assemblyPath = AppContext.BaseDirectory;
                NeedStandaloneUpdateClientSupport = true;
                _logger.LogInformation($"Current app context was located at '{assemblyPath}'");
                _logger.LogInformation("Unable get location from assembly, probably program released as a single file. A standalone updater client will be required to perform upgrade.");
            }
#endif

            if (assemblyPath.IndexOf(temppath, StringComparison.OrdinalIgnoreCase) != -1)
            {
                UpdateTempRoot = File.Exists(assemblyPath) ? Path.GetDirectoryName(assemblyPath) : assemblyPath;
                IsInUpdateMode = true;
            }
            else
            {
                if (Directory.Exists(temppath))
                {
                    try
                    {
                        Directory.Delete(temppath, true);
                    }
                    catch (Exception e)
                    {
                    }
                }

                UpdateTempRoot = Path.Combine(temppath, Guid.NewGuid().ToString("N").Substring(0, 8));
                IsInUpdateMode = false;

                //尝试自动加载升级属性
                var assembly = Assembly.GetEntryAssembly() ?? TryGetCallingAssembly();
                if (assembly != null)
                {
                    var atts = assembly.GetCustomAttributes(false);

                    foreach (var item in atts)
                    {
                        if (item is UpdateableAttribute || item is Updatable2Attribute)
                        {
                            UpdateAttribute = item;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     获得或设置相关联的主窗口。所有的提示界面将会以其为父窗口
        /// </summary>
        public static Form MainWindow { get; set; }

        /// <summary>
        ///     获得或设置是否在正式进行更新前先通知用户
        /// </summary>
        public bool PromptUserBeforeAutomaticUpgrade
        {
            get => _promptUserBeforeAutomaticUpgrade || (UpdateInfo != null && UpdateInfo.PromptUserBeforeAutomaticUpgrade);
            set => _promptUserBeforeAutomaticUpgrade = value;
        }

        /// <summary> 获得或设置是否正在更新模式中 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool IsInUpdateMode { get; private set; }

        /// <summary>
        ///     主要交互界面
        /// </summary>
        internal string UpdateMainFormType { get; set; }


        /// <summary>
        ///     获得或设置一个值，指示着当自动更新的时候是否将应用程序目录中的所有进程都作为主进程请求结束
        /// </summary>
        public bool AutoEndProcessesWithinAppDir
        {
            get => _autoEndProcessesWithinAppDir || (UpdateInfo != null && UpdateInfo.AutoEndProcessesWithinAppDir);
            set => _autoEndProcessesWithinAppDir = value;
        }

        /// <summary>
        ///     外部要结束的进程ID列表
        /// </summary>
        public IList<int> ExternalProcessID { get; private set; }

        /// <summary>
        ///     外部要结束的进程名称
        /// </summary>
        public IList<string> ExternalProcessName { get; private set; }


        /// <summary>
        ///     获得更新中发生的错误
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary> 获得或设置下载链接 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateDownloadUrl
        {
            get
            {
                if (UpdateAttribute is Updatable2Attribute) return (UpdateAttribute as Updatable2Attribute).UrlTemplate;
                if (UpdateAttribute is UpdateableAttribute) return (UpdateAttribute as UpdateableAttribute).UpdateUrl;

                return _updateDownloadUrl;
            }
            set => _updateDownloadUrl = value;
        }

        /// <summary> 获得或设置XML信息文件名 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateInfoFileName
        {
            get
            {
                if (UpdateAttribute is Updatable2Attribute)
                {
                    return (UpdateAttribute as Updatable2Attribute).InfoFileName;
                }

                return _updateInfoFileName;
            }
            set => _updateInfoFileName = value;
        }

        /// <summary> 获得或设置当前的更新支持信息 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public object UpdateAttribute
        {
            get => _updateAttribute;
            set
            {
                if (value != null && (!(value is UpdateableAttribute) && !(value is Updatable2Attribute)))
                {
                    throw new InvalidOperationException("设置的参数值不是正确的标记，仅支持 UpdateableAttribute 或 Updatable2Attribute。");
                }

                _updateAttribute = value;
            }
        }

        /// <summary> 获得或设置当前应用程序的路径 </summary>
        /// <value></value>
        /// <remarks>如果设置的是相对路径，那么最终设置的结果将是当前的应用程序目录和设置值组合起来的路径</remarks>
        /// <exception cref="T:System.ArgumentException">当设置的值是null或空字符串时抛出此异常</exception>
        public string ApplicationDirectory
        {
            get => _applicationDirectory;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("ApplicationDirectory can not be null or empty.");

                _applicationDirectory = Path.IsPathRooted(value) ? value : Path.Combine(_applicationDirectory, value);
            }
        }

        /// <summary> 获得或设置用于下载更新信息文件的地址 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateInfoFileUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(UpdateInfoFileName)) return string.Format(UpdateDownloadUrl.Replace(@"\", @"\\"), UpdateInfoFileName);
                return UpdateDownloadUrl;
            }
        }

        /// <summary> 获得或设置当前的版本 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public Version CurrentVersion { get; set; }

        /// <summary> 获得或设置当前的更新信息 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public UpdateInfo UpdateInfo { get; internal set; }

        /// <summary> 获得或设置最后的版本 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public Version LatestVersion => UpdateInfo == null ? null : new Version(UpdateInfo.AppVersion);

        /// <summary> 获得或设置是否启用内置的提示对话框 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool EnableEmbedDialog { get; set; }

        /// <summary> 获得或设置是否正在进行更新中 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool IsInUpdating { get; internal set; }

        /// <summary> 获得或设置同时下载的文件数 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MultipleDownloadCount { get; set; }

        /// <summary> 获得或设置重试的最大次数 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MaxiumRetryDownloadCount { get; set; }

        /// <summary> 获得当前更新的临时目录 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateTempRoot
        {
            get;
        }

        /// <summary> 获得当前更新信息文件保存的路径 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateInfoFilePath => _updateInfoFilePath ?? (_updateInfoFilePath = Path.Combine(UpdateTempRoot, "update.xml"));

        /// <summary> 获得当前要下载的包文件信息保存的路径 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdatePackageListPath => _updatePackageListPath ?? (_updatePackageListPath = Path.Combine(UpdateTempRoot, "packages.xml"));

        /// <summary> 获得当前要保留的文件信息保存的路径 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string PreserveFileListPath => _preserveFileListPath ?? (_preserveFileListPath = Path.Combine(UpdateTempRoot, "reservefile.xml"));

        /// <summary> 获得当前下载的包文件目录 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdatePackagePath => _updatePackagePath ?? (_updatePackagePath = Path.Combine(UpdateTempRoot, "packages"));

        /// <summary> 获得当前下载解包后的新文件路径 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateNewFilePath => _updateNewFilePath ?? (_updateNewFilePath = Path.Combine(UpdateTempRoot, "files"));

        /// <summary> 获得当前更新过程中备份文件的路径 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UpdateRollbackPath => _updateRollbackPath ?? (_updateRollbackPath = Path.Combine(UpdateTempRoot, "backup"));

        /// <summary>
        ///     获得一个值，表示当前的自动升级信息是否已经下载完全
        /// </summary>
        public bool IsUpdateInfoDownloaded => UpdateInfo != null || File.Exists(UpdateInfoFilePath);

        /// <summary> 获得或设置服务器用户名密码标记 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public NetworkCredential NetworkCredential { get; set; }

        /// <summary> 获得或设置用于下载的代理服务器地址 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string ProxyAddress { get; set; }

        /// <summary> 获得或设置网络请求的 UserAgent </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string UserAgent { get; set; }

        /// <summary> 获得是否找到更新的标记位 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool HasUpdate { get; internal set; }

        /// <summary> 获得表示是否当前版本过低而无法升级的标记位 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public bool CurrentVersionTooLow { get; internal set; }

        /// <summary>
        /// 获得或设置是否自动开启日志记录
        /// </summary>
        public bool AutoStartLog { get; set; } = true;

        /// <summary> 获得或设置日志文件名 </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string LogFile
        {
            get => _logFile;
            set
            {
                if (string.Compare(_logFile, value, StringComparison.OrdinalIgnoreCase) == 0) return;

                _logFile = value;

                var lm = LogManager.Instance;
                lm.RemoveAllLogTargets();
                if (!string.IsNullOrEmpty(_logFile))
                {
                    if (!Path.IsPathRooted(_logFile)) _logFile = Environment.ExpandEnvironmentVariables("%TEMP%\\" + _logFile);
                    Directory.CreateDirectory(Path.GetDirectoryName(_logFile));
                    lm.AddLogTarget(new FileLogTarget(_logFile));
                }
            }
        }

        /// <summary>
        ///     获得或设置是否不经提示便自动更新
        /// </summary>
        public bool ForceUpdate
        {
            get => _forceUpdate || (UpdateInfo != null && UpdateInfo.ForceUpdate);
            set => _forceUpdate = value;
        }

        /// <summary>
        ///     获得或设置是否强制更新，否则退出
        /// </summary>
        public bool MustUpdate
        {
            get => _mustUpdate || (UpdateInfo != null && UpdateInfo.MustUpdate);
            set => _mustUpdate = value;
        }

        /// <summary>
        ///     获得或设置是否在更新时自动结束进程
        /// </summary>
        public bool AutoKillProcesses
        {
            get => _autoKillProcesses || (UpdateInfo != null && UpdateInfo.AutoKillProcesses);
            set => _autoKillProcesses = value;
        }

        /// <summary>
        ///     获得或设置是否自动退出当前进程
        /// </summary>
        public bool AutoExitCurrentProcess
        {
            get => _autoExitCurrentProcess || (UpdateInfo != null && UpdateInfo.AutoExitCurrentProcess);
            set => _autoExitCurrentProcess = value;
        }


        /// <summary>
        ///     是否隐藏所有对话框显示
        /// </summary>
        public bool HiddenUI
        {
            get => _hiddenUI;
            set
            {
                _hiddenUI = value;
                if (value)
                {
                    ForceUpdate       = true;
                    AutoKillProcesses = true;
                }
            }
        }

        /// <summary>
        ///     获得更新程序是否已经成功启动了
        /// </summary>
        public bool? IsUpdaterSuccessfullyStarted { get; internal set; }

        /// <summary>
        ///     设置当出现错误的时候，是否按照有更新但是未更新处理。
        ///     这个选项影响设置必须强制更新的选项。
        ///     如果检测更新遇到错误，此选项设置为false时，则按照“未找到更新”处理；如果此选项设置为true，则按照“有更新但是没有更新”处理，会强制退出软件。
        /// </summary>
        public bool TreatErrorAsNotUpdated { get; set; }

        /// <summary>
        ///     获得组件状态
        /// </summary>
        public Dictionary<string, bool> ComponentStatus { get; private set; }

        /// <summary>
        ///     获得或设置是否自动关闭之前的找到更新的提示框
        /// </summary>
        public bool AutoClosePreviousPopup { get; set; }

        /// <summary>
        ///     获得或设置更新对话框主题配置
        /// </summary>
        public DialogStyle DialogStyle
        {
            get => UpdateInfo?.DialogStyle ?? _dialogStyle ?? DialogStyle.Default;
            set => _dialogStyle = value;
        }

        /// <summary>
        ///     获得或设置是否需要独立的更新客户端支持
        /// </summary>
        public bool NeedStandaloneUpdateClientSupport { get; internal set; }

        /// <summary>
        ///     获得或设置标记位，指定在复制引用程序集时是否忽略位置检测
        /// </summary>
        public bool CopyAssemblyIgnoreLocationTest { get; set; }

        /// <summary>
        ///     获得或设置是否允许不安全的SSL证书
        /// </summary>
        public bool AllowUnsafeSslCertificate { get; set; }

        /// <summary>
        ///     初始化一些升级的参数
        /// </summary>
        public void Init()
        {
            if (!Uri.TryCreate(UpdateDownloadUrl, UriKind.RelativeOrAbsolute, out var uri))
            {
                throw new ArgumentException("Invalid uri.", nameof(UpdateDownloadUrl));
            }

            //本地路径
            var defaultInfoName = "update_c.xml";
            if (uri.IsFile)
            {
                var downloadUrl = UpdateDownloadUrl;

                if (!string.IsNullOrEmpty(UpdateInfoFileName))
                {
                    //有设置文件名，则检查有没有占位模板；没有的话，则自动附加一个
                    if (!UpdateDownloadUrl.Contains("{0}"))
                    {
                        if (downloadUrl[downloadUrl.Length - 1] != Path.DirectorySeparatorChar)
                            downloadUrl += Path.DirectorySeparatorChar;
                        downloadUrl += "{0}";
                    }
                }
                else
                {
                    if (downloadUrl[downloadUrl.Length - 1] != Path.DirectorySeparatorChar)
                    {
                        if (!downloadUrl.Contains("{0}"))
                        {
                            //没有指定新文件的名字，则取文件名后，再使用目录组合
                            UpdateInfoFileName = Path.GetFileName(UpdateDownloadUrl);
                            downloadUrl        = Path.Combine(Path.GetDirectoryName(downloadUrl), "{0}");
                        }
                        else
                        {
                            UpdateInfoFileName = defaultInfoName;
                        }
                    }
                    else
                    {
                        //是目录
                        UpdateInfoFileName = defaultInfoName;
                        if (!downloadUrl.Contains("{0}"))
                            downloadUrl += "{0}";
                    }
                }

                UpdateDownloadUrl = downloadUrl;
            }
            else
            {
                //网址模式
                if (string.IsNullOrEmpty(UpdateInfoFileName))
                {
                    if (uri.LocalPath.Contains("{0}"))
                    {
                        UpdateInfoFileName = defaultInfoName;
                    }
                    else
                    {
                        if (uri.LocalPath.EndsWith("/"))
                        {
                            UpdateInfoFileName = defaultInfoName;
                            uri                = new Uri(new Uri(uri, "{0}"), uri.Query);
                        }
                        else
                        {
                            UpdateInfoFileName = uri.Segments[uri.Segments.Length - 1];
                            uri                = new Uri(new Uri(uri, "./{0}"), uri.Query);
                        }
                    }
                }
                else
                {
                    if (!uri.LocalPath.Contains("{0}"))
                    {
                        //有文件，没模板，自动在最后拼接
                        if (uri.LocalPath.EndsWith("/"))
                        {
                            uri = new Uri(new Uri(uri, "./{0}"), uri.Query);
                        }
                        else
                        {
                            uri = new Uri(new Uri(uri, $"./{uri.Segments[uri.Segments.Length - 1]}/{{0}}"), uri.Query);
                        }
                    }
                }

                UpdateDownloadUrl = uri.ToString();
            }
        }

        /// <summary>
        ///     初始化当前的版本信息
        /// </summary>
        void InitializeCurrentVersion()
        {
            var processModule = Process.GetCurrentProcess().MainModule;
            if (processModule == null)
            {
                throw new InvalidOperationException("Can not get the main module of current process.");
            }

            CurrentVersion       = ExtensionMethod.ConvertVersionInfo(processModule.FileVersionInfo);
            ApplicationDirectory = Path.GetDirectoryName(processModule.FileName);
        }

        /// <summary>
        ///     尝试从程序集中获得升级属性
        /// </summary>
        /// <returns></returns>
        static Assembly TryGetCallingAssembly()
        {
            _logger.LogInformation("Tring get entry assembly from stack trace.");
            try
            {
                var st    = new StackTrace();
                var frame = st.GetFrame(st.FrameCount - 1);

                var assembly = frame.GetMethod().DeclaringType.Assembly;
                _logger.LogInformation("Got entry assembly from stack trace. Assembly full name=" + assembly.FullName);

                return assembly;
            }
            catch (Exception ex)
            {
                _logger.LogError("unable to get entry assembly from stacktrace. error = " + ex.Message, ex);
                return null;
            }
        }

        /// <summary> 获得指定下载包的完整路径 </summary>
        /// <param name="packageName" type="string">文件名</param>
        /// <returns>完整路径</returns>
        public string GetUpdatePackageFullUrl(string packageName)
        {
            return string.Format(UpdateDownloadUrl.Replace("\\", "\\\\"), packageName);
        }

        /// <summary> 创建新的WebClient </summary>
        /// <returns></returns>
        public WebClient CreateWebClient()
        {
            var client = new WebClientWrapper();
            ResetWebClient(client);

            if (!string.IsNullOrEmpty(ProxyAddress))
            {
                client.Proxy = new WebProxy(ProxyAddress);
                if (NetworkCredential != null)
                {
                    client.Proxy.Credentials = NetworkCredential;
                }
            }
            else if (NetworkCredential != null)
            {
                client.UseDefaultCredentials = false;
                client.Credentials           = NetworkCredential;
            }

            return client;
        }

        public virtual void ResetWebClient(WebClient client)
        {
            client.Headers.Clear();

            var _ua = UserAgent;
            if (string.IsNullOrEmpty(_ua))
            {
                _ua = "Fish SimpleUpdater v" + Updater.UpdaterClientVersion;
            }

            client.Headers.Add(HttpRequestHeader.UserAgent, _ua);
            //client.Headers.Add(HttpRequestHeader.IfNoneMatch, "DisableCache");
            client.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
        }

        private UpdateCheckState  _checkState;
        public event EventHandler CheckStateChanged;

        /// <summary>
        /// 获得当前更新的状态
        /// </summary>
        public UpdateCheckState CheckState
        {
            get => _checkState;
            internal set
            {
                if (_checkState == value) return;

                _checkState = value;
                OnCheckStateChanged();
            }
        }

        protected virtual void OnCheckStateChanged() { CheckStateChanged?.Invoke(this, EventArgs.Empty); }
    }
}
