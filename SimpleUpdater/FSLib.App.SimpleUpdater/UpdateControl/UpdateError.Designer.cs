namespace FSLib.App.SimpleUpdater.UpdateControl
{
	partial class UpdateError
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
			this.label1 = new System.Windows.Forms.Label();
			this.lblDesc = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.Warning;
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(24, 24);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(33, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(161, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "更新发生错误，请稍后重试";
			// 
			// lblDesc
			// 
			this.lblDesc.Location = new System.Drawing.Point(9, 33);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(185, 35);
			this.lblDesc.TabIndex = 2;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(73, 59);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(54, 26);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// UpdateError
			// 
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Name = "UpdateError";
			this.Size = new System.Drawing.Size(212, 88);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Button btnClose;
	}
}
