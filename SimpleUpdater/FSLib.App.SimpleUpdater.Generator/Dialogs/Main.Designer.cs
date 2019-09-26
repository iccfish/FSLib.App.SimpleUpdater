namespace FSLib.App.SimpleUpdater.Generator.Dialogs
{
	using Controls;

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
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tpInfo = new System.Windows.Forms.TabPage();
			this.chkRandomPackageName = new System.Windows.Forms.CheckBox();
			this.chkCleanTargetDirectory = new System.Windows.Forms.CheckBox();
			this.btnBind = new System.Windows.Forms.Button();
			this.chkHideAfter = new System.Windows.Forms.CheckBox();
			this.chkHideBefore = new System.Windows.Forms.CheckBox();
			this.browseFile = new System.Windows.Forms.Button();
			this.btnBrowseFolder = new System.Windows.Forms.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.txtCompFlag = new System.Windows.Forms.TextBox();
			this.txtPackageExtension = new System.Windows.Forms.TextBox();
			this.fileAfterExecute = new FSLib.App.SimpleUpdater.Generator.Controls.FileComboBox();
			this.filePreExecute = new FSLib.App.SimpleUpdater.Generator.Controls.FileComboBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.tpDesc = new System.Windows.Forms.TabPage();
			this.lnkBindDescToFile = new System.Windows.Forms.LinkLabel();
			this.btnClearRtf = new System.Windows.Forms.Button();
			this.btnEditRtf = new System.Windows.Forms.Button();
			this.btnBrowserRtf = new System.Windows.Forms.Button();
			this.rtfPath = new System.Windows.Forms.TextBox();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.tpServerOpt = new System.Windows.Forms.TabPage();
			this.gpUpdatePing = new System.Windows.Forms.GroupBox();
			this.txtPing = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tpOption1 = new System.Windows.Forms.TabPage();
			this.options = new FSLib.App.SimpleUpdater.Generator.Controls.OptionTab();
			this.tpOption2 = new System.Windows.Forms.TabPage();
			this.label18 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.chkOptForceUpdate = new System.Windows.Forms.CheckBox();
			this.chkOptRequireAdminPrivilege = new System.Windows.Forms.CheckBox();
			this.chkAutoCloseSucceed = new System.Windows.Forms.CheckBox();
			this.chkStillProptUserInfo = new System.Windows.Forms.CheckBox();
			this.chkOptError = new System.Windows.Forms.CheckBox();
			this.chkOptAutoExitProcess = new System.Windows.Forms.CheckBox();
			this.chkAutoEndAppDirProcesses = new System.Windows.Forms.CheckBox();
			this.chkOptAutoKillProcess = new System.Windows.Forms.CheckBox();
			this.chkOptMustUpdate = new System.Windows.Forms.CheckBox();
			this.tpFile = new System.Windows.Forms.TabPage();
			this.fileConfig = new FSLib.App.SimpleUpdater.Generator.Controls.FileConfiguration();
			this.tpTheme = new System.Windows.Forms.TabPage();
			this.tpCmd = new System.Windows.Forms.TabPage();
			this.argumentGenerator1 = new FSLib.App.SimpleUpdater.Generator.Controls.ArgumentGenerator();
			this.tpAbout = new System.Windows.Forms.TabPage();
			this.aboutPanel1 = new FSLib.App.SimpleUpdater.Generator.Controls.AboutPanel();
			this.illist = new System.Windows.Forms.ImageList(this.components);
			this.fbd = new System.Windows.Forms.FolderBrowserDialog();
			this.tip = new System.Windows.Forms.ToolTip(this.components);
			this.btnSaveProject = new System.Windows.Forms.Button();
			this.btnOpenProject = new System.Windows.Forms.Button();
			this.themeConfig1 = new FSLib.App.SimpleUpdater.Generator.Controls.ThemeConfig();
			((System.ComponentModel.ISupportInitialize)(this.epp)).BeginInit();
			this.tcMain.SuspendLayout();
			this.tpInfo.SuspendLayout();
			this.tpDesc.SuspendLayout();
			this.tpServerOpt.SuspendLayout();
			this.gpUpdatePing.SuspendLayout();
			this.tpOption1.SuspendLayout();
			this.tpOption2.SuspendLayout();
			this.tpFile.SuspendLayout();
			this.tpTheme.SuspendLayout();
			this.tpCmd.SuspendLayout();
			this.tpAbout.SuspendLayout();
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
			this.tip.SetToolTip(this.txtAppName, resources.GetString("txtAppName.ToolTip"));
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
			this.tip.SetToolTip(this.txtTimeout, resources.GetString("txtTimeout.ToolTip"));
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
			// tcMain
			// 
			this.tcMain.Controls.Add(this.tpInfo);
			this.tcMain.Controls.Add(this.tpDesc);
			this.tcMain.Controls.Add(this.tpServerOpt);
			this.tcMain.Controls.Add(this.tpOption1);
			this.tcMain.Controls.Add(this.tpOption2);
			this.tcMain.Controls.Add(this.tpFile);
			this.tcMain.Controls.Add(this.tpTheme);
			this.tcMain.Controls.Add(this.tpCmd);
			this.tcMain.Controls.Add(this.tpAbout);
			this.tcMain.ImageList = this.illist;
			resources.ApplyResources(this.tcMain, "tcMain");
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			// 
			// tpInfo
			// 
			this.tpInfo.Controls.Add(this.chkRandomPackageName);
			this.tpInfo.Controls.Add(this.chkCleanTargetDirectory);
			this.tpInfo.Controls.Add(this.btnBind);
			this.tpInfo.Controls.Add(this.chkHideAfter);
			this.tpInfo.Controls.Add(this.chkHideBefore);
			this.tpInfo.Controls.Add(this.browseFile);
			this.tpInfo.Controls.Add(this.btnBrowseFolder);
			this.tpInfo.Controls.Add(this.txtPackagePath);
			this.tpInfo.Controls.Add(this.txtNewSoftDir);
			this.tpInfo.Controls.Add(this.label1);
			this.tpInfo.Controls.Add(this.label2);
			this.tpInfo.Controls.Add(this.label25);
			this.tpInfo.Controls.Add(this.label23);
			this.tpInfo.Controls.Add(this.label10);
			this.tpInfo.Controls.Add(this.label3);
			this.tpInfo.Controls.Add(this.txtCompFlag);
			this.tpInfo.Controls.Add(this.txtPackageExtension);
			this.tpInfo.Controls.Add(this.txtTimeout);
			this.tpInfo.Controls.Add(this.label4);
			this.tpInfo.Controls.Add(this.txtAfterExecuteArgs);
			this.tpInfo.Controls.Add(this.label5);
			this.tpInfo.Controls.Add(this.txtPreExecuteArgs);
			this.tpInfo.Controls.Add(this.label6);
			this.tpInfo.Controls.Add(this.fileAfterExecute);
			this.tpInfo.Controls.Add(this.label7);
			this.tpInfo.Controls.Add(this.filePreExecute);
			this.tpInfo.Controls.Add(this.label24);
			this.tpInfo.Controls.Add(this.label22);
			this.tpInfo.Controls.Add(this.label9);
			this.tpInfo.Controls.Add(this.txtAppName);
			this.tpInfo.Controls.Add(this.txtPublishUrl);
			this.tpInfo.Controls.Add(this.txtAppVersion);
			resources.ApplyResources(this.tpInfo, "tpInfo");
			this.tpInfo.Name = "tpInfo";
			this.tpInfo.UseVisualStyleBackColor = true;
			// 
			// chkRandomPackageName
			// 
			resources.ApplyResources(this.chkRandomPackageName, "chkRandomPackageName");
			this.chkRandomPackageName.Name = "chkRandomPackageName";
			this.tip.SetToolTip(this.chkRandomPackageName, resources.GetString("chkRandomPackageName.ToolTip"));
			this.chkRandomPackageName.UseVisualStyleBackColor = true;
			// 
			// chkCleanTargetDirectory
			// 
			resources.ApplyResources(this.chkCleanTargetDirectory, "chkCleanTargetDirectory");
			this.chkCleanTargetDirectory.ForeColor = System.Drawing.Color.Red;
			this.chkCleanTargetDirectory.Name = "chkCleanTargetDirectory";
			this.tip.SetToolTip(this.chkCleanTargetDirectory, resources.GetString("chkCleanTargetDirectory.ToolTip"));
			this.chkCleanTargetDirectory.UseVisualStyleBackColor = true;
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
			// label25
			// 
			resources.ApplyResources(this.label25, "label25");
			this.label25.Name = "label25";
			// 
			// label23
			// 
			resources.ApplyResources(this.label23, "label23");
			this.label23.Name = "label23";
			// 
			// txtCompFlag
			// 
			resources.ApplyResources(this.txtCompFlag, "txtCompFlag");
			this.txtCompFlag.Name = "txtCompFlag";
			// 
			// txtPackageExtension
			// 
			resources.ApplyResources(this.txtPackageExtension, "txtPackageExtension");
			this.txtPackageExtension.Name = "txtPackageExtension";
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
			this.tip.SetToolTip(this.fileAfterExecute, resources.GetString("fileAfterExecute.ToolTip"));
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
			this.tip.SetToolTip(this.filePreExecute, resources.GetString("filePreExecute.ToolTip"));
			// 
			// label24
			// 
			resources.ApplyResources(this.label24, "label24");
			this.label24.Name = "label24";
			// 
			// label22
			// 
			resources.ApplyResources(this.label22, "label22");
			this.label22.Name = "label22";
			// 
			// tpDesc
			// 
			this.tpDesc.Controls.Add(this.lnkBindDescToFile);
			this.tpDesc.Controls.Add(this.btnClearRtf);
			this.tpDesc.Controls.Add(this.btnEditRtf);
			this.tpDesc.Controls.Add(this.btnBrowserRtf);
			this.tpDesc.Controls.Add(this.rtfPath);
			this.tpDesc.Controls.Add(this.txtUrl);
			this.tpDesc.Controls.Add(this.label13);
			this.tpDesc.Controls.Add(this.label12);
			this.tpDesc.Controls.Add(this.label11);
			this.tpDesc.Controls.Add(this.label8);
			this.tpDesc.Controls.Add(this.txtDesc);
			resources.ApplyResources(this.tpDesc, "tpDesc");
			this.tpDesc.Name = "tpDesc";
			this.tpDesc.UseVisualStyleBackColor = true;
			// 
			// lnkBindDescToFile
			// 
			resources.ApplyResources(this.lnkBindDescToFile, "lnkBindDescToFile");
			this.lnkBindDescToFile.Name = "lnkBindDescToFile";
			this.lnkBindDescToFile.TabStop = true;
			this.tip.SetToolTip(this.lnkBindDescToFile, resources.GetString("lnkBindDescToFile.ToolTip"));
			// 
			// btnClearRtf
			// 
			resources.ApplyResources(this.btnClearRtf, "btnClearRtf");
			this.btnClearRtf.Name = "btnClearRtf";
			this.btnClearRtf.UseVisualStyleBackColor = true;
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
			// tpServerOpt
			// 
			this.tpServerOpt.Controls.Add(this.gpUpdatePing);
			resources.ApplyResources(this.tpServerOpt, "tpServerOpt");
			this.tpServerOpt.Name = "tpServerOpt";
			this.tpServerOpt.UseVisualStyleBackColor = true;
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
			// tpOption1
			// 
			this.tpOption1.Controls.Add(this.options);
			resources.ApplyResources(this.tpOption1, "tpOption1");
			this.tpOption1.Name = "tpOption1";
			this.tpOption1.UseVisualStyleBackColor = true;
			// 
			// options
			// 
			resources.ApplyResources(this.options, "options");
			this.options.Name = "options";
			// 
			// tpOption2
			// 
			this.tpOption2.Controls.Add(this.label18);
			this.tpOption2.Controls.Add(this.label20);
			this.tpOption2.Controls.Add(this.label21);
			this.tpOption2.Controls.Add(this.label19);
			this.tpOption2.Controls.Add(this.label17);
			this.tpOption2.Controls.Add(this.label16);
			this.tpOption2.Controls.Add(this.label15);
			this.tpOption2.Controls.Add(this.chkOptForceUpdate);
			this.tpOption2.Controls.Add(this.chkOptRequireAdminPrivilege);
			this.tpOption2.Controls.Add(this.chkAutoCloseSucceed);
			this.tpOption2.Controls.Add(this.chkStillProptUserInfo);
			this.tpOption2.Controls.Add(this.chkOptError);
			this.tpOption2.Controls.Add(this.chkOptAutoExitProcess);
			this.tpOption2.Controls.Add(this.chkAutoEndAppDirProcesses);
			this.tpOption2.Controls.Add(this.chkOptAutoKillProcess);
			this.tpOption2.Controls.Add(this.chkOptMustUpdate);
			resources.ApplyResources(this.tpOption2, "tpOption2");
			this.tpOption2.Name = "tpOption2";
			this.tpOption2.UseVisualStyleBackColor = true;
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
			// chkAutoCloseSucceed
			// 
			resources.ApplyResources(this.chkAutoCloseSucceed, "chkAutoCloseSucceed");
			this.chkAutoCloseSucceed.ForeColor = System.Drawing.Color.MediumVioletRed;
			this.chkAutoCloseSucceed.Name = "chkAutoCloseSucceed";
			this.chkAutoCloseSucceed.UseVisualStyleBackColor = true;
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
			// tpFile
			// 
			this.tpFile.Controls.Add(this.fileConfig);
			resources.ApplyResources(this.tpFile, "tpFile");
			this.tpFile.Name = "tpFile";
			this.tpFile.UseVisualStyleBackColor = true;
			// 
			// fileConfig
			// 
			resources.ApplyResources(this.fileConfig, "fileConfig");
			this.fileConfig.Name = "fileConfig";
			this.fileConfig.NewVersionFolder = null;
			// 
			// tpTheme
			// 
			this.tpTheme.Controls.Add(this.themeConfig1);
			resources.ApplyResources(this.tpTheme, "tpTheme");
			this.tpTheme.Name = "tpTheme";
			this.tpTheme.UseVisualStyleBackColor = true;
			// 
			// tpCmd
			// 
			this.tpCmd.Controls.Add(this.argumentGenerator1);
			resources.ApplyResources(this.tpCmd, "tpCmd");
			this.tpCmd.Name = "tpCmd";
			this.tpCmd.UseVisualStyleBackColor = true;
			// 
			// argumentGenerator1
			// 
			resources.ApplyResources(this.argumentGenerator1, "argumentGenerator1");
			this.argumentGenerator1.Name = "argumentGenerator1";
			// 
			// tpAbout
			// 
			this.tpAbout.Controls.Add(this.aboutPanel1);
			resources.ApplyResources(this.tpAbout, "tpAbout");
			this.tpAbout.Name = "tpAbout";
			this.tpAbout.UseVisualStyleBackColor = true;
			// 
			// aboutPanel1
			// 
			resources.ApplyResources(this.aboutPanel1, "aboutPanel1");
			this.aboutPanel1.Name = "aboutPanel1";
			// 
			// illist
			// 
			this.illist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("illist.ImageStream")));
			this.illist.TransparentColor = System.Drawing.Color.Transparent;
			this.illist.Images.SetKeyName(0, "info_16.png");
			this.illist.Images.SetKeyName(1, "bubble_16.png");
			this.illist.Images.SetKeyName(2, "monitor_16.png");
			this.illist.Images.SetKeyName(3, "gear_16.png");
			this.illist.Images.SetKeyName(4, "folder_16.png");
			this.illist.Images.SetKeyName(5, "wand_16.png");
			this.illist.Images.SetKeyName(6, "preferences-desktop-theme.png");
			this.illist.Images.SetKeyName(7, "globe_16.png");
			// 
			// tip
			// 
			this.tip.AutomaticDelay = 0;
			this.tip.AutoPopDelay = 20000;
			this.tip.InitialDelay = 20;
			this.tip.ReshowDelay = 100;
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
			// themeConfig1
			// 
			resources.ApplyResources(this.themeConfig1, "themeConfig1");
			this.themeConfig1.Name = "themeConfig1";
			// 
			// Main
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnOpenProject);
			this.Controls.Add(this.btnSaveProject);
			this.Controls.Add(this.tcMain);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.pbProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Main";
			((System.ComponentModel.ISupportInitialize)(this.epp)).EndInit();
			this.tcMain.ResumeLayout(false);
			this.tpInfo.ResumeLayout(false);
			this.tpInfo.PerformLayout();
			this.tpDesc.ResumeLayout(false);
			this.tpDesc.PerformLayout();
			this.tpServerOpt.ResumeLayout(false);
			this.gpUpdatePing.ResumeLayout(false);
			this.gpUpdatePing.PerformLayout();
			this.tpOption1.ResumeLayout(false);
			this.tpOption2.ResumeLayout(false);
			this.tpOption2.PerformLayout();
			this.tpFile.ResumeLayout(false);
			this.tpTheme.ResumeLayout(false);
			this.tpCmd.ResumeLayout(false);
			this.tpAbout.ResumeLayout(false);
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
		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.TabPage tpInfo;
		private System.Windows.Forms.TabPage tpOption1;
		private Controls.OptionTab options;
		private System.Windows.Forms.Button browseFile;
		private System.Windows.Forms.Button btnBrowseFolder;
		private System.Windows.Forms.TextBox txtPackagePath;
		private System.Windows.Forms.TextBox txtNewSoftDir;
		private System.Windows.Forms.FolderBrowserDialog fbd;
		private System.Windows.Forms.ToolTip tip;
		private System.Windows.Forms.TabPage tpFile;
		private Controls.FileConfiguration fileConfig;
		private System.Windows.Forms.TabPage tpAbout;
		private AboutPanel aboutPanel1;
		private System.Windows.Forms.CheckBox chkHideAfter;
		private System.Windows.Forms.CheckBox chkHideBefore;
		private System.Windows.Forms.TabPage tpCmd;
		private ArgumentGenerator argumentGenerator1;
		private System.Windows.Forms.TabPage tpDesc;
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
		private System.Windows.Forms.TabPage tpServerOpt;
		private System.Windows.Forms.GroupBox gpUpdatePing;
		private System.Windows.Forms.TextBox txtPing;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Button btnBind;
		private System.Windows.Forms.LinkLabel lnkBindDescToFile;
		private System.Windows.Forms.TabPage tpOption2;
		private System.Windows.Forms.CheckBox chkOptForceUpdate;
		private System.Windows.Forms.CheckBox chkOptError;
		private System.Windows.Forms.CheckBox chkOptAutoExitProcess;
		private System.Windows.Forms.CheckBox chkOptAutoKillProcess;
		private System.Windows.Forms.CheckBox chkOptMustUpdate;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox chkStillProptUserInfo;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.CheckBox chkOptRequireAdminPrivilege;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.TextBox txtPackageExtension;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.CheckBox chkRandomPackageName;
		private System.Windows.Forms.CheckBox chkCleanTargetDirectory;
		private System.Windows.Forms.TextBox txtCompFlag;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Button btnClearRtf;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.CheckBox chkAutoEndAppDirProcesses;
		private System.Windows.Forms.CheckBox chkAutoCloseSucceed;
		private System.Windows.Forms.TabPage tpTheme;
		private System.Windows.Forms.ImageList illist;
		private ThemeConfig themeConfig1;
	}
}