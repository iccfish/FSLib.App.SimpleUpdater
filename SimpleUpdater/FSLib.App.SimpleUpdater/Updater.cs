using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using FSLib.App.SimpleUpdater.Annotations;
using FSLib.App.SimpleUpdater.Dialogs;
using FSLib.App.SimpleUpdater.Wrapper;


namespace FSLib.App.SimpleUpdater
{
	using Defination;

	using Logs;

	/// <summary>
	/// 自动更新操作类
	/// </summary>
	public partial class Updater : IDisposable
	{
		/// <summary> 获得当前更新的上下文 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public UpdateContext Context { get; private set; }

		private static ILogger _logger = LogManager.Instance.GetLogger<Updater>();

		#region 构造函数

		/// <summary>
		/// 手动创建更新类，并指定当前版本和应用程序目录
		/// </summary>
		/// <param name="appVersion">指定的应用程序版本</param>
		/// <param name="appDirectory">指定的应用程序路径</param>
		protected Updater(Version appVersion, string appDirectory)
			: this()
		{
			if (appVersion != null)
				Context.CurrentVersion = appVersion;
			if (!string.IsNullOrEmpty(appDirectory))
				Context.ApplicationDirectory = appDirectory;
		}

		/// <summary>
		/// 使用指定的信息类来提供应用程序需要的信息
		/// </summary>
		protected Updater()
		{
			Context = new UpdateContext();
			PackagesToUpdate = new List<PackageInfo>();
			UpdatesFound += Instance_UpdatesFound;
			MinmumVersionRequired += Instance_MinmumVersionRequired;
			if (_instance == null) _instance = this;

			//初始化工作参数
			InitializeParameters();
		}

		#region Dispose 模式

		bool _disposed;

		/// <summary>
		/// 释放
		/// </summary>
		/// <param name="disposing">是否是主动调用</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;

			_disposed = true;

			if (disposing)
			{
				Context.LogFile = null;
			}
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
		}

		~Updater()
		{
			Dispose(false);
		}

		#endregion

		#endregion

		#region 初始化函数

		/// <summary>
		/// 初始化工作参数
		/// </summary>
		void InitializeParameters()
		{
			var args = Environment.GetCommandLineArgs();
			if (args.Length < 2 || (args[1] != "/startupdate" && args[1] != "/selfupdate")) return;

			var index = 2;
			while (index < args.Length)
			{
				var name = args[index++];

				switch (name)
				{
					case "/ui":
						Context.UpdateMainFormType = args[index++];
						break;
					case "/assembly":
						LoadExtraAssemblies(args[index++]);
						break;
					case "/cv": Context.CurrentVersion = new Version(args[index++]); break;
					case "/ad": Context.ApplicationDirectory = args[index++]; break;
					case "/url": Context.UpdateDownloadUrl = args[index++]; break;
					case "/infofile": Context.UpdateInfoFileName = args[index++]; break;
					case "/proxy": Context.ProxyAddress = args[index++]; break;
					case "/cred":
						var value = args[index++];
						if (!string.IsNullOrEmpty(value))
						{
							var info = value.Split(':');
							if (info.Length == 2 && !string.IsNullOrEmpty(info[0]))
							{
								Context.NetworkCredential = new NetworkCredential(info[0], info[1]);
							}
						}
						break;
					case "/p":
						var pi = args[index++];
						if (pi.StartsWith("*")) Context.ExternalProcessID.Add(int.Parse(pi.Remove(0, 1)));
						else Context.ExternalProcessName.Add(pi);
						break;
					case "/log": Context.LogFile = args[index++]; break;
					case "/forceupdate": Context.ForceUpdate = true; break;
					case "/autokill": Context.AutoKillProcesses = true; break;
					case "/noui": Context.HiddenUI = true; break;
				}
			}
		}

		#endregion

		#region 检查更新部分

		/// <summary>
		/// 开始检测更新
		/// </summary>
		/// <returns>返回是否成功开始检查</returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public virtual bool BeginCheckUpdateInProcess()
		{
			var url = Context.UpdateInfoFileUrl;

			if (string.IsNullOrEmpty(url)) throw new InvalidOperationException(SR.Updater_AssemblyNotMarkedAsUpdateable);
			if (Context.IsInUpdating) return false;

			var bgw = new BackgroundWorker()
			{
				WorkerSupportReportProgress = true
			};
			bgw.DoWork += DownloadUpdateInfoInternal;
			bgw.WorkFailed += (s, e) =>
			{
				Context.IsInUpdating = false;
				Context.Exception = e.Exception;
				_logger.LogError("Loading update infomation failed: " + e.Exception.Message, e.Exception);
				OnError();
				OnCheckUpdateComplete();
			};
			bgw.WorkCompleted += (s, e) =>
			{
				if (Context.UpdateInfo == null) return;

				Context.IsInUpdating = false;

				if (Context.CurrentVersionTooLow)
				{
					OnMinmumVersionRequired();
					Context.Exception = new VersionTooLowException(Context.CurrentVersion, new Version(Context.UpdateInfo.RequiredMinVersion));
					OnError();
				}
				else if (!Context.HasUpdate)
				{
					OnNoUpdatesFound();
				}
				else OnUpdatesFound();
				OnCheckUpdateComplete();
			};
			bgw.WorkerProgressChanged += (s, e) => OnOperationProgressChanged(e);
			Context.IsInUpdating = true;
			bgw.RunWorkASync();
			//启动外部清理进程。
			CleanTemp();

			return true;
		}

		/// <summary>
		/// 下载更新信息
		/// </summary>
		/// <exception cref="System.ApplicationException">服务器返回了不正确的更新结果</exception>
		void DownloadUpdateInfoInternal(object sender, RunworkEventArgs e)
		{
			//下载更新信息
			e.PostEvent(OnDownloadUpdateInfo);

			Context.Init();

			var localFile = Context.UpdateInfoFilePath;

			if (File.Exists(localFile))
			{
				_logger.LogInformation("Reading local update information file '" + localFile + "'");
				Context.UpdateInfoTextContent = File.ReadAllText(localFile, Encoding.UTF8);
			}
			else
			{
				//下载信息时不直接下载到文件中.这样不会导致始终创建文件夹
				Exception ex = null;
				byte[] data = null;
				var url = Context.UpdateInfoFileUrl;

				var client = Context.CreateWebClient();
				client.DownloadProgressChanged += (x, y) => e.ReportProgress((int)y.TotalBytesToReceive, (int)y.BytesReceived);

				//远程下载。为了支持进度显示，这里必须使用异步下载
				using (var wHandler = new AutoResetEvent(false))
				{
					client.DownloadDataCompleted += (x, y) =>
					{
						ex = y.Error;
						if (ex == null)
						{
							data = y.Result;
						}
						wHandler.Set();
					};
					_logger.LogInformation("Downloading update info from '" + url + "'");
					client.DownloadDataAsync(new Uri(url));
					//等待下载完成
					wHandler.WaitOne();
				}
				if (ex != null) throw ex;

				_logger.LogInformation("Got data of " + (data == null ? "<null>" : data.Length.ToString() + "bytes"));
				if (data != null && data.Length > 0x10)
				{
					//不是<xml标记，则执行解压缩
					if (BitConverter.ToInt32(data, 0) != 0x6D783F3C && BitConverter.ToInt32(data, 0) != 0x3CBFBBEF)
					{
						_logger.LogInformation("Trying to decompress data...");
						data = ExtensionMethod.Decompress(data);
					}
					Context.UpdateInfoTextContent = Encoding.UTF8.GetString(data);
				}

				//是否返回了正确的结果?
				if (string.IsNullOrEmpty(Context.UpdateInfoTextContent))
				{
					throw new ApplicationException("Unable to read update info.");
				}

			}
			e.PostEvent(OnDownloadUpdateInfoFinished);
			if ((Context.UpdateInfo = XMLSerializeHelper.XmlDeserializeFromString<UpdateInfo>(Context.UpdateInfoTextContent)) == null)
			{
				throw new ApplicationException("Update info download failed");
			}
			_logger.LogInformation($"Server version: {Context.UpdateInfo.AppVersion}");
			_logger.LogInformation($"Current Version: {Context.CurrentVersion}");
			//设置必须的属性
			if (Context.UpdateInfo.MustUpdate)
			{
				Context.AutoKillProcesses = true;
				_logger.LogInformation("Flag 'AUTO_CLOSE_PROCESS' was set.");
				Context.AutoEndProcessesWithinAppDir = true;
				_logger.LogInformation("Flag 'AUTO_CLOSE_PROCESS_IN_SAME_DIR' was set.");
				Context.ForceUpdate = true;
				_logger.LogInformation("Flag 'FORCE_UPDATE' was set.");
			}

			//判断升级
			if (!string.IsNullOrEmpty(Context.UpdateInfo.RequiredMinVersion) && Context.CurrentVersion < new Version(Context.UpdateInfo.RequiredMinVersion))
			{
				Context.CurrentVersionTooLow = true;
				_logger.LogWarning($"Unable to perform update due to current application version was too low. Minimum version '{Context.UpdateInfo.RequiredMinVersion}' was required, current was '{Context.CurrentVersion}'");
			}
			else
			{
				Context.HasUpdate = new Version(Context.UpdateInfo.AppVersion) > Context.CurrentVersion;
				_logger.LogInformation("Updates found: " + Context.HasUpdate);
			}

			if (Context.HasUpdate)
			{
				//判断要升级的包
				if (PackagesToUpdate == null || PackagesToUpdate.Count == 0)
				{
					var pkgList = Context.UpdatePackageListPath;
					_logger.LogInformation($"External package list: {pkgList}");

					if (File.Exists(pkgList))
					{
						_logger.LogInformation("External package list: load success.");
						PackagesToUpdate = XMLSerializeHelper.XmlDeserializeFromString<List<PackageInfo>>(File.ReadAllText(pkgList, Encoding.UTF8));
						PackagesToUpdate.ForEach(s => s.Context = Context);
					}
					else
					{
						_logger.LogInformation("External package list: not exist, recreating.");
						GatheringDownloadPackages(e);
					}

					var preserveFileList = Context.PreserveFileListPath;
					_logger.LogInformation($"External reserve list: {preserveFileList}");
					if (File.Exists(preserveFileList))
					{
						_logger.LogInformation("External reserve list: load success.");
						var list = XMLSerializeHelper.XmlDeserializeFromString<List<string>>(File.ReadAllText(preserveFileList, Encoding.UTF8));
						list.ForEach(s => FileInstaller.PreservedFiles.Add(s, null));
					}
					else
					{
						_logger.LogInformation("External reserve list: not exist, recreating.");
					}
				}
			}

			//如果没有要升级的包？虽然很奇怪，但依然当作不需要升级
			if (Context.HasUpdate)
			{
				if (PackagesToUpdate.Count == 0)
				{
					Context.HasUpdate = false;
					_logger.LogWarning("Warning: updates found, but no package was marked as updatable, so treat as no update.");
				}
			}
		}

		#region 确定要下载的包

		/// <summary> 生成下载列表 </summary>
		void GatheringDownloadPackages(RunworkEventArgs rt)
		{
			if (PackagesToUpdate.Count > 0) return;

			var info = Context.UpdateInfo;

			_logger.LogInformation("Begin create update package list.");
			rt.PostEvent(OnGatheringPackages);

			if (!string.IsNullOrEmpty(info.Package) && (info.Packages == null || info.Packages.Count == 0))
			{
				//必须更新的包
				_logger.LogInformation("Adding main package.");
				PackagesToUpdate.Add(new PackageInfo()
				{
					FilePath = "",
					FileSize = 0,
					PackageHash = info.MD5,
					Method = UpdateMethod.Always,
					PackageName = info.Package,
					PackageSize = info.PackageSize,
					VerificationLevel = FileVerificationLevel.Hash,
					Version = "0.0.0.0",
					Context = Context
				});
			}
			if (info.Packages != null)
			{
				//判断增量升级包
				var index = 0;
				foreach (var pkg in info.Packages)
				{
					rt.ReportProgress(++index, info.Packages.Count);
					var localPath = Path.Combine(Context.ApplicationDirectory, pkg.FilePath); //对比本地路径
					pkg.Context = Context;

					if (pkg.Method == UpdateMethod.Always)
					{
						_logger.LogInformation("Package '" + pkg.PackageName + "' was added as always update.");
						PackagesToUpdate.Add(pkg);
						continue;
					}
					//判断组件标记
					if (!string.IsNullOrEmpty(pkg.ComponentId) && !CheckComponentFlag(pkg.ComponentId))
					{
						_logger.LogInformation("Component '" + pkg.ComponentId + "' marked as false, package '" + pkg.PackageName + "' skipped.");
						continue;
					}


					//存在即跳过，或版本比较
					if (!File.Exists(localPath))
					{
						if (Utility.HasMethod(pkg.Method, UpdateMethod.SkipIfNotExist))
						{
							_logger.LogInformation("Local file '" + pkg.FilePath + "' not exist, package '' was skipped due to config as skip if not exist.");
						}
						else
						{

							PackagesToUpdate.Add(pkg);
							_logger.LogInformation("Local file '" + pkg.FilePath + "' not exist, package '" + pkg.PackageName + "' was added.");
						}
						continue;
					}
					//如果存在即跳过……那么你好去跳过了。
					if (Utility.HasMethod(pkg.Method, UpdateMethod.SkipIfExists))
					{
						AddPackageToPreserveList(pkg);
						_logger.LogInformation("Local file '" + pkg.FilePath + "' exists, package '" + pkg.PackageName + "' was skipped.");
						continue;
					}

					var isNewer = (pkg.HasVerifyFlag(FileVerificationLevel.Size) && new FileInfo(localPath).Length != pkg.FileSize)
						||
						(pkg.HasVerifyFlag(FileVerificationLevel.Version) && (string.IsNullOrEmpty(pkg.Version) || ExtensionMethod.CompareVersion(localPath, pkg.Version)))
						||
						(pkg.HasVerifyFlag(FileVerificationLevel.Hash) && ExtensionMethod.GetFileHash(localPath, pkg.IsHashMd5) != pkg.FileHash)
						;

					if (isNewer)
					{
						_logger.LogInformation("Package '" + pkg.PackageName + "' was added due to server version newer.");
						pkg.Context = Context;
						PackagesToUpdate.Add(pkg);
					}
					else
					{
						AddPackageToPreserveList(pkg);
						_logger.LogInformation("Package '" + pkg.PackageName + "' was skipped due to server version was lower or same.");
					}
				}
			}

			if (!Context.NeedStandaloneUpdateClientSupport && info.UpdaterClient != null && info.UseServerUpdateClient)
			{
				var currentVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				if (new Version(info.UpdaterClient.Version) != ExtensionMethod.ConvertVersionInfo(currentVersion))
				{
					_logger.LogInformation($"server updater client not same, useing server version due to update policy (server={info.UpdaterClient.Version}, local={currentVersion.FileVersion}). External update process will using newer updater client to execute.");
					Context.NeedStandaloneUpdateClientSupport = true;
				}
			}

			if (PackagesToUpdate.Count > 0 && Context.NeedStandaloneUpdateClientSupport)
			{
				if (info.UpdaterClient == null)
					throw new ApplicationException("in single file release mode, updater client need a standalone client lib to perform upgrade. Please make sure the update packages was built with an updated package builder.");

				if (!ExtensionMethod.Any(PackagesToUpdate, s => s.PackageName == info.UpdaterClient.PackageName))
				{
					_logger.LogInformation($"adding standalone updater lib to package list ({info.UpdaterClient.PackageName}).");
					PackagesToUpdate.Add(info.UpdaterClient);
				}
			}

			rt.PostEvent(OnGatheredPackages);
			_logger.LogInformation("done create update package list.");
		}

		/// <summary>
		/// 将指定包的文件添加到忽略列表
		/// </summary>
		/// <param name="pkg"></param>
		void AddPackageToPreserveList(PackageInfo pkg)
		{
			if (pkg == null || pkg.Files == null) return;

			var reserveDic = FileInstaller.PreservedFiles;
			foreach (var file in pkg.Files)
			{
				if (!reserveDic.ContainsKey(file))
				{
					_logger.LogInformation($"adding file '{file}' reserve file list due to skipped package download.");
					reserveDic.Add(file, null);
				}
			}
		}

		#endregion

		#endregion

		#region 更新主要步骤


		#region 关闭外部程序

		/// <summary>
		/// 关闭主程序进程
		/// </summary>
		bool CloseApplication(RunworkEventArgs e)
		{
			_logger.LogInformation("开始关闭进程");

			e.ReportProgress(0, 0, "正在关闭进程....");
			var closeApplication = new List<Process>();

			foreach (var pid in Context.ExternalProcessID)
			{
				try
				{
					closeApplication.Add(Process.GetProcessById(pid));
					_logger.LogInformation($"添加进程PID={pid}到等待关闭列表");
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"添加进程PID={pid}到等待关闭列表时出错：{ex.Message}");
				}
			}
			foreach (var pn in Context.ExternalProcessName)
			{
				closeApplication.AddRange(Process.GetProcessesByName(pn));
				_logger.LogInformation($"添加进程名={pn}到等待关闭列表");
			}

			if (closeApplication.Count > 0)
			{
				//是否强制关闭进程？
				if (Context.AutoKillProcesses)
				{
					_logger.LogInformation("已开启自动结束所有进程。正在结束进程。");
					foreach (var s in closeApplication)
					{
						if (s.HasExited)
						{
							_logger.LogInformation($"进程 PID={s.Id} 已经提前退出。");
						}
						else
						{
							try
							{
								s.Kill();
								_logger.LogInformation($"进程 [PID={s.Id}] 已经成功结束。");
							}
							catch (Exception ex)
							{
								_logger.LogError($"进程【{s.Id}】结束失败：{ex.Message}", ex);

								return false;
							}
						}
					}
					return true;
				}

				var evt = new QueryCloseApplicationEventArgs(closeApplication, NotifyUserToCloseApp);
				e.PostEvent(_ => OnQueryCloseApplication(evt));
				while (!evt.IsCancelled.HasValue)
				{
					Thread.Sleep(100);
				}
				return !evt.IsCancelled.Value;
			}

			return true;
		}
		/// <summary>
		/// 提示用户关闭程序
		/// </summary>
		/// <returns></returns>
		void NotifyUserToCloseApp(QueryCloseApplicationEventArgs e)
		{
			using (var ca = new CloseApp())
			{
				ca.AttachProcessList(e.Processes);
				e.IsCancelled = ca.ShowDialog() != DialogResult.OK;
			}
		}
		#endregion

		#region 外部进程调度

		/// <summary>
		/// 替换环境变量
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		string ReplaceEnvVar(string v)
		{
			if (string.IsNullOrEmpty(v))
				return v;
			v = v.Replace("$appdir$", "\"" + Context.ApplicationDirectory + "\"");

			return v;
		}

		/// <summary> 设置启动信息的环境变量 </summary>
		/// <param name="psi" type="System.Diagnostics.ProcessStartInfo">类型为 <see>System.Diagnostics.ProcessStartInfo</see> 的参数</param>
		void SetProcessEnvVar(ProcessStartInfo psi)
		{
			var fileName = psi.FileName;
			var extension = Path.GetExtension(fileName).ToLower();

			if (extension == ".bat" || extension == ".cmd")
			{
				//命令行模式，为了能使用环境变量，替换命令行
				psi.FileName = "cmd.exe";
				psi.Arguments = "/c \"" + fileName + "\" " + psi.Arguments;
				extension = ".exe";
			}
			if (extension != ".exe" && extension != ".com") return;

			psi.UseShellExecute = false;
			var props = typeof(UpdateContext).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (var p in props)
			{
				if (p.PropertyType != typeof(bool) && p.PropertyType != typeof(int) && p.PropertyType != typeof(string) && p.PropertyType != typeof(Version)) continue;

				var value = (p.GetValue(Context, null) ?? "").ToString();
				if (value.Length > 255) continue;   //当属性内容过长时, 忽略设置此属性, 防止设置导致环境变量过长抛出异常.

				if (!psi.EnvironmentVariables.ContainsKey(p.Name)) psi.EnvironmentVariables.Add(p.Name, value);
			}

			props = typeof(UpdateInfo).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (var p in props)
			{
				if (p.PropertyType != typeof(bool) && p.PropertyType != typeof(int) && p.PropertyType != typeof(string) && p.PropertyType != typeof(Version)) continue;

				var value = (p.GetValue(Context.UpdateInfo, null) ?? "").ToString();
				if (value.Length > 255) continue;   //当属性内容过长时, 忽略设置此属性, 防止设置导致环境变量过长抛出异常.
				if (!psi.EnvironmentVariables.ContainsKey(p.Name)) psi.EnvironmentVariables.Add(p.Name, value);
			}
		}

		/// <summary>
		/// 执行外部进程
		/// </summary>
		/// <param name="program"></param>
		/// <param name="arguments"></param>
		/// <param name="waitingForExit">是否等待进程退出</param>
		/// <param name="hide">是否隐藏进程执行的窗口</param>
		/// <returns></returns>
		bool RunExternalProgram(RunworkEventArgs e, string program, string arguments, bool waitingForExit, bool hide)
		{
			if (string.IsNullOrEmpty(program)) return true;

			var psi = new ProcessStartInfo(program, ReplaceEnvVar(arguments))
			{
				WindowStyle = hide ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal
			};
			SetProcessEnvVar(psi);
			_logger.LogInformation($"Executing external process, path: {psi.FileName}, arguments:{psi.Arguments}");
			e.PostEvent(RunExternalProcess, this, new RunExternalProcessEventArgs(psi));
			var p = Process.Start(psi);

			if (waitingForExit)
			{
				_logger.LogInformation("Waiting for external process exit...");
				if (Context.UpdateInfo.ExecuteTimeout > 0)
				{
					p.WaitForExit(1000 * Context.UpdateInfo.ExecuteTimeout);
				}
				else p.WaitForExit();

				Action<Process> actor = m =>
				{
					var pet = new ProgramExecuteTimeout();
					if (pet.ShowDialog() == DialogResult.OK)
					{
						if (!m.HasExited) m.Kill();
					}
				};
				while (!p.HasExited)
				{
					Application.OpenForms[0].Invoke(actor, p);
					if (!p.HasExited) p.WaitForExit(1000 * Context.UpdateInfo.ExecuteTimeout);
				}
			}
			_logger.LogInformation("External process exited.");

			return true;
		}
		/// <summary>
		/// 执行外部进程-安装后
		/// </summary>
		/// <returns></returns>
		bool RunExternalProgramAfter(RunworkEventArgs e)
		{
			if (string.IsNullOrEmpty(Context.UpdateInfo.FileExecuteAfter)) return true;
			return RunExternalProgram(e, Path.Combine(Context.ApplicationDirectory, Context.UpdateInfo.FileExecuteAfter), Context.UpdateInfo.ExecuteArgumentAfter, false, Context.UpdateInfo.HideAfterExecuteWindow);
		}

		/// <summary>
		/// 执行外部进程-安装前
		/// </summary>
		/// <returns></returns>
		bool RunExternalProgramBefore(RunworkEventArgs e)
		{
			if (string.IsNullOrEmpty(Context.UpdateInfo.FileExecuteBefore)) return true;
			return RunExternalProgram(e, Path.Combine(Context.UpdateNewFilePath, Context.UpdateInfo.FileExecuteBefore), Context.UpdateInfo.ExecuteArgumentBefore, true, Context.UpdateInfo.HideBeforeExecuteWindow);
		}
		#endregion

		#region 更新包下载

		#region 公共属性

		/// <summary> 获得当前需要下载的升级包列表 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public List<PackageInfo> PackagesToUpdate { get; private set; }

		/// <summary> 获得当前需要下载的升级包数目 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public int PackageCount
		{
			get { return PackagesToUpdate.Count; }
		}

		/// <summary> 获得已完成下载的任务个数 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public int DownloadedPackageCount
		{
			get
			{
				return ExtensionMethod.Count(PackagesToUpdate, s => s.IsDownloaded);
			}
		}

		/// <summary> 获得正在下载的任务个数 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public int DownloadingPackageCount
		{
			get
			{
				return ExtensionMethod.Count(PackagesToUpdate, s => s.IsDownloading);
			}
		}

		#endregion


		/// <summary> 执行下载 </summary>
		bool DownloadPackages(RunworkEventArgs rt)
		{
			Directory.CreateDirectory(Context.UpdatePackagePath);

			_logger.LogInformation("Begin download package.");

			var workerCount = Math.Max(1, Context.MultipleDownloadCount);
			var workers = new List<WebClient>(workerCount);
			var evt = new AutoResetEvent(false);
			var hasError = false;

			//download redirect
			if (!string.IsNullOrEmpty(Context.UpdateInfo.PackageUrlTemplate))
			{
				_logger.LogInformation("redirect download location to {0}", Context.UpdateInfo.PackageUrlTemplate);
				Context.UpdateDownloadUrl = Context.UpdateInfo.PackageUrlTemplate;
				Context.UpdateInfoFileName = null;
				Context.Init();
			}

			//Ping
			if (!string.IsNullOrEmpty(Context.UpdateInfo.UpdatePingUrl))
			{
				try
				{
					Context.CreateWebClient().UploadData(new Uri(Context.UpdateInfo.UpdatePingUrl), new byte[0]);
				}
				catch (Exception)
				{
				}
			}

			//生成下载队列
			_logger.LogInformation($"Inintializing {workerCount} webClients");
			for (var i = 0; i < workerCount; i++)
			{
				var clnt = Context.CreateWebClient();
				clnt.DownloadFileCompleted += (s, e) =>
				{
					var pkg = e.UserState as PackageInfo;
					var cnt = s as WebClient;

					pkg.LastError = e.Error;
					if (e.Error != null)
					{
						_logger.LogWarning($"Package '{pkg.PackageName}' download failed, error:{e.Error.Message}");
						rt.PostEvent(PackageDownloadFailed, this, new PackageEventArgs(pkg));
					}
					else if (pkg.IsLocalFileValid != true)
					{
						_logger.LogWarning($"The hash verification of package '{pkg.PackageName}' failed, excepted:{pkg.PackageHash}, actual:{pkg.LocalHash}");
						pkg.LastError = new Exception("Hash mismatch or file not exist.");
						rt.PostEvent(PackageHashMismatch, this, new PackageEventArgs(pkg));
					}

					if (pkg.LastError != null)
					{
						//如果出错，且重试次数在限制范围内，则重试
						pkg.IncreaseFailureCounter();
						if (pkg.RetryCount <= Context.MaxiumRetryDownloadCount)
						{
							_logger.LogWarning($"Package '{pkg.PackageName}' download failed, begin NO.{pkg.RetryCount} retry, max try count set to {Context.MaxiumRetryDownloadCount}");
							cnt.DownloadFileAsync(new Uri(pkg.SourceUri), pkg.LocalSavePath, pkg);
							rt.PostEvent(PackageDownloadRetried, this, new PackageEventArgs(pkg));
							return;
						}
						//标记出错
						hasError = true;
					}

					//包下载完成事件
					pkg.IsDownloading = false;
					pkg.IsDownloaded = pkg.LastError == null;
					rt.PostEvent(PackageDownloadFinished, this, new PackageEventArgs(e.UserState as PackageInfo));

					lock (PackagesToUpdate)
					{
						_logger.LogInformation($"Package NO.{pkg.PackageName} download finished:{(pkg.IsDownloaded ? "succeed" : "failed")}");
						evt.Set();
					}
				};
				clnt.DownloadProgressChanged += (s, e) =>
					{
						var pkg = e.UserState as PackageInfo;
						pkg.DownloadedSize = e.BytesReceived;
						pkg.PackageSize = e.TotalBytesToReceive > 0 ? e.TotalBytesToReceive : pkg.PackageSize;
						rt.PostEvent(DownloadProgressChanged, this,
									 new PackageDownloadProgressChangedEventArgs(pkg, pkg.PackageSize, pkg.DownloadedSize, e.ProgressPercentage)
									);
					};
				workers.Add(clnt);
			}

			//开始处理事务
			while (!hasError)
			{
				var breakFlag = false;
				lock (PackagesToUpdate)
				{
					//没有错误，则分配下个任务
					WebClient client;
					while ((client = workers.Find(s => !s.IsBusy)) != null)
					{
						var nextPkg = PackagesToUpdate.Find(s => !s.IsDownloading && !s.IsDownloaded);
						if (nextPkg == null)
						{
							breakFlag = true;
							break;
						}

						nextPkg.IsDownloading = true;
						_logger.LogInformation($"Start download package '{nextPkg.PackageName}'");
						rt.PostEvent(PackageDownload, this, new PackageEventArgs(nextPkg));
						Context.ResetWebClient(client);
						client.DownloadFileAsync(new Uri(Context.GetUpdatePackageFullUrl(nextPkg.PackageName)), nextPkg.LocalSavePath, nextPkg);
					}
				}
				if (breakFlag) break;
				evt.WaitOne();
				_logger.LogInformation("Thread sync signal received.");
			}
			//不管任何原因中止下载，到这里时都需要等待所有客户端中止
			while (true)
			{
				//出错了，那么对所有的客户端发出中止命令。这里不需要判断是否忙碌。
				if (hasError)
				{
					_logger.LogWarning("Errors happed, cancelling all downloads.");
					workers.ForEach(s => s.CancelAsync());
				}
				lock (PackagesToUpdate)
				{
					_logger.LogInformation("Waiting for download queue complete.");
					if (workers.FindIndex(s => s.IsBusy) == -1) break;
				}
				evt.WaitOne();
			}
			_logger.LogInformation("Complete package download.");

			var errorPkgs = ExtensionMethod.ToList(ExtensionMethod.Where(PackagesToUpdate, s => s.LastError != null));
			if (errorPkgs.Count > 0) throw new PackageDownloadException(errorPkgs.ToArray());

			return !hasError;
		}

		#endregion

		/// <summary>
		/// 开始进行更新
		/// </summary>
		internal void BeginUpdate()
		{
			var bgw = new BackgroundWorker() { WorkerSupportReportProgress = true };
			bgw.DoWork += UpdateInternal;
			bgw.WorkerProgressChanged += (s, e) => OnOperationProgressChanged(e);
			bgw.WorkFailed += (s, e) =>
			{
				Context.IsInUpdating = false;
				Context.Exception = e.Exception;
				_logger.LogError("Update abored due to error:" + e.Exception.Message, e.Exception);
				OnError();
			};
			bgw.WorkCompleted += (s, e) =>
			{
				Context.IsInUpdating = false;
			};
			Context.IsInUpdating = true;
			bgw.RunWorkASync();
			CleanTemp();
		}

		FileInstaller _installer;

		/// <summary> 获得当前用于安装文件的对象 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public FileInstaller FileInstaller => _installer ?? (_installer = new FileInstaller());


		//BMK 更新主函数 (正式更新)
		/// <summary>
		/// 运行更新进程(主更新进程)
		/// </summary>
		/// <exception cref="System.InvalidProgramException"></exception>
		/// <exception cref="System.Exception"></exception>
		void UpdateInternal(object sender, RunworkEventArgs e)
		{
			DownloadUpdateInfoInternal(sender, e);

			//下载升级包。下载完成的时候校验也就完成了
			if (!DownloadPackages(e)) return;

			//解压缩升级包
			ExtractPackage(e);

			//关闭主程序
			if (!CloseApplication(e)) throw new OperationCanceledException(SR.Updater_UpdateCanceledByCloseApp);

			//运行安装前进程
			e.PostEvent(OnExecuteExternalProcessBefore);
			RunExternalProgramBefore(e);

			//安装文件
			e.PostEvent(OnInstallUpdates);
			FileInstaller.UpdateInfo = Context.UpdateInfo;
			FileInstaller.ApplicationRoot = Context.ApplicationDirectory;
			FileInstaller.WorkingRoot = Context.UpdateTempRoot;
			FileInstaller.SourceFolder = Context.UpdateNewFilePath;
			if (!FileInstaller.Install(e))
			{
				throw FileInstaller.Exception;
			}

			//运行安装后进程
			e.PostEvent(OnExecuteExternalProcessAfter);
			RunExternalProgramAfter(e);

			//完成更新
			e.PostEvent(OnUpdateFinsihed);
		}


		/// <summary>
		/// 解开安装包
		/// </summary>
		void ExtractPackage(RunworkEventArgs rt)
		{
			_logger.LogInformation("Start extract packages.");
			rt.PostEvent(() => OnPackageExtractionBegin(new PackageEventArgs(null)));

			var count = PackagesToUpdate.Count;
			var index = 0;
			var fze = new ICCEmbedded.SharpZipLib.Zip.FastZipEvents();
			fze.ProcessFile += (s, e) => rt.ReportProgress(count, index, string.Format(SR.Updater_ExtractingFile, e.Name));
			var fz = new ICCEmbedded.SharpZipLib.Zip.FastZip(fze);
			if (!string.IsNullOrEmpty(Context.UpdateInfo.PackagePassword))
			{
				fz.Password = Context.UpdateInfo.PackagePassword;
			}

			foreach (var pkg in PackagesToUpdate)
			{
				index++;

				_logger.LogInformation("Extract " + pkg.PackageName);
				rt.PostEvent(() => OnPackageExtractionBegin(new PackageEventArgs(pkg)));

				fz.ExtractZip(pkg.LocalSavePath, Context.UpdateNewFilePath, null);

				rt.PostEvent(() => OnPackageExtractionEnd(new PackageEventArgs(pkg)));
				_logger.LogInformation("Done extraction " + pkg.PackageName);
			}

			//开始解压独立包
			if (Context.NeedStandaloneUpdateClientSupport)
			{
				var pkg = ExtensionMethod.First(PackagesToUpdate, ele => ele.PackageName == Context.UpdateInfo.UpdaterClient.PackageName);
				fz.ExtractZip(pkg.LocalSavePath, Context.UpdateTempRoot, null);
			}

			rt.PostEvent(() => OnPackageExtractionEnd(new PackageEventArgs(null)));
			_logger.LogInformation("Done package extraction.");
		}

		#endregion

		#region 启动外部更新进程

		/// <summary>
		/// 确保更新已经启动
		/// </summary>
		internal void EnsureUpdateStarted()
		{
			if (Context.IsUpdaterSuccessfullyStarted == true)
			{
				//启动成功，而且指定了自动解除当前进程时，则自动退出
				if (Context.AutoExitCurrentProcess)
					TerminateProcess(this);
			}
			else if (Context.IsUpdaterSuccessfullyStarted == false && Context.MustUpdate)
			{
				//启动失败，但是要求强行更新时，则退出
				TerminateProcess(this);
			}
		}

		/// <summary>
		/// 强行中止当前进程
		/// </summary>
		internal static void TerminateProcess(object sender, int exitCode = 0)
		{
			var e = new CancelableEventArgs();
			OnRequireTerminateProcess(sender, e);
			if (e.IsCancelled)
				return;

			Environment.Exit(exitCode);
		}

		KeyValuePair<string, byte[]> ReadEmbedStream(string name, string fileName = null)
		{
			fileName = fileName ?? name;
			using (var stream = typeof(Updater).Assembly.GetManifestResourceStream($"FSLib.App.SimpleUpdater.Utilities.{name}.gz"))
			{
				using (var ms = new MemoryStream())
				{
					var buf = new byte[0x400];
					var count = 0;
					while ((count = stream.Read(buf, 0, buf.Length)) > 0)
						ms.Write(buf, 0, count);
					ms.Flush();

					return new KeyValuePair<string, byte[]>(fileName, ms.ToArray());
				}
			}
		}

		void CopyUtilityExecutable(string targetPath = null)
		{
			var targetFiles = new List<KeyValuePair<string, byte[]>>();
			if (targetPath == null)
			{
				targetPath = Context.UpdateTempRoot;
			}

#if NET20
			targetFiles.Add(ReadEmbedStream("Utilities_Net20.exe", "FSLib.App.Utilities.exe"));
			targetFiles.Add(ReadEmbedStream("app.config", "FSLib.App.Utilities.exe.config"));
#elif NET40 || NET45
			targetFiles.Add(ReadEmbedStream("Utilities_Net40.exe", "FSLib.App.Utilities.exe"));
			targetFiles.Add(ReadEmbedStream("app.config", "FSLib.App.Utilities.exe.config"));
#elif NET5_0
			targetFiles.Add(ReadEmbedStream("FSLib.App.Utilities.dll"));
			targetFiles.Add(ReadEmbedStream("FSLib.App.Utilities.exe"));
			targetFiles.Add(ReadEmbedStream("FSLib.App.Utilities.runtimeconfig.json"));
#endif

			foreach (var kvp in targetFiles)
			{
				try
				{
					var target = Path.Combine(targetPath, kvp.Key);
					if (!File.Exists(target))
						File.WriteAllBytes(target, ExtensionMethod.Decompress(kvp.Value));
				}
				catch (Exception e)
				{
					_logger.LogError($"Unable write target file --> {kvp.Key} error --> {e.Message}", e);
				}
			}
		}


		/// <summary>
		/// 复制更新程序到临时目录并启动
		/// </summary>
		bool CopyAndStartUpdater(string[] ownerProcessList)
		{
			//写入更新文件
			var updateinfoFile = Context.UpdateInfoFilePath;
			Directory.CreateDirectory(Path.GetDirectoryName(updateinfoFile));
			File.WriteAllText(updateinfoFile, Context.UpdateInfoTextContent, Encoding.UTF8);
			//写入包列表
			XMLSerializeHelper.XmlSerilizeToFile(PackagesToUpdate, Context.UpdatePackageListPath);
			XMLSerializeHelper.XmlSerilizeToFile(ExtensionMethod.ToList(FileInstaller.PreservedFiles.Keys), Context.PreserveFileListPath);

			//启动外部程序
			var currentAssembly = Assembly.GetExecutingAssembly();
			if(!Context.NeedStandaloneUpdateClientSupport && CopyAssemblyToUpdateRoot(currentAssembly) == null)
			{
				throw new Exception("Unable create updater utilities.");
			}

			var tempExePath = Path.Combine(Context.UpdateTempRoot, "FSLib.App.Utilities.exe");

#if !STANDALONE

			CopyUtilityExecutable();

#endif

			//启动
			var sb = new StringBuilder(0x400);
			sb.AppendFormat("/startupdate /cv \"{0}\" ", Context.CurrentVersion.ToString());
			sb.AppendFormat("/log \"{0}\" ", Utility.SafeQuotePathInCommandLine(Context.LogFile ?? ""));
			sb.AppendFormat("/ad \"{0}\" ", Utility.SafeQuotePathInCommandLine(Context.ApplicationDirectory));
			sb.AppendFormat("/url \"{0}\" ", Context.UpdateDownloadUrl);
			sb.AppendFormat("/infofile \"{0}\" ", Context.UpdateInfoFileName);
			sb.AppendFormat("/proxy \"{0}\" ", Context.ProxyAddress ?? "");
			if (Context.NetworkCredential != null)
				sb.AppendFormat("/cred \"{0}\" ", string.Format("{0}:{1}", Context.NetworkCredential.UserName, Context.NetworkCredential.Password));
			if (Context.AutoKillProcesses) sb.Append("/autokill ");
			if (Context.ForceUpdate) sb.Append("/forceupdate ");
			if (Context.HiddenUI) sb.Append("/noui ");
			if (_mainFormType != null)
			{
				CopyAssemblyToUpdateRoot(_mainFormType.Assembly);
				sb.Append("/ui \"" + _mainFormType.AssemblyQualifiedName + "\" ");

				if (!(_usingAssemblies ?? (_usingAssemblies = new List<Assembly>())).Contains(_mainFormType.Assembly))
					_usingAssemblies.Add(_mainFormType.Assembly);
			}
			if (_usingAssemblies != null && _usingAssemblies.Count > 0)
			{
				var assemblyNames = new List<string>();
				foreach (var assembly in _usingAssemblies)
				{
					if (CopyAssemblyToUpdateRoot(assembly) == true)
						assemblyNames.Add(Path.GetFileName(assembly.Location));
				}
				if (assemblyNames.Count > 0)
				{
					sb.Append("/assembly \"" + string.Join(";", assemblyNames.ToArray()) + "\" ");
				}
			}

			FetchProcessList(ownerProcessList).ForEach(s => sb.AppendFormat("/p \"{0}\" ", s));

			var psi = new ProcessStartInfo(tempExePath, sb.ToString())
			{
				UseShellExecute = true
			};
			//检测是否要管理员权限
			if (Environment.OSVersion.Version.Major > 5 && (Context.UpdateInfo.RequreAdminstrorPrivilege || !EnsureAdminPrivilege()))
				psi.Verb = "runas";

			OnExternalUpdateStart();

			_logger.LogInformation("Start external update process, path=" + psi.FileName + ", arguments=" + psi.Arguments);
			try
			{
				Process.Start(psi);
				OnExternalUpdateStarted();
			}
			catch (Exception ex)
			{
				Context.Exception = ex;
				OnUpdateCancelled();

				return false;
			}

			return true;
		}

		Dictionary<Assembly, string> _assemblies = new Dictionary<Assembly, string>();

		/// <summary>
		/// 复制指定程序集到目录
		/// </summary>
		/// <param name="assembly"></param>
		bool? CopyAssemblyToUpdateRoot(Assembly assembly)
		{
			if (_assemblies.ContainsKey(assembly))
				return true;

			var location = assembly.Location;
			if (string.IsNullOrEmpty(location))
			{
				_logger.LogError($"Unable locate assembly '{assembly.FullName}'");
				return null;
			}

			if (!location.StartsWith(Context.ApplicationDirectory, StringComparison.OrdinalIgnoreCase)) return false;

			var dest = Path.Combine(Context.UpdateTempRoot, Path.GetFileName(location));
			_logger.LogInformation(string.Format("Copying assembly {0} to {1}", location, dest));
			File.Copy(location, dest, true);
			_assemblies.Add(assembly, null);

			//如果有pdb文件, 那么也复制 
			location = Path.ChangeExtension(location, "pdb");
			if (File.Exists(location))
			{
				_logger.LogInformation(string.Format("Copying pdb file {0} to {1}", location, dest));
				dest = Path.ChangeExtension(dest, "pdb");
				File.Copy(location, dest, true);
			}

			foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
			{
				_logger.LogInformation("Checking assembly reference..." + referencedAssembly.FullName);
				try
				{
					if (CopyAssemblyToUpdateRoot(Assembly.Load(referencedAssembly)) == null)
						return null;
				}
				catch (Exception ex)
				{

					_logger.LogError("Error checking assembly reference : " + ex.Message, ex);
					return null;
				}
			}

			return true;
		}

		/// <summary>
		/// 确认当前用户对当前目录是否具有操作权限
		/// </summary>
		/// <returns></returns>
		bool EnsureAdminPrivilege()
		{
			var root = Context.ApplicationDirectory;
			var tempfile = Path.Combine(root, DateTime.Now.Ticks + ".tmp");
			_logger.LogInformation("Checking if current process can write to application directory directly without admin privilege.");
			try
			{
				File.Create(tempfile).Close();
				File.Delete(tempfile);
				_logger.LogInformation("Permission check complete. no admin privilege required.");

				return true;
			}
			catch (Exception)
			{
				_logger.LogInformation("Permission denied. admin privilege required to perform operation. using /runas to perform update.");
				return false;
			}
		}

		/// <summary>
		/// 获得自动更新所需要结束的进程列表
		/// </summary>
		/// <param name="ownerProcess"></param>
		/// <returns></returns>
		List<string> FetchProcessList(string[] ownerProcess)
		{
			var list = new List<string>();
			var mainProcessID = Process.GetCurrentProcess().Id;
			list.Add("*" + mainProcessID);
			list.AddRange(ExtensionMethod.Select(Context.ExternalProcessID, s => "*" + s));
			if (Context.AutoEndProcessesWithinAppDir)
			{
				Func<Process, string> pathLookup = _ =>
				{
					try
					{
						return _.MainModule.FileName;
					}
					catch (Exception)
					{
						return string.Empty;
					}
				};
				//获得所有进程
				var processes = Process.GetProcesses();
				//查找当前目录下的进程, 并加入到列表
				foreach (var s in processes)
				{
					if (!Context.ExternalProcessID.Contains(s.Id) && s.Id != mainProcessID && pathLookup(s).StartsWith(Context.ApplicationDirectory, StringComparison.OrdinalIgnoreCase))
						list.Add("*" + s.Id);
				}
			}

			if (ownerProcess != null) list.AddRange(ownerProcess);

			return list;
		}

		/// <summary>
		/// 启动进程外更新程序
		/// </summary>
		/// <param name="ownerProcess">主进程列表</param>
		public bool StartExternalUpdater(string[] ownerProcess)
		{
			var result = CopyAndStartUpdater(ownerProcess);
			Context.IsUpdaterSuccessfullyStarted = result;

			return result;

		}

		/// <summary>
		/// 启动进程外更新程序
		/// </summary>
		public bool StartExternalUpdater()
		{
			return StartExternalUpdater(null);
		}
		#endregion

		#region 额外调用

		/// <summary>
		/// 加载额外调用
		/// </summary>
		/// <param name="namelist"></param>
		void LoadExtraAssemblies(string namelist)
		{
			var assFiles = namelist.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var assFile in assFiles)
			{
				_logger.LogInformation("begin tring loading file '" + assFile + "'....");

				try
				{
					var path = Path.Combine(Context.UpdateTempRoot, assFile);
					var assembly = Assembly.LoadFile(path);

					_logger.LogInformation("assembly loaded. checking for interface notify.");
					//检查接口调用
					var types = assembly.GetTypes();
					foreach (var type in types)
					{
						if (type.GetInterface(typeof(IUpdateNotify).FullName) != null)
						{
							_logger.LogInformation("IUpdateNotify detected. Create object instance and invoke Init() method.");

							var obj = Activator.CreateInstance(type) as IUpdateNotify;
							obj.Init(this);
						}
					}
				}
				catch (Exception ex)
				{
					_logger.LogError("file loading failed. error: " + ex.Message, ex);
				}
				_logger.LogInformation("ending loading file '" + assFile + "'....");
			}
		}

		List<Assembly> _usingAssemblies;
		Type _mainFormType;

		/// <summary>
		/// 引用指定的程序集
		/// </summary>
		/// <param name="assemblies"></param>
		public void UsingAssembly(params Assembly[] assemblies)
		{
			(_usingAssemblies ?? (_usingAssemblies = new List<Assembly>())).AddRange(assemblies);
		}

		/// <summary>
		/// 使用指定的界面作为界面
		/// </summary>
		/// <typeparam name="T">界面UI的类型</typeparam>
		public void UsingFormUI<T>() where T : AbstractUpdateBase
		{
			_mainFormType = typeof(T);
		}

		#endregion

		#region 临时目录清理

		bool _hasCleanProcessStarted;

		/// <summary>
		/// 清理临时目录
		/// </summary>
		void CleanTemp()
		{
			if (_hasCleanProcessStarted) return;

			if (!Directory.Exists(Context.UpdateTempRoot))
			{
				_logger.LogInformation("No need to clean due to temporary directory was not created.");
				return;
			}
			_logger.LogInformation("Starting external cleanup process.");

			var utilityPath = Path.GetTempPath();
			var localPath = Path.Combine(utilityPath, "FSLib.App.Utilities.exe");
			var arg = "deletetmp \"" + Process.GetCurrentProcess().Id + "\" \"" + Utility.SafeQuotePathInCommandLine(Context.UpdateTempRoot) + "\"";

			CopyUtilityExecutable(utilityPath);

			Process.Start(localPath, arg);
			_hasCleanProcessStarted = true;
		}



		#endregion
	}
}
