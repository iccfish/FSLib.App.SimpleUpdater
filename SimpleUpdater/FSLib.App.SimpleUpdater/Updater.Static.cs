using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	using System.Threading;
	using System.Windows.Forms;

	using Annotations;

	using FSLib.App.SimpleUpdater.Defination;
	using FSLib.App.SimpleUpdater.Dialogs;
	using FSLib.App.SimpleUpdater.Wrapper;

	partial class Updater
	{
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
		/// <param name="servers">要使用的服务器地址</param>
		/// <returns></returns>
		public static Updater CreateUpdaterInstance(params UpdateServerInfo[] servers)
		{
			return CreateUpdaterInstance(false, servers);
		}

		/// <summary>
		/// 创建自动更新客户端
		/// </summary>
		/// <param name="servers">要使用的服务器地址列表</param>
		/// <param name="switchIfNoUpdate">当没有找到更新时，是否也切换服务器地址。默认为 <see langword="false"/></param>
		/// <returns></returns>
		public static Updater CreateUpdaterInstance(bool switchIfNoUpdate, params UpdateServerInfo[] servers)
		{
			return CreateUpdaterInstance(null, null, switchIfNoUpdate, servers);
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
			return CreateUpdaterInstance(appVersion, appDirectory, false, servers);
		}

		/// <summary>
		/// 创建自动更新客户端
		/// </summary>
		/// <param name="appVersion">应用程序版本，留空将会使用自动判断</param>
		/// <param name="appDirectory">应用程序目录，留空将会使用自动判断</param>
		/// <param name="servers">升级服务器地址</param>
		/// <param name="switchIfNoUpdate">当没有找到更新时，是否也切换服务器地址。默认为 <see langword="false"/></param>
		/// <returns></returns>
		public static Updater CreateUpdaterInstance(Version appVersion, string appDirectory, bool switchIfNoUpdate, params UpdateServerInfo[] servers)
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
					_instance = new MultiServerUpdater(servers) { SwitchIfNoUpdatesFound = switchIfNoUpdate };
				else _instance = new MultiServerUpdater(appVersion, appDirectory, servers) { SwitchIfNoUpdatesFound = switchIfNoUpdate };
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

			ShowUiForm(new Dialogs.MinmumVersionRequired());
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

				ShowUiForm(new UpdateFound());
			}
			updater.EnsureUpdateStarted();
		}

		/// <summary>
		/// 显示UI窗体
		/// </summary>
		/// <param name="from"></param>
		public static void ShowUiForm(Form from)
		{
			DispatchOnUiThread(_ =>
			{
				if (_ == null)
					from.ShowDialog();
				else
					from.Show(_);
			});
		}

		/// <summary>
		/// 调度线程到UI线程
		/// </summary>
		public static void DispatchOnUiThread([NotNull] Action<IWin32Window> action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			//线程调度
			var parentForm = UpdateContext.MainWindow;

			if (parentForm != null)
			{
				if (parentForm.InvokeRequired)
					parentForm.Invoke(action, (IWin32Window)parentForm);
				else
					action(parentForm);
			}
			else if (Application.MessageLoop)
			{
				action(null);
			}
			else
			{
				//启动单独的线程，并设置STA标记
				//不设置STA标记的时候，当更新信息是网页的话会报错
				var ts = new Thread(() => action(null));
				ts.IsBackground = false;
				ts.SetApartmentState(ApartmentState.STA);
				ts.Start();
			}
		}


#endregion
	}
}
