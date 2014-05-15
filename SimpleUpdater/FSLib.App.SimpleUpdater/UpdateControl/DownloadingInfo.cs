using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public partial class DownloadingInfo : ControlBase
	{
		public DownloadingInfo()
		{
			InitializeComponent();
			SetProgress(0);

			if (Program.IsRunning)
			{
				var ui = Updater.Instance;
				ui.DownloadUpdateInfo += Instance_DownloadUpdateInfo;
				ui.GatheringPackages += (s, e) =>
				{
					StepTitle = "正在计算要下载的文件信息……";
				};
			}
		}

		void Instance_DownloadUpdateInfo(object sender, EventArgs e)
		{
			HideControls();
			this.Show();
		}
	}
}
