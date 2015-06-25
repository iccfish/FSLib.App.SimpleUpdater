using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FSLib.App.SimpleUpdater.Generator.Defination;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using SimpleUpdater.Defination;

	class FileListView : System.Windows.Forms.ListView
	{
		private Dictionary<string, FileTreeItem> _files;
		Dictionary<string, Version> _versions;
		private System.Windows.Forms.ColumnHeader colDetectMethod;
		private System.Windows.Forms.ColumnHeader colExt;
		private System.Windows.Forms.ColumnHeader colIndex;
		private System.Windows.Forms.ColumnHeader colPath;
		private System.Windows.Forms.ColumnHeader colSize;
		private System.Windows.Forms.ColumnHeader colVersion;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imgs;
		private ColumnHeader colUpdateMethod;
		private ColumnHeader colFlag;
		private UpdateInfo _updateInfo;

		public FileListView()
		{
			if (!Program.Running) return;

			InitializeComponent();
			_versions = new Dictionary<string, Version>(StringComparer.OrdinalIgnoreCase);

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
		}

		/// <summary> 获得或设置当前的文件列表 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Dictionary<string, FileTreeItem> Files
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


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileListView));
			this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDetectMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colExt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imgs = new System.Windows.Forms.ImageList(this.components);
			this.colUpdateMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFlag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
			// colUpdateMethod
			// 
			this.colUpdateMethod.Text = "更新方式";
			// 
			// colFlag
			// 
			this.colFlag.Text = "组件标记";
			// 
			// FileListView
			// 
			this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colPath,
            this.colExt,
            this.colUpdateMethod,
            this.colDetectMethod,
            this.colFlag,
            this.colSize,
            this.colVersion});
			this.FullRowSelect = true;
			this.HideSelection = false;
			this.ShowGroups = false;
			this.ShowItemToolTips = true;
			this.SmallImageList = this.imgs;
			this.View = System.Windows.Forms.View.Details;
			this.ResumeLayout(false);

		}

		/// <summary>
		/// 将文件列表加入到显示列表中
		/// </summary>
		void LoadFilesIntoList()
		{
			Items.Clear();

			if (Files == null)
				return;

			//加载文件列表
			var index = 0;
			BeginUpdate();
			foreach (var item in Files)
			{
				//文件类型
				var ext = item.Value.FileInfo.Extension.Trim('.');
				var isKey = ext.Equals("exe", StringComparison.OrdinalIgnoreCase) || ext.Equals("dll", StringComparison.OrdinalIgnoreCase);


				var lv = new ListViewItem((++index).ToString(), isKey ? 0 : 1);

				lv.SubItems.Add(item.Value.FileInfo.Name);
				lv.SubItems.Add(ext);
				//更新方式
				lv.SubItems.Add("");
				//检测方式
				lv.SubItems.Add("");
				lv.SubItems.Add("");

				lv.SubItems.Add(Wrapper.ExtensionMethod.ToSizeDescription(item.Value.FileInfo.Length));

				lv.SubItems.Add(GetVersion(item.Key, item.Value.FileInfo.FullName).ToString());
				lv.Tag = item;

				Items.Add(lv);
				UpdateVerificationLevelDesc(lv);
				UpdateUpdateMethodDesc(lv);
				UpdateFlagDesc(lv);

				//挂载事件
				var treeItem = item.Value;
				treeItem.UpdateMethodChagned -= TreeItem_UpdateMethodChagned;
				treeItem.VerificationLevelChanged -= TreeItem_VerificationLevelChanged;
				treeItem.FlagChanged -= TreeItem_FlagChanged;

				treeItem.UpdateMethodChagned += TreeItem_UpdateMethodChagned;
				treeItem.VerificationLevelChanged += TreeItem_VerificationLevelChanged;
				treeItem.FlagChanged += TreeItem_FlagChanged;
			}
			EndUpdate();
			AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void TreeItem_FlagChanged(object sender, EventArgs e)
		{
			var item = Items.Cast<ListViewItem>().FirstOrDefault(s => ((KeyValuePair<string, FileTreeItem>)s.Tag).Value == sender);
			if (item == null)
				return;

			UpdateFlagDesc(item);
			AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void TreeItem_VerificationLevelChanged(object sender, EventArgs e)
		{
			var item = Items.Cast<ListViewItem>().FirstOrDefault(s => ((KeyValuePair<string, FileTreeItem>)s.Tag).Value == sender);
			if (item == null)
				return;

			UpdateVerificationLevelDesc(item);
			AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void TreeItem_UpdateMethodChagned(object sender, EventArgs e)
		{
			var item = Items.Cast<ListViewItem>().FirstOrDefault(s => ((KeyValuePair<string, FileTreeItem>)s.Tag).Value == sender);
			if (item == null)
				return;

			UpdateUpdateMethodDesc(item);
			AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}


		/// <summary> 更新指定项的检测方式 </summary>
		void UpdateVerificationLevelDesc(ListViewItem lvitem)
		{
			var item = (KeyValuePair<string, FileTreeItem>)lvitem.Tag;
			var um = item.Value.UpdateMethod;

			if (!Utility.HasMethod(um, UpdateMethod.VersionCompare))
			{
				lvitem.SubItems[4].Text = "--";
				return;
			}

			var fv = item.Value.VerificationLevel;
			var fvd = fv.ToDisplayString();

			lvitem.SubItems[4].Text = fvd.Trim(new char[] { ',', ' ' });
		}

		/// <summary> 更新指定项的检测方式 </summary>
		void UpdateUpdateMethodDesc(ListViewItem lvitem)
		{
			var item = (KeyValuePair<string, FileTreeItem>)lvitem.Tag;
			var um = item.Value.UpdateMethod;

			lvitem.SubItems[3].Text = um.ToDisplayString();
			UpdateVerificationLevelDesc(lvitem);
		}

		/// <summary> 更新指定项的检测方式 </summary>
		void UpdateFlagDesc(ListViewItem lvitem)
		{
			var item = (KeyValuePair<string, FileTreeItem>)lvitem.Tag;

			lvitem.SubItems[5].Text = item.Value.Flag;
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
