using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using FSLib.App.SimpleUpdater.Defination;
	using FSLib.App.SimpleUpdater.Generator.Defination;

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

			var proj = UpdatePackageBuilder.Instance.AuProject;
			_files = di.GetFiles()
						.Select(s =>
						{
							var rp = s.FullName.Remove(0, root.Length).Trim('\\');
							var pi = proj.FindProjectItem(rp);

							return new FileTreeItem(s, pi == null ? UpdateMethod.AsProject : pi.UpdateMethod, pi == null ? FileVerificationLevel.None : pi.FileVerificationLevel, rp, pi == null ? null : pi.Flag);
						})
						.ToDictionary(
									 s => s.RelativePath
				);
		}

		string _path;
		string _root;

		private Dictionary<string, FileTreeItem> _files;

		/// <summary> 获得当前文件夹的文件 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Dictionary<string, FileTreeItem> Files
		{
			get { return _files; }
		}

		/// <summary>
		/// 获得当前节点下的所有文件信息
		/// </summary>
		public virtual KeyValuePair<string, FileTreeItem>[] AllFiles
		{
			get
			{
				return Files.AsEnumerable().Concat(Nodes.Cast<FolderNode>().SelectMany(s => s.AllFiles)).ToArray();
			}
		}
	}
}
