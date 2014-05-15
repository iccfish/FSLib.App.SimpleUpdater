using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSLib.App.SimpleUpdater.Wrapper;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public partial class RunUpdate : FSLib.App.SimpleUpdater.UpdateControl.ControlBase
	{
		public RunUpdate()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{

				//Updater.Instance.DownloadPackage += Instance_DownloadPackage;
				//Updater.Instance.DownloadPackageFinished += Instance_DownloadPackageFinished;
				//Updater.Instance.DownloadProgressChanged += Instance_DownloadProgressChanged;
				//Updater.Instance.InstallUpdates += Instance_InstallUpdates;
				//Updater.Instance.QueryCloseApplication += Instance_QueryCloseApplication;
				//Updater.Instance.VerifyPackage += Instance_VerifyPackage;
				//Updater.Instance.VerifyPackageFinished += Instance_VerifyPackageFinished;

				var u = Updater.Instance;
				u.PackageDownload += (s, e) =>
				{
					if (!Visible)
					{
						HideControls();
						Show();
					}
					StepTitle = "正在下载升级包……";
				};
				u.DownloadProgressChanged += (s, e) =>
				{
					var count = u.PackagesToUpdate.Count;
					//var downloading = ExtensionMethod.Count(u.PackagesToUpdate, m => m.IsDownloading);
					var downloaded = ExtensionMethod.Count(u.PackagesToUpdate, m => m.IsDownloaded || m.IsDownloading);
					var totalSize = ExtensionMethod.Sum(u.PackagesToUpdate, m => m.PackageSize);
					var downloadedSize = ExtensionMethod.Sum(u.PackagesToUpdate, m => m.DownloadedSize);

					SetProgress((int)downloadedSize, (int)totalSize);
					StepDesc = string.Format("下载中...{0}/{1} ({2}/{3})", count, downloaded, ExtensionMethod.ToSizeDescription(downloadedSize), ExtensionMethod.ToSizeDescription(totalSize));

				};
				u.PackageExtractionBegin += (s, e) =>
				{
					SetProgress(0);

					StepTitle = "正在解压缩升级包……";
					if (e.Package != null)
					{
						StepDesc = e.Package.PackageName;
					}
				};
				u.QueryCloseApplication += (s, e) =>
				{
					SetProgress(0);
					StepTitle = "正在请求关闭应用程序...";
					StepDesc = string.Empty;
				};
				u.InstallUpdates += (s, e) =>
				{
					SetProgress(0);
					StepTitle = "正在安装升级包...";
					StepDesc = string.Empty;
				};
				u.FileInstaller.DeleteFileStart += (s, e) =>
				{
					SetProgress(0);
					StepTitle = "正在删除原始文件...";
					StepDesc = string.Empty;
				};
				u.FileInstaller.InstallFileStart += (s, e) =>
				{
					SetProgress(0);
					StepTitle = "正在安装新文件...";
					StepDesc = string.Empty;
				};
				u.FileInstaller.DeleteFile += (s, e) =>
				{
					StepDesc = e.Source;
					SetProgress(e.TotalCount > 0 ? Math.Min((int)(e.CurrentCount * 1.0 / e.TotalCount * 100), 100) : 0, 100);
				};
				u.FileInstaller.InstallFile += (s, e) =>
				{
					StepDesc = e.Source;
					SetProgress(e.CurrentCount, e.TotalCount);
				};
				u.RunExternalProcess += (s, e) =>
				{
					StepTitle = "正在执行外部进程.....";
					SetProgress(0);
					StepDesc = System.IO.Path.GetFileName(e.ProcessStartInfo.FileName);
				};
			}
		}
	}
}
