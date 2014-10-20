using FSLib.App.SimpleUpdater.Generator.Controls;

namespace FSLib.App.SimpleUpdater.Generator
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtAppName = new System.Windows.Forms.TextBox();
			this.txtPublishUrl = new System.Windows.Forms.TextBox();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.btnCreate = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtAfterExecuteArgs = new System.Windows.Forms.TextBox();
			this.txtPreExecuteArgs = new System.Windows.Forms.TextBox();
			this.txtAppVersion = new System.Windows.Forms.TextBox();
			this.txtTimeout = new System.Windows.Forms.TextBox();
			this.txtNewSoftDir = new System.Windows.Forms.TextBox();
			this.txtPackagePath = new System.Windows.Forms.TextBox();
			this.epp = new System.Windows.Forms.ErrorProvider(this.components);
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.btnOpen = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnBind = new System.Windows.Forms.Button();
			this.chkHideAfter = new System.Windows.Forms.CheckBox();
			this.chkHideBefore = new System.Windows.Forms.CheckBox();
			this.browseFile = new System.Windows.Forms.Button();
			this.btnBrowseFolder = new System.Windows.Forms.Button();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.lnkBindDescToFile = new System.Windows.Forms.LinkLabel();
			this.btnEditRtf = new System.Windows.Forms.Button();
			this.btnBrowserRtf = new System.Windows.Forms.Button();
			this.rtfPath = new System.Windows.Forms.TextBox();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.gpUpdatePing = new System.Windows.Forms.GroupBox();
			this.txtPing = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage8 = new System.Windows.Forms.TabPage();
			this.label18 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.chkOptForceUpdate = new System.Windows.Forms.CheckBox();
			this.chkOptRequireAdminPrivilege = new System.Windows.Forms.CheckBox();
			this.chkStillProptUserInfo = new System.Windows.Forms.CheckBox();
			this.chkOptError = new System.Windows.Forms.CheckBox();
			this.chkOptAutoExitProcess = new System.Windows.Forms.CheckBox();
			this.chkAutoEndAppDirProcesses = new System.Windows.Forms.CheckBox();
			this.chkOptAutoKillProcess = new System.Windows.Forms.CheckBox();
			this.chkOptMustUpdate = new System.Windows.Forms.CheckBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.fbd = new System.Windows.Forms.FolderBrowserDialog();
			this.tip = new System.Windows.Forms.ToolTip(this.components);
			this.btnSaveProject = new System.Windows.Forms.Button();
			this.btnOpenProject = new System.Windows.Forms.Button();
			this.fileAfterExecute = new FSLib.App.SimpleUpdater.Generator.Controls.FileComboBox();
			this.filePreExecute = new FSLib.App.SimpleUpdater.Generator.Controls.FileComboBox();
			this.options = new FSLib.App.SimpleUpdater.Generator.Controls.OptionTab();
			this.fileConfig = new FSLib.App.SimpleUpdater.Generator.Controls.FileConfiguration();
			this.argumentGenerator1 = new FSLib.App.SimpleUpdater.Generator.Controls.ArgumentGenerator();
			this.aboutPanel1 = new FSLib.App.SimpleUpdater.Generator.Controls.AboutPanel();
			((System.ComponentModel.ISupportInitialize)(this.epp)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.tabPage7.SuspendLayout();
			this.gpUpdatePing.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage8.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// txtAppName
			// 
			resources.ApplyResources(this.txtAppName, "txtAppName");
			this.txtAppName.Name = "txtAppName";
			// 
			// txtPublishUrl
			// 
			resources.ApplyResources(this.txtPublishUrl, "txtPublishUrl");
			this.txtPublishUrl.Name = "txtPublishUrl";
			this.tip.SetToolTip(this.txtPublishUrl, resources.GetString("txtPublishUrl.ToolTip"));
			// 
			// pbProgress
			// 
			resources.ApplyResources(this.pbProgress, "pbProgress");
			this.pbProgress.Name = "pbProgress";
			// 
			// btnCreate
			// 
			resources.ApplyResources(this.btnCreate, "btnCreate");
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// lblStatus
			// 
			resources.ApplyResources(this.lblStatus, "lblStatus");
			this.lblStatus.Name = "lblStatus";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// txtAfterExecuteArgs
			// 
			resources.ApplyResources(this.txtAfterExecuteArgs, "txtAfterExecuteArgs");
			this.txtAfterExecuteArgs.Name = "txtAfterExecuteArgs";
			this.tip.SetToolTip(this.txtAfterExecuteArgs, resources.GetString("txtAfterExecuteArgs.ToolTip"));
			// 
			// txtPreExecuteArgs
			// 
			resources.ApplyResources(this.txtPreExecuteArgs, "txtPreExecuteArgs");
			this.txtPreExecuteArgs.Name = "txtPreExecuteArgs";
			this.tip.SetToolTip(this.txtPreExecuteArgs, resources.GetString("txtPreExecuteArgs.ToolTip"));
			// 
			// txtAppVersion
			// 
			resources.ApplyResources(this.txtAppVersion, "txtAppVersion");
			this.txtAppVersion.Name = "txtAppVersion";
			this.tip.SetToolTip(this.txtAppVersion, resources.GetString("txtAppVersion.ToolTip"));
			// 
			// txtTimeout
			// 
			resources.ApplyResources(this.txtTimeout, "txtTimeout");
			this.txtTimeout.Name = "txtTimeout";
			// 
			// txtNewSoftDir
			// 
			resources.ApplyResources(this.txtNewSoftDir, "txtNewSoftDir");
			this.txtNewSoftDir.Name = "txtNewSoftDir";
			this.tip.SetToolTip(this.txtNewSoftDir, resources.GetString("txtNewSoftDir.ToolTip"));
			// 
			// txtPackagePath
			// 
			resources.ApplyResources(this.txtPackagePath, "txtPackagePath");
			this.txtPackagePath.Name = "txtPackagePath";
			this.tip.SetToolTip(this.txtPackagePath, resources.GetString("txtPackagePath.ToolTip"));
			// 
			// epp
			// 
			this.epp.ContainerControl = this;
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// btnOpen
			// 
			resources.ApplyResources(this.btnOpen, "btnOpen");
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Controls.Add(this.tabPage7);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage8);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage4);
			resources.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnBind);
			this.tabPage1.Controls.Add(this.chkHideAfter);
			this.tabPage1.Controls.Add(this.chkHideBefore);
			this.tabPage1.Controls.Add(this.browseFile);
			this.tabPage1.Controls.Add(this.btnBrowseFolder);
			this.tabPage1.Controls.Add(this.txtPackagePath);
			this.tabPage1.Controls.Add(this.txtNewSoftDir);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.txtTimeout);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.txtAfterExecuteArgs);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.txtPreExecuteArgs);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.fileAfterExecute);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.filePreExecute);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.txtAppName);
			this.tabPage1.Controls.Add(this.txtPublishUrl);
			this.tabPage1.Controls.Add(this.txtAppVersion);
			resources.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// btnBind
			// 
			resources.ApplyResources(this.btnBind, "btnBind");
			this.btnBind.Name = "btnBind";
			this.tip.SetToolTip(this.btnBind, resources.GetString("btnBind.ToolTip"));
			this.btnBind.UseVisualStyleBackColor = true;
			// 
			// chkHideAfter
			// 
			resources.ApplyResources(this.chkHideAfter, "chkHideAfter");
			this.chkHideAfter.Name = "chkHideAfter";
			this.tip.SetToolTip(this.chkHideAfter, resources.GetString("chkHideAfter.ToolTip"));
			this.chkHideAfter.UseVisualStyleBackColor = true;
			// 
			// chkHideBefore
			// 
			resources.ApplyResources(this.chkHideBefore, "chkHideBefore");
			this.chkHideBefore.Name = "chkHideBefore";
			this.tip.SetToolTip(this.chkHideBefore, resources.GetString("chkHideBefore.ToolTip"));
			this.chkHideBefore.UseVisualStyleBackColor = true;
			// 
			// browseFile
			// 
			resources.ApplyResources(this.browseFile, "browseFile");
			this.browseFile.Name = "browseFile";
			this.browseFile.UseVisualStyleBackColor = true;
			this.browseFile.Click += new System.EventHandler(this.browseFile_Click);
			// 
			// btnBrowseFolder
			// 
			resources.ApplyResources(this.btnBrowseFolder, "btnBrowseFolder");
			this.btnBrowseFolder.Name = "btnBrowseFolder";
			this.btnBrowseFolder.UseVisualStyleBackColor = true;
			this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.lnkBindDescToFile);
			this.tabPage6.Controls.Add(this.btnEditRtf);
			this.tabPage6.Controls.Add(this.btnBrowserRtf);
			this.tabPage6.Controls.Add(this.rtfPath);
			this.tabPage6.Controls.Add(this.txtUrl);
			this.tabPage6.Controls.Add(this.label13);
			this.tabPage6.Controls.Add(this.label12);
			this.tabPage6.Controls.Add(this.label11);
			this.tabPage6.Controls.Add(this.label8);
			this.tabPage6.Controls.Add(this.txtDesc);
			resources.ApplyResources(this.tabPage6, "tabPage6");
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// lnkBindDescToFile
			// 
			resources.ApplyResources(this.lnkBindDescToFile, "lnkBindDescToFile");
			this.lnkBindDescToFile.Name = "lnkBindDescToFile";
			this.lnkBindDescToFile.TabStop = true;
			this.tip.SetToolTip(this.lnkBindDescToFile, resources.GetString("lnkBindDescToFile.ToolTip"));
			// 
			// btnEditRtf
			// 
			resources.ApplyResources(this.btnEditRtf, "btnEditRtf");
			this.btnEditRtf.Name = "btnEditRtf";
			this.btnEditRtf.UseVisualStyleBackColor = true;
			// 
			// btnBrowserRtf
			// 
			resources.ApplyResources(this.btnBrowserRtf, "btnBrowserRtf");
			this.btnBrowserRtf.Name = "btnBrowserRtf";
			this.btnBrowserRtf.UseVisualStyleBackColor = true;
			// 
			// rtfPath
			// 
			resources.ApplyResources(this.rtfPath, "rtfPath");
			this.rtfPath.Name = "rtfPath";
			this.rtfPath.ReadOnly = true;
			this.tip.SetToolTip(this.rtfPath, resources.GetString("rtfPath.ToolTip"));
			// 
			// txtUrl
			// 
			resources.ApplyResources(this.txtUrl, "txtUrl");
			this.txtUrl.Name = "txtUrl";
			// 
			// label13
			// 
			resources.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			// 
			// label12
			// 
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// txtDesc
			// 
			resources.ApplyResources(this.txtDesc, "txtDesc");
			this.txtDesc.Name = "txtDesc";
			// 
			// tabPage7
			// 
			this.tabPage7.Controls.Add(this.gpUpdatePing);
			resources.ApplyResources(this.tabPage7, "tabPage7");
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.UseVisualStyleBackColor = true;
			// 
			// gpUpdatePing
			// 
			this.gpUpdatePing.Controls.Add(this.txtPing);
			this.gpUpdatePing.Controls.Add(this.label14);
			resources.ApplyResources(this.gpUpdatePing, "gpUpdatePing");
			this.gpUpdatePing.Name = "gpUpdatePing";
			this.gpUpdatePing.TabStop = false;
			// 
			// txtPing
			// 
			resources.ApplyResources(this.txtPing, "txtPing");
			this.txtPing.Name = "txtPing";
			// 
			// label14
			// 
			resources.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.options);
			resources.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage8
			// 
			this.tabPage8.Controls.Add(this.label18);
			this.tabPage8.Controls.Add(this.label20);
			this.tabPage8.Controls.Add(this.label21);
			this.tabPage8.Controls.Add(this.label19);
			this.tabPage8.Controls.Add(this.label17);
			this.tabPage8.Controls.Add(this.label16);
			this.tabPage8.Controls.Add(this.label15);
			this.tabPage8.Controls.Add(this.chkOptForceUpdate);
			this.tabPage8.Controls.Add(this.chkOptRequireAdminPrivilege);
			this.tabPage8.Controls.Add(this.chkStillProptUserInfo);
			this.tabPage8.Controls.Add(this.chkOptError);
			this.tabPage8.Controls.Add(this.chkOptAutoExitProcess);
			this.tabPage8.Controls.Add(this.chkAutoEndAppDirProcesses);
			this.tabPage8.Controls.Add(this.chkOptAutoKillProcess);
			this.tabPage8.Controls.Add(this.chkOptMustUpdate);
			resources.ApplyResources(this.tabPage8, "tabPage8");
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.UseVisualStyleBackColor = true;
			// 
			// label18
			// 
			resources.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			// 
			// label20
			// 
			resources.ApplyResources(this.label20, "label20");
			this.label20.Name = "label20";
			// 
			// label21
			// 
			resources.ApplyResources(this.label21, "label21");
			this.label21.Name = "label21";
			// 
			// label19
			// 
			resources.ApplyResources(this.label19, "label19");
			this.label19.Name = "label19";
			// 
			// label17
			// 
			resources.ApplyResources(this.label17, "label17");
			this.label17.Name = "label17";
			// 
			// label16
			// 
			resources.ApplyResources(this.label16, "label16");
			this.label16.Name = "label16";
			// 
			// label15
			// 
			resources.ApplyResources(this.label15, "label15");
			this.label15.Name = "label15";
			// 
			// chkOptForceUpdate
			// 
			resources.ApplyResources(this.chkOptForceUpdate, "chkOptForceUpdate");
			this.chkOptForceUpdate.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkOptForceUpdate.Name = "chkOptForceUpdate";
			this.chkOptForceUpdate.UseVisualStyleBackColor = true;
			// 
			// chkOptRequireAdminPrivilege
			// 
			resources.ApplyResources(this.chkOptRequireAdminPrivilege, "chkOptRequireAdminPrivilege");
			this.chkOptRequireAdminPrivilege.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkOptRequireAdminPrivilege.Name = "chkOptRequireAdminPrivilege";
			this.chkOptRequireAdminPrivilege.UseVisualStyleBackColor = true;
			// 
			// chkStillProptUserInfo
			// 
			resources.ApplyResources(this.chkStillProptUserInfo, "chkStillProptUserInfo");
			this.chkStillProptUserInfo.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkStillProptUserInfo.Name = "chkStillProptUserInfo";
			this.chkStillProptUserInfo.UseVisualStyleBackColor = true;
			// 
			// chkOptError
			// 
			resources.ApplyResources(this.chkOptError, "chkOptError");
			this.chkOptError.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkOptError.Name = "chkOptError";
			this.chkOptError.UseVisualStyleBackColor = true;
			// 
			// chkOptAutoExitProcess
			// 
			resources.ApplyResources(this.chkOptAutoExitProcess, "chkOptAutoExitProcess");
			this.chkOptAutoExitProcess.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkOptAutoExitProcess.Name = "chkOptAutoExitProcess";
			this.chkOptAutoExitProcess.UseVisualStyleBackColor = true;
			// 
			// chkAutoEndAppDirProcesses
			// 
			resources.ApplyResources(this.chkAutoEndAppDirProcesses, "chkAutoEndAppDirProcesses");
			this.chkAutoEndAppDirProcesses.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkAutoEndAppDirProcesses.Name = "chkAutoEndAppDirProcesses";
			this.chkAutoEndAppDirProcesses.UseVisualStyleBackColor = true;
			// 
			// chkOptAutoKillProcess
			// 
			resources.ApplyResources(this.chkOptAutoKillProcess, "chkOptAutoKillProcess");
			this.chkOptAutoKillProcess.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkOptAutoKillProcess.Name = "chkOptAutoKillProcess";
			this.chkOptAutoKillProcess.UseVisualStyleBackColor = true;
			// 
			// chkOptMustUpdate
			// 
			resources.ApplyResources(this.chkOptMustUpdate, "chkOptMustUpdate");
			this.chkOptMustUpdate.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkOptMustUpdate.Name = "chkOptMustUpdate";
			this.chkOptMustUpdate.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.fileConfig);
			resources.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.argumentGenerator1);
			resources.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.aboutPanel1);
			resources.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tip
			// 
			this.tip.AutomaticDelay = 100;
			this.tip.AutoPopDelay = 10000;
			this.tip.InitialDelay = 0;
			this.tip.ReshowDelay = 20;
			this.tip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.tip.ToolTipTitle = "提示";
			// 
			// btnSaveProject
			// 
			resources.ApplyResources(this.btnSaveProject, "btnSaveProject");
			this.btnSaveProject.Name = "btnSaveProject";
			this.btnSaveProject.UseVisualStyleBackColor = true;
			this.btnSaveProject.Click += new System.EventHandler(this.btnSaveProject_Click);
			// 
			// btnOpenProject
			// 
			resources.ApplyResources(this.btnOpenProject, "btnOpenProject");
			this.btnOpenProject.Name = "btnOpenProject";
			this.btnOpenProject.UseVisualStyleBackColor = true;
			this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
			// 
			// fileAfterExecute
			// 
			this.fileAfterExecute.AllowDrop = true;
			this.fileAfterExecute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fileAfterExecute.FileTypeFilter = "cmd,exe,bat,com";
			resources.ApplyResources(this.fileAfterExecute, "fileAfterExecute");
			this.fileAfterExecute.Name = "fileAfterExecute";
			this.fileAfterExecute.PreferFileName = null;
			this.fileAfterExecute.RootPath = null;
			this.fileAfterExecute.SelectedFileName = "";
			this.fileAfterExecute.ShowEmptyEntry = true;
			// 
			// filePreExecute
			// 
			this.filePreExecute.AllowDrop = true;
			this.filePreExecute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.filePreExecute.FileTypeFilter = "cmd,exe,bat,com";
			this.filePreExecute.FormattingEnabled = true;
			resources.ApplyResources(this.filePreExecute, "filePreExecute");
			this.filePreExecute.Name = "filePreExecute";
			this.filePreExecute.PreferFileName = null;
			this.filePreExecute.RootPath = null;
			this.filePreExecute.SelectedFileName = "";
			this.filePreExecute.ShowEmptyEntry = true;
			// 
			// options
			// 
			resources.ApplyResources(this.options, "options");
			this.options.Name = "options";
			// 
			// fileConfig
			// 
			resources.ApplyResources(this.fileConfig, "fileConfig");
			this.fileConfig.Name = "fileConfig";
			this.fileConfig.NewVersionFolder = null;
			// 
			// argumentGenerator1
			// 
			resources.ApplyResources(this.argumentGenerator1, "argumentGenerator1");
			this.argumentGenerator1.Name = "argumentGenerator1";
			// 
			// aboutPanel1
			// 
			resources.ApplyResources(this.aboutPanel1, "aboutPanel1");
			this.aboutPanel1.Name = "aboutPanel1";
			// 
			// Main
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnOpenProject);
			this.Controls.Add(this.btnSaveProject);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.pbProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Main";
			((System.ComponentModel.ISupportInitialize)(this.epp)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage6.ResumeLayout(false);
			this.tabPage6.PerformLayout();
			this.tabPage7.ResumeLayout(false);
			this.gpUpdatePing.ResumeLayout(false);
			this.gpUpdatePing.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage8.ResumeLayout(false);
			this.tabPage8.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtAppName;
		private System.Windows.Forms.TextBox txtAppVersion;
		private System.Windows.Forms.TextBox txtPublishUrl;
		private System.Windows.Forms.ProgressBar pbProgress;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private FileComboBox filePreExecute;
		private FileComboBox fileAfterExecute;
		private System.Windows.Forms.TextBox txtPreExecuteArgs;
		private System.Windows.Forms.TextBox txtAfterExecuteArgs;
		private System.Windows.Forms.ErrorProvider epp;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtTimeout;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private Controls.OptionTab options;
		private System.Windows.Forms.Button browseFile;
		private System.Windows.Forms.Button btnBrowseFolder;
		private System.Windows.Forms.TextBox txtPackagePath;
		private System.Windows.Forms.TextBox txtNewSoftDir;
		private System.Windows.Forms.FolderBrowserDialog fbd;
		private System.Windows.Forms.ToolTip tip;
		private System.Windows.Forms.TabPage tabPage3;
		private Controls.FileConfiguration fileConfig;
		private System.Windows.Forms.TabPage tabPage4;
		private AboutPanel aboutPanel1;
		private System.Windows.Forms.CheckBox chkHideAfter;
		private System.Windows.Forms.CheckBox chkHideBefore;
		private System.Windows.Forms.TabPage tabPage5;
		private ArgumentGenerator argumentGenerator1;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtDesc;
		private System.Windows.Forms.TextBox rtfPath;
		private System.Windows.Forms.TextBox txtUrl;
		private System.Windows.Forms.Button btnEditRtf;
		private System.Windows.Forms.Button btnBrowserRtf;
		private System.Windows.Forms.Button btnOpenProject;
		private System.Windows.Forms.Button btnSaveProject;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.GroupBox gpUpdatePing;
		private System.Windows.Forms.TextBox txtPing;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Button btnBind;
		private System.Windows.Forms.LinkLabel lnkBindDescToFile;
		private System.Windows.Forms.TabPage tabPage8;
		private System.Windows.Forms.CheckBox chkOptForceUpdate;
		private System.Windows.Forms.CheckBox chkOptError;
		private System.Windows.Forms.CheckBox chkOptAutoExitProcess;
		private System.Windows.Forms.CheckBox chkAutoEndAppDirProcesses;
		private System.Windows.Forms.CheckBox chkOptAutoKillProcess;
		private System.Windows.Forms.CheckBox chkOptMustUpdate;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox chkStillProptUserInfo;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.CheckBox chkOptRequireAdminPrivilege;
	}
}