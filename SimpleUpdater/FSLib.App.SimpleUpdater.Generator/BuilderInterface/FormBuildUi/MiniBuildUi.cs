using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator.BuilderInterface.FormBuildUi
{
	using Wrapper;

	using ZipBuilder;

	public partial class MiniBuildUi : Form
	{
		public MiniBuildUi()
		{
			InitializeComponent();
		}

		public void SetSuccess()
		{
			lblStatus.Text = "构建成功！";
			pg.Style = ProgressBarStyle.Continuous;
			pg.Value = 100;
		}

		public void SetFailed()
		{
			lblStatus.Text = "构建失败！";
			pg.Value = 100;
			pg.Style = ProgressBarStyle.Continuous;
		}

		public void SetProgress(bool indeterminate, int percentage, string description)
		{
			if (InvokeRequired)
			{
				this.Invoke(SetProgress, indeterminate, percentage, description);
				return;
			}

			if (indeterminate)
			{
				pg.Style = ProgressBarStyle.Marquee;
			}
			else
			{
				pg.Style = ProgressBarStyle.Continuous;
				pg.Value = percentage;
			}

			if (!string.IsNullOrEmpty(description))
				lblStatus.Text = description;
		}
	}
}
