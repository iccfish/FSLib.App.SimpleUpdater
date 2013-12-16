namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	partial class FileConfiguration
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

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.folderTree = new FSLib.App.SimpleUpdater.Generator.Controls.FileSysTree();
			this.filelist = new FSLib.App.SimpleUpdater.Generator.Controls.FileListView();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 342);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(732, 25);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoEllipsis = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Image = global::FSLib.App.SimpleUpdater.Generator.Properties.Resources.info_16;
			this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(732, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "   请谨慎选择更新方式，尤其是当存在更新前和更新后自定义操作的时候";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.splitContainer1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(732, 342);
			this.panel2.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.folderTree);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.filelist);
			this.splitContainer1.Size = new System.Drawing.Size(732, 342);
			this.splitContainer1.SplitterDistance = 161;
			this.splitContainer1.TabIndex = 0;
			// 
			// folderTree
			// 
			this.folderTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.folderTree.Location = new System.Drawing.Point(0, 0);
			this.folderTree.Name = "folderTree";
			this.folderTree.Root = null;
			this.folderTree.Size = new System.Drawing.Size(161, 342);
			this.folderTree.TabIndex = 0;
			// 
			// filelist
			// 
			this.filelist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filelist.Files = null;
			this.filelist.Location = new System.Drawing.Point(0, 0);
			this.filelist.Name = "filelist";
			this.filelist.Size = new System.Drawing.Size(567, 342);
			this.filelist.TabIndex = 0;
			this.filelist.UseCompatibleStateImageBehavior = false;
			// 
			// FileConfiguration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "FileConfiguration";
			this.Size = new System.Drawing.Size(732, 367);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private FileSysTree folderTree;
		private FileListView filelist;
		private System.Windows.Forms.Label label1;
	}
}
