namespace FSLib.App.SimpleUpdater.UpdateControl
{
	partial class RunUpdate
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblDesc = new System.Windows.Forms.Label();
			this.lblProgressDesc = new System.Windows.Forms.Label();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.Info;
			this.pictureBox1.Location = new System.Drawing.Point(3, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(24, 24);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// lblDesc
			// 
			this.lblDesc.Location = new System.Drawing.Point(33, 12);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(161, 46);
			this.lblDesc.TabIndex = 1;
			this.lblDesc.Text = "正在更新...";
			// 
			// lblProgressDesc
			// 
			this.lblProgressDesc.AutoSize = true;
			this.lblProgressDesc.Location = new System.Drawing.Point(3, 62);
			this.lblProgressDesc.Name = "lblProgressDesc";
			this.lblProgressDesc.Size = new System.Drawing.Size(0, 12);
			this.lblProgressDesc.TabIndex = 2;
			// 
			// pbProgress
			// 
			this.pbProgress.Location = new System.Drawing.Point(3, 74);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(206, 13);
			this.pbProgress.TabIndex = 3;
			// 
			// RunUpdate
			// 
			this.Controls.Add(this.pbProgress);
			this.Controls.Add(this.lblProgressDesc);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.pictureBox1);
			this.Name = "RunUpdate";
			this.Size = new System.Drawing.Size(212, 88);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblProgressDesc;
		private System.Windows.Forms.ProgressBar pbProgress;
	}
}
