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
	using FSLib.App.SimpleUpdater.Generator.Defination;
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
			filelist.SelectedIndexChanged += Filelist_SelectedIndexChanged;
			InitVersionEditor();
		}

		FileTreeItem[] _items;

		private void Filelist_SelectedIndexChanged(object sender, EventArgs e)
		{
			_items = filelist.SelectedItems.Cast<ListViewItem>().Select(s => ((KeyValuePair<string, FileTreeItem>)s.Tag).Value).ToArray();
			RefreshStatus();
			RefreshItemFlags();
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


		#region 编辑

		void InitVersionEditor()
		{
			tst.Enabled = tsv.Enabled = false;
			tstAsProject.Click += (s, e) => Array.ForEach(_items, _ => _.UpdateMethod = UpdateMethod.AsProject);
			tstIgnore.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.UpdateMethod = UpdateMethod.Ignore;
				});
			};
			tstAlways.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.UpdateMethod = UpdateMethod.Always;
				});
			};
			tstSkipNotExists.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.UpdateMethod = Utility.SetOrClearUpdateMethodFlag(_.UpdateMethod & (~UpdateMethod.Always) & (~UpdateMethod.Ignore), UpdateMethod.SkipIfNotExist, tstSkipNotExists.Checked);
				});
			};
			tstSkipExists.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.UpdateMethod = Utility.SetOrClearUpdateMethodFlag(_.UpdateMethod & (~UpdateMethod.Always) & (~UpdateMethod.Ignore) & (~UpdateMethod.VersionCompare), UpdateMethod.SkipIfExists, tstSkipExists.Checked);
				});
			};
			tstVersion.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.UpdateMethod = Utility.SetOrClearUpdateMethodFlag(_.UpdateMethod & (~UpdateMethod.Always) & (~UpdateMethod.Ignore) & (~UpdateMethod.SkipIfExists), UpdateMethod.VersionCompare, tstVersion.Checked);
				});
			};
			tsvAsProject.Click += (s, e) => Array.ForEach(_items, _ => _.VerificationLevel = FileVerificationLevel.None);
			tsvMd5.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.VerificationLevel = Utility.SetOrClearUpdateMethodFlag(_.VerificationLevel, FileVerificationLevel.Hash, tsvMd5.Checked);
				});
			};
			tsvSize.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.VerificationLevel = Utility.SetOrClearUpdateMethodFlag(_.VerificationLevel, FileVerificationLevel.Size, tsvSize.Checked);
				});
			};
			tsvVersion.Click += (s, e) =>
			{
				Array.ForEach(_items, _ =>
				{
					_.VerificationLevel = Utility.SetOrClearUpdateMethodFlag(_.VerificationLevel, FileVerificationLevel.Version, tsvVersion.Checked);
				});
			};
			foreach (var button in tst.Items.OfType<ToolStripButton>().Union(tsv.Items.OfType<ToolStripButton>()))
			{
				button.CheckOnClick = true;
				button.Click += (s, e) => RefreshStatus();
			}
			UpdatePackageBuilder.Instance.ProjectLoaded += (s, e) =>
			{
				BindCompFlags();
			};
			BindCompFlags();

			tsflag.SelectedIndexChanged += (s, e) =>
			{
				var index = tsflag.SelectedIndex;
				if (index == -1 || index == 1)
				{
					tsflag.SelectedIndex = 0;
					return;
				}

				var flag = tsflag.SelectedIndex == 0 ? null : tsflag.SelectedItem.ToString();
				Array.ForEach(_items, _ => _.Flag = flag);
				RefreshItemFlags();
			};
		}

		void BindCompFlags()
		{
			var proj = UpdatePackageBuilder.Instance.AuProject;
			if (proj == null)
				return;

			proj.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "ComponentFlags")
				{
					RefreshFlags();
				}
			};
			RefreshFlags();
		}

		void RefreshFlags()
		{
			tsflag.Items.Clear();
			var proj = UpdatePackageBuilder.Instance.AuProject;
			if (proj == null)
				return;

			var items = proj.ComponentFlags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
			tsflag.Items.Add("<没有选择>");
			tsflag.Items.Add("<选择了多个项>");
			tsflag.Items.AddRange(items.Select(s => (object)s).ToArray());
		}

		void RefreshItemFlags()
		{
			var flags = _items.Select(s => s.Flag).Where(s => !s.IsNullOrEmpty()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
			if (flags.Length == 0)
				tsflag.SelectedIndex = 0;
			else if (flags.Length > 1)
				tsflag.SelectedIndex = 1;
			else
			{
				for (int i = 2; i < tsflag.Items.Count; i++)
				{
					if (tsflag.Items[i].ToString() == flags[0])
					{
						tsflag.SelectedIndex = i;
						break;
					}
				}
			}
		}

		void RefreshStatus()
		{
			if (_items == null || _items.Length == 0)
			{
				tst.Enabled = tsv.Enabled = false;
				return;
			}
			tst.Enabled = true;

			//跟随项目
			var f1 = _items.Select(s => s.UpdateMethod == UpdateMethod.AsProject).Distinct().ToArray();
			if (f1.Length > 1)
			{
				tstAsProject.CheckState = CheckState.Indeterminate;
			}
			else
			{
				tstAsProject.Checked = f1[0];
			}
			//忽略
			f1 = _items.Select(s => Utility.HasMethod(s.UpdateMethod, UpdateMethod.Ignore)).Distinct().ToArray();
			if (f1.Length > 1)
			{
				tstIgnore.CheckState = CheckState.Indeterminate;
			}
			else
			{
				tstIgnore.Checked = f1[0];
			}
			//始终
			f1 = _items.Select(s => Utility.HasMethod(s.UpdateMethod, UpdateMethod.Always)).Distinct().ToArray();
			if (f1.Length > 1)
			{
				tstAlways.CheckState = CheckState.Indeterminate;
			}
			else
			{
				tstAlways.Checked = f1[0];
			}
			//不存在跳过
			f1 = _items.Select(s => Utility.HasMethod(s.UpdateMethod, UpdateMethod.SkipIfNotExist)).Distinct().ToArray();
			if (f1.Length > 1)
			{
				tstSkipNotExists.CheckState = CheckState.Indeterminate;
			}
			else
			{
				tstSkipNotExists.Checked = f1[0];
			}
			//存在跳过
			f1 = _items.Select(s => Utility.HasMethod(s.UpdateMethod, UpdateMethod.SkipIfExists)).Distinct().ToArray();
			if (f1.Length > 1)
			{
				tstSkipExists.CheckState = CheckState.Indeterminate;
			}
			else
			{
				tstSkipExists.Checked = f1[0];
			}
			//版本更新
			f1 = _items.Select(s => Utility.HasMethod(s.UpdateMethod, UpdateMethod.VersionCompare)).Distinct().ToArray();
			if (f1.Length > 1)
			{
				tstVersion.CheckState = CheckState.Indeterminate;
			}
			else
			{
				tstVersion.Checked = f1[0];
			}

			//比较方式
			tsv.Enabled = tstVersion.CheckState != CheckState.Unchecked;
			f1 = _items.Select(s => s.VerificationLevel == FileVerificationLevel.None).Distinct().ToArray();
			tsvAsProject.CheckState = f1.Length > 1 ? CheckState.Unchecked : f1[0] ? CheckState.Checked : CheckState.Unchecked;
			f1 = _items.Select(s => Utility.HasMethod(s.VerificationLevel, FileVerificationLevel.Hash)).Distinct().ToArray();
			tsvMd5.CheckState = f1.Length > 1 ? CheckState.Unchecked : f1[0] ? CheckState.Checked : CheckState.Unchecked;
			f1 = _items.Select(s => Utility.HasMethod(s.VerificationLevel, FileVerificationLevel.Version)).Distinct().ToArray();
			tsvVersion.CheckState = f1.Length > 1 ? CheckState.Unchecked : f1[0] ? CheckState.Checked : CheckState.Unchecked;
			f1 = _items.Select(s => Utility.HasMethod(s.VerificationLevel, FileVerificationLevel.Size)).Distinct().ToArray();
			tsvSize.CheckState = f1.Length > 1 ? CheckState.Unchecked : f1[0] ? CheckState.Checked : CheckState.Unchecked;
		}

		#endregion
	}
}
