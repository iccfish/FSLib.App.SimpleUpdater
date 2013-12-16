namespace FSLib.App.SimpleUpdater.UpdateControl
{
	partial class DownloadingInfo
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.lblDesc = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblDesc
			// 
			this.lblDesc.Location = new System.Drawing.Point(28, 31);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(172, 16);
			this.lblDesc.TabIndex = 3;
			this.lblDesc.Text = "正在获得更新信息，请稍等...";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.Info;
			this.pictureBox1.Location = new System.Drawing.Point(3, 26);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(24, 24);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 67);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(162, 17);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 4;
			// 
			// DownloadingInfo
			// 
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.pictureBox1);
			this.Name = "DownloadingInfo";
			this.Size = new System.Drawing.Size(200, 98);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ProgressBar progressBar1;
	}
}
