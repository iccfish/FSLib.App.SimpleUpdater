using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public partial class NoUpdateFound : FSLib.App.SimpleUpdater.UpdateControl.ControlBase
	{
		public NoUpdateFound()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{
				Updater.Instance.NoUpdatesFound += Instance_NoUpdatesFound;
			}
		}

		void Instance_NoUpdatesFound(object sender, EventArgs e)
		{
			HideControls();
			this.Show();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.FindForm().Close();
		}
	}
}
