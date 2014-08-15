using System;
using System.Windows.Forms;
using FSLib.App.SimpleUpdater.Wrapper;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	/// <summary>
	/// 内置的找到更新对话框，表示找到了更新
	/// </summary>
	public partial class UpdateFound : Form
	{
		/// <summary>
		///   构造函数
		/// </summary>
		public UpdateFound()
		{
			InitializeComponent();

			this.lnkSoft.Click += (s, e) => System.Diagnostics.Process.Start(Updater.Instance.Context.UpdateInfo.PublishUrl);
		}

		private void UpdateFound_Load(object sender, EventArgs e)
		{
			if (Updater.Instance == null) return;

			var ui = Updater.Instance.Context.UpdateInfo;

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
			}

			var pkgSize = ExtensionMethod.Sum(Updater.Instance.PackagesToUpdate, s => s.PackageSize);
			lblSize.Text = string.Format(FSLib.App.SimpleUpdater.SR.UpdateFound_EstimateDownloadSize, (pkgSize == 0 ? "<未知>" : ExtensionMethod.ToSizeDescription(pkgSize)));

			this.lnkSoft.Visible = !string.IsNullOrEmpty(ui.PublishUrl);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			btnUpdate.Focus();
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			if (Updater.Instance == null) return;
			Updater.Instance.StartExternalUpdater();
			Close();
		}

		private void lnkCance_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Updater.Instance.OnUpdateCancelled();
			Close();
		}
	}
}
