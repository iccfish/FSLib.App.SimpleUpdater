using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public partial class DownloadingInfo : FSLib.App.SimpleUpdater.UpdateControl.ControlBase
	{
		public DownloadingInfo()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{
				var ui = Updater.Instance;
				ui.DownloadUpdateInfo += Instance_DownloadUpdateInfo;
				ui.GatheringPackages += (s, e) =>
				{
					lblDesc.Text = "正在计算要下载的文件信息……";
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
