using System;
using System.Windows.Forms;
using FSLib.App.SimpleUpdater.Wrapper;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	public partial class UpdateFound : Form
	{
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
			else if (!string.IsNullOrEmpty(ui.RtfUpdateNote))
			{
				var rtf = new RichTextBox()
				{
					ScrollBars = RichTextBoxScrollBars.Vertical,
					Dock = DockStyle.Fill,
					ReadOnly = true,
					BackColor = System.Drawing.SystemColors.Window,
					BorderStyle = BorderStyle.None
				};
				rtf.Rtf = System.Text.Encoding.UTF8.GetString(Wrapper.ExtensionMethod.Decompress(Convert.FromBase64String(ui.RtfUpdateNote)));
				controlContainer.Controls.Add(rtf);
			}
			else
			{
				var txt = new TextBox
				{
					Multiline = true,
					Dock = DockStyle.Fill,
					ScrollBars = ScrollBars.Vertical,
					ReadOnly = true,
					Text = ui.Desc
				};
				controlContainer.Controls.Add(txt);
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

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Updater.Instance.OnUpdateCancelled();
			Close();
		}
	}
}
