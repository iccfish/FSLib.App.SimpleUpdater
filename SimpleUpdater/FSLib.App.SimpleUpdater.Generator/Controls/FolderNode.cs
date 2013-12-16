using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	class FolderNode : System.Windows.Forms.TreeNode
	{
		public FolderNode(string path, string root)
		{
			_path = path;
			_root = root;
			Text = System.IO.Path.GetFileName(path);
			ImageKey = SelectedImageKey = "folder";

			var di = new System.IO.DirectoryInfo(path);
			try
			{
				var dics = di.GetDirectories();
				dics.ForEach(s => Nodes.Add(new FolderNode(s.FullName, root)));
			}
			catch (Exception)
			{
				return;
			}

			_files = di.GetFiles().ToDictionary(s => s.FullName.Remove(0, root.Length).Trim('\\'), StringComparer.OrdinalIgnoreCase);
		}

		string _path;
		string _root;

		private Dictionary<string, FileInfo> _files;

		/// <summary> 获得当前文件夹的文件 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Dictionary<string, FileInfo> Files
		{
			get { return _files; }
		}

		/// <summary>
		/// 获得当前节点下的所有文件信息
		/// </summary>
		public virtual KeyValuePair<string,FileInfo>[] AllFiles
		{
			get
			{
				return Files.AsEnumerable().Concat(Nodes.Cast<FolderNode>().SelectMany(s=>s.AllFiles)).ToArray();
			}
		}
	}
}
