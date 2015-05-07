using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Web.UI;
using System.Windows.Forms;
using System.Diagnostics;
using FSLib.App.SimpleUpdater.Annotations;
using FSLib.App.SimpleUpdater.Dialogs;
using FSLib.App.SimpleUpdater.Wrapper;


namespace FSLib.App.SimpleUpdater
{
	using Defination;

	/// <summary>
	/// 自动更新操作类
	/// </summary>
	public class Updater : IDisposable
	{
		/// <summary> 获得当前更新的上下文 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public UpdateContext Context { get; private set; }

		#region 确认没有更新才启动

		/// <summary>
		/// 确认是否有更新，再继续后面的操作。
		/// </summary>
		/// <typeparam name="T">要显示的UI界面</typeparam>
		/// <param name="updateFoundAction">发现更新的委托。如果此委托为null或返回null，则显示内置的更新对话框。如果此委托返回true，则启动更新后自动退出；如果此委托返回false，则忽略更新并按照正常的操作流程继续。</param>
		/// <param name="errorHandler">检查更新发生错误的委托</param>
		public void EnsureNoUpdate(Func<bool?> updateFoundAction = null, Action<Exception> errorHandler = null)
		{
			EnsureNoUpdate<Form>(null, null, null);
		}

		/// <summary>
		/// 确认是否有更新，再继续后面的操作。
		/// </summary>
		/// <typeparam name="T">要显示的UI界面</typeparam>
		/// <param name="updateFoundAction">发现更新的委托。如果此委托为null或返回null，则显示内置的更新对话框。如果此委托返回true，则启动更新后自动退出；如果此委托返回false，则忽略更新并按照正常的操作流程继续。</param>
		/// <param name="errorHandler">检查更新发生错误的委托</param>
		/// <param name="updateUi">用于显示状态的UI界面</param>
		public void EnsureNoUpdate<T>(Func<bool?> updateFoundAction = null, Action<Exception> errorHandler = null, T updateUi = null) where T : Form
		{
			Application.EnableVisualStyles();
			var ui = (Form)updateUi ?? new EnsureUpdate();

			var continueProcess = false;
			Context.EnableEmbedDialog = false;

			using (ui)
			{
				ui.Shown += (s, e) => BeginCheckUpdateInProcess();
				EventHandler updateFound = null, versionErrorHandler = null, ueEventHandler = null, noupdateFoundHandler = null;

				var unscribeAllEvents = new Action<Updater, bool, bool>((s, cont, close) =>
				{
					Delegate.RemoveAll(s.UpdatesFound, updateFound);
					Delegate.RemoveAll(s.MinmumVersionRequired, versionErrorHandler);
					Delegate.RemoveAll(s.Error, ueEventHandler);
					Delegate.RemoveAll(s.NoUpdatesFound, noupdateFoundHandler);

					continueProcess = cont;
					if (close)
						ui.Close();
				});
				noupdateFoundHandler = (s, e) => unscribeAllEvents(s as Updater, true, true);
				updateFound = (s, e) =>
				{
					var client = s as Updater;
					var context = client.Context;

					if (context.MustUpdate || context.ForceUpdate)
					{
						Instance_UpdatesFound(s, e);
						unscribeAllEvents(client, false, true);
					}
					else
					{
						if (updateFoundAction != null)
						{
							var result = updateFoundAction();
							if (result == true)
							{
								client.StartExternalUpdater();
								unscribeAllEvents(s as Updater, false, true);

								return;
							}
							if (result == false)
							{
								unscribeAllEvents(s as Updater, true, true);

								return;
							}
						}
						context.EnableEmbedDialog = true;
						Instance_UpdatesFound(s, e);
						unscribeAllEvents(s as Updater, true, true);
					}
				};
				versionErrorHandler = (s, e) =>
				{
					if (errorHandler != null)
					{
						errorHandler(new Exception(string.Format(SR.MinmumVersionRequired_Desc, (s as Updater).Context.UpdateInfo.RequiredMinVersion)));
					}
					else
					{
						MessageBox.Show(string.Format(SR.MinmumVersionRequired_Desc, (s as Updater).Context.UpdateInfo.RequiredMinVersion, (s as Updater).Context.CurrentVersion), SR.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					unscribeAllEvents(s as Updater, false, true);
				};
				ueEventHandler = (s, e) =>
				{
					var err = (s as Updater).Context.Exception;
					if (errorHandler != null)
					{
						errorHandler(err);
					}
					else
					{
						MessageBox.Show(String.Format(SR.Updater_UnableToCheckUpdate, err.Message), SR.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					unscribeAllEvents(s as Updater, Context.TreatErrorAsNotUpdated, true);
				};
				UpdatesFound += updateFound;
				Error += ueEventHandler;
				MinmumVersionRequired += versionErrorHandler;
				NoUpdatesFound += noupdateFoundHandler;

				ui.ShowDialog();
			}

			if (!continueProcess)
			{
				Environment.Exit(0);
			}
		}


		#endregion

		#region 静态对象

		static Updater()
		{
			var ass = System.Reflection.Assembly.GetExecutingAssembly();
			UpdaterClientVersion = ExtensionMethod.ConvertVersionInfo(System.Diagnostics.FileVersionInfo.GetVersionInfo(ass.Location)).ToString();
		}

		static Updater _instance;

		/// <summary>
		/// 确认没有重复调用
		/// </summary>
		static void CheckInitialized()
		{
			if (_instance == null)
				return;

			throw new InvalidOperationException("Updater 已经被初始化。此方法调用之前，不可先调用任何可能会导致Updater被初始化的操作。");
		}

		/// <summary>
		/// 获得当前的更新客户端版本
		/// </summary>
		public static string UpdaterClientVersion { get; private set; }

		/// <summary>
		/// 当前的更新实例。直接访问本属性将会获得默认创建的Updater实例。要使用更多信息创建，请使用 <see cref="CreateUpdaterInstance"/> 方法，请确保在使用本属性之前创建。
		/// </summary>
		public static Updater Instance
		{
			get { return _instance ?? (_instance = CreateUpdaterInstance((Version)null, null)); }
		}

		/// <summary>
		/// 创建自动更新客户端
		/// </summary>
		/// <returns></returns>
		public static Updater CreateUpdaterInstance(string templateUrl, string xmlFileName)
		{
			return CreateUpdaterInstance(null, null, new UpdateServerInfo[] { new UpdateServerInfo(templateUrl, xmlFileName) });
		}


		/// <summary>
		/// 创建自动更新客户端
		/// </summary>
		/// <param name="servers"></param>
		/// <returns></returns>
		public static Updater CreateUpdaterInstance(params UpdateServerInfo[] servers)
		{
			return CreateUpdaterInstance(null, null, servers);
		}

		/// <summary>
		/// 创建自动更新客户端
		/// </summary>
		/// <param name="appVersion">应用程序版本，留空将会使用自动判断</param>
		/// <param name="appDirectory">应用程序目录，留空将会使用自动判断</param>
		/// <param name="servers">升级服务器地址</param>
		/// <returns></returns>
		public static Updater CreateUpdaterInstance(Version appVersion, string appDirectory, params UpdateServerInfo[] servers)
		{
			CheckInitialized();

			if (servers == null || servers.Length < 2)
			{
				if (appVersion == null && string.IsNullOrEmpty(appDirectory))
					_instance = new Updater();
				else _instance = new Updater(appVersion, appDirectory);

				if (servers.Length > 0)
				{
					_instance.Context.UpdateDownloadUrl = servers[0].Url;
					_instance.Context.UpdateInfoFileName = servers[0].InfoFileName;
				}
			}
			else
			{
				if (appVersion == null && string.IsNullOrEmpty(appDirectory))
					_instance = new MultiServerUpdater(servers);
				else _instance = new MultiServerUpdater(appVersion, appDirectory, servers);
			}

			return _instance;
		}

		/// <summary>
		/// 提供一个最简单的自动更新入口
		/// </summary>
		/// <returns>返回是否开始检查操作</returns>
		public static bool CheckUpdateSimple()
		{
			return CheckUpdateSimple(string.Empty);
		}

		/// <summary>
		/// 提供一个最简单的自动更新入口
		/// </summary>
		/// <param name="updateUrl">更新URL. 如果不传递或传递空的地址, 请使用 <see cref="T:FSLib.App.SimpleUpdater.UpdateableAttribute"/> 属性来标记更新地址</param>
		/// <returns>返回是否开始检查操作</returns>
		[Obsolete("这是一个不被推荐的检测更新方式")]
		public static bool CheckUpdateSimple(string updateUrl)
		{
			if (_instance == null)
				_instance = CreateUpdaterInstance(null, null, new UpdateServerInfo[] { new UpdateServerInfo(updateUrl, null) });
			else if (!string.IsNullOrEmpty(updateUrl))
			{
				_instance.Context.UpdateDownloadUrl = updateUrl;
				_instance.Context.UpdateInfoFileName = null;
			}
			_instance.Context.EnableEmbedDialog = true;

			return Instance.BeginCheckUpdateInProcess();
		}

		/// <summary>
		/// 提供一个最简单的自动更新入口
		/// </summary>
		/// <param name="templateUrl">更新模板URL. 如果不传递或传递空的地址, 请使用 <see cref="T:FSLib.App.SimpleUpdater.UpdateableAttribute"/> 属性来标记更新地址</param>
		/// <param name="xmlFileName">更新XML信息文件名</param>
		/// <returns>返回是否开始检查操作</returns>
		public static bool CheckUpdateSimple(string templateUrl, string xmlFileName)
		{
			if (_instance == null)
				_instance = CreateUpdaterInstance(null, null, new UpdateServerInfo[] { new UpdateServerInfo(templateUrl, xmlFileName) });
			else if (!string.IsNullOrEmpty(templateUrl))
			{
				_instance.Context.UpdateDownloadUrl = templateUrl;
				_instance.Context.UpdateInfoFileName = xmlFileName;
			}
			_instance.Context.EnableEmbedDialog = true;

			return Instance.BeginCheckUpdateInProcess();
		}

		//要求最低版本
		static void Instance_MinmumVersionRequired(object sender, EventArgs e)
		{
			var updater = sender as Updater;
			if (!updater.Context.EnableEmbedDialog) return;

			new Dialogs.MinmumVersionRequired().ShowDialog();
		}

		//找到更新，启动更新
		static void Instance_UpdatesFound(object sender, EventArgs e)
		{
			var updater = sender as Updater;

			if (updater.Context.MustUpdate)
			{
				if (updater.Context.PromptUserBeforeAutomaticUpgrade)
					MessageBox.Show(string.Format(SR.Updater_AutomaticUpgradeTipForce,
												updater.Context.UpdateInfo.AppName,
												updater.Context.CurrentVersion,
												updater.Context.UpdateInfo.AppVersion), SR.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
				updater.StartExternalUpdater();
			}
			else if (updater.Context.ForceUpdate)
			{
				if (updater.Context.PromptUserBeforeAutomaticUpgrade)
					MessageBox.Show(string.Format(SR.Updater_AutomaticUpgradeTipNotForce,
												updater.Context.UpdateInfo.AppName,
												updater.Context.CurrentVersion,
												updater.Context.UpdateInfo.AppVersion), SR.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
				updater.StartExternalUpdater();
			}
			else
			{
				if (!updater.Context.EnableEmbedDialog) return;

				if (Application.MessageLoop)
				{
					new UpdateFound().Show();
				}
				else
				{
					//启动单独的线程，并设置STA标记
					//不设置STA标记的时候，当更新信息是网页的话会报错
					var ts = new Thread(() =>
					  {
						  new UpdateFound().ShowDialog();
					  });
					ts.IsBackground = false;
					ts.SetApartmentState(ApartmentState.STA);
					ts.Start();
				}
			}
			updater.EnsureUpdateStarted();
		}


		#endregion

		#region 构造函数

		/// <summary>
		/// 手动创建更新类，并指定当前版本和应用程序目录
		/// </summary>
		/// <param name="appVersion">指定的应用程序版本</param>
		/// <param name="appDirectory">指定的应用程序路径</param>
		protected Updater(Version appVersion, string appDirectory)
			: this()
		{
			Context.CurrentVersion = appVersion;
			Context.ApplicationDirectory = appDirectory;
		}

		/// <summary>
		/// 使用指定的信息类来提供应用程序需要的信息
		/// </summary>
		protected Updater()
		{
			Trace.AutoFlush = true;
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

		#region 事件区域


		/// <summary>
		/// 正在关闭主程序
		/// </summary>
		public event EventHandler<QueryCloseApplicationEventArgs> QueryCloseApplication;

		/// <summary>
		/// 引发 <see cref="QueryCloseApplication"/> 事件
		/// </summary>
		protected virtual void OnQueryCloseApplication(QueryCloseApplicationEventArgs e)
		{
			if (QueryCloseApplication == null)
				return;

			QueryCloseApplication(this, e);
		}

		/// <summary>
		/// 正在安装更新
		/// </summary>
		public event EventHandler InstallUpdates;

		/// <summary>
		/// 引发 <see cref="InstallUpdates"/> 事件
		/// </summary>
		protected virtual void OnInstallUpdates()
		{
			if (InstallUpdates == null)
				return;

			InstallUpdates(this, EventArgs.Empty);
		}

		/// <summary>
		/// 已经完成更新
		/// </summary>
		public event EventHandler UpdateFinished;

		/// <summary>
		/// 引发 <see cref="UpdateFinsihed"/> 事件
		/// </summary>
		protected virtual void OnUpdateFinsihed()
		{
			if (UpdateFinished == null)
				return;

			UpdateFinished(this, EventArgs.Empty);
		}


		/// <summary>
		/// 更新中发生错误
		/// </summary>
		public event EventHandler Error;

		/// <summary>
		/// 引发 <see cref="Error" /> 事件
		/// </summary>
		protected virtual void OnError()
		{
			CleanTemp();
			var handler = Error;
			if (handler != null)
				handler(this, EventArgs.Empty);

			if (!Context.IsInUpdateMode && Context.MustUpdate && Context.TreatErrorAsNotUpdated)
				TerminateProcess(this);
		}


		/// <summary>
		/// 不满足最低版本要求
		/// </summary>
		public event EventHandler MinmumVersionRequired;

		/// <summary>
		/// 引发 <see cref="MinmumVersionRequired" /> 事件
		/// </summary>
		protected virtual void OnMinmumVersionRequired()
		{
			if (MinmumVersionRequired == null)
				return;

			MinmumVersionRequired(this, EventArgs.Empty);

			if (Context.MustUpdate && Context.TreatErrorAsNotUpdated)
				TerminateProcess(this);
		}



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
								Context.NetworkCredential = new System.Net.NetworkCredential(info[0], info[1]);
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
		/// 开始下载更新信息文件
		/// </summary>
		public event EventHandler DownloadUpdateInfo;

		/// <summary>
		/// 引发 <see cref="DownloadUpdateInfo"/> 事件
		/// </summary>
		protected virtual void OnDownloadUpdateInfo()
		{
			if (DownloadUpdateInfo == null)
				return;

			DownloadUpdateInfo(this, EventArgs.Empty);
		}

		/// <summary>
		/// 结束下载更新信息文件
		/// </summary>
		public event EventHandler DownloadUpdateInfoFinished;

		/// <summary>
		/// 引发 <see cref="DownloadUpdateInfoFinished"/> 事件
		/// </summary>
		public virtual void OnDownloadUpdateInfoFinished()
		{
			if (DownloadUpdateInfoFinished == null)
				return;

			DownloadUpdateInfoFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 没有发现更新
		/// </summary>
		public event EventHandler NoUpdatesFound;

		/// <summary>
		/// 引发 <see cref="NoUpdatesFound"/> 事件
		/// </summary>
		protected virtual void OnNoUpdatesFound()
		{
			if (NoUpdatesFound == null)
				return;

			NoUpdatesFound(this, EventArgs.Empty);
		}

		/// <summary>
		/// 发现了更新
		/// </summary>
		public event EventHandler UpdatesFound;

		/// <summary>
		/// 引发 <see cref="UpdatesFound"/> 事件
		/// </summary>
		protected virtual void OnUpdatesFound()
		{
			if (UpdatesFound == null)
				return;

			UpdatesFound(this, EventArgs.Empty);
			EnsureUpdateStarted();
		}

		/// <summary>
		/// 更新操作已经被用户取消
		/// </summary>
		public event EventHandler UpdateCancelled;

		/// <summary>
		/// 引发 <see cref="UpdateCancelled" /> 事件
		/// </summary>
		internal virtual void OnUpdateCancelled()
		{
			CleanTemp();
			var handler = UpdateCancelled;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}


		/// <summary>
		/// 开始检测更新
		/// </summary>
		/// <returns>返回是否成功开始检查</returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public bool BeginCheckUpdateInProcess()
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
				Trace.TraceWarning("检测更新信息失败：" + e.Exception.Message, e.Exception.ToString());
				OnError();
			};
			bgw.WorkCompleted += (s, e) =>
			{
				if (Context.UpdateInfo == null) return;

				Context.IsInUpdating = false;

				if (Context.CurrentVersionTooLow)
				{
					OnMinmumVersionRequired();
				}
				else if (!Context.HasUpdate)
				{
					OnNoUpdatesFound();
				}
				else OnUpdatesFound();

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

			//下载信息时不直接下载到文件中.这样不会导致始终创建文件夹
			Exception ex = null;
			byte[] data = null;
			var url = Context.RandomUrl(Context.UpdateInfoFileUrl);

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
				Trace.TraceInformation("正在从 " + url + " 下载升级信息");
				client.DownloadDataAsync(new Uri(url));
				//等待下载完成
				wHandler.WaitOne();
			}
			Trace.TraceInformation("服务器返回数据----->" + (data == null ? "<null>" : data.Length.ToString() + "字节"));
			if (data != null && data.Length > 0x10)
			{
				//不是<xml标记，则执行解压缩
				if (BitConverter.ToInt32(data, 0) != 0x6D783F3C && BitConverter.ToInt32(data, 0) != 0x3CBFBBEF)
				{
					Trace.TraceInformation("数据非正常数据, 正在执行解压缩");
					data = ExtensionMethod.Decompress(data);
				}
				Context.UpdateInfoTextContent = Encoding.UTF8.GetString(data);
			}

			if (ex != null) throw ex;
			e.PostEvent(OnDownloadUpdateInfoFinished);

			//是否返回了正确的结果?
			if (string.IsNullOrEmpty(Context.UpdateInfoTextContent))
			{
				throw new ApplicationException("服务器返回了不正确的更新结果");
			}
			if (Context.UpdateInfo == null)
			{
				if (string.IsNullOrEmpty(Context.UpdateInfoTextContent))
				{
					Trace.TraceInformation("正在读取本地升级信息文件");
					Context.UpdateInfoTextContent = System.IO.File.ReadAllText(Context.UpdateInfoFilePath, System.Text.Encoding.UTF8);
				}
				Context.UpdateInfo = XMLSerializeHelper.XmlDeserializeFromString<UpdateInfo>(Context.UpdateInfoTextContent);
			}
			if (Context.UpdateInfo == null)
			{
				throw new ApplicationException("未能成功加载升级信息");
			}
			//设置必须的属性
			if (Context.UpdateInfo.MustUpdate)
			{
				Context.AutoKillProcesses = true;
				Context.AutoEndProcessesWithinAppDir = true;
				Context.ForceUpdate = true;
			}

			//判断升级
			if (!string.IsNullOrEmpty(Context.UpdateInfo.RequiredMinVersion) && Context.CurrentVersion < new Version(Context.UpdateInfo.RequiredMinVersion))
			{
				Context.CurrentVersionTooLow = true;
			}
			else
			{
				Context.HasUpdate = new Version(Context.UpdateInfo.AppVersion) > Context.CurrentVersion;
			}

			if (Context.HasUpdate)
			{

				//判断要升级的包
				if (PackagesToUpdate == null || PackagesToUpdate.Count == 0)
				{
					var pkgList = Context.UpdatePackageListPath;
					Trace.TraceInformation("外部升级包列表：{0}", pkgList);

					if (System.IO.File.Exists(pkgList))
					{
						PackagesToUpdate = XMLSerializeHelper.XmlDeserializeFromString<List<PackageInfo>>(System.IO.File.ReadAllText(pkgList, Encoding.UTF8));
						PackagesToUpdate.ForEach(s => s.Context = Context);
					}
					else
					{
						GatheringDownloadPackages(e);
					}

					var preserveFileList = Context.PreserveFileListPath;
					Trace.TraceInformation("外部文件保留列表：{0}", preserveFileList);
					if (System.IO.File.Exists(preserveFileList))
					{
						var list = XMLSerializeHelper.XmlDeserializeFromString<List<string>>(System.IO.File.ReadAllText(preserveFileList, Encoding.UTF8));
						list.ForEach(s => FileInstaller.PreservedFiles.Add(s, null));
					}
				}
			}

			//如果没有要升级的包？虽然很奇怪，但依然当作不需要升级
			Context.HasUpdate &= PackagesToUpdate.Count > 0;
		}
		#region 确定要下载的包

		/// <summary> 确定需要下载的包 </summary>
		/// <remarks></remarks>
		public event EventHandler GatheringPackages;

		/// <summary> 引发 <see cref="GatheringPackages"/> 事件 </summary>
		protected virtual void OnGatheringPackages()
		{
			var handler = GatheringPackages;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		/// <summary> 已确定要下载的包 </summary>
		/// <remarks></remarks>
		public event EventHandler GatheredPackages;

		/// <summary> 引发 <see cref="GatheredPackages"/> 事件 </summary>
		protected virtual void OnGatheredPackages()
		{
			var handler = GatheredPackages;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		/// <summary> 生成下载列表 </summary>
		void GatheringDownloadPackages(RunworkEventArgs rt)
		{
			if (PackagesToUpdate.Count > 0) return;

			Trace.TraceInformation("正在确定需要下载的升级包");
			rt.PostEvent(OnGatheringPackages);

			if (!string.IsNullOrEmpty(Context.UpdateInfo.Package) && (Context.UpdateInfo.Packages == null || Context.UpdateInfo.Packages.Count == 0))
			{
				//必须更新的包
				Trace.TraceInformation("正在添加必须升级的主要安装包");
				PackagesToUpdate.Add(new PackageInfo()
				{
					FilePath = "",
					FileSize = 0,
					PackageHash = Context.UpdateInfo.MD5,
					Method = UpdateMethod.Always,
					PackageName = Context.UpdateInfo.Package,
					PackageSize = Context.UpdateInfo.PackageSize,
					VerificationLevel = FileVerificationLevel.Hash,
					Version = "0.0.0.0",
					Context = Context
				});
			}
			if (Context.UpdateInfo.Packages != null)
			{
				//判断增量升级包
				var index = 0;
				foreach (var pkg in Context.UpdateInfo.Packages)
				{
					rt.ReportProgress(++index, Context.UpdateInfo.Packages.Count);
					var localPath = System.IO.Path.Combine(Context.ApplicationDirectory, pkg.FilePath); //对比本地路径
					pkg.Context = Context;

					if (pkg.Method == UpdateMethod.Always)
					{
						Trace.TraceInformation("标记为始终更新，添加升级包 【" + pkg.PackageName + "】");
						PackagesToUpdate.Add(pkg);
						continue;
					}
					//存在即跳过，或版本比较
					if (!System.IO.File.Exists(localPath))
					{
						PackagesToUpdate.Add(pkg);
						Trace.TraceInformation("本地路径【" + pkg.FilePath + "】不存在，添加升级包 【" + pkg.PackageName + "】");
						continue;
					}
					//如果存在即跳过……那么你好去跳过了。
					if (pkg.Method == UpdateMethod.SkipIfExists)
					{
						AddPackageToPreserveList(pkg);
						Trace.TraceInformation("本地路径【" + pkg.FilePath + "】已经存在，跳过升级包 【" + pkg.PackageName + "】");
						continue;
					}

					var isNewer = false;
					if ((pkg.VerificationLevel & FileVerificationLevel.Version) == FileVerificationLevel.Version)
					{
						isNewer |= string.IsNullOrEmpty(pkg.Version) || ExtensionMethod.CompareVersion(localPath, pkg.Version);
					}
					if ((pkg.VerificationLevel & FileVerificationLevel.Hash) == FileVerificationLevel.Hash)
					{
						isNewer |= ExtensionMethod.GetFileHash(localPath) != pkg.FileHash;
					}
					if ((pkg.VerificationLevel & FileVerificationLevel.Size) == FileVerificationLevel.Size)
					{
						isNewer |= new System.IO.FileInfo(localPath).Length != pkg.FileSize;
					}

					if (isNewer)
					{
						Trace.TraceInformation("服务器版本更新，添加升级包 【" + pkg.PackageName + "】");
						pkg.Context = Context;
						PackagesToUpdate.Add(pkg);
					}
					else
					{
						AddPackageToPreserveList(pkg);
						Trace.TraceInformation("服务器版本更旧或相同，跳过升级包 【" + pkg.PackageName + "】");
					}
				}
			}

			rt.PostEvent(OnGatheredPackages);
			Trace.TraceInformation("完成确定需要下载的升级包");
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
					Trace.TraceInformation("添加 {0} 到保持文件列表，因为下载过程中会跳过，所以不可以删除", file);
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
			Trace.TraceInformation("开始关闭进程");

			e.ReportProgress(0, 0, "正在关闭进程....");
			var closeApplication = new List<Process>();

			foreach (var pid in Context.ExternalProcessID)
			{
				try
				{
					closeApplication.Add(Process.GetProcessById(pid));
					Trace.TraceInformation("添加进程PID=" + pid + "到等待关闭列表");
				}
				catch (Exception ex)
				{
					Trace.TraceInformation("添加进程PID=" + pid + "到等待关闭列表时出错：" + ex.Message);
				}
			}
			foreach (var pn in Context.ExternalProcessName)
			{
				closeApplication.AddRange(Process.GetProcessesByName(pn));
				Trace.TraceInformation("添加进程名=" + pn + "到等待关闭列表");
			}

			if (closeApplication.Count > 0)
			{
				//是否强制关闭进程？
				if (Context.AutoKillProcesses)
				{
					Trace.TraceInformation("已开启自动结束所有进程。正在结束进程。");
					foreach (var s in closeApplication)
					{
						if (s.HasExited)
						{
							Trace.TraceInformation("进程【" + s.ProcessName + "】已经提前退出。");
						}
						else
						{
							try
							{
								s.Kill();
								Trace.TraceInformation("进程【" + s.ProcessName + "】已经成功结束。");
							}
							catch (Exception ex)
							{
								Trace.TraceError("进程【" + s.ProcessName + "】结束失败：" + ex.Message);

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

		/// <summary> 启动外部进程 </summary>
		/// <remarks></remarks>
		public event EventHandler<RunExternalProcessEventArgs> RunExternalProcess;

		/// <summary>
		/// 引发 <see cref="RunExternalProcess" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnRunExternalProcess(RunExternalProcessEventArgs ea)
		{
			var handler = RunExternalProcess;
			if (handler != null)
				handler(this, ea);
		}


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
			var extension = System.IO.Path.GetExtension(fileName).ToLower();

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
			Trace.TraceInformation("正在执行外部进程，路径：{0}，参数：{1}", psi.FileName, psi.Arguments);
			e.PostEvent(RunExternalProcess, this, new RunExternalProcessEventArgs(psi));
			var p = Process.Start(psi);

			if (waitingForExit)
			{
				Trace.TraceInformation("等待外部进程执行完毕");
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
			Trace.TraceInformation("外部进程执行完毕");

			return true;
		}
		/// <summary>
		/// 执行外部进程-安装后
		/// </summary>
		/// <returns></returns>
		bool RunExternalProgramAfter(RunworkEventArgs e)
		{
			if (string.IsNullOrEmpty(Context.UpdateInfo.FileExecuteAfter)) return true;
			return RunExternalProgram(e, System.IO.Path.Combine(Context.ApplicationDirectory, Context.UpdateInfo.FileExecuteAfter), Context.UpdateInfo.ExecuteArgumentAfter, false, Context.UpdateInfo.HideAfterExecuteWindow);
		}

		/// <summary>
		/// 执行外部进程-安装前
		/// </summary>
		/// <returns></returns>
		bool RunExternalProgramBefore(RunworkEventArgs e)
		{
			if (string.IsNullOrEmpty(Context.UpdateInfo.FileExecuteBefore)) return true;
			return RunExternalProgram(e, System.IO.Path.Combine(Context.UpdateNewFilePath, Context.UpdateInfo.FileExecuteBefore), Context.UpdateInfo.ExecuteArgumentBefore, true, Context.UpdateInfo.HideBeforeExecuteWindow);
		}
		#endregion

		#region 版本校验

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
				return Wrapper.ExtensionMethod.Count(PackagesToUpdate, s => s.IsDownloaded);
			}
		}

		/// <summary> 获得正在下载的任务个数 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public int DownloadingPackageCount
		{
			get
			{
				return Wrapper.ExtensionMethod.Count(PackagesToUpdate, s => s.IsDownloading);
			}
		}

		#endregion

		#region 事件

		/// <summary> 下载进度发生变化事件 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageDownloadProgressChangedEventArgs> DownloadProgressChanged;

		/// <summary>
		/// 引发 <see cref="DownloadProgressChanged"/> 事件
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnDownloadProgressChanged(PackageDownloadProgressChangedEventArgs e)
		{
			var handler = DownloadProgressChanged;

			if (handler != null) handler(this, e);
		}

		/// <summary> 开始下载指定的包 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageDownload;

		/// <summary>
		/// 引发 <see cref="PackageDownload" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		public virtual void OnPackageDownload(PackageEventArgs ea)
		{
			var handler = PackageDownload;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary> 指定的包下载完成 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageDownloadFinished;

		/// <summary>
		/// 引发 <see cref="PackageDownloadFinished" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		public virtual void OnPackageDownloadFinished(PackageEventArgs ea)
		{
			var handler = PackageDownloadFinished;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary> 包下载失败 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageDownloadFailed;

		/// <summary>
		/// 引发 <see cref="PackageDownloadFailed" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		public virtual void OnPackageDownloadFailed(PackageEventArgs ea)
		{
			var handler = PackageDownloadFailed;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary> 下载的包Hash不对 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageHashMismatch;

		/// <summary>
		/// 引发 <see cref="PackageHashMismatch" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		public virtual void OnPackageHashMismatch(PackageEventArgs ea)
		{
			var handler = PackageHashMismatch;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary> 包重试下载 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageDownloadRetried;

		/// <summary>
		/// 引发 <see cref="PackageDownloadRetried" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		public virtual void OnPackageDownloadRetried(PackageEventArgs ea)
		{
			var handler = PackageDownloadRetried;
			if (handler != null)
				handler(this, ea);
		}

		#endregion

		/// <summary> 执行下载 </summary>
		bool DownloadPackages(Wrapper.RunworkEventArgs rt)
		{
			System.IO.Directory.CreateDirectory(Context.UpdatePackagePath);

			Trace.TraceInformation("开始下载网络更新包");

			var workerCount = Math.Max(1, Context.MultipleDownloadCount);
			var workers = new List<WebClient>(workerCount);
			var evt = new AutoResetEvent(false);
			var hasError = false;

			//download redirect
			if (!string.IsNullOrEmpty(Context.UpdateInfo.PackageUrlTemplate))
			{
				Trace.TraceInformation("已经重定向下载包地址到 {0}", Context.UpdateInfo.PackageUrlTemplate);
				Context.UpdateDownloadUrl = Context.UpdateInfo.PackageUrlTemplate;
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
			Trace.TraceInformation("正在初始化 {0} 个WebClient", workerCount);
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
						Trace.TraceWarning("包【" + pkg.PackageName + "】下载失败：" + e.Error.Message);
						rt.PostEvent(PackageDownloadFailed, this, new PackageEventArgs(pkg));
					}
					else if (pkg.IsLocalFileValid != true)
					{
						Trace.TraceWarning("包【" + pkg.PackageName + "】MD5校验失败", "错误");
						pkg.LastError = new Exception("不文件哈希值不正确或文件不存在");
						rt.PostEvent(PackageHashMismatch, this, new PackageEventArgs(pkg));
					}

					if (pkg.LastError != null)
					{
						//如果出错，且重试次数在限制范围内，则重试
						pkg.IncreaseFailureCounter();
						if (pkg.RetryCount <= Context.MaxiumRetryDownloadCount)
						{
							Trace.TraceWarning("包【" + pkg.PackageName + "】未能成功下载，正在进行第 " + pkg.RetryCount + " 次重试，最大重试次数为 " + Context.MaxiumRetryDownloadCount, "错误");
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
						Trace.TraceInformation("包【" + pkg.PackageName + "】下载操作完成：" + (pkg.IsDownloaded ? "下载成功" : "下载失败"));
						evt.Set();
					}
				};
				clnt.DownloadProgressChanged += (s, e) =>
					{
						var pkg = e.UserState as PackageInfo;
						pkg.DownloadedSize = e.BytesReceived;
						pkg.PackageSize = e.TotalBytesToReceive > 0 ? e.TotalBytesToReceive : pkg.PackageSize;
						rt.PostEvent(DownloadProgressChanged, this,
									 new PackageDownloadProgressChangedEventArgs(pkg, pkg.PackageSize,
																				 pkg.DownloadedSize, e.ProgressPercentage));
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
						Trace.TraceInformation("包【" + nextPkg.PackageName + "】开始下载");
						rt.PostEvent(PackageDownload, this, new PackageEventArgs(nextPkg));
						Context.ResetWebClient(client);
						client.DownloadFileAsync(new Uri(Context.RandomUrl(Context.GetUpdatePackageFullUrl(nextPkg.PackageName))), nextPkg.LocalSavePath, nextPkg);
					}
				}
				if (breakFlag) break;
				evt.WaitOne();
				Trace.TraceInformation("线程同步事件已收到");
			}
			//不管任何原因中止下载，到这里时都需要等待所有客户端中止
			while (true)
			{
				//出错了，那么对所有的客户端发出中止命令。这里不需要判断是否忙碌。
				if (hasError)
				{
					Trace.TraceWarning("出现错误，正在取消所有包的下载队列");
					workers.ForEach(s => s.CancelAsync());
				}
				lock (PackagesToUpdate)
				{
					Trace.TraceInformation("等待下载队列完成操作");
					if (workers.FindIndex(s => s.IsBusy) == -1) break;
				}
				evt.WaitOne();
			}
			Trace.TraceInformation("完成下载网络更新包");

			var errorPkgs = ExtensionMethod.ToList(ExtensionMethod.Where(PackagesToUpdate, s => s.LastError != null));
			if (errorPkgs.Count > 0) throw new PackageDownloadException(errorPkgs.ToArray());

			return !hasError;
		}

		#endregion

		#region 自定义操作

		#endregion

		/// <summary> 操作进度发生变更 </summary>
		/// <remarks></remarks>
		public event EventHandler<RunworkEventArgs> OperationProgressChanged;

		/// <summary>
		/// 引发 <see cref="OperationProgressChanged"/> 事件
		/// </summary>
		public virtual void OnOperationProgressChanged(RunworkEventArgs ea)
		{
			var op = OperationProgressChanged;
			if (op != null)
			{
				op(this, ea);
			}
		}


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
				Trace.TraceWarning("更新中断，发生错误：" + e.Exception.Message, e.Exception.ToString());
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
		public FileInstaller FileInstaller
		{
			get { return _installer ?? (_installer = new FileInstaller()); }
		}

		/// <summary>
		/// 正在执行安装前进程
		/// </summary>
		public event EventHandler ExecuteExternalProcessBefore;

		/// <summary>
		/// 引发 <see cref="ExecuteExternalProcessBefore" /> 事件
		/// </summary>
		protected virtual void OnExecuteExternalProcessBefore()
		{
			var handler = ExecuteExternalProcessBefore;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 正在执行安装后进程
		/// </summary>
		public event EventHandler ExecuteExternalProcessAfter;

		/// <summary>
		/// 引发 <see cref="ExecuteExternalProcessAfter" /> 事件
		/// </summary>
		protected virtual void OnExecuteExternalProcessAfter()
		{
			var handler = ExecuteExternalProcessAfter;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

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
			if (!CloseApplication(e)) throw new Exception(SR.Updater_UpdateCanceledByCloseApp);

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

		/// <summary> 开始解包 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageExtractionBegin;

		/// <summary>
		/// 引发 <see cref="PackageExtractionBegin" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnPackageExtractionBegin(PackageEventArgs ea)
		{
			var handler = PackageExtractionBegin;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary> 解包完成 </summary>
		/// <remarks></remarks>
		public event EventHandler<PackageEventArgs> PackageExtractionEnd;

		/// <summary>
		/// 引发 <see cref="PackageExtractionEnd" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnPackageExtractionEnd(PackageEventArgs ea)
		{
			var handler = PackageExtractionEnd;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary>
		/// 解开安装包
		/// </summary>
		void ExtractPackage(RunworkEventArgs rt)
		{
			Trace.TraceInformation("开始解压缩升级包");
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
				index ++;

				Trace.TraceInformation("正在解压缩 " + pkg.PackageName);
				rt.PostEvent(() => OnPackageExtractionBegin(new PackageEventArgs(pkg)));

				fz.ExtractZip(pkg.LocalSavePath, Context.UpdateNewFilePath, null);

				rt.PostEvent(() => OnPackageExtractionEnd(new PackageEventArgs(pkg)));
				Trace.TraceInformation("完成解压缩 " + pkg.PackageName);
			}

			rt.PostEvent(() => OnPackageExtractionEnd(new PackageEventArgs(null)));
			Trace.TraceInformation("完成解压缩升级包");
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

		/// <summary>
		/// 正在中止当前进程
		/// </summary>
		public static event EventHandler<CancelableEventArgs> RequireTerminateProcess;

		/// <summary>
		/// 引发 <see cref="RequireTerminateProcess" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		internal static void OnRequireTerminateProcess(object sender, CancelableEventArgs ea)
		{
			var handler = RequireTerminateProcess;
			if (handler != null)
				handler(sender, ea);
		}

		/// <summary>
		/// 即将启动外部启动更新进程
		/// </summary>
		public event EventHandler ExternalUpdateStart;

		/// <summary>
		/// 引发 <see cref="ExternalUpdateStart" /> 事件
		/// </summary>
		protected virtual void OnExternalUpdateStart()
		{
			var handler = ExternalUpdateStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 已经启动外部启动更新进程
		/// </summary>
		public event EventHandler ExternalUpdateStarted;

		/// <summary>
		/// 引发 <see cref="ExternalUpdateStart" /> 事件
		/// </summary>
		protected virtual void OnExternalUpdateStarted()
		{
			var handler = ExternalUpdateStarted;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 复制更新程序到临时目录并启动
		/// </summary>
		bool CopyAndStartUpdater(string[] ownerProcessList)
		{
			//写入更新文件
			var updateinfoFile = Context.UpdateInfoFilePath;
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(updateinfoFile));
			System.IO.File.WriteAllText(updateinfoFile, Context.UpdateInfoTextContent, System.Text.Encoding.UTF8);
			//写入包列表
			XMLSerializeHelper.XmlSerilizeToFile(PackagesToUpdate, Context.UpdatePackageListPath);
			XMLSerializeHelper.XmlSerilizeToFile(ExtensionMethod.ToList(FileInstaller.PreservedFiles.Keys), Context.PreserveFileListPath);

			//启动外部程序
			var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			CopyAssemblyToUpdateRoot(currentAssembly);

			var tempExePath = System.IO.Path.Combine(Context.UpdateTempRoot, System.IO.Path.GetFileName(currentAssembly.Location));

#if !STANDALONE
			tempExePath = System.IO.Path.Combine(Context.UpdateTempRoot, "AutoUpdater.exe");
			System.IO.File.WriteAllBytes(tempExePath, ExtensionMethod.Decompress(Properties.Resources.FSLib_App_Utilities_exe));
#endif
			//写入配置文件。以便于多Framework支持。。
			System.IO.File.WriteAllBytes(tempExePath + ".config", Properties.Resources.appconfig);

			//生成新的日志地址
			var logPath = "";
			if (!string.IsNullOrEmpty(Context.LogFile))
			{
				logPath = System.IO.Path.Combine(
					System.IO.Path.GetDirectoryName(Context.LogFile),
					System.IO.Path.GetFileNameWithoutExtension(Context.LogFile) + "_1" +
					System.IO.Path.GetExtension(Context.LogFile)
				);
			}

			//启动
			var sb = new StringBuilder(0x400);
			sb.AppendFormat("/log \"{0}\" ", Utility.SafeQuotePathInCommandLine(logPath));
			sb.AppendFormat("/startupdate /cv \"{0}\" ", Context.CurrentVersion.ToString());
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
				sb.Append("/ui \"" + _mainFormType.FullName + "\" ");

				if (!(_usingAssemblies ?? (_usingAssemblies = new List<Assembly>())).Contains(_mainFormType.Assembly))
					_usingAssemblies.Add(_mainFormType.Assembly);
			}
			if (_usingAssemblies != null && _usingAssemblies.Count > 0)
			{
				var assemblyNames = new List<string>();
				foreach (var assembly in _usingAssemblies)
				{
					if (CopyAssemblyToUpdateRoot(assembly))
						assemblyNames.Add(System.IO.Path.GetFileNameWithoutExtension(assembly.Location));
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

			Trace.TraceInformation("启动外部更新进程, 路径=" + psi.FileName + ", 参数=" + psi.Arguments);
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
		bool CopyAssemblyToUpdateRoot(Assembly assembly)
		{
			if (_assemblies.ContainsKey(assembly))
				return false;

			var location = assembly.Location;
			if (!location.StartsWith(Context.ApplicationDirectory, StringComparison.OrdinalIgnoreCase)) return false;

			var dest = System.IO.Path.Combine(Context.UpdateTempRoot, System.IO.Path.GetFileName(location));
			Trace.TraceInformation(string.Format("复制引用程序集 {0} 到 {1}", location, dest));
			System.IO.File.Copy(location, dest, true);
			_assemblies.Add(assembly, null);

			//如果有pdb文件, 那么也复制
			location = System.IO.Path.ChangeExtension(location, "pdb");
			if (System.IO.File.Exists(location))
			{
				Trace.TraceInformation(string.Format("复制PDB文件 {0} 到 {1}", location, dest));
				dest = System.IO.Path.ChangeExtension(dest, "pdb");
				System.IO.File.Copy(location, dest, true);
			}

			Array.ForEach(assembly.GetReferencedAssemblies(), s => CopyAssemblyToUpdateRoot(Assembly.Load(s)));

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
			Trace.TraceInformation("check if current process can write to application directory directly without admin privilege.");
			try
			{
				File.Create(tempfile).Close();
				File.Delete(tempfile);
				Trace.TraceInformation("permission check complete. no admin privilege required.");

				return true;
			}
			catch (Exception)
			{
				Trace.TraceInformation("permission denied. admin privilege required to perform operation. using /runas to perform update.");
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
			var mainProcessID = System.Diagnostics.Process.GetCurrentProcess().Id;
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
				var processes = System.Diagnostics.Process.GetProcesses();
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
				Trace.TraceInformation("begin tring loading file '" + assFile + "'....");

				try
				{
					var path = Path.Combine(Context.UpdateTempRoot, assFile);
					var assembly = System.Reflection.Assembly.LoadFile(path);

					Trace.TraceInformation("assembly loaded. checking for interface notify.");
					//检查接口调用
					var types = assembly.GetTypes();
					foreach (var type in types)
					{
						if (type.GetInterface(typeof(IUpdateNotify).FullName) != null)
						{
							Trace.TraceInformation("IUpdateNotify detected. Create object instance and invoke Init() method.");

							var obj = Activator.CreateInstance(type) as IUpdateNotify;
							obj.Init(this);
						}
					}
				}
				catch (Exception ex)
				{
					Trace.TraceError("file loading failed. error: " + ex.ToString());
				}
				Trace.TraceInformation("ending loading file '" + assFile + "'....");
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

			if (!System.IO.Directory.Exists(Context.UpdateTempRoot))
			{
				Trace.TraceInformation("未生成临时目录，不需要清理");
				return;
			}
			Trace.TraceInformation("启动外部清理进程。");

			var localpath = Environment.ExpandEnvironmentVariables(@"%TEMP%\FSLib.DeleteTmp_" + new Random().Next(100000) + ".exe");
			System.IO.File.WriteAllBytes(localpath, ExtensionMethod.Decompress(Properties.Resources.FSLib_App_Utilities_exe));
			//写入配置文件
			System.IO.File.WriteAllBytes(localpath + ".config", Properties.Resources.appconfig);

			var arg = "deletetmp \"" + Process.GetCurrentProcess().Id + "\" \"" + Utility.SafeQuotePathInCommandLine(Context.UpdateTempRoot) + "\"";
			Process.Start(localpath, arg);
			_hasCleanProcessStarted = true;
		}



		#endregion
	}
}
