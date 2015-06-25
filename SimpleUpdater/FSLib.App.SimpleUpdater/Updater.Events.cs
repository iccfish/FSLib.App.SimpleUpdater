using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	using FSLib.App.SimpleUpdater.Defination;
	using FSLib.App.SimpleUpdater.Wrapper;

	partial class Updater
	{
		#region 事件区域


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
		/// 检测组件标记
		/// </summary>
		/// <param name="compId">组件ID</param>
		/// <returns></returns>
		bool CheckComponentFlag(string compId)
		{
			var dic = Context.ComponentStatus;
			if (dic.ContainsKey(compId))
				return dic[compId];

			var ea = new RequestCheckComponentFlagEventArgs(compId);
			OnRequestCheckComponentFlag(ea);
			dic.Add(compId, ea.Valid);

			return ea.Valid;
		}

		/// <summary>
		/// 请求检测组件状态位
		/// </summary>
		public event EventHandler<RequestCheckComponentFlagEventArgs> RequestCheckComponentFlag;

		/// <summary>
		/// 引发 <see cref="RequestCheckComponentFlag" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnRequestCheckComponentFlag(RequestCheckComponentFlagEventArgs ea)
		{
			var handler = RequestCheckComponentFlag;
			if (handler != null)
				handler(this, ea);
		}

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
		/// 检测更新完成
		/// </summary>
		public event EventHandler CheckUpdateComplete;

		/// <summary>
		/// 引发 <see cref="CheckUpdateComplete"/> 事件
		/// </summary>
		protected virtual void OnCheckUpdateComplete()
		{
			var handler = CheckUpdateComplete;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion


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
		#endregion

		#region 包下载事件

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


		#region 更新流程-执行进程以及进程关闭相关

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
		#endregion




		#region 更新流程-解压缩更新包

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
		#endregion

	}
}
