namespace FSLib.App.SimpleUpdater.Generator.Dialogs
{
	partial class PackageGenerateResult
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageGenerateResult));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.list = new System.Windows.Forms.ListView();
			this.colPackageName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPackageSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imgList = new System.Windows.Forms.ImageList(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.txtMd5 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtVersion = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Generator.Properties.Resources.accept;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(76, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(209, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "包文件已生成完成，这里是生成结果。";
			// 
			// list
			// 
			this.list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPackageName,
            this.colPackageSize,
            this.colDescription});
			this.list.FullRowSelect = true;
			this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.list.HideSelection = false;
			this.list.Location = new System.Drawing.Point(12, 69);
			this.list.MultiSelect = false;
			this.list.Name = "list";
			this.list.Size = new System.Drawing.Size(658, 344);
			this.list.SmallImageList = this.imgList;
			this.list.TabIndex = 2;
			this.list.UseCompatibleStateImageBehavior = false;
			this.list.View = System.Windows.Forms.View.Details;
			// 
			// colPackageName
			// 
			this.colPackageName.Text = "包名";
			// 
			// colPackageSize
			// 
			this.colPackageSize.Text = "大小";
			// 
			// colDescription
			// 
			this.colDescription.Text = "描述信息";
			// 
			// imgList
			// 
			this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
			this.imgList.TransparentColor = System.Drawing.Color.Transparent;
			this.imgList.Images.SetKeyName(0, "未命名-1.png");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(350, 422);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "MD5";
			// 
			// txtMd5
			// 
			this.txtMd5.Location = new System.Drawing.Point(379, 419);
			this.txtMd5.Name = "txtMd5";
			this.txtMd5.ReadOnly = true;
			this.txtMd5.Size = new System.Drawing.Size(291, 21);
			this.txtMd5.TabIndex = 4;
			this.txtMd5.Text = "选定一个包文件来查看MD5";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 422);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 12);
			this.label3.TabIndex = 3;
			this.label3.Text = "版本";
			// 
			// txtVersion
			// 
			this.txtVersion.Location = new System.Drawing.Point(43, 419);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.ReadOnly = true;
			this.txtVersion.Size = new System.Drawing.Size(291, 21);
			this.txtVersion.TabIndex = 4;
			this.txtVersion.Text = "选定一个包文件来查看包含文件的版本";
			// 
			// PackageGenerateResult
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(686, 443);
			this.Controls.Add(this.txtVersion);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtMd5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.list);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PackageGenerateResult";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "升级包生成结果";
			this.Load += new System.EventHandler(this.PackageGenerateResult_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView list;
		private System.Windows.Forms.ColumnHeader colPackageName;
		private System.Windows.Forms.ColumnHeader colPackageSize;
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMd5;
		private System.Windows.Forms.ImageList imgList;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtVersion;
	}
}