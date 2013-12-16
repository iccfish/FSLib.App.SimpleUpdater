namespace FSLib.App.SimpleUpdater.Generator.Dialogs
{
	partial class SelectVerificationLevel
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
			this.chkSize = new System.Windows.Forms.CheckBox();
			this.chkVersion = new System.Windows.Forms.CheckBox();
			this.chkContent = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// chkSize
			// 
			this.chkSize.AutoSize = true;
			this.chkSize.Location = new System.Drawing.Point(24, 21);
			this.chkSize.Name = "chkSize";
			this.chkSize.Size = new System.Drawing.Size(264, 16);
			this.chkSize.TabIndex = 0;
			this.chkSize.Text = "文件大小：当文件大小不一致时，即视为不同";
			this.chkSize.UseVisualStyleBackColor = true;
			// 
			// chkVersion
			// 
			this.chkVersion.AutoSize = true;
			this.chkVersion.Location = new System.Drawing.Point(24, 43);
			this.chkVersion.Name = "chkVersion";
			this.chkVersion.Size = new System.Drawing.Size(438, 16);
			this.chkVersion.TabIndex = 0;
			this.chkVersion.Text = "文件版本：当文件版本不同时，视为不同。仅对可执行文件或DLL等文件起效。";
			this.chkVersion.UseVisualStyleBackColor = true;
			// 
			// chkContent
			// 
			this.chkContent.AutoSize = true;
			this.chkContent.Location = new System.Drawing.Point(24, 65);
			this.chkContent.Name = "chkContent";
			this.chkContent.Size = new System.Drawing.Size(438, 28);
			this.chkContent.TabIndex = 0;
			this.chkContent.Text = "文件内容：使用MD5对文件内容进行对比，不同则视为不同。这种方法最严格。\r\n但是检测需要消耗时间，时间的多少视文件大小而不同，请谨慎选择。";
			this.chkContent.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkContent.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Image = global::FSLib.App.SimpleUpdater.Generator.Properties.Resources.block_16;
			this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCancel.Location = new System.Drawing.Point(374, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 31);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "取消(&C)";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Image = global::FSLib.App.SimpleUpdater.Generator.Properties.Resources.tick_16;
			this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOk.Location = new System.Drawing.Point(288, 104);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(80, 31);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "确定(&O)";
			this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// SelectVerificationLevel
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(482, 147);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.chkContent);
			this.Controls.Add(this.chkVersion);
			this.Controls.Add(this.chkSize);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectVerificationLevel";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "设置比较类型";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkSize;
		private System.Windows.Forms.CheckBox chkVersion;
		private System.Windows.Forms.CheckBox chkContent;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}