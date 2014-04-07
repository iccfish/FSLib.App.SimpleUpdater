using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSLib;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using SimpleUpdater.Defination;

	public partial class OptionTab : UserControl
	{
		/// <summary>
		/// 根据配置更新界面
		/// </summary>
		/// <param name="info"></param>
		public void UpdateInterface(UpdateInfo info)
		{
			this.deletePreviousFileMode.SelectedIndex = (int)info.DeleteMethod;
			this.deleteRules.Text = info.DeleteFileLimits.IsEmpty() ? "" : string.Join(Environment.NewLine, info.DeleteFileLimits);
			this.requiredMinVersion.Text = info.RequiredMinVersion;
			this.txtPackagePassword.Text = info.PackagePassword ?? "";
		}

		/// <summary>
		/// 保存设置到配置中
		/// </summary>
		/// <param name="info"></param>
		public void SaveSetting(UpdateInfo info)
		{
			info.DeleteFileLimits = this.deleteRules.Lines;
			info.DeleteMethod = (FSLib.App.SimpleUpdater.DeletePreviousProgramMethod)this.deletePreviousFileMode.SelectedIndex;
			info.RequiredMinVersion = this.requiredMinVersion.Text;
			info.PackagePassword = this.txtPackagePassword.Text;
		}

		public OptionTab()
		{
			InitializeComponent();

			this.deletePreviousFileMode.SelectedIndexChanged += (_, __) =>
			{
				this.gpSetDeleteSyntax.Visible = this.deletePreviousFileMode.SelectedIndex > 0;
				this.gpSetDeleteSyntax.Text = this.deletePreviousFileMode.SelectedIndex == 1 ? "要保留的文件或路径" : "要删除的文件或文件夹";
			};
			this.deletePreviousFileMode.SelectedIndex = 0;
			this.txtPackagePassword.Text = "";
		}

		/// <summary>
		/// 获得或设置是否使用增量更新
		/// </summary>
		public bool EnableIncreaseUpdate
		{
			get { return chkUseIncreaseUpdate.Checked; }
			set { chkUseIncreaseUpdate.Checked = value; }
		}


		/// <summary>
		/// 获得或设置是否创建兼容的升级包选项
		/// </summary>
		public bool CreateCompatiblePackage
		{
			get { return chkCreateCompatiblePackage.Checked; }
			set { chkCreateCompatiblePackage.Checked = value; }
		}

		/// <summary>
		/// 获得或设置是否压缩XML文件选项
		/// </summary>
		public bool CompressXmlFile
		{
			get { return chkCompressUpdateInfo.Checked; }
			set { chkCompressUpdateInfo.Checked = value; }
		}
	}
}
