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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileConfiguration));
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.folderTree = new FSLib.App.SimpleUpdater.Generator.Controls.FileSysTree();
			this.filelist = new FSLib.App.SimpleUpdater.Generator.Controls.FileListView();
			this.tsv = new System.Windows.Forms.ToolStrip();
			this.toolStripButton12 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsvAsProject = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.tsvSize = new System.Windows.Forms.ToolStripButton();
			this.tsvVersion = new System.Windows.Forms.ToolStripButton();
			this.tsvMd5 = new System.Windows.Forms.ToolStripButton();
			this.tst = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tstAsProject = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tstIgnore = new System.Windows.Forms.ToolStripButton();
			this.tstAlways = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tstSkipNotExists = new System.Windows.Forms.ToolStripButton();
			this.tstSkipExists = new System.Windows.Forms.ToolStripButton();
			this.tstVersion = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.tsflag = new System.Windows.Forms.ToolStripComboBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tsv.SuspendLayout();
			this.tst.SuspendLayout();
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
			this.splitContainer1.Panel2.Controls.Add(this.tsv);
			this.splitContainer1.Panel2.Controls.Add(this.tst);
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
			this.filelist.Location = new System.Drawing.Point(0, 50);
			this.filelist.Name = "filelist";
			this.filelist.Size = new System.Drawing.Size(567, 292);
			this.filelist.TabIndex = 0;
			this.filelist.UpdateInfo = null;
			this.filelist.UseCompatibleStateImageBehavior = false;
			// 
			// tsv
			// 
			this.tsv.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripSeparator2,
            this.tsvAsProject,
            this.toolStripSeparator5,
            this.tsvSize,
            this.tsvVersion,
            this.tsvMd5,
            this.toolStripSeparator7,
            this.toolStripButton12,
            this.tsflag});
			this.tsv.Location = new System.Drawing.Point(0, 25);
			this.tsv.Name = "tsv";
			this.tsv.Size = new System.Drawing.Size(567, 25);
			this.tsv.TabIndex = 2;
			this.tsv.Text = "toolStrip2";
			// 
			// toolStripButton12
			// 
			this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton12.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
			this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton12.Name = "toolStripButton12";
			this.toolStripButton12.Size = new System.Drawing.Size(56, 22);
			this.toolStripButton12.Text = "组件标记";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tsvAsProject
			// 
			this.tsvAsProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsvAsProject.Image = ((System.Drawing.Image)(resources.GetObject("tsvAsProject.Image")));
			this.tsvAsProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsvAsProject.Name = "tsvAsProject";
			this.tsvAsProject.Size = new System.Drawing.Size(60, 22);
			this.tsvAsProject.Text = "跟随项目";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// tsvSize
			// 
			this.tsvSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsvSize.Image = ((System.Drawing.Image)(resources.GetObject("tsvSize.Image")));
			this.tsvSize.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsvSize.Name = "tsvSize";
			this.tsvSize.Size = new System.Drawing.Size(36, 22);
			this.tsvSize.Text = "大小";
			// 
			// tsvVersion
			// 
			this.tsvVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsvVersion.Image = ((System.Drawing.Image)(resources.GetObject("tsvVersion.Image")));
			this.tsvVersion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsvVersion.Name = "tsvVersion";
			this.tsvVersion.Size = new System.Drawing.Size(36, 22);
			this.tsvVersion.Text = "版本";
			// 
			// tsvMd5
			// 
			this.tsvMd5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsvMd5.Image = ((System.Drawing.Image)(resources.GetObject("tsvMd5.Image")));
			this.tsvMd5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsvMd5.Name = "tsvMd5";
			this.tsvMd5.Size = new System.Drawing.Size(72, 22);
			this.tsvMd5.Text = "内容(MD5)";
			// 
			// tst
			// 
			this.tst.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tst.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator3,
            this.tstAsProject,
            this.toolStripSeparator4,
            this.tstIgnore,
            this.tstAlways,
            this.toolStripSeparator1,
            this.tstSkipNotExists,
            this.tstSkipExists,
            this.toolStripSeparator6,
            this.tstVersion});
			this.tst.Location = new System.Drawing.Point(0, 0);
			this.tst.Name = "tst";
			this.tst.Size = new System.Drawing.Size(567, 25);
			this.tst.TabIndex = 1;
			this.tst.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(80, 22);
			this.toolStripLabel1.Text = "文件更新方式";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// tstAsProject
			// 
			this.tstAsProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tstAsProject.Image = ((System.Drawing.Image)(resources.GetObject("tstAsProject.Image")));
			this.tstAsProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tstAsProject.Name = "tstAsProject";
			this.tstAsProject.Size = new System.Drawing.Size(60, 22);
			this.tstAsProject.Text = "跟随项目";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// tstIgnore
			// 
			this.tstIgnore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tstIgnore.Image = ((System.Drawing.Image)(resources.GetObject("tstIgnore.Image")));
			this.tstIgnore.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tstIgnore.Name = "tstIgnore";
			this.tstIgnore.Size = new System.Drawing.Size(36, 22);
			this.tstIgnore.Text = "忽略";
			// 
			// tstAlways
			// 
			this.tstAlways.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tstAlways.Image = ((System.Drawing.Image)(resources.GetObject("tstAlways.Image")));
			this.tstAlways.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tstAlways.Name = "tstAlways";
			this.tstAlways.Size = new System.Drawing.Size(60, 22);
			this.tstAlways.Text = "始终更新";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tstSkipNotExists
			// 
			this.tstSkipNotExists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tstSkipNotExists.Image = ((System.Drawing.Image)(resources.GetObject("tstSkipNotExists.Image")));
			this.tstSkipNotExists.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tstSkipNotExists.Name = "tstSkipNotExists";
			this.tstSkipNotExists.Size = new System.Drawing.Size(84, 22);
			this.tstSkipNotExists.Text = "不存在则跳过";
			// 
			// tstSkipExists
			// 
			this.tstSkipExists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tstSkipExists.Image = ((System.Drawing.Image)(resources.GetObject("tstSkipExists.Image")));
			this.tstSkipExists.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tstSkipExists.Name = "tstSkipExists";
			this.tstSkipExists.Size = new System.Drawing.Size(72, 22);
			this.tstSkipExists.Text = "存在则跳过";
			// 
			// tstVersion
			// 
			this.tstVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tstVersion.Image = ((System.Drawing.Image)(resources.GetObject("tstVersion.Image")));
			this.tstVersion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tstVersion.Name = "tstVersion";
			this.tstVersion.Size = new System.Drawing.Size(60, 22);
			this.tstVersion.Text = "比较版本";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripLabel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.toolStripLabel2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLabel2.Image")));
			this.toolStripLabel2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(80, 22);
			this.toolStripLabel2.Text = "版本比较方式";
			// 
			// tsflag
			// 
			this.tsflag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tsflag.Name = "tsflag";
			this.tsflag.Size = new System.Drawing.Size(121, 25);
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
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.tsv.ResumeLayout(false);
			this.tsv.PerformLayout();
			this.tst.ResumeLayout(false);
			this.tst.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private FileSysTree folderTree;
		private FileListView filelist;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStrip tsv;
		private System.Windows.Forms.ToolStripButton tsvSize;
		private System.Windows.Forms.ToolStripButton tsvVersion;
		private System.Windows.Forms.ToolStripButton tsvMd5;
		private System.Windows.Forms.ToolStrip tst;
		private System.Windows.Forms.ToolStripButton tstAlways;
		private System.Windows.Forms.ToolStripButton tstSkipExists;
		private System.Windows.Forms.ToolStripButton tstIgnore;
		private System.Windows.Forms.ToolStripButton tstVersion;
		private System.Windows.Forms.ToolStripLabel toolStripButton12;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tstSkipNotExists;
		private System.Windows.Forms.ToolStripButton tstAsProject;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton tsvAsProject;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripComboBox tsflag;
	}
}
