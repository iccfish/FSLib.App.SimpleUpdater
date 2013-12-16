namespace FSLib.App.SimpleUpdater.Dialogs
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.slideComponent1 = new FSLib.App.SimpleUpdater.Wrapper.SlideComponent();
			this.runUpdate1 = new FSLib.App.SimpleUpdater.UpdateControl.RunUpdate();
			this.updateFinished1 = new FSLib.App.SimpleUpdater.UpdateControl.UpdateFinished();
			this.updateError1 = new FSLib.App.SimpleUpdater.UpdateControl.UpdateError();
			this.noUpdateFound1 = new FSLib.App.SimpleUpdater.UpdateControl.NoUpdateFound();
			this.downloadingInfo1 = new FSLib.App.SimpleUpdater.UpdateControl.DownloadingInfo();
			this.SuspendLayout();
			// 
			// slideComponent1
			// 
			this.slideComponent1.AlwaysSetLocation = false;
			this.slideComponent1.AttachedForm = this;
			this.slideComponent1.DirectX = FSLib.App.SimpleUpdater.Wrapper.SlideComponent.FlyXDirection.None;
			this.slideComponent1.MoveSpeedX = 0;
			this.slideComponent1.MoveSpeedY = 8;
			// 
			// runUpdate1
			// 
			resources.ApplyResources(this.runUpdate1, "runUpdate1");
			this.runUpdate1.Name = "runUpdate1";
			// 
			// updateFinished1
			// 
			resources.ApplyResources(this.updateFinished1, "updateFinished1");
			this.updateFinished1.Name = "updateFinished1";
			// 
			// updateError1
			// 
			resources.ApplyResources(this.updateError1, "updateError1");
			this.updateError1.Name = "updateError1";
			// 
			// noUpdateFound1
			// 
			resources.ApplyResources(this.noUpdateFound1, "noUpdateFound1");
			this.noUpdateFound1.Name = "noUpdateFound1";
			// 
			// downloadingInfo1
			// 
			resources.ApplyResources(this.downloadingInfo1, "downloadingInfo1");
			this.downloadingInfo1.Name = "downloadingInfo1";
			// 
			// MainWindow
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.downloadingInfo1);
			this.Controls.Add(this.runUpdate1);
			this.Controls.Add(this.updateFinished1);
			this.Controls.Add(this.updateError1);
			this.Controls.Add(this.noUpdateFound1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainWindow";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private FSLib.App.SimpleUpdater.Wrapper.SlideComponent slideComponent1;
		private FSLib.App.SimpleUpdater.UpdateControl.UpdateFinished updateFinished1;
		private FSLib.App.SimpleUpdater.UpdateControl.UpdateError updateError1;
		private FSLib.App.SimpleUpdater.UpdateControl.NoUpdateFound noUpdateFound1;
		private FSLib.App.SimpleUpdater.UpdateControl.RunUpdate runUpdate1;
		private FSLib.App.SimpleUpdater.UpdateControl.DownloadingInfo downloadingInfo1;
	}
}