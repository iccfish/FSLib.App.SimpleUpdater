using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	public partial class ArgumentGenerator : UserControl
	{
		public ArgumentGenerator()
		{
			InitializeComponent();
			txtProxy_TextChanged(null, null);
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			var sb = new System.Text.StringBuilder();
			sb.Append("/startupdate");
			AppendArgument(sb, "cv", txtVer);
			AppendArgument(sb, "ad", txtAd);
			AppendArgument(sb, "url", txtUrl);
			AppendArgument(sb, "infofile", txtInfoFile);
			AppendArgument(sb, "proxy", txtProxy);
			if (!txtProxy.Text.IsNullOrEmpty())
			{
				sb.AppendFormat(" /cred {0}:{1}", txtProxyUser.Text, txtProxyPwd.Text);
			}
			AppendArgument(sb, "log", txtLog);
			AppendArgument(sb, "autokill", chkForceKill);
			AppendArgument(sb, "forceupdate", chkForceUpdate);
			AppendArgument(sb, "p", txtExternalProcess);
			AppendArgument(sb, "hideCheckUI", chkHideUi);

			txtArg.Text = sb.ToString();
		}

		void AppendArgument(System.Text.StringBuilder sb, string name, Control ctl)
		{
			if (ctl is CheckBox)
			{
				var cb = ctl as CheckBox;
				if (cb.Checked)
				{
					sb.Append(" /" + name);
				}
				return;
			}

			if (ctl is TextBox)
			{
				var txt = ctl as TextBox;
				if (!string.IsNullOrEmpty(txt.Text))
				{
					var list = txt.Text.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
					list.ForEach(s => sb.AppendFormat(" /{0} \"{1}\"", name, txt.Text));
				}
				return;
			}
		}

		private void txtProxy_TextChanged(object sender, EventArgs e)
		{
			txtProxyPwd.Enabled = txtProxyUser.Enabled = !string.IsNullOrEmpty(txtProxy.Text);
		}

		private void lnkMoreLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.fishlee.net/soft/simple_autoupdater/options.html#log");
		}
	}
}
