using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using SimpleUpdater.Defination;

	public partial class FileConfiguration : UserControl
	{
		private string _newVersionFolder;

		public FileConfiguration()
		{
			InitializeComponent();

			folderTree.NodeMouseClick += (s, e) =>
			{
				if (e.Node == _selectedNode || !(e.Node is FolderNode)) return;
				_selectedNode = e.Node as FolderNode;
				filelist.Files = _selectedNode.Files;
			};

			Load += FileConfiguration_Load;
		}

		void FileConfiguration_Load(object sender, EventArgs e)
		{
		}

		FolderNode _selectedNode;

		/// <summary> 获得或设置最新版应用软件的目录 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string NewVersionFolder
		{
			get { return _newVersionFolder; }
			set
			{
				if (value == _newVersionFolder) return;
				_newVersionFolder = value;

				this.Enabled = !string.IsNullOrEmpty(value);
				folderTree.Root = value;

				var tn = folderTree.SelectedNode as FolderNode;
				if (tn != null) filelist.Files = tn.Files;
			}
		}

		public UpdateMethod GetFileUpdateMethod(string path)
		{
			return filelist.GetFileUpdateMethod(path);
		}

		public FileVerificationLevel GetFileVerificationLevel(string path)
		{
			return filelist.GetFileVerificationLevel(path);
		}

		/// <summary>
		/// 获得指定文件的版本
		/// </summary>
		/// <param name="pathKey">相对路径</param>
		/// <param name="fullPath">完整路径</param>
		/// <returns></returns>
		public Version GetFileVersion(string pathKey, string fullPath)
		{
			return filelist.GetVersion(pathKey, fullPath);
		}

		/// <summary>
		/// 获得所有文件
		/// </summary>
		public KeyValuePair<string, FileInfo>[] AllFiles
		{
			get
			{
				return (folderTree.Nodes[0] as RootNode).AllFiles;
			}
		}


		/// <summary>
		/// 获得是否有需要开启增量更新的文件
		/// </summary>
		public bool HasIncreaseUpdateFile
		{
			get { return filelist.HasIncreaseUpdateFile; }
		}

	}
}
