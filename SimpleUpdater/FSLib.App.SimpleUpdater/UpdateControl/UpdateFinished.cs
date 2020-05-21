using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public partial class UpdateFinished : FSLib.App.SimpleUpdater.UpdateControl.ControlBase
	{
		public UpdateFinished()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{
				Updater.Instance.UpdateFinished += Instance_UpdateFinished;
			}
		}

		void Instance_UpdateFinished(object sender, EventArgs e)
		{
			this.HideControls();
			this.Show();

			if (Updater.Instance.Context.UpdateInfo.AutoCloseSucceedWindow)
			{
				AutoClose(Updater.Instance.Context.UpdateInfo.AutoCloseSucceedTimeout);
			}
		}
	}
}
