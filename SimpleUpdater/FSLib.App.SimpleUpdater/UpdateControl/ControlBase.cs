using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using FSLib.App.SimpleUpdater.Dialogs;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	public class ControlBase : UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.Size = new Size(200, 98);
			if (Program.IsRunning)
			{
				this.Location = new Point(12, 30);
				this.BackColor = Color.Transparent;
				this.Visible = false;
				Size = new Size(216, 88);

				foreach (Control ctl in this.Controls)
				{
					if (ctl is Button) (ctl as Button).ForeColor = SystemColors.ControlText;
				}
			}
		}

		/// <summary>
		/// 请求主窗体隐藏所有控件
		/// </summary>
		protected void HideControls()
		{
			var mw = this.FindForm() as MainWindow;
			mw.HideAllControls();
		}
	}
}
