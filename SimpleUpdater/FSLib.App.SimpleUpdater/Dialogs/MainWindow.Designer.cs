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
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panMain = new System.Windows.Forms.Panel();
			this.downloadingInfo1 = new FSLib.App.SimpleUpdater.UpdateControl.DownloadingInfo();
			this.runUpdate1 = new FSLib.App.SimpleUpdater.UpdateControl.RunUpdate();
			this.updateFinished1 = new FSLib.App.SimpleUpdater.UpdateControl.UpdateFinished();
			this.updateError1 = new FSLib.App.SimpleUpdater.UpdateControl.UpdateError();
			this.noUpdateFound1 = new FSLib.App.SimpleUpdater.UpdateControl.NoUpdateFound();
			this.slideComponent1 = new FSLib.App.SimpleUpdater.Wrapper.SlideComponent();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(163)))), ((int)(((byte)(78)))));
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.label1);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.AUTOMATIC_UPDATES_16x16_32;
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Name = "label1";
			// 
			// panMain
			// 
			this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panMain.Controls.Add(this.downloadingInfo1);
			this.panMain.Controls.Add(this.runUpdate1);
			this.panMain.Controls.Add(this.updateFinished1);
			this.panMain.Controls.Add(this.updateError1);
			this.panMain.Controls.Add(this.noUpdateFound1);
			resources.ApplyResources(this.panMain, "panMain");
			this.panMain.Name = "panMain";
			// 
			// downloadingInfo1
			// 
			this.downloadingInfo1.BackColor = System.Drawing.Color.White;
			resources.ApplyResources(this.downloadingInfo1, "downloadingInfo1");
			this.downloadingInfo1.Image = ((System.Drawing.Image)(resources.GetObject("downloadingInfo1.Image")));
			this.downloadingInfo1.Name = "downloadingInfo1";
			this.downloadingInfo1.ShowCloseButton = false;
			this.downloadingInfo1.ShowProgress = true;
			// 
			// runUpdate1
			// 
			resources.ApplyResources(this.runUpdate1, "runUpdate1");
			this.runUpdate1.Image = ((System.Drawing.Image)(resources.GetObject("runUpdate1.Image")));
			this.runUpdate1.Name = "runUpdate1";
			this.runUpdate1.ShowCloseButton = false;
			this.runUpdate1.ShowProgress = true;
			// 
			// updateFinished1
			// 
			resources.ApplyResources(this.updateFinished1, "updateFinished1");
			this.updateFinished1.Image = ((System.Drawing.Image)(resources.GetObject("updateFinished1.Image")));
			this.updateFinished1.Name = "updateFinished1";
			this.updateFinished1.ShowCloseButton = true;
			this.updateFinished1.ShowProgress = false;
			// 
			// updateError1
			// 
			resources.ApplyResources(this.updateError1, "updateError1");
			this.updateError1.Image = ((System.Drawing.Image)(resources.GetObject("updateError1.Image")));
			this.updateError1.Name = "updateError1";
			this.updateError1.ShowCloseButton = true;
			this.updateError1.ShowProgress = false;
			// 
			// noUpdateFound1
			// 
			resources.ApplyResources(this.noUpdateFound1, "noUpdateFound1");
			this.noUpdateFound1.Image = ((System.Drawing.Image)(resources.GetObject("noUpdateFound1.Image")));
			this.noUpdateFound1.Name = "noUpdateFound1";
			this.noUpdateFound1.ShowCloseButton = true;
			this.noUpdateFound1.ShowProgress = false;
			// 
			// slideComponent1
			// 
			this.slideComponent1.AlwaysSetLocation = false;
			this.slideComponent1.AttachedForm = this;
			this.slideComponent1.DirectX = FSLib.App.SimpleUpdater.Wrapper.SlideComponent.FlyXDirection.None;
			this.slideComponent1.MoveSpeedX = 0;
			this.slideComponent1.MoveSpeedY = 8;
			// 
			// MainWindow
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.panMain);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainWindow";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panMain;
		private System.Windows.Forms.Label label1;
		private UpdateControl.DownloadingInfo downloadingInfo1;
		private UpdateControl.RunUpdate runUpdate1;
		private UpdateControl.UpdateFinished updateFinished1;
		private UpdateControl.UpdateError updateError1;
		private UpdateControl.NoUpdateFound noUpdateFound1;
		private Wrapper.SlideComponent slideComponent1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}