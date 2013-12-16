using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	public class FileListView : System.Windows.Forms.ListView
	{
		private Dictionary<string, FileInfo> _files;
		Dictionary<string, UpdateMethod> _updateMethods;
		Dictionary<string, FileVerificationLevel> _verifyLevels;
		Dictionary<string, Version> _versions;
		private System.Windows.Forms.ColumnHeader colDetectMethod;
		private System.Windows.Forms.ColumnHeader colExt;
		private System.Windows.Forms.ColumnHeader colIndex;
		private System.Windows.Forms.ColumnHeader colPath;
		private System.Windows.Forms.ColumnHeader colSize;
		private System.Windows.Forms.ColumnHeader colVersion;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imgs;
		private ContextMenuStrip itemCtxMenu;
		private ToolStripMenuItem tsUpdateMethodAlways;
		private ToolStripMenuItem tsUpdateMethodCompare;
		private ToolStripMenuItem tsUpdateMethodOnlyNotExist;
		private UpdateInfo _updateInfo;

		public FileListView()
		{
			if (!Program.Running) return;

			InitializeComponent();
			_versions = new Dictionary<string, Version>(StringComparer.OrdinalIgnoreCase);
			_verifyLevels = new Dictionary<string, FileVerificationLevel>();
			_updateMethods = new Dictionary<string, UpdateMethod>();

			var sorter = new FileListViewSorter()
			{
				Order = SortOrder.Ascending,
				SortColumn = 0,
				View = this
			};
			ColumnClick += (s, e) =>
			{
				if (e.Column == sorter.SortColumn)
				{
					if (sorter.Order == SortOrder.None) sorter.Order = SortOrder.Ascending;
					else sorter.Order = sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
				}
				else
				{
					sorter.SortColumn = e.Column;
				}
				Sort();
			};
			ListViewItemSorter = sorter;
			itemCtxMenu.Opening += (s, e) =>
			{
				if (this.SelectedItems.Count == 0)
				{
					e.Cancel = true;
					return;
				};

				//取同一个组
				tsUpdateMethodAlways.Checked = tsUpdateMethodCompare.Checked = tsUpdateMethodOnlyNotExist.Checked = false;

				var groups = SelectedItems.Cast<ListViewItem>().Select(m => m.Group).Distinct().ToArray();
				if (groups.Length > 1)
				{
					var group = groups[0];
					if (group == Groups[0]) tsUpdateMethodAlways.Checked = true;
					else if (group == Groups[1]) tsUpdateMethodCompare.Checked = true;
					else tsUpdateMethodOnlyNotExist.Checked = true;
				}
			};
			tsUpdateMethodOnlyNotExist.Click += (s, e) => SetSelectedItemUpdateMethod(UpdateMethod.SkipIfExists);
			tsUpdateMethodCompare.Click += (s, e) => SetSelectedItemUpdateMethod(UpdateMethod.VersionCompare);
			tsUpdateMethodAlways.Click += (s, e) => SetSelectedItemUpdateMethod(UpdateMethod.Always);
		}

		/// <summary> 获得或设置当前的文件列表 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Dictionary<string, FileInfo> Files
		{
			get { return _files; }
			set { _files = value; LoadFilesIntoList(); }
		}

		/// <summary>
		/// 
		/// </summary>
		public UpdateInfo UpdateInfo
		{
			get { return _updateInfo; }
			set
			{
				_updateInfo = value;
				if (value != null)
				{
				}
			}
		}

		/// <summary> 获得列表中指定文件的路径 </summary>
		/// <param name="pathkey" type="string">类型为 <see>System.String</see> 的参数</param>
		/// <returns></returns>
		public Version GetVersion(string pathkey, string fullPath)
		{
			if (_versions.ContainsKey(pathkey)) return _versions[pathkey];

			var ver = Wrapper.ExtensionMethod.ConvertVersionInfo(System.Diagnostics.FileVersionInfo.GetVersionInfo(fullPath));
			_versions.Add(pathkey, ver);

			return ver;
		}

		/// <summary> 获得更新方式 </summary>
		/// <param name="path" type="string">类型为 <see>string</see> 的参数</param>
		/// <returns></returns>
		public UpdateMethod GetFileUpdateMethod(string path)
		{
			if (_updateMethods.ContainsKey(path)) return _updateMethods[path];
			return UpdateMethod.Always;
		}

		/// <summary> 获得指定路径的文件更新类型 </summary>
		/// <param name="path" type="string">类型为 <see>string</see> 的参数</param>
		/// <returns></returns>
		public FileVerificationLevel GetFileVerificationLevel(string path)
		{
			return _verifyLevels.ContainsKey(path) ? _verifyLevels[path] : FileVerificationLevel.Size & FileVerificationLevel.Hash & FileVerificationLevel.Version;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileListView));
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("始终更新的文件", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("依赖于对比检测的文件", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("不存在时才更新", System.Windows.Forms.HorizontalAlignment.Left);
			this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDetectMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colExt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imgs = new System.Windows.Forms.ImageList(this.components);
			this.itemCtxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsUpdateMethodAlways = new System.Windows.Forms.ToolStripMenuItem();
			this.tsUpdateMethodCompare = new System.Windows.Forms.ToolStripMenuItem();
			this.tsUpdateMethodOnlyNotExist = new System.Windows.Forms.ToolStripMenuItem();
			this.itemCtxMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// colIndex
			// 
			this.colIndex.Text = "序号";
			// 
			// colPath
			// 
			this.colPath.Text = "文件路径";
			// 
			// colSize
			// 
			this.colSize.Text = "文件大小";
			// 
			// colVersion
			// 
			this.colVersion.Text = "版本";
			// 
			// colDetectMethod
			// 
			this.colDetectMethod.Text = "检测方式";
			// 
			// colExt
			// 
			this.colExt.Text = "扩展名";
			// 
			// imgs
			// 
			this.imgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgs.ImageStream")));
			this.imgs.TransparentColor = System.Drawing.Color.Transparent;
			this.imgs.Images.SetKeyName(0, "1111.png");
			this.imgs.Images.SetKeyName(1, "2222.png");
			// 
			// itemCtxMenu
			// 
			this.itemCtxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUpdateMethodAlways,
            this.tsUpdateMethodCompare,
            this.tsUpdateMethodOnlyNotExist});
			this.itemCtxMenu.Name = "itemCtxMenu";
			this.itemCtxMenu.Size = new System.Drawing.Size(185, 70);
			// 
			// tsUpdateMethodAlways
			// 
			this.tsUpdateMethodAlways.Name = "tsUpdateMethodAlways";
			this.tsUpdateMethodAlways.Size = new System.Drawing.Size(184, 22);
			this.tsUpdateMethodAlways.Text = "始终更新";
			// 
			// tsUpdateMethodCompare
			// 
			this.tsUpdateMethodCompare.Name = "tsUpdateMethodCompare";
			this.tsUpdateMethodCompare.Size = new System.Drawing.Size(184, 22);
			this.tsUpdateMethodCompare.Text = "按需更新...";
			// 
			// tsUpdateMethodOnlyNotExist
			// 
			this.tsUpdateMethodOnlyNotExist.Name = "tsUpdateMethodOnlyNotExist";
			this.tsUpdateMethodOnlyNotExist.Size = new System.Drawing.Size(184, 22);
			this.tsUpdateMethodOnlyNotExist.Text = "仅当不存在时才更新";
			// 
			// FileListView
			// 
			this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colPath,
            this.colExt,
            this.colDetectMethod,
            this.colSize,
            this.colVersion});
			this.ContextMenuStrip = this.itemCtxMenu;
			this.FullRowSelect = true;
			listViewGroup1.Header = "始终更新的文件";
			listViewGroup1.Name = "gpAlways";
			listViewGroup2.Header = "依赖于对比检测的文件";
			listViewGroup2.Name = "gpVersionCompare";
			listViewGroup3.Header = "不存在时才更新";
			listViewGroup3.Name = "gpExistAndSkip";
			this.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
			this.HideSelection = false;
			this.ShowItemToolTips = true;
			this.SmallImageList = this.imgs;
			this.View = System.Windows.Forms.View.Details;
			this.itemCtxMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		/// <summary>
		/// 将文件列表加入到显示列表中
		/// </summary>
		void LoadFilesIntoList()
		{
			Items.Clear();

			if (Files == null) return;

			//加载文件列表
			var index = 0;
			foreach (var item in Files)
			{
				//更新方式
				var um = GetFileUpdateMethod(item.Key);

				//文件类型
				var ext = item.Value.Extension.Trim('.');
				var isKey = ext.Equals("exe", StringComparison.OrdinalIgnoreCase) || ext.Equals("dll", StringComparison.OrdinalIgnoreCase);


				var lv = new ListViewItem((++index).ToString(), isKey ? 0 : 1);
				if (um == UpdateMethod.SkipIfExists)
					lv.Group = Groups[2];
				else
					lv.Group = Groups[0];
				if (um == UpdateMethod.VersionCompare) lv.Group = Groups[1];

				lv.SubItems.Add(item.Value.Name);
				lv.SubItems.Add(ext);
				//检测方式
				lv.SubItems.Add("");

				lv.SubItems.Add(Wrapper.ExtensionMethod.ToSizeDescription(item.Value.Length));

				lv.SubItems.Add(GetVersion(item.Key, item.Value.FullName).ToString());
				lv.Tag = item;

				Items.Add(lv);
				UpdateVerificationLevelDesc(lv);
			}
			AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		void SetSelectedItemUpdateMethod(UpdateMethod method)
		{
			var items = SelectedItems.Cast<ListViewItem>().Where(s => GetFileUpdateMethod(((KeyValuePair<string, FileInfo>)s.Tag).Key) != method).ToArray();
			if (items.Length == 0) return;

			var group = Groups[(int)method];
			var verifyLevel = FileVerificationLevel.Hash | FileVerificationLevel.Size | FileVerificationLevel.Version;
			if (method == UpdateMethod.VersionCompare)
			{
				var dlg = new Dialogs.SelectVerificationLevel() { FileVerificationLevel = verifyLevel };
				if (dlg.ShowDialog() != DialogResult.OK) return;

				verifyLevel = dlg.FileVerificationLevel;
				if ((verifyLevel & FileVerificationLevel.Version) != FileVerificationLevel.None
					&&
					items.Any(s => _versions[((KeyValuePair<string, FileInfo>)s.Tag).Key].IsIllegal())
					&&
					MessageBox.Show("您已经选择版本比较选项，但当前选择的文件中有一个或多个无法识别出有效的版本（主版本号为0），是否确认继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes
				)
				{
					return;
				}
			}

			foreach (var item in items)
			{
				var tag = (KeyValuePair<string, FileInfo>)item.Tag;
				if (GetFileUpdateMethod(tag.Key) == method) continue;

				item.Group = group;

				//处理更新方式
				if (method != UpdateMethod.VersionCompare && _verifyLevels.ContainsKey(tag.Key))
				{
					_verifyLevels.Remove(tag.Key);
				}
				if (method == UpdateMethod.Always)
				{
					if (_updateMethods.ContainsKey(tag.Key)) _updateMethods.Remove(tag.Key);
				}
				else
				{
					if (_updateMethods.ContainsKey(tag.Key)) _updateMethods[tag.Key] = method;
					else _updateMethods.Add(tag.Key, method);
				}

				if (method == UpdateMethod.VersionCompare)
				{
					if (_verifyLevels.ContainsKey(tag.Key)) _verifyLevels[tag.Key] = verifyLevel;
					else _verifyLevels.Add(tag.Key, verifyLevel);
				}

				UpdateVerificationLevelDesc(item);
			}

			AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}
		/// <summary> 更新指定项的检测方式 </summary>
		void UpdateVerificationLevelDesc(ListViewItem lvitem)
		{
			var item = (KeyValuePair<string, FileInfo>)lvitem.Tag;
			var um = GetFileUpdateMethod(item.Key);

			if (um != UpdateMethod.VersionCompare)
			{
				lvitem.SubItems[3].Text = "";
				return;
			}

			var fv = GetFileVerificationLevel(item.Key);
			var fvd = "";

			if ((fv & FileVerificationLevel.Version) == FileVerificationLevel.Version)
			{
				fvd = "版本";
			}
			if ((fv & FileVerificationLevel.Hash) == FileVerificationLevel.Hash)
			{
				fvd += ", MD5";
			}
			if ((fv & FileVerificationLevel.Size) == FileVerificationLevel.Size)
			{
				fvd += ", 大小";
			}

			lvitem.SubItems[3].Text = fvd.Trim(new char[] { ',', ' ' });
		}

		/// <summary>
		/// 获得是否有需要开启增量更新的文件
		/// </summary>
		public bool HasIncreaseUpdateFile
		{
			get { return _updateMethods.Count > 0; }
		}


		UpdateInfo _currentUpdateInfo;

		/// <summary>
		/// 获得或设置当前的升级信息
		/// </summary>
		public UpdateInfo CurrentUpdateInfo
		{
			get { return _currentUpdateInfo; }
			set
			{
				_currentUpdateInfo = value;
				if (value == null) return;

				//从列表中刷新更新方式
				_verifyLevels.Clear();
				_updateMethods.Clear();

				if (value.Packages != null)
				{
					value.Packages.Where(s => s.Method != UpdateMethod.Always)
						.ForEach(s =>
							{
								_updateMethods.Add(s.FilePath, s.Method);
								_verifyLevels.Add(s.FilePath, s.VerificationLevel);
							});
				}

				//重新加载
				LoadFilesIntoList();
			}
		}

	}

	internal class FileListViewSorter : System.Collections.IComparer
	{
		public SortOrder Order { get; set; }

		public int SortColumn { get; set; }

		public FileListView View { get; set; }
		#region Implementation of IComparer

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <returns>
		/// Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y. 
		/// </returns>
		/// <param name="y">The second object to compare. </param><param name="x">The first object to compare. </param><exception cref="T:System.ArgumentException">Neither x nor y implements the <see cref="T:System.IComparable"/> interface.-or- x and y are of different types and neither one can handle comparisons with the other. </exception><filterpriority>2</filterpriority>
		public int Compare(object x, object y)
		{
			if (Order == SortOrder.None) return 0;

			var lx = (ListViewItem)x;
			var ly = (ListViewItem)y;

			if (SortColumn == 0)
			{
				return (Order == SortOrder.Ascending ? 1 : -1) * (int.Parse(lx.SubItems[0].Text) - int.Parse(ly.SubItems[0].Text));
			}
			if (SortColumn == 4)
			{
				var sizex = ((KeyValuePair<string, FileInfo>)lx.Tag).Value.Length;
				var sizey = ((KeyValuePair<string, FileInfo>)ly.Tag).Value.Length;

				if (sizex > sizey) return Order == SortOrder.Ascending ? 1 : -1;
				if (sizex < sizey) return Order == SortOrder.Ascending ? -1 : 1;
				return 0;
			}
			if (SortColumn == 5)
			{
				//因为列表里的文件肯定都检测过版本，所以这里的取版本的函数第二个参数偷懒直接传递null了。
				var vx = View.GetVersion(((KeyValuePair<string, FileInfo>)lx.Tag).Key, null);
				var vy = View.GetVersion(((KeyValuePair<string, FileInfo>)ly.Tag).Key, null);

				if (vx > vy) return Order == SortOrder.Ascending ? 1 : -1;
				if (vx < vy) return Order == SortOrder.Ascending ? -1 : 1;
				return 0;
			}

			return string.Compare(lx.SubItems[SortColumn].Text, ly.SubItems[SortColumn].Text, true) * (Order == SortOrder.Ascending ? 1 : -1);
		}

		#endregion
	}
}
