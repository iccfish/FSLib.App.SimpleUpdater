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
					lblDesc.Text = "正在下载升级包……";
				};
				u.DownloadProgressChanged += (s, e) =>
				{
					var count = u.PackagesToUpdate.Count;
					//var downloading = ExtensionMethod.Count(u.PackagesToUpdate, m => m.IsDownloading);
					var downloaded = ExtensionMethod.Count(u.PackagesToUpdate, m => m.IsDownloaded || m.IsDownloading);
					var totalSize = ExtensionMethod.Sum(u.PackagesToUpdate, m => m.PackageSize);
					var downloadedSize = ExtensionMethod.Sum(u.PackagesToUpdate, m => m.DownloadedSize);

					pbProgress.Style = totalSize > 0 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
					if (totalSize > 0)
					{
						pbProgress.Value = Math.Min((int)(downloadedSize * 1.0 / totalSize * 100), 100);
					}
					lblProgressDesc.Text = string.Format("下载中...{0}/{1} ({2}/{3})", count, downloaded, ExtensionMethod.ToSizeDescription(downloadedSize), ExtensionMethod.ToSizeDescription(totalSize));

				};
				u.PackageExtractionBegin += (s, e) =>
				{
					pbProgress.Style = ProgressBarStyle.Marquee;

					lblDesc.Text = "正在解压缩升级包……";
					if (e.Package != null)
					{
						lblProgressDesc.Text = e.Package.PackageName;
					}
				};
				u.QueryCloseApplication += (s, e) =>
				{
					pbProgress.Style = ProgressBarStyle.Marquee;
					lblDesc.Text = "正在请求关闭应用程序...";
					lblProgressDesc.Text = string.Empty;
				};
				u.InstallUpdates += (s, e) =>
				{
					pbProgress.Style = ProgressBarStyle.Marquee;
					lblDesc.Text = "正在安装升级包...";
					lblProgressDesc.Text = string.Empty;
				};
				u.FileInstaller.DeleteFileStart += (s, e) =>
				{
					pbProgress.Style = ProgressBarStyle.Marquee;
					lblDesc.Text = "正在删除原始文件...";
					lblProgressDesc.Text = string.Empty;
				};
				u.FileInstaller.InstallFileStart += (s, e) =>
				{
					pbProgress.Style = ProgressBarStyle.Marquee;
					lblDesc.Text = "正在安装新文件...";
					lblProgressDesc.Text = string.Empty;
				};
				u.FileInstaller.DeleteFile += (s, e) =>
				{
					lblProgressDesc.Text = e.Source;
					pbProgress.Style = e.TotalCount > 0 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
					if (e.TotalCount > 0)
					{
						pbProgress.Value = (int)(e.CurrentCount * 1.0 / e.TotalCount * 100);
					}
				};
				u.FileInstaller.InstallFile += (s, e) =>
				{
					lblProgressDesc.Text = e.Source;
					pbProgress.Style = e.TotalCount > 0 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
					if (e.TotalCount > 0)
					{
						pbProgress.Value = (int)(e.CurrentCount * 1.0 / e.TotalCount * 100);
					}
				};
				u.RunExternalProcess += (s, e) =>
				{
					lblDesc.Text = "正在执行外部进程.....";
					pbProgress.Style = ProgressBarStyle.Marquee;
					lblProgressDesc.Text = System.IO.Path.GetFileName(e.ProcessStartInfo.FileName);
				};
			}
		}
	}
}
