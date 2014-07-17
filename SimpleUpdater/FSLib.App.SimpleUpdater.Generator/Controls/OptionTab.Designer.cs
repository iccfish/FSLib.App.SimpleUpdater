namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	partial class OptionTab
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.deletePreviousFileMode = new System.Windows.Forms.ComboBox();
			this.deleteRules = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.gpSetDeleteSyntax = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.requiredMinVersion = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPackagePassword = new System.Windows.Forms.TextBox();
			this.tip = new System.Windows.Forms.ToolTip(this.components);
			this.chkUseIncreaseUpdate = new System.Windows.Forms.CheckBox();
			this.chkCreateCompatiblePackage = new System.Windows.Forms.CheckBox();
			this.chkCompressUpdateInfo = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCheckTypeDesc = new System.Windows.Forms.Label();
			this.rbVersionCheck = new System.Windows.Forms.RadioButton();
			this.rbOnlyNotExist = new System.Windows.Forms.RadioButton();
			this.rbIgnore = new System.Windows.Forms.RadioButton();
			this.rbAlways = new System.Windows.Forms.RadioButton();
			this.gpSetDeleteSyntax.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(3, 173);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "更新时删除原始文件";
			// 
			// deletePreviousFileMode
			// 
			this.deletePreviousFileMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.deletePreviousFileMode.FormattingEnabled = true;
			this.deletePreviousFileMode.Items.AddRange(new object[] {
            "仅覆盖, 不主动删除",
            "清空原程序目录",
            "仅删除指定文件和目录"});
			this.deletePreviousFileMode.Location = new System.Drawing.Point(150, 169);
			this.deletePreviousFileMode.Name = "deletePreviousFileMode";
			this.deletePreviousFileMode.Size = new System.Drawing.Size(218, 20);
			this.deletePreviousFileMode.TabIndex = 1;
			// 
			// deleteRules
			// 
			this.deleteRules.Location = new System.Drawing.Point(5, 20);
			this.deleteRules.Multiline = true;
			this.deleteRules.Name = "deleteRules";
			this.deleteRules.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.deleteRules.Size = new System.Drawing.Size(694, 125);
			this.deleteRules.TabIndex = 2;
			this.deleteRules.WordWrap = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 148);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(512, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "使用相对路径判断，不包括程序所在目录；使用正则表达式语法，一行一条记录。";
			// 
			// gpSetDeleteSyntax
			// 
			this.gpSetDeleteSyntax.Controls.Add(this.deleteRules);
			this.gpSetDeleteSyntax.Controls.Add(this.label2);
			this.gpSetDeleteSyntax.Location = new System.Drawing.Point(7, 195);
			this.gpSetDeleteSyntax.Name = "gpSetDeleteSyntax";
			this.gpSetDeleteSyntax.Size = new System.Drawing.Size(705, 167);
			this.gpSetDeleteSyntax.TabIndex = 4;
			this.gpSetDeleteSyntax.TabStop = false;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new System.Drawing.Point(5, 14);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(122, 12);
			this.label8.TabIndex = 5;
			this.label8.Text = "支持更新的最低版本";
			// 
			// requiredMinVersion
			// 
			this.requiredMinVersion.Location = new System.Drawing.Point(133, 11);
			this.requiredMinVersion.Name = "requiredMinVersion";
			this.requiredMinVersion.Size = new System.Drawing.Size(117, 21);
			this.requiredMinVersion.TabIndex = 6;
			this.tip.SetToolTip(this.requiredMinVersion, "低于此版本的软件将会要求用户进行手动更新");
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(266, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "升级文件包密码";
			// 
			// txtPackagePassword
			// 
			this.txtPackagePassword.Location = new System.Drawing.Point(368, 10);
			this.txtPackagePassword.Name = "txtPackagePassword";
			this.txtPackagePassword.PasswordChar = '*';
			this.txtPackagePassword.Size = new System.Drawing.Size(117, 21);
			this.txtPackagePassword.TabIndex = 8;
			this.tip.SetToolTip(this.txtPackagePassword, "用于加密生成的压缩文件包");
			// 
			// tip
			// 
			this.tip.AutoPopDelay = 10000;
			this.tip.InitialDelay = 500;
			this.tip.IsBalloon = true;
			this.tip.ReshowDelay = 500;
			this.tip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.tip.ToolTipTitle = "帮助";
			// 
			// chkUseIncreaseUpdate
			// 
			this.chkUseIncreaseUpdate.AutoSize = true;
			this.chkUseIncreaseUpdate.Checked = true;
			this.chkUseIncreaseUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUseIncreaseUpdate.Location = new System.Drawing.Point(522, 13);
			this.chkUseIncreaseUpdate.Name = "chkUseIncreaseUpdate";
			this.chkUseIncreaseUpdate.Size = new System.Drawing.Size(96, 16);
			this.chkUseIncreaseUpdate.TabIndex = 9;
			this.chkUseIncreaseUpdate.Text = "使用增量更新";
			this.chkUseIncreaseUpdate.UseVisualStyleBackColor = true;
			// 
			// chkCreateCompatiblePackage
			// 
			this.chkCreateCompatiblePackage.Checked = true;
			this.chkCreateCompatiblePackage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCreateCompatiblePackage.Location = new System.Drawing.Point(7, 78);
			this.chkCreateCompatiblePackage.Name = "chkCreateCompatiblePackage";
			this.chkCreateCompatiblePackage.Size = new System.Drawing.Size(705, 33);
			this.chkCreateCompatiblePackage.TabIndex = 9;
			this.chkCreateCompatiblePackage.Text = "创建兼容的升级信息包。如果不选择此选项且有文件使用增量更新方式发布，在2.0.0.0版本之前（不支持增量更新的自动更新客户端）将无法使用此处的安装包进行更新。";
			this.chkCreateCompatiblePackage.UseVisualStyleBackColor = true;
			// 
			// chkCompressUpdateInfo
			// 
			this.chkCompressUpdateInfo.Checked = true;
			this.chkCompressUpdateInfo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCompressUpdateInfo.Location = new System.Drawing.Point(7, 37);
			this.chkCompressUpdateInfo.Name = "chkCompressUpdateInfo";
			this.chkCompressUpdateInfo.Size = new System.Drawing.Size(705, 44);
			this.chkCompressUpdateInfo.TabIndex = 9;
			this.chkCompressUpdateInfo.Text = "生成压缩版的升级信息文件。这将有助于在启用增量更新时大幅减小升级信息文件大小。请使用 “update_c.xml” 作为升级信息文件名。建议选中『创建兼容的升级信" +
    "息包』，否则2.0.0.0之前的自动升级客户端将无法执行升级。";
			this.chkCompressUpdateInfo.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label4.Location = new System.Drawing.Point(5, 125);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(135, 12);
			this.label4.TabIndex = 10;
			this.label4.Text = "默认文件检测更新模式";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblCheckTypeDesc);
			this.panel1.Controls.Add(this.rbVersionCheck);
			this.panel1.Controls.Add(this.rbOnlyNotExist);
			this.panel1.Controls.Add(this.rbIgnore);
			this.panel1.Controls.Add(this.rbAlways);
			this.panel1.Location = new System.Drawing.Point(153, 113);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(552, 37);
			this.panel1.TabIndex = 11;
			// 
			// lblCheckTypeDesc
			// 
			this.lblCheckTypeDesc.AutoSize = true;
			this.lblCheckTypeDesc.Location = new System.Drawing.Point(378, 12);
			this.lblCheckTypeDesc.Name = "lblCheckTypeDesc";
			this.lblCheckTypeDesc.Size = new System.Drawing.Size(137, 12);
			this.lblCheckTypeDesc.TabIndex = 1;
			this.lblCheckTypeDesc.Text = "点击选项时选择比较类型";
			// 
			// rbVersionCheck
			// 
			this.rbVersionCheck.AutoSize = true;
			this.rbVersionCheck.Checked = true;
			this.rbVersionCheck.Location = new System.Drawing.Point(277, 10);
			this.rbVersionCheck.Name = "rbVersionCheck";
			this.rbVersionCheck.Size = new System.Drawing.Size(95, 16);
			this.rbVersionCheck.TabIndex = 0;
			this.rbVersionCheck.TabStop = true;
			this.rbVersionCheck.Text = "比较版本更新";
			this.rbVersionCheck.UseVisualStyleBackColor = true;
			// 
			// rbOnlyNotExist
			// 
			this.rbOnlyNotExist.AutoSize = true;
			this.rbOnlyNotExist.Location = new System.Drawing.Point(169, 10);
			this.rbOnlyNotExist.Name = "rbOnlyNotExist";
			this.rbOnlyNotExist.Size = new System.Drawing.Size(95, 16);
			this.rbOnlyNotExist.TabIndex = 0;
			this.rbOnlyNotExist.Text = "不存在时更新";
			this.rbOnlyNotExist.UseVisualStyleBackColor = true;
			// 
			// rbIgnore
			// 
			this.rbIgnore.AutoSize = true;
			this.rbIgnore.Location = new System.Drawing.Point(15, 10);
			this.rbIgnore.Name = "rbIgnore";
			this.rbIgnore.Size = new System.Drawing.Size(71, 16);
			this.rbIgnore.TabIndex = 0;
			this.rbIgnore.Text = "忽略更新";
			this.rbIgnore.UseVisualStyleBackColor = true;
			// 
			// rbAlways
			// 
			this.rbAlways.AutoSize = true;
			this.rbAlways.Location = new System.Drawing.Point(92, 10);
			this.rbAlways.Name = "rbAlways";
			this.rbAlways.Size = new System.Drawing.Size(71, 16);
			this.rbAlways.TabIndex = 0;
			this.rbAlways.Text = "始终更新";
			this.rbAlways.UseVisualStyleBackColor = true;
			// 
			// OptionTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.chkCreateCompatiblePackage);
			this.Controls.Add(this.chkCompressUpdateInfo);
			this.Controls.Add(this.chkUseIncreaseUpdate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtPackagePassword);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.requiredMinVersion);
			this.Controls.Add(this.gpSetDeleteSyntax);
			this.Controls.Add(this.deletePreviousFileMode);
			this.Controls.Add(this.label1);
			this.Name = "OptionTab";
			this.Size = new System.Drawing.Size(732, 367);
			this.gpSetDeleteSyntax.ResumeLayout(false);
			this.gpSetDeleteSyntax.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox deletePreviousFileMode;
		private System.Windows.Forms.TextBox deleteRules;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox gpSetDeleteSyntax;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox requiredMinVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPackagePassword;
		private System.Windows.Forms.ToolTip tip;
		private System.Windows.Forms.CheckBox chkUseIncreaseUpdate;
		private System.Windows.Forms.CheckBox chkCreateCompatiblePackage;
		private System.Windows.Forms.CheckBox chkCompressUpdateInfo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblCheckTypeDesc;
		private System.Windows.Forms.RadioButton rbVersionCheck;
		private System.Windows.Forms.RadioButton rbOnlyNotExist;
		private System.Windows.Forms.RadioButton rbAlways;
		private System.Windows.Forms.RadioButton rbIgnore;
	}
}
