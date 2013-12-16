using System;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	public partial class ProgramExecuteTimeout : Form
	{
		public ProgramExecuteTimeout()
		{
			InitializeComponent();
		}

		private void btnWait_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnKill_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
