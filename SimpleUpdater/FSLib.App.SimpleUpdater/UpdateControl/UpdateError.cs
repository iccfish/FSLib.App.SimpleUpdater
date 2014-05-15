using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public partial class UpdateError : FSLib.App.SimpleUpdater.UpdateControl.ControlBase
	{
		public UpdateError()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{
				Updater.Instance.Error += Instance_Error;
			}
		}

		void Instance_Error(object sender, EventArgs e)
		{
			HideControls();
			this.Visible = true;
			StepDesc = Updater.Instance.Context.Exception.Message;
			System.Diagnostics.Trace.TraceWarning(Updater.Instance.Context.Exception.ToString());

#if DEBUG
			System.Windows.Forms.MessageBox.Show(Updater.Instance.Context.Exception.ToString());
#endif
		}
	}
}
