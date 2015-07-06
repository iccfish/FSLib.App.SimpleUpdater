namespace FSLib.App.SimpleUpdater.Defination
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Net.Cache;
	using System.Reflection;
	using FSLib.App.SimpleUpdater.Dialogs;

	/// <summary> 表示当前更新的上下文环境 </summary>
	/// <remarks></remarks>
	public class UpdateContext
	{
		public UpdateContext()
		{
			//CurrentVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion);
			var processModule = Process.GetCurrentProcess().MainModule;

			CurrentVersion = new Version(processModule.FileVersionInfo.FileVersion);
			ApplicationDirectory = System.IO.Path.GetDirectoryName(processModule.FileName);
			AutoEndProcessesWithinAppDir = true;
			ExternalProcessID = new List<int>();
			ExternalProcessName = new List<string>();
			MultipleDownloadCount = 3;
			MaxiumRetryDownloadCount = 3;
			EnableEmbedDialog = true;
			ComponentStatus = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

			//如果当前启动路径位于TEMP目录下，则处于临时路径模式
			var temppath = System.IO.Path.GetTempPath();
			var assemblyPath = Assembly.GetExecutingAssembly().Location;
			if (assemblyPath.IndexOf(temppath, StringComparison.OrdinalIgnoreCase) != -1)
			{
				UpdateTempRoot = System.IO.Path.GetDirectoryName(assemblyPath);
				IsInUpdateMode = true;
			}
			else
			{
				UpdateTempRoot = System.IO.Path.Combine(temppath, Guid.NewGuid().ToString());
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

			AppendRandomTagInDownloadUrl = true;
		}

		/// <summary>
		/// 尝试从程序集中获得升级属性
		/// </summary>
		/// <returns></returns>
		static Assembly TryGetCallingAssembly()
		{
			Trace.TraceInformation("Tring get entry assembly from stack trace.");
			try
			{
				var st = new StackTrace();
				var frame = st.GetFrame(st.FrameCount - 1);

				var assembly = frame.GetMethod().DeclaringType.Assembly;
				Trace.TraceInformation("Got entry assembly from stack trace. Assembly full name=" + assembly.FullName);

				return assembly;
			}
			catch (Exception ex)
			{
				Trace.TraceError("unable to get entry assembly from stacktrace. error = " + ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// 获得或设置是否在正式进行更新前先通知用户
		/// </summary>
		public bool PromptUserBeforeAutomaticUpgrade
		{
			get { return _promptUserBeforeAutomaticUpgrade || (UpdateInfo != null && UpdateInfo.PromptUserBeforeAutomaticUpgrade); }
			set { _promptUserBeforeAutomaticUpgrade = value; }
		}

		/// <summary> 获得或设置是否正在更新模式中 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public bool IsInUpdateMode { get; private set; }

		/// <summary>
		/// 主要交互界面
		/// </summary>
		internal string UpdateMainFormType { get; set; }


		/// <summary>
		/// 获得或设置一个值，指示着当自动更新的时候是否将应用程序目录中的所有进程都作为主进程请求结束
		/// </summary>
		public bool AutoEndProcessesWithinAppDir
		{
			get { return _autoEndProcessesWithinAppDir || (UpdateInfo != null && UpdateInfo.AutoEndProcessesWithinAppDir); }
			set { _autoEndProcessesWithinAppDir = value; }
		}

		/// <summary>
		/// 外部要结束的进程ID列表
		/// </summary>
		public IList<int> ExternalProcessID { get; private set; }

		/// <summary>
		/// 外部要结束的进程名称
		/// </summary>
		public IList<string> ExternalProcessName { get; private set; }


		/// <summary>
		/// 获得更新中发生的错误
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
			set { _updateDownloadUrl = value; }
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
			set { _updateInfoFileName = value; }
		}

		/// <summary> 获得或设置当前的更新支持信息 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public object UpdateAttribute
		{
			get
			{
				return _updateAttribute;

			}
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
			get { return _applicationDirectory; }
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentException("ApplicationDirectory can not be null or empty.");

				_applicationDirectory = System.IO.Path.IsPathRooted(value) ? value : System.IO.Path.Combine(_applicationDirectory, value);
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

		/// <summary> 获得指定下载包的完整路径 </summary>
		/// <param name="packageName" type="string">文件名</param>
		/// <returns>完整路径</returns>
		public string GetUpdatePackageFullUrl(string packageName)
		{
			if (!string.IsNullOrEmpty(UpdateInfoFileName)) return string.Format(UpdateDownloadUrl.Replace("\\", "\\\\"), packageName);
			return (UpdateDownloadUrl.Substring(0, UpdateDownloadUrl.LastIndexOf("/") + 1) + packageName);
		}

		/// <summary> 获得或设置当前的版本 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Version CurrentVersion { get; set; }

		/// <summary> 获得或设置更新信息文件的文本 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UpdateInfoTextContent { get; internal set; }

		/// <summary> 获得或设置当前的更新信息 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public UpdateInfo UpdateInfo { get; internal set; }

		/// <summary> 获得或设置最后的版本 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Version LatestVersion { get { return UpdateInfo == null ? null : new Version(UpdateInfo.AppVersion); } }

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
			private set;
		}

		string _updateInfoFilePath;

		/// <summary> 获得当前更新信息文件保存的路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UpdateInfoFilePath
		{
			get { return _updateInfoFilePath ?? (_updateInfoFilePath = System.IO.Path.Combine(UpdateTempRoot, "update.xml")); }
		}


		string _updatePackageListPath;

		/// <summary> 获得当前要下载的包文件信息保存的路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UpdatePackageListPath
		{
			get { return _updatePackageListPath ?? (_updatePackageListPath = System.IO.Path.Combine(UpdateTempRoot, "packages.xml")); }
		}

		string _preserveFileListPath;

		/// <summary> 获得当前要保留的文件信息保存的路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string PreserveFileListPath
		{
			get { return _preserveFileListPath ?? (_preserveFileListPath = System.IO.Path.Combine(UpdateTempRoot, "reservefile.xml")); }
		}


		string _updatePackagePath;

		/// <summary> 获得当前下载的包文件目录 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UpdatePackagePath
		{
			get { return _updatePackagePath ?? (_updatePackagePath = System.IO.Path.Combine(UpdateTempRoot, "packages")); }
		}

		string _updateNewFilePath;

		/// <summary> 获得当前下载解包后的新文件路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UpdateNewFilePath
		{
			get { return _updateNewFilePath ?? (_updateNewFilePath = System.IO.Path.Combine(UpdateTempRoot, "files")); }
		}

		string _updateRollbackPath;
		private object _updateAttribute;
		private string _updateInfoFileName;
		private string _updateDownloadUrl;
		private string _logFile;
		bool _autoExitCurrentProcess;

		/// <summary> 获得当前更新过程中备份文件的路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UpdateRollbackPath
		{
			get { return _updateRollbackPath ?? (_updateRollbackPath = System.IO.Path.Combine(UpdateTempRoot, "backup")); }
		}

		/// <summary>
		/// 获得一个值，表示当前的自动升级信息是否已经下载完全
		/// </summary>
		public bool IsUpdateInfoDownloaded
		{
			get
			{
				return !string.IsNullOrEmpty(UpdateInfoTextContent) || System.IO.File.Exists(UpdateInfoFilePath);
			}
		}

		/// <summary> 获得或设置服务器用户名密码标记 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public System.Net.NetworkCredential NetworkCredential { get; set; }

		/// <summary> 获得或设置用于下载的代理服务器地址 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string ProxyAddress { get; set; }

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
				client.Credentials = NetworkCredential;
			}

			return client;
		}

		public virtual void ResetWebClient(WebClient client)
		{
			client.Headers.Clear();
			client.Headers.Add(HttpRequestHeader.UserAgent, "Fish SimpleUpdater v" + Updater.UpdaterClientVersion);
			client.Headers.Add(HttpRequestHeader.IfNoneMatch, "DisableCache");
			client.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
			client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
		}

		/// <summary> 获得是否找到更新的标记位 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public bool HasUpdate { get; internal set; }

		/// <summary> 获得表示是否当前版本过低而无法升级的标记位 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public bool CurrentVersionTooLow { get; internal set; }

		/// <summary> 获得或设置日志文件名 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string LogFile
		{
			get { return _logFile; }
			set
			{
				if (string.Compare(_logFile, value, true) == 0) return;

				_logFile = value;
				if (_logger != null)
				{
					_logger.Close();
					Trace.Listeners.Remove(_logger);
				}
				if (!string.IsNullOrEmpty(_logFile))
				{
					if (!System.IO.Path.IsPathRooted(_logFile)) _logFile = Environment.ExpandEnvironmentVariables("%TEMP%\\" + _logFile);

					if (System.IO.File.Exists(_logFile)) System.IO.File.Delete(_logFile);
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_logFile));

					_logger = new TextWriterTraceListener(_logFile);
					//_logger.TraceOutputOptions = System.Diagnostics.TraceOptions.DateTime | System.Diagnostics.TraceOptions.ProcessId | System.Diagnostics.TraceOptions.ThreadId | System.Diagnostics.TraceOptions.LogicalOperationStack;
					Trace.Listeners.Add(_logger);
				}
			}
		}

		/// <summary>
		/// 获得或设置是否不经提示便自动更新
		/// </summary>
		public bool ForceUpdate
		{
			get { return _forceUpdate || (UpdateInfo != null && UpdateInfo.ForceUpdate); }
			set { _forceUpdate = value; }
		}

		/// <summary>
		/// 获得或设置是否强制更新，否则退出
		/// </summary>
		public bool MustUpdate
		{
			get { return _mustUpdate || (UpdateInfo != null && UpdateInfo.MustUpdate); }
			set { _mustUpdate = value; }
		}

		/// <summary>
		/// 获得或设置是否在更新时自动结束进程
		/// </summary>
		public bool AutoKillProcesses
		{
			get { return _autoKillProcesses || (UpdateInfo != null && UpdateInfo.AutoKillProcesses); }
			set { _autoKillProcesses = value; }
		}

		/// <summary>
		/// 获得或设置是否自动退出当前进程
		/// </summary>
		public bool AutoExitCurrentProcess
		{
			get { return _autoExitCurrentProcess || (UpdateInfo != null && UpdateInfo.AutoExitCurrentProcess); }
			set { _autoExitCurrentProcess = value; }
		}


		/// <summary>
		/// 是否隐藏所有对话框显示
		/// </summary>
		public bool HiddenUI
		{
			get { return _hiddenUI; }
			set
			{
				_hiddenUI = value;
				if (value)
				{
					ForceUpdate = true;
					AutoKillProcesses = true;
				}
			}
		}

		TextWriterTraceListener _logger;
		private string _applicationDirectory;
		bool _hiddenUI;
		bool _forceUpdate;
		bool _autoKillProcesses;
		bool _mustUpdate;
		bool _autoEndProcessesWithinAppDir;
		bool _promptUserBeforeAutomaticUpgrade;

		/// <summary>
		/// 获得或设置是否在下载地址中附加随机码以避免缓存。默认值：true
		/// </summary>
		public bool AppendRandomTagInDownloadUrl { get; set; }

		/// <summary>
		/// 随机化网址
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		internal string RandomUrl(string url)
		{
			if (String.IsNullOrEmpty(url))
				throw new ArgumentException("url is null or empty.", "url");

			if (!AppendRandomTagInDownloadUrl || url.IndexOf('/') == -1 || url.IndexOf("http", StringComparison.OrdinalIgnoreCase) != 0)
				return url;

			if (url.IndexOf('?') == -1)
				return url + "?" + new Random().NextDouble().ToString();

			return url + "&" + new Random().NextDouble().ToString();
		}

		/// <summary>
		/// 获得更新程序是否已经成功启动了
		/// </summary>
		public bool? IsUpdaterSuccessfullyStarted { get; internal set; }

		/// <summary>
		/// 设置当出现错误的时候，是否按照有更新但是未更新处理。
		/// 这个选项影响设置必须强制更新的选项。
		/// 如果检测更新遇到错误，此选项设置为false时，则按照“未找到更新”处理；如果此选项设置为true，则按照“有更新但是没有更新”处理，会强制退出软件。
		/// </summary>
		public bool TreatErrorAsNotUpdated { get; set; }

		/// <summary>
		/// 获得组件状态
		/// </summary>
		public Dictionary<string, bool> ComponentStatus { get; private set; }
	}
}

