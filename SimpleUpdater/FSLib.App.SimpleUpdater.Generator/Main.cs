using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace FSLib.App.SimpleUpdater.Generator
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
			InitWorker();
			InitDropSupport();
			InitEvents();
			InitProjectProcess();

			this.btnOpen.Click += btnOpen_Click;
			fileConfig.NewVersionFolder = "";
		}

		#region 拖放支持

		void InitDropSupport()
		{
			this.AllowDrop = true;
			this.txtNewSoftDir.AllowDrop = true;

			//自身
			this.DragEnter += (s, e) =>
			{
				System.Collections.Specialized.StringCollection files;
				var doe = e.Data as DataObject;
				if (
					!doe.ContainsFileDropList()
					||
					(files = doe.GetFileDropList()).Count == 0
					||
					!System.IO.File.Exists(files[0])
					||
					!(files[0].EndsWith(".xml", StringComparison.OrdinalIgnoreCase) || files[0].EndsWith(".auproj", StringComparison.OrdinalIgnoreCase))
					) return;

				e.Effect = DragDropEffects.Link;
			};
			this.DragDrop += (s, e) =>
			{
				var file = (e.Data as DataObject).GetFileDropList()[0];
				if (string.Compare(System.IO.Path.GetExtension(file), ".auproj", true) == 0)
					OpenProject(file);
				else
					OpenXML(file);
			};
			//升级包
			this.txtNewSoftDir.DragEnter += (s, e) =>
			{
				System.Collections.Specialized.StringCollection files;
				var doe = e.Data as DataObject;
				if (
					!doe.ContainsFileDropList()
					||
					(files = doe.GetFileDropList()).Count == 0
					||
					!System.IO.Directory.Exists(files[0])
					) return;

				e.Effect = DragDropEffects.Link;
			};
			txtPackagePath.AllowDrop = true;
			txtPackagePath.DragEnter += (s, e) =>
			{
				System.Collections.Specialized.StringCollection files;
				var doe = e.Data as DataObject;
				if (
					!doe.ContainsFileDropList()
					||
					(files = doe.GetFileDropList()).Count == 0
					||
					!System.IO.Directory.Exists(files[0])
					) return;

				e.Effect = DragDropEffects.Link;
			};

			this.txtNewSoftDir.DragDrop += (s, e) =>
			{
				this.SelectedNewSoftDirPath = (e.Data as DataObject).GetFileDropList()[0];
			};
			this.txtPackagePath.DragDrop += (s, e) =>
			{
				this.SelectedPackagePath = (e.Data as DataObject).GetFileDropList()[0];
			};

			//RTF
			rtfPath.AllowDrop = true;
			rtfPath.DragEnter += (s, e) =>
			{
				System.Collections.Specialized.StringCollection files;
				var doe = e.Data as DataObject;
				if (
					!doe.ContainsFileDropList()
					||
					(files = doe.GetFileDropList()).Count == 0
					||
					!System.IO.File.Exists(files[0])
					||
					!files[0].EndsWith(".rtf", StringComparison.OrdinalIgnoreCase)
					) return;

				e.Effect = DragDropEffects.Link;
			};
			rtfPath.DragDrop += (s, e) =>
			{
				var file = (e.Data as DataObject).GetFileDropList()[0];
				rtfPath.Text = file;
			};

		}

		#endregion

		#region 界面响应函数

		/// <summary>
		/// 初始化界面函数
		/// </summary>
		void InitEvents()
		{
			var ofd = new OpenFileDialog()
			{
				Title = "打开RTF文件....",
				Filter = "RTF文件(*.rtf)|*.rtf"
			};
			btnBrowserRtf.Click += (s, e) =>
			{
				if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
				rtfPath.Text = ofd.FileName;
			};
			btnEditRtf.Click += (s, e) =>
			{
				if (string.IsNullOrEmpty(rtfPath.Text) || !System.IO.File.Exists(rtfPath.Text)) return;
				try
				{
					System.Diagnostics.Process.Start(rtfPath.Text);
				}
				catch (Exception ex)
				{
					Information("无法打开相关的应用程序 - " + ex.Message);
				}
			};

		}

		void Information(string message)
		{
			MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// 打开
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnOpen_Click(object sender, EventArgs e)
		{
			var open = new OpenFileDialog()
			{
				Title = "打开升级信息文件",
				Filter = "XML信息文件(*.xml)|*.xml"
			};
			if (open.ShowDialog() != DialogResult.OK) return;
			OpenXML(open.FileName);
		}

		/// <summary>
		/// 打开配置文件
		/// </summary>
		/// <param name="path"></param>
		void OpenXML(string path)
		{
			UpdateInfo ui;
			if (!ExtensionMethods.IsCompressedXmlFile(path))
			{
				ui = typeof(UpdateInfo).XmlDeserializeFile(path) as UpdateInfo;
			}
			else
			{
				ui = ExtensionMethods.DecompressFile(path).XmlDeserializeFromStream<UpdateInfo>();
			}

			if (ui == null) Information("无法加载信息文件，请确认选择正确的文件");
			else
			{
				this.SelectedPackagePath = System.IO.Path.GetDirectoryName(path);
				CurrentUpdateInfo = ui;
			}
		}

		/// <summary>
		/// 创建升级包
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCreate_Click(object sender, EventArgs e)
		{
			epp.Clear();

			//检查增量更新
			if (fileConfig.HasIncreaseUpdateFile && !options.EnableIncreaseUpdate)
			{
				if (MessageBox.Show("您已经设置部分文件为条件更新，这需要开启增量更新，但是当前尚未打开。这将会导致这些文件的设置失效，是否确定继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK) return;
			}
			//检查文件存在
			if (System.IO.Directory.GetFiles(SelectedPackagePath).Length > 0 || System.IO.Directory.GetDirectories(SelectedPackagePath).Length > 0)
			{
				if (MessageBox.Show("自动更新程序将会生成一个或多个文件（包括xml、zip文件等），而您当前选择的升级包保存文件夹不是空的，这可能会导致同名的文件被覆盖。确定继续吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK) return;
			}

			if (string.IsNullOrEmpty(this.txtAppName.Text)) { epp.SetError(this.txtAppName, "请输入应用程序名"); return; }
			try
			{
				new Version(this.txtAppVersion.Text);
			}
			catch (Exception)
			{
				epp.SetError(this.txtAppVersion, "请输入版本号");
				return;
			}
			if (!System.IO.Directory.Exists(this.SelectedNewSoftDirPath)) { epp.SetError(this.txtNewSoftDir, "请选择新程序的目录"); return; }
			if (string.IsNullOrEmpty(this.SelectedPackagePath)) { epp.SetError(this.txtPackagePath, "请选择打包后的组件和升级信息文件所在路径"); return; }
			if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.SelectedPackagePath))) { epp.SetError(this.txtPackagePath, "文件包所在目录不存在"); return; }

			//删除原始包的文件
			if (CurrentUpdateInfo != null)
			{
				var root = txtPackagePath.Text;
				var mainPkgPath = System.IO.Path.Combine(root, CurrentUpdateInfo.Package);
				if (System.IO.File.Exists(mainPkgPath)) System.IO.File.Delete(mainPkgPath);

				if (CurrentUpdateInfo.Packages != null)
				{
					CurrentUpdateInfo.Packages.ForEach(s =>
						{
							var path = System.IO.Path.Combine(root, s.PackageName);
							if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
						});
				}
			}

			Create();
		}


		#endregion

		#region 主要创建流程

		T Invoke<T>(Func<T> predicate)
		{
			return (T)Invoke((Delegate)predicate);
		}

		void Invoke(Action action)
		{
			Invoke((Delegate)action);
		}

		/// <summary>
		/// 初始化线程类
		/// </summary>
		void InitWorker()
		{
			bgw.WorkerProgressChanged += (s, e) =>
			{
				lblStatus.Text = e.Progress.StateMessage;
				this.pbProgress.Value = e.Progress.TaskPercentage;
				pbProgress.Style = e.Progress.TaskCount > 0 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
			};
			bgw.WorkCompleted += (s, e) =>
			{
				btnCreate.Enabled = true;
				this.pbProgress.Visible = false;
			};
			bgw.WorkFailed += (s, e) =>
			{
				btnCreate.Enabled = true;
				this.pbProgress.Visible = false;
				Information("出现错误：" + e.Exception.ToString());
			};
			bgw.DoWork += CreatePackage;
			this.FormClosing += (s, e) =>
			{
				e.Cancel = !this.btnCreate.Enabled;
			};
		}

		//创建信息的具体操作函数
		void CreatePackage(object sender, Wrapper.RunworkEventArgs e)
		{
			var info = new UpdateInfo()
			{
				AppName = Invoke(() => this.txtAppName.Text),
				AppVersion = Invoke(() => this.txtAppVersion.Text),
				Desc = Invoke(() => this.txtDesc.Text),
				ExecuteArgumentAfter = Invoke(() => this.txtAfterExecuteArgs.Text),
				ExecuteArgumentBefore = Invoke(() => this.txtPreExecuteArgs.Text),
				PublishUrl = Invoke(() => this.txtPublishUrl.Text),
				FileExecuteAfter = Invoke(() => this.fileAfterExecute.SelectedFileName),
				FileExecuteBefore = Invoke(() => this.filePreExecute.SelectedFileName),
				MD5 = "",
				Package = System.IO.Path.GetFileName(Invoke(() => this.txtPackagePath.Text)),
				ExecuteTimeout = Invoke(() => txtTimeout.Text.ToInt32()),
				PackageSize = 0,
				RequiredMinVersion = "",
				HideAfterExecuteWindow = Invoke(() => chkHideAfter.Checked),
				HideBeforeExecuteWindow = Invoke(() => chkHideBefore.Checked),
				WebUpdateNote = Invoke(() => txtUrl.Text)
			};
			var fileRtf = Invoke(() => rtfPath.Text);
			if (System.IO.File.Exists(fileRtf))
			{
				info.RtfUpdateNote = Convert.ToBase64String(ExtensionMethods.CompressBuffer(System.IO.File.ReadAllBytes(fileRtf)));
			}

			Invoke(() => this.options.SaveSetting(info));

			//创建主要信息文件
			var builder = new UpdatePackageBuilder()
			{
				UpdateInfo = info,
				CompressXmlFile = Invoke(() => options.CompressXmlFile),
				EnableIncreaseUpdate = Invoke(() => options.EnableIncreaseUpdate && fileConfig.HasIncreaseUpdateFile),
				CreateCompatiblePackage = Invoke(() => options.CreateCompatiblePackage),
				GetVersionHandler = fileConfig.GetFileVersion,
				GetUpdateMethodHandler = fileConfig.GetFileUpdateMethod,
				GetVerificationLevelHandler = fileConfig.GetFileVerificationLevel,
				AllFiles = Invoke(() => fileConfig.AllFiles),
				PackagePath = SelectedPackagePath
			};
			builder.Build(e);

			e.ReportProgress(0, 0, "生成成功！");

			Invoke(() =>
			{
				CurrentUpdateInfo = info;
				new Dialogs.PackageGenerateResult()
				{
					PackageResult = builder.Result,
					UpdateInfo = info
				}.ShowDialog();
			});
		}

		void Create()
		{
			this.btnCreate.Enabled = false;
			this.pbProgress.Visible = true;
			bgw.RunWorkASync();
		}

		Wrapper.BackgroundWorker bgw = new Wrapper.BackgroundWorker()
		{
			WorkerSupportReportProgress = true
		};

		#endregion

		#region 界面属性

		/// <summary>
		/// 获得或设置文件包路径
		/// </summary>
		public string SelectedPackagePath
		{
			get { return this.txtPackagePath.Text; }
			set
			{
				this.txtPackagePath.Text = value;
				TryLoadXml(value);
			}
		}

		/// <summary>
		/// 获得或设置选定的新软件目录
		/// </summary>
		public string SelectedNewSoftDirPath
		{
			get { return this.txtNewSoftDir.Text; }
			set
			{
				this.txtNewSoftDir.Text = value;
				Environment.CurrentDirectory = value;
				filePreExecute.RootPath = fileAfterExecute.RootPath = value;
				fileConfig.NewVersionFolder = value;
			}
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
				if (_currentUpdateInfo == value) return;

				_currentUpdateInfo = value;
				options.UpdateInterface(value);
				fileConfig.CurrentUpdateInfo = value;

				if (value != null)
				{
					txtAfterExecuteArgs.Text = value.ExecuteArgumentAfter;
					txtAppName.Text = value.AppName;
					txtAppVersion.Text = value.AppVersion;
					txtDesc.Text = value.Desc;
					txtPreExecuteArgs.Text = value.ExecuteArgumentBefore;
					txtPublishUrl.Text = value.PublishUrl;
					txtTimeout.Text = value.ExecuteTimeout.ToString();
					chkHideBefore.Checked = value.HideBeforeExecuteWindow;
					chkHideAfter.Checked = value.HideAfterExecuteWindow;
					fileAfterExecute.PreferFileName = value.FileExecuteAfter;
					filePreExecute.PreferFileName = value.FileExecuteBefore;
					txtUrl.Text = value.WebUpdateNote;
				}
			}
		}

		#endregion

		#region 界面响应函数
		private void btnBrowseFolder_Click(object sender, EventArgs e)
		{
			fbd.Description = "请选择包含最新版软件的目录";
			if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			SelectedNewSoftDirPath = fbd.SelectedPath;
		}

		private void browseFile_Click(object sender, EventArgs e)
		{
			fbd.Description = "请选择要放置升级包的目录。建议选择空目录";
			if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			SelectedPackagePath = fbd.SelectedPath;
		}

		private void txtNewSoftDir_TextChanged(object sender, EventArgs e)
		{
			if (System.IO.Directory.Exists(txtNewSoftDir.Text))
			{
				filePreExecute.RootPath = fileAfterExecute.RootPath = this.SelectedNewSoftDirPath;
			}
		}

		private void txtPackagePath_TextChanged(object sender, EventArgs e)
		{
			TryLoadXml(txtPackagePath.Text);
		}
		#endregion

		#region 升级项目

		AuProject _project;
		public AuProject Project
		{
			get { return _project; }
			set
			{
				if (_project == value)
					return;

				if (_project != null)
				{

				}
			}
		}
		string _auProjectPath;

		SaveFileDialog _auProjSaveDlg = new SaveFileDialog()
		{
			Filter = "升级项目文件(*.auproj)|*.auproj",
			Title = "保存升级项目..."
		};

		OpenFileDialog _auProjOpenDlg = new OpenFileDialog()
		{
			Filter = "升级项目文件(*.auproj)|*.auproj",
			Title = "保存升级项目..."
		};

		void InitProjectProcess()
		{
			_auProject = new AuProject();
		}

		private void btnSaveProject_Click(object sender, EventArgs e)
		{
			_auProject = _auProject ?? new AuProject();
			_auProject.ApplicationDirectory = txtNewSoftDir.Text;
			_auProject.DestinationDirectory = txtPackagePath.Text;
			_auProject.UpdateRtfNotePath = rtfPath.Text;

			if (string.IsNullOrEmpty(_auProjectPath))
			{
				if (_auProjSaveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
				_auProjectPath = _auProjSaveDlg.FileName;
			}
			_auProject.Save(_auProjectPath);
			Information("项目已经保存");
		}

		private void btnOpenProject_Click(object sender, EventArgs e)
		{
			if (_auProjOpenDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
			OpenProject(_auProjOpenDlg.FileName);
		}


		void OpenProject(string path)
		{
			_auProjectPath = path;
			_auProject = AuProject.LoadFile(_auProjectPath);

			if (_auProjectPath == null)
			{
				Information("打开项目时出错!");
				return;
			}

			SelectedNewSoftDirPath = _auProject.ApplicationDirectory;
			txtPackagePath.Text = _auProject.DestinationDirectory;
			rtfPath.Text = _auProject.UpdateRtfNotePath;

			TryLoadXml(_auProject.DestinationDirectory);
		}


		string _lastLoadPath;

		/// <summary>
		/// 尝试自动打开升级信息
		/// </summary>
		/// <param name="directory"></param>
		void TryLoadXml(string directory)
		{
			if (directory == _lastLoadPath || string.IsNullOrEmpty(directory) || !System.IO.Directory.Exists(directory)) return;
			_lastLoadPath = directory;

			var fileNames = new[] { "update_c.xml", "update.xml" };
			foreach (var fn in fileNames)
			{
				var updateFile = System.IO.Path.Combine(directory, fn);
				if (System.IO.File.Exists(updateFile))
				{
					OpenXML(updateFile);
					break;
				}
			}
		}

		#endregion
	}
}
