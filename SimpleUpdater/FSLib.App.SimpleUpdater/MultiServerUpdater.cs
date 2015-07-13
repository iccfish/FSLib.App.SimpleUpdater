using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using FSLib.App.SimpleUpdater.Defination;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 支持多服务器升级的自动升级类
	/// </summary>
	class MultiServerUpdater : Updater
	{
		/// <summary>
		/// 获得当前使用的备用服务器列表
		/// </summary>
		public Queue<UpdateServerInfo> Servers { get; private set; }

		/// <summary>
		/// 获得或设置当没有找到更新的时候是否也切换服务器地址。默认为 <see langword="false" />
		/// </summary>
		public bool SwitchIfNoUpdatesFound { get; set; }


		public MultiServerUpdater(params UpdateServerInfo[] servers)
			: base()
		{
			Servers = new Queue<UpdateServerInfo>(servers);
			InitServerInfo();
		}

		/// <param name="appVersion">指定的应用程序版本</param>
		/// <param name="appDirectory">指定的应用程序路径</param>

		public MultiServerUpdater(Version appVersion, string appDirectory, params UpdateServerInfo[] servers)
			: base(appVersion, appDirectory)
		{
			Servers = new Queue<UpdateServerInfo>(servers);
			InitServerInfo();
		}

		/// <summary>
		/// 初始化服务器信息
		/// </summary>
		void InitServerInfo()
		{
			if (string.IsNullOrEmpty(Context.UpdateDownloadUrl))
				PeekNextServer();
		}

		#region Overrides of Updater

		/// <summary>
		/// 选择下一个服务器
		/// </summary>
		void PeekNextServer()
		{
			var server = Servers.Dequeue();
			Context.UpdateDownloadUrl = server.Url;
			Context.UpdateInfoFileName = server.InfoFileName;
		}

		/// <summary>
		/// 引发 <see cref="Updater.Error" /> 事件
		/// </summary>
		protected override void OnError()
		{
			var ex = Context.Exception;

			if (ex is WebException && Servers.Count > 0)
			{
				var server = Servers.Dequeue();
				Context.UpdateDownloadUrl = server.Url;
				Context.UpdateInfoFileName = server.InfoFileName;

				Trace.TraceWarning("尝试更新时出现服务器错误。正尝试自动切换至其它的服务器节点。已切换至 " + server.Url);

				BeginCheckUpdateInProcess();
			}
			else
			{
				base.OnError();
			}
		}

		/// <summary>
		/// 引发 <see cref="Updater.NoUpdatesFound"/> 事件
		/// </summary>
		protected override void OnNoUpdatesFound()
		{

			if (SwitchIfNoUpdatesFound && Servers.Count > 0)
			{
				var server = Servers.Dequeue();
				Context.UpdateDownloadUrl = server.Url;
				Context.UpdateInfoFileName = server.InfoFileName;

				Trace.TraceWarning("没有找到更新。正尝试自动切换至其它的服务器节点。已切换至 " + server.Url);

				BeginCheckUpdateInProcess();
			}
			else
			{
				base.OnNoUpdatesFound();
			}
		}

		#endregion
	}
}
