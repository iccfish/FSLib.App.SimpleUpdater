using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	public partial class MinmumVersionRequired : Form
	{
		public MinmumVersionRequired()
		{
			InitializeComponent();
		}

		private void MinmumVersionRequired_Load(object sender, EventArgs e)
		{
			var updater = Updater.Instance;
			var info = updater.Context.UpdateInfo;

			lblDesc.Text += "\r\n\r\n" + string.Format(SR.MinmumVersionRequired_Desc, info.RequiredMinVersion, updater.Context.CurrentVersion);
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			var updater = Updater.Instance;

			try
			{
				System.Diagnostics.Process.Start(updater.Context.UpdateInfo.PublishUrl);
			}
			catch (Exception)
			{

			}
			Close();
		}
	}
}
