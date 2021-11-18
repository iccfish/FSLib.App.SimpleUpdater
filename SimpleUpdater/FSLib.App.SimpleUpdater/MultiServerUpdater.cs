using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using FSLib.App.SimpleUpdater.Defination;

namespace FSLib.App.SimpleUpdater
{
	using Logs;

	/// <summary>
	/// 支持多服务器升级的自动升级类
	/// </summary>
	class MultiServerUpdater : Updater
	{
		int _serverIndex = 0;
		private static ILogger _logger = LogManager.Instance.GetLogger<MultiServerUpdater>();

		/// <summary>
		/// 获得当前使用的备用服务器列表
		/// </summary>
		public List<UpdateServerInfo> Servers { get; private set; }

		/// <summary>
		/// 获得或设置当没有找到更新的时候是否也切换服务器地址。默认为 <see langword="false" />
		/// </summary>
		public bool SwitchIfNoUpdatesFound { get; set; }


		public MultiServerUpdater(params UpdateServerInfo[] servers)
			: this(null, null, servers)
		{
		}

		/// <param name="appVersion">指定的应用程序版本</param>
		/// <param name="appDirectory">指定的应用程序路径</param>

		public MultiServerUpdater(Version appVersion, string appDirectory, params UpdateServerInfo[] servers)
			: base(appVersion, appDirectory)
		{
			Servers = new List<UpdateServerInfo>();
			Servers.AddRange(servers);
		}

		#region Overrides of Updater

		/// <summary>
		/// 选择下一个服务器
		/// </summary>
		bool PeekNextServer()
		{
			if (Servers.Count == 0)
				return false;

			if (_serverIndex >= Servers.Count)
			{
				_serverIndex = 0;
				return false;
			}

			var server = Servers[_serverIndex++];
			Context.UpdateDownloadUrl = server.Url;
			Context.UpdateInfoFileName = server.InfoFileName;

			return true;
		}

		/// <summary>
		/// 引发 <see cref="Updater.Error" /> 事件
		/// </summary>
		protected override void OnError()
		{
			if (PeekNextServer())
			{
				_logger.LogWarning("尝试更新时出现服务器错误。正尝试自动切换至其它的服务器节点。已切换至 " + Context.UpdateDownloadUrl);
				base.BeginCheckUpdateInProcess();
			}
			else
			{
				_logger.LogWarning("尝试更新时出现服务器错误，且服务器已遍历完成。");
				base.OnError();
			}
		}

		/// <summary>
		/// 引发 <see cref="Updater.NoUpdatesFound"/> 事件
		/// </summary>
		protected override void OnNoUpdatesFound()
		{

			if (SwitchIfNoUpdatesFound)
			{
				if (PeekNextServer())
				{
					_logger.LogWarning("没有找到更新，且服务器已遍历完成。");
				}
				else
				{
					_logger.LogWarning("没有找到更新。正尝试自动切换至其它的服务器节点。已切换至 " + Context.UpdateDownloadUrl);
					base.BeginCheckUpdateInProcess();
				}
			}
			else
			{
				base.OnNoUpdatesFound();
			}
		}

		/// <summary>
		/// 开始检测更新
		/// </summary>
		/// <returns>返回是否成功开始检查</returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public override bool BeginCheckUpdateInProcess()
		{
			//重置服务器
			_serverIndex = 0;
			PeekNextServer();

			return base.BeginCheckUpdateInProcess();
		}

		#endregion
	}
}
