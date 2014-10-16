using System;
using System.Windows.Forms;
using FSLib.App.SimpleUpdater.Wrapper;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	using System.Diagnostics;

	/// <summary>
	/// 内置的找到更新对话框，表示找到了更新
	/// </summary>
	public partial class UpdateFound : Form
	{
		bool _autoUpdateStarted;

		/// <summary>
		///   构造函数
		/// </summary>
		public UpdateFound()
		{
			InitializeComponent();

			this.lnkSoft.Click += (s, e) => System.Diagnostics.Process.Start(Updater.Instance.Context.UpdateInfo.PublishUrl);
			FormClosing += UpdateFound_FormClosing;
		}

		void UpdateFound_FormClosing(object sender, FormClosingEventArgs e)
		{
			var updater = Updater.Instance;

			if (updater.Context.IsUpdaterSuccessfullyStarted == null && updater.Context.MustUpdate)
			{
				var dlgResult = MessageBox.Show(FSLib.App.SimpleUpdater.SR.UpdatesFound_CriticalUpdateWarning, SR.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dlgResult == DialogResult.No)
				{
					e.Cancel = true;
					return;
				}
			}

			updater.EnsureUpdateStarted();
		}

		private void UpdateFound_Load(object sender, EventArgs e)
		{
			if (Updater.Instance == null) return;

			var ctx = Updater.Instance.Context;
			var ui = ctx.UpdateInfo;

			lblFound.Text = ui.AppName;
			lblVersion.Text += ui.AppVersion;

			if (!string.IsNullOrEmpty(ui.WebUpdateNote))
			{
				var wb = new WebBrowser()
				{
					Dock = DockStyle.Fill,
					ScrollBarsEnabled = true
				};
				controlContainer.Controls.Add(wb);
				wb.Navigate(ui.WebUpdateNote);
			}
			else
			{
				var rtf = new RichTextBox()
				{
					ScrollBars = RichTextBoxScrollBars.Vertical,
					Dock = DockStyle.Fill,
					ReadOnly = true,
					BackColor = System.Drawing.SystemColors.Window,
					BorderStyle = BorderStyle.None
				};
				if (!string.IsNullOrEmpty(ui.RtfUpdateNote))
				{
					rtf.Rtf = System.Text.Encoding.UTF8.GetString(Wrapper.ExtensionMethod.Decompress(Convert.FromBase64String(ui.RtfUpdateNote)));
				}
				else
				{
					rtf.Text = ui.Desc;
				}
				controlContainer.Controls.Add(rtf);
				rtf.LinkClicked += rtf_LinkClicked;
			}

			var pkgSize = ExtensionMethod.Sum(Updater.Instance.PackagesToUpdate, s => s.PackageSize);
			lblSize.Text = string.Format(FSLib.App.SimpleUpdater.SR.UpdateFound_EstimateDownloadSize, (pkgSize == 0 ? SR.Unknown : ExtensionMethod.ToSizeDescription(pkgSize)));

			lnkSoft.Visible = !string.IsNullOrEmpty(ui.PublishUrl);

			if (ctx.MustUpdate)
			{
				//必须升级
				lnkCance.Visible = false;
			}
		}

		void rtf_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			try
			{
				Process.Start(e.LinkText);
			}
			catch (Exception ex)
			{

			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			btnUpdate.Focus();
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			var updater = Updater.Instance;
			if (updater == null) return;

			updater.StartExternalUpdater();
			Close();
		}


		private void lnkCance_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Updater.Instance.OnUpdateCancelled();
			Close();
		}
	}
}
