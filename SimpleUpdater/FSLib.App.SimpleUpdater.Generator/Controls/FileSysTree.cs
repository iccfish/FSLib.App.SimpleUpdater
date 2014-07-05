using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using Defination;

	public class FileSysTree : System.Windows.Forms.TreeView
	{
		private System.Windows.Forms.ImageList imgList;
		private System.ComponentModel.IContainer components;

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSysTree));
			this.imgList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imgList
			// 
			this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
			this.imgList.TransparentColor = System.Drawing.Color.Transparent;
			this.imgList.Images.SetKeyName(0, "home");
			this.imgList.Images.SetKeyName(1, "folder");
			// 
			// FileSysTree
			// 
			this.ImageIndex = 0;
			this.ImageList = this.imgList;
			this.LineColor = System.Drawing.Color.Black;
			this.SelectedImageIndex = 0;
			this.ResumeLayout(false);
		}

		void BindProject(AuProject project)
		{
			Root = project.ParseFullPath(project.DestinationDirectory);
			project.PropertyChanged += (s, ee) =>
			{
				if (ee.PropertyName == "ApplicationDirectory")
					Root = project.ParseFullPath(project.ApplicationDirectory);
			};
			Root = project.ParseFullPath(project.ApplicationDirectory);
		}

		public FileSysTree()
		{
			if (Program.Running)
			{
				InitializeComponent();
				HideSelection = false;

				//绑定
				UpdatePackageBuilder.Instance.ProjectLoaded += (s, e) =>
				{
					BindProject(e.AuProject);
				};
				if (UpdatePackageBuilder.Instance.AuProject != null)
					BindProject(UpdatePackageBuilder.Instance.AuProject);
			}
		}

		string _root;

		/// <summary> 获得或设置当前浏览的根路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string Root
		{
			get { return _root; }
			set
			{
				if (value == _root) return;

				_root = value;
				if (string.IsNullOrEmpty(_root))
				{
					Nodes.Clear();
					SelectedNode = Nodes.Add("none", "请选择程序文件夹...");

				}
				else
				{
					Nodes.Clear();
					var node = new RootNode(value);
					Nodes.Add(node);
					SelectedNode = node;
				}

				ExpandAll();
			}
		}
	}
}
