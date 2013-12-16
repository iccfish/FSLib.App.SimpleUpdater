using System;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Program.IsRunning)
			{
				HideAllControls();
				Updater upd = Updater.Instance;
				//调用默认的关闭进程处理
				upd.QueryCloseApplication += (_s, _e) => _e.CallDefaultBeihavior();
				upd.UpdateCancelled += (_, __) => Environment.Exit(0);
				upd.UpdatesFound += upd_UpdatesFound;

				if (upd.Context.IsUpdateInfoDownloaded) { upd.BeginUpdate(); }
				else upd.BeginCheckUpdateInProcess();
			}
		}

		void upd_UpdatesFound(object sender, EventArgs e)
		{
			var u = Updater.Instance;

			//是否强制更新？
			if (u.Context.ForceUpdate)
			{
				StartUpdate();
				return;
			}
			var dlg = new UpdateFound();
			if (dlg.ShowDialog() == DialogResult.OK)
				StartUpdate();
			else Close();
		}

		void StartUpdate()
		{
			var updater = Updater.Instance;
			if (updater.Context.IsInUpdateMode)
				updater.BeginUpdate();
			else
			{
				updater.StartExternalUpdater();
				Close();
			}
		}

		/// <summary>
		/// 隐藏所有控件
		/// </summary>
		internal void HideAllControls()
		{
			foreach (Control item in this.Controls)
			{
				if (item is UpdateControl.ControlBase) item.Visible = false;
			}
		}
	}
}
