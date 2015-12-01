using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	using System.Windows.Forms;
	using FSLib.App.SimpleUpdater.Defination;
	using FSLib.App.SimpleUpdater.Wrapper;

	/// <summary>
	/// 可供使用的更新抽象基类
	/// </summary>
	public class AbstractUpdateBase : Form
	{
		/// <summary>
		///
		/// </summary>
		public AbstractUpdateBase()
		{
			if (!Program.IsRunning)
				return;


			AutoStartUpdate = true;
			InitUpdaterParameter();

			Shown += AbstractUpdateBase_Shown;

			//挂钩事件
			var instance = UpdaterInstance;
			instance.UpdateCancelled += (s, e) => OnUpdateCancelled();
			instance.QueryCloseApplication += (s, e) => OnRequestCloseApplication(e);
			instance.GatheringPackages += (s, e) => OnGatheringPackages();
			instance.GatheredPackages += (s, e) => OnGatheredPackages();
			instance.DownloadUpdateInfo += (s, e) => OnDownloadUpdateInfo();
			instance.DownloadUpdateInfoFinished += (s, e) => OnDownloadUpdateInfoFinished();
			instance.Error += (s, e) => OnError(UpdaterInstance.Context.Exception);
			instance.NoUpdatesFound += (s, e) => OnNoUpdateFound();
			instance.UpdateFinished += (s, e) => OnUpdateFinished();
			instance.OperationProgressChanged += (s, e) => OnOperationProgressChanged(e);
			//下载和更新安装相关的事件
			instance.PackageDownload += (s, e) => OnPackageDownload(e);
			instance.PackageHashMismatch += (s, e) => OnPackageDownloadMismatch(e);
			instance.PackageDownloadFailed += (s, e) => OnPackageDownloadFailed(e);
			instance.PackageDownloadRetried += (s, e) => OnPackageDownloadRetry(e);
			instance.PackageDownloadFinished += (s, e) => OnPackageDownloadFinished(e);
			instance.DownloadProgressChanged += (s, e) => OnPackageDownloadProgressChanged(e);
			instance.PackageExtractionBegin += (s, e) => OnDecompressPackage(e);
			instance.PackageExtractionEnd += (s, e) => OnDecompressPackageFinished(e);
			instance.InstallUpdates += (s, e) => OnInstallUpdate();
			instance.FileInstaller.InstallFileStart += (s, e) => OnInstallFiles();
			instance.FileInstaller.InstallFileFinished += (s, e) => OnInstallFiles();
			instance.FileInstaller.InstallFile += (s, e) => OnInstallFile(e);
			instance.FileInstaller.DeleteFileStart += (s, e) => OnRemoveFiles();
			instance.FileInstaller.DeleteFileFinished += (s, e) => OnRemoveFilesFinished();
			instance.FileInstaller.DeleteFile += (s, e) => OnRemoveFile(e);
			instance.FileInstaller.RollbackStart += (s, e) => OnRollbackFiles();
			instance.FileInstaller.RollbackFinished += (s, e) => OnRollbackFilesFinished();
			instance.FileInstaller.RollbackFile += (s, e) => OnRollbackFile(e);
			//进程相关
			instance.ExecuteExternalProcessAfter += (s, e) => OnExecuteExternalProgramAfter();
			instance.ExecuteExternalProcessBefore += (s, e) => OnExecuteExternalProgramBefore();
			instance.ExternalUpdateStart += (s, e) => OnExternalUpdaterStart();
			instance.ExternalUpdateStarted += (s, e) => OnExternalUpdaterStarted();
			instance.RunExternalProcess += (s, e) => OnExecuteExternalProcess(e);

			//misc
			instance.MinmumVersionRequired += (s, e) => OnMiniumalVersionRequired();
		}

		private void AbstractUpdateBase_Shown(object sender, EventArgs e)
		{
			if (AutoStartUpdate)
			{
				StartUpdate();
			}
		}

		/// <summary>
		/// 要求初始化请求参数
		/// </summary>
		protected virtual void InitUpdaterParameter()
		{

		}

		/// <summary>
		/// 开始检测或正式更新。这步操作将会判断当前状态，并确定是要开始检测还是直接开始下载更新
		/// </summary>
		protected virtual void StartUpdate()
		{
			if (UpdaterInstance.Context.IsUpdateInfoDownloaded) StartDownloadUpdate();
			else StartCheckUpdate();
		}

		/// <summary>
		/// 开始检测更新
		/// </summary>
		protected virtual void StartCheckUpdate()
		{
			UpdaterInstance.BeginCheckUpdateInProcess();
		}

		/// <summary>
		/// 开始正式更新
		/// </summary>
		protected virtual void StartDownloadUpdate()
		{
			if (UpdaterInstance.Context.IsInUpdateMode)
				UpdaterInstance.BeginUpdate();
			else
			{
				UpdaterInstance.StartExternalUpdater();
				Close();
			}
		}

		/// <summary>
		/// 更新已取消
		/// </summary>
		protected virtual void OnUpdateCancelled()
		{
			Environment.Exit(0);
		}

		/// <summary>
		/// 已找到更新
		/// </summary>
		protected virtual void OnUpdatesFound()
		{
			var u = UpdaterInstance;

			//是否强制更新？
			if (u.Context.ForceUpdate)
			{
				StartDownloadUpdate();
				return;
			}
			foreach (Form openForm in Application.OpenForms)
			{
				if (openForm is UpdateFound)
					return;
			}

			using (var dlg = new UpdateFound())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
					StartDownloadUpdate();
				else Close();
			}
		}

		/// <summary>
		/// 请求关闭已启动的程序
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRequestCloseApplication(QueryCloseApplicationEventArgs e)
		{
			e.CallDefaultBeihavior();
		}

		/// <summary>
		/// 正在下载更新信息
		/// </summary>
		protected virtual void OnDownloadUpdateInfo()
		{

		}

		/// <summary>
		/// 已完成下载更新信息
		/// </summary>
		protected virtual void OnDownloadUpdateInfoFinished()
		{

		}

		/// <summary>
		/// 没有找到更新
		/// </summary>
		protected virtual void OnNoUpdateFound()
		{

		}

		/// <summary>
		/// 发生错误
		/// </summary>
		/// <param name="ex">错误信息</param>
		protected virtual void OnError(Exception ex)
		{

		}

		/// <summary>
		/// 正在计算下载所需要的压缩包信息
		/// </summary>
		protected virtual void OnGatheringPackages()
		{

		}

		/// <summary>
		/// 已经完成计算下载所需要的压缩包信息
		/// </summary>
		protected virtual void OnGatheredPackages()
		{

		}

		/// <summary>
		/// 已完成更新
		/// </summary>
		protected virtual void OnUpdateFinished()
		{

		}


		/// <summary>
		/// 开始下载包
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPackageDownload(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 重试下载包
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPackageDownloadRetry(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 下载包失败
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPackageDownloadFailed(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 下载包成功，但是文件不符
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPackageDownloadMismatch(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 下载包失败
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPackageDownloadFinished(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 包下载进度变更
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPackageDownloadProgressChanged(PackageDownloadProgressChangedEventArgs e)
		{

		}

		/// <summary>
		/// 开始解压缩包
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDecompressPackage(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 解压缩包完成
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDecompressPackageFinished(PackageEventArgs e)
		{

		}

		/// <summary>
		/// 开始安装更新
		/// </summary>
		protected virtual void OnInstallUpdate()
		{

		}

		/// <summary>
		/// 正在删除旧文件
		/// </summary>
		protected virtual void OnRemoveFiles()
		{
		}

		/// <summary>
		/// 删除旧文件完成
		/// </summary>
		protected virtual void OnRemoveFilesFinished()
		{
		}

		/// <summary>
		/// 开始安装更新文件
		/// </summary>
		protected virtual void OnInstallFiles()
		{
		}

		/// <summary>
		/// 更新文件安装完成
		/// </summary>
		protected virtual void OnInstallFilesFinished()
		{
		}

		/// <summary>
		/// 正在安装文件
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnInstallFile(InstallFileEventArgs e)
		{

		}

		/// <summary>
		/// 正在删除文件
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRemoveFile(InstallFileEventArgs e)
		{
		}


		/// <summary>
		/// 开始回滚文件
		/// </summary>
		protected virtual void OnRollbackFiles()
		{
		}

		/// <summary>
		/// 回滚文件完成
		/// </summary>
		protected virtual void OnRollbackFilesFinished()
		{
		}

		/// <summary>
		/// 正在回滚文件
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRollbackFile(InstallFileEventArgs e)
		{

		}

		/// <summary>
		/// 正在运行更新前进程
		/// </summary>
		protected virtual void OnExecuteExternalProgramBefore()
		{

		}

		/// <summary>
		/// 正在运行更新后进程
		/// </summary>
		protected virtual void OnExecuteExternalProgramAfter()
		{

		}

		/// <summary>
		/// 正在启动外部更新辅助进程
		/// </summary>
		protected virtual void OnExternalUpdaterStart() { }

		/// <summary>
		/// 外部更新辅助进程已启动
		/// </summary>
		protected virtual void OnExternalUpdaterStarted()
		{
		}

		/// <summary>
		/// 要求最低版本
		/// </summary>
		protected virtual void OnMiniumalVersionRequired() { }


		/// <summary>
		/// 进度发生变化。此进度包含更新信息下载/文件安装/解压缩等等，但不包含包下载
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnOperationProgressChanged(RunworkEventArgs e) { }

		/// <summary>
		/// 正在执行外部进程
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnExecuteExternalProcess(RunExternalProcessEventArgs e) { }

		/// <summary>
		/// 获得或设置是否自动开始更新
		/// </summary>
		protected bool AutoStartUpdate { get; set; }

		/// <summary>
		/// 返回当前的更新实例
		/// </summary>
		protected Updater UpdaterInstance { get { return Updater.Instance; } }
	}
}
