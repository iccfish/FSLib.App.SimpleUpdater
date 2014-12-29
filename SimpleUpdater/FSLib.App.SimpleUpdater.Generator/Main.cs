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
	using System.IO;

	using Defination;

	using SimpleUpdater.Defination;

	public partial class Main : Form
	{
		/// <summary>
		/// 获得或设置加载时要打开的项目
		/// </summary>
		public string PreloadFile { get; set; }

		/// <summary>
		/// 是否在打开项目后自动构建
		/// </summary>
		public bool AutoBuild { get; set; }

		public Main()
		{
			InitializeComponent();
			InitWorker();
			InitDropSupport();
			InitEvents();
			InitProjectProcess();

			this.btnOpen.Click += btnOpen_Click;
			this.Load += Main_Load;
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
				else if (file.EndsWith("update.xml", StringComparison.OrdinalIgnoreCase) || file.EndsWith("update_c.xml", StringComparison.OrdinalIgnoreCase))
				{
					//打开xml
					var project = AuProject.LoadFromOldProject(file);
					if (project == null)
						Information("无法打开文件 " + file + " !");
					else
						UpdatePackageBuilder.Instance.AuProject = project;
				}
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
				UpdatePackageBuilder.Instance.AuProject.ApplicationDirectory = (e.Data as DataObject).GetFileDropList()[0];
			};
			this.txtPackagePath.DragDrop += (s, e) =>
			{
				var project = UpdatePackageBuilder.Instance.AuProject;
				project.DestinationDirectory = (e.Data as DataObject).GetFileDropList()[0];
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
				UpdatePackageBuilder.Instance.AuProject.UpdateRtfNotePath = file;
			};

		}

		#endregion

		#region 界面响应函数
		void Main_Load(object sender, EventArgs e)
		{
			Text += System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;

			if (!string.IsNullOrEmpty(PreloadFile))
			{
				OpenProject(PreloadFile);
				PreloadFile = null;

				if (AutoBuild && UpdatePackageBuilder.Instance.AuProject != null)
				{
					Create();
				}
			}
		}


		OpenFileDialog _bindVersionFileDialog = new OpenFileDialog()
		{
			Filter = "带有版本信息的文件(*.exe;*.dll)|*.exe;*.dll",
			Title = "绑定版本信息到文件"
		};
		OpenFileDialog _bindDescFile = new OpenFileDialog()
		{
			Filter = "更新说明文件(*.txt)|*.txt",
			Title = "绑定更新说明到文件"
		};


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
				UpdatePackageBuilder.Instance.AuProject.UpdateRtfNotePath = ofd.FileName;
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
			btnBind.Click += (s, e) =>
			{
				if (string.IsNullOrEmpty(UpdatePackageBuilder.Instance.AuProject.VersionUpdateSrc))
				{
					if (_bindVersionFileDialog.ShowDialog() == DialogResult.OK)
						UpdatePackageBuilder.Instance.AuProject.VersionUpdateSrc = _bindVersionFileDialog.FileName;
				}
				else
				{
					UpdatePackageBuilder.Instance.AuProject.VersionUpdateSrc = null;
				}
			};
			lnkBindDescToFile.Click += (s, e) =>
			{
				if (string.IsNullOrEmpty(UpdatePackageBuilder.Instance.AuProject.UpdateContentSrc))
				{
					if (_bindDescFile.ShowDialog() == DialogResult.OK)
						UpdatePackageBuilder.Instance.AuProject.UpdateContentSrc = _bindDescFile.FileName;
				}
				else
				{
					UpdatePackageBuilder.Instance.AuProject.UpdateContentSrc = null;
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
			var file = open.FileName;

			var project = AuProject.LoadFromOldProject(file);

			if (project != null)
			{
				UpdatePackageBuilder.Instance.AuProject = project;
			}
			else
			{
				Information("无法打开指定的文件 " + file + " ！");
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

			var project = UpdatePackageBuilder.Instance.AuProject;

			//检查增量更新
			if (project.Files.Any(s => s.UpdateMethod != UpdateMethod.Always || s.UpdateMethod != UpdateMethod.Ignore) && !project.EnableIncreaseUpdate)
			{
				if (MessageBox.Show("您已经设置部分文件为条件更新，这需要开启增量更新，但是当前尚未打开。这将会导致这些文件的设置失效，是否确定继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK) return;
			}
			//检查文件存在
			if (System.IO.Directory.GetFiles(project.ParseFullPath(project.DestinationDirectory), "*.*", SearchOption.AllDirectories).Length > 0)
			{
				if (MessageBox.Show("自动更新程序将会生成一个或多个文件（包括xml、zip文件等），而您当前选择的升级包保存文件夹不是空的，这可能会导致同名的文件被覆盖。确定继续吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK) return;
			}

			if (string.IsNullOrEmpty(this.txtAppName.Text)) { epp.SetError(this.txtAppName, "请输入应用程序名"); return; }

			if (string.IsNullOrEmpty(project.VersionUpdateSrc))
			{
				try
				{
					new Version(this.txtAppVersion.Text);
				}
				catch (Exception)
				{
					epp.SetError(this.txtAppVersion, "请输入版本号");
					return;
				}
			}
			if (!System.IO.Directory.Exists(project.ParseFullPath(project.ApplicationDirectory)))
			{
				epp.SetError(this.txtNewSoftDir, "请选择新程序的目录");
				return;
			}
			if (string.IsNullOrEmpty(project.DestinationDirectory))
			{
				epp.SetError(this.txtPackagePath, "请选择打包后的组件和升级信息文件所在路径");
				return;
			}
			if (!System.IO.Directory.Exists(project.ParseFullPath(project.DestinationDirectory)))
			{
				epp.SetError(this.txtPackagePath, "文件包所在目录不存在");
				return;
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
				lblStatus.Text = "生成失败：" + e.Exception.Message;
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
			//创建主要信息文件
			var builder = UpdatePackageBuilder.Instance;
			builder.Build(e);

			e.ReportProgress(0, 0, "生成成功！");

			Invoke(() =>
			{
				new Dialogs.PackageGenerateResult()
				{
					PackageResult = builder.Result,
					UpdateInfo = builder.BuiltUpdateInfo
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

		#region 界面响应函数

		private void btnBrowseFolder_Click(object sender, EventArgs e)
		{
			fbd.Description = "请选择包含最新版软件的目录";
			if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			var project = UpdatePackageBuilder.Instance.AuProject;
			project.ApplicationDirectory = fbd.SelectedPath;
		}

		private void browseFile_Click(object sender, EventArgs e)
		{
			fbd.Description = "请选择要放置升级包的目录。建议选择空目录";
			if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			var project = UpdatePackageBuilder.Instance.AuProject;
			project.DestinationDirectory = fbd.SelectedPath;
		}
		#endregion

		#region 升级项目

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
			var upb = UpdatePackageBuilder.Instance;
			upb.ProjectClosed += upb_ProjectClosed;
			upb.ProjectLoaded += upb_ProjectLoaded;
			if (upb.AuProject != null)
				upb_ProjectLoaded(null, new PackageEventArgs(upb.AuProject));

			txtTimeout.TextChanged += (s, e) =>
			{
				var project = UpdatePackageBuilder.Instance.AuProject;
				if (project != null)
					project.UpdateInfo.ExecuteTimeout = txtTimeout.Text.ToInt32();
			};
		}

		void upb_ProjectLoaded(object sender, PackageEventArgs e)
		{
			var project = e.AuProject;
			txtNewSoftDir.AddDataBinding(project, s => s.Text, s => s.ApplicationDirectory);
			txtPackagePath.AddDataBinding(project, s => s.Text, s => s.DestinationDirectory);
			rtfPath.AddDataBinding(project, s => s.Text, s => s.UpdateRtfNotePath);

			fileAfterExecute.AddDataBinding(project, s => s.RootPath, s => s.ApplicationDirectory);
			filePreExecute.AddDataBinding(project, s => s.RootPath, s => s.ApplicationDirectory);
			btnBind.Text = string.IsNullOrEmpty(project.VersionUpdateSrc) ? "绑定" : "取消绑定";
			txtAppVersion.Enabled = string.IsNullOrEmpty(project.VersionUpdateSrc);
			lnkBindDescToFile.Text = string.IsNullOrEmpty(project.UpdateContentSrc) ? "绑定到文件" : "已绑定（" + project.UpdateContentSrc + "），点击取消绑定";
			txtDesc.Enabled = string.IsNullOrEmpty(project.UpdateContentSrc);

			var ui = UpdatePackageBuilder.Instance.AuProject.UpdateInfo;
			txtAfterExecuteArgs.AddDataBinding(ui, s => s.Text, s => s.ExecuteArgumentAfter);
			txtAppName.AddDataBinding(ui, s => s.Text, s => s.AppName);
			txtAppVersion.AddDataBinding(ui, s => s.Text, s => s.AppVersion);
			txtDesc.AddDataBinding(ui, s => s.Text, s => s.Desc);
			txtPreExecuteArgs.AddDataBinding(ui, s => s.Text, s => s.ExecuteArgumentBefore);
			txtPublishUrl.AddDataBinding(ui, s => s.Text, s => s.PublishUrl);
			txtTimeout.Text = ui.ExecuteTimeout.ToString();
			chkHideBefore.AddDataBinding(ui, s => s.Checked, s => s.HideBeforeExecuteWindow);
			chkHideAfter.AddDataBinding(ui, s => s.Checked, s => s.HideAfterExecuteWindow);
			fileAfterExecute.AddDataBinding(ui, s => s.SelectedFileName, s => s.FileExecuteAfter);
			filePreExecute.AddDataBinding(ui, s => s.SelectedFileName, s => s.FileExecuteBefore);
			txtUrl.AddDataBinding(ui, s => s.Text, s => s.WebUpdateNote);
			txtPing.AddDataBinding(ui, s => s.Text, s => s.UpdatePingUrl);

			chkOptAutoExitProcess.AddDataBinding(ui, s => s.Checked, s => s.AutoExitCurrentProcess);
			chkOptAutoKillProcess.AddDataBinding(ui, s => s.Checked, s => s.AutoKillProcesses);
			chkOptForceUpdate.AddDataBinding(ui, s => s.Checked, s => s.ForceUpdate);
			chkOptMustUpdate.AddDataBinding(ui, s => s.Checked, s => s.MustUpdate);
			chkAutoEndAppDirProcesses.AddDataBinding(ui, s => s.Checked, s => s.AutoEndProcessesWithinAppDir);
			chkStillProptUserInfo.AddDataBinding(ui, s => s.Checked, s => s.PromptUserBeforeAutomaticUpgrade);
			chkOptError.AddDataBinding(ui, s => s.Checked, s => s.TreatErrorAsNotUpdated);
			chkOptRequireAdminPrivilege.AddDataBinding(ui, s => s.Checked, s => s.RequreAdminstrorPrivilege);
			txtPackageExtension.AddDataBinding(project, s => s.Text, s => s.PackageExtension);

			project.PropertyChanged += (ss, ee) =>
			{
				var p = ss as AuProject;
				if (ee.PropertyName == "VersionUpdateSrc")
				{
					btnBind.Text = string.IsNullOrEmpty(p.VersionUpdateSrc) ? "绑定" : "取消绑定";
					txtAppVersion.Enabled = string.IsNullOrEmpty(p.VersionUpdateSrc);
				}
				else if (ee.PropertyName == "UpdateContentSrc")
				{
					lnkBindDescToFile.Text = string.IsNullOrEmpty(p.UpdateContentSrc) ? "绑定到文件" : "取消绑定";
					txtDesc.Enabled = string.IsNullOrEmpty(p.UpdateContentSrc);
				}

			};
			ui.PropertyChanged += (ss, se) =>
			{
				var u = (ss as UpdateInfo);
				if (se.PropertyName == "ExecuteTimeout")
					txtTimeout.Text = u.ExecuteTimeout.ToString();
			};
			txtTimeout.Text = ui.ExecuteTimeout.ToString();
			UpdatePackageBuilder.Instance.AutoLoadInformations();
		}


		void upb_ProjectClosed(object sender, PackageEventArgs e)
		{
			txtNewSoftDir.DataBindings.Clear();
			txtPackagePath.DataBindings.Clear();
			rtfPath.DataBindings.Clear();

			txtAfterExecuteArgs.DataBindings.Clear();
			txtAppName.DataBindings.Clear();
			txtAppVersion.DataBindings.Clear();
			txtDesc.DataBindings.Clear();
			txtPreExecuteArgs.DataBindings.Clear();
			txtPublishUrl.DataBindings.Clear();
			txtTimeout.DataBindings.Clear();
			chkHideBefore.DataBindings.Clear();
			chkHideAfter.DataBindings.Clear();
			fileAfterExecute.DataBindings.Clear();
			filePreExecute.DataBindings.Clear();
			txtUrl.DataBindings.Clear();
			txtPing.DataBindings.Clear();

			chkOptAutoExitProcess.DataBindings.Clear();
			chkOptAutoKillProcess.DataBindings.Clear();
			chkOptForceUpdate.DataBindings.Clear();
			chkOptMustUpdate.DataBindings.Clear();
			chkAutoEndAppDirProcesses.DataBindings.Clear();
			chkStillProptUserInfo.DataBindings.Clear();
			chkOptError.DataBindings.Clear();
			chkOptRequireAdminPrivilege.DataBindings.Clear();
			txtPackageExtension.DataBindings.Clear();
		}

		private void btnSaveProject_Click(object sender, EventArgs e)
		{
			var projectPath = UpdatePackageBuilder.Instance.AuProject.ProjectFilePath;
			if (string.IsNullOrEmpty(projectPath))
			{
				if (_auProjSaveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
				projectPath = _auProjSaveDlg.FileName;
			}
			UpdatePackageBuilder.Instance.AuProject.Save(projectPath);
			Information("项目已经保存");
		}

		private void btnOpenProject_Click(object sender, EventArgs e)
		{
			if (_auProjOpenDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
			OpenProject(_auProjOpenDlg.FileName);
		}


		void OpenProject(string path)
		{
			var project = AuProject.LoadFile(path);

			if (project == null)
			{
				Information("打开项目时出错!");
				return;
			}

			UpdatePackageBuilder.Instance.AuProject = project;
		}

		#endregion
	}
}
