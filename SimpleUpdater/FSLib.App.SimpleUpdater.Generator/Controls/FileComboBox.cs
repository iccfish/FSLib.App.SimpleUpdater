using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using SimpleUpdater.Annotations;

	public class FileComboBox : ComboBox, INotifyPropertyChanged
	{
		public FileComboBox()
		{
			this.DropDownStyle = ComboBoxStyle.DropDownList;

			this.SizeChanged += (object sender, EventArgs e) => this.Size = new Size(this.Width, 20);
			SelectedIndexChanged += (s, e) => OnPropertyChanged("SelectedFileName");
		}

		/// <summary>
		/// 获得或设置优先选择的文件名
		/// </summary>
		public string PreferFileName
		{
			get { return _preferFileName; }
			set
			{
				_preferFileName = value;
				if (this.Items.Count == 0) return;

				if (string.IsNullOrEmpty(value)) SelectedIndex = 0;
				else
				{
					for (int i = 0; i < Items.Count; i++)
					{
						if (string.Compare(Items[i].ToString(), value, true) == 0)
						{
							SelectedIndex = i;
							break;
						}
					}
				}
			}
		}

		string _rootPath;

		/// <summary>
		/// 绑定到目录
		/// </summary>
		[Description("将文件列表绑定到目录")]
		public string RootPath
		{
			get { return _rootPath; }
			set
			{
				if (_rootPath == value || string.IsNullOrEmpty(value)) return;
				_rootPath = value.EnsureEndWith(@"\");
				LoadFiles();
				OnPropertyChanged("RootPath");
			}
		}

		private string[] _filetypefilter;

		/// <summary>
		/// 允许拖放的文件类型(多个之间用|隔开)
		/// </summary>
		[Description("显示的文件类型(多个之间用 ',' 隔开)")]
		public string FileTypeFilter
		{
			get
			{
				return _filetypefilter == null ? string.Empty : string.Join(",", _filetypefilter);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					_filetypefilter = null;
				}
				else
				{
					_filetypefilter = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					if (_filetypefilter.Length == 0) _filetypefilter = null;
				}
				_fileCheckReg = null;
				LoadFiles();
			}
		}

		Regex _fileCheckReg;
		string _fullpath;

		/// <summary>
		/// 加载文件列表
		/// </summary>
		protected void LoadFiles()
		{
			this.Items.Clear();
			if (ShowEmptyEntry) this.Items.Add("<未选择>");

			if (string.IsNullOrEmpty(RootPath))
				return;

			_fullpath = UpdatePackageBuilder.Instance.AuProject.ParseFullPath(RootPath);
			if (!System.IO.Directory.Exists(_fullpath))
			{
				return;
			}

			var files = System.IO.Directory.GetFiles(_fullpath, "*.*", System.IO.SearchOption.AllDirectories).AsEnumerable();
			var selectedIndex = 0;

			if (_fileCheckReg == null && _filetypefilter != null) _fileCheckReg = new Regex(@"^.*\.(" + string.Join("|", _filetypefilter) + ")$", RegexOptions.IgnoreCase);
			if (_fileCheckReg != null) files = files.Where(s => _fileCheckReg.IsMatch(System.IO.Path.GetFileName(s)));

			files.ForEach(s =>
			{
				var path = s.Remove(0, _fullpath.Length);
				Items.Add(path);
				if (string.Compare(path, PreferFileName, true) == 0) selectedIndex = Items.Count - 1;
			});
			if (this.SelectedIndex == -1 && this.Items.Count > 0) this.SelectedIndex = selectedIndex;
		}

		/// <summary>
		/// 单独添加一个文件到列表,并返回对应的索引
		/// </summary>
		/// <param name="path"></param>
		protected int AddFileToList(string path)
		{
			return this.Items.Add(path);
		}

		/// <summary>
		/// 获得或设置当前选中的文件名
		/// </summary>
		public string SelectedFileName
		{
			get
			{
				if (SelectedIndex < (this.ShowEmptyEntry ? 1 : 0)) return string.Empty;
				else return this.SelectedItem.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (ShowEmptyEntry) this.SelectedIndex = 0;
					return;
				}

				string p = value.ToLower();

				foreach (var item in this.Items)
				{
					if (item.ToString().ToLower() == p)
					{
						this.SelectedItem = item;
						break;
					}
				}
				PreferFileName = value;
				OnPropertyChanged("SelectedFileName");
			}
		}

		/// <summary>
		/// 获得选中的文件
		/// </summary>
		[Browsable(false)]
		public string SelectedFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(RootPath) || this.SelectedIndex < (this.ShowEmptyEntry ? 1 : 0)) return string.Empty;
				else return string.Format("{0}\\{1}", RootPath.Trim(System.IO.Path.DirectorySeparatorChar), this.SelectedFileName);
			}
		}

		private bool _showEmptyEntry;
		string _preferFileName;

		/// <summary>
		/// 获得或设置是否显示空行
		/// </summary>
		public bool ShowEmptyEntry
		{
			get
			{
				return _showEmptyEntry;
			}
			set
			{
				if (_showEmptyEntry == value) return;
				_showEmptyEntry = value;

				if (value)
				{
					this.Items.Insert(0, "<未选择>");
				}
				else
				{
					this.Items.RemoveAt(0);
					if (this.Items.Count > 0 && this.SelectedIndex == -1) this.SelectedIndex = 0;
				}
			}
		}

		/// <summary>
		/// 获得列表
		/// </summary>
		[System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ObjectCollection Items
		{
			get { return base.Items; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
