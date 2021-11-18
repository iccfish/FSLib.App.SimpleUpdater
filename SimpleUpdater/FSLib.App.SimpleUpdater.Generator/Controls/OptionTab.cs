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
	using Defination;

	using Dialogs;

	using SimpleUpdater.Defination;

	public partial class OptionTab : UserControl
	{
		private AuProject _project;

		public OptionTab()
		{
			InitializeComponent();

			this.deletePreviousFileMode.SelectedIndexChanged += (_, __) =>
			{
				this.gpSetDeleteSyntax.Visible = this.deletePreviousFileMode.SelectedIndex > 0;
				this.gpSetDeleteSyntax.Text = this.deletePreviousFileMode.SelectedIndex == 1 ? "要保留的文件或路径" : "要删除的文件或文件夹";

				if (UpdatePackageBuilder.Instance.AuProject != null)
					UpdatePackageBuilder.Instance.AuProject.UpdateInfo.DeleteMethod = (DeletePreviousProgramMethod)this.deletePreviousFileMode.SelectedIndex;
			};
			this.deletePreviousFileMode.SelectedIndex = 0;
			this.txtPackagePassword.Text = "";
			deleteRules.TextChanged += (s, e) =>
			{
				UpdatePackageBuilder.Instance.AuProject.UpdateInfo.DeleteFileLimits = deleteRules.Lines;
			};
			rbAlways.Click += (s, e) =>
			{
				UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = Utility.SetOrClearUpdateMethodFlag(UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod, UpdateMethod.Always, rbAlways.Checked);
			};
			chkSkipIfNotExist.Click += (s, e) =>
			{
				UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = Utility.SetOrClearUpdateMethodFlag(UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod, UpdateMethod.SkipIfNotExist, chkSkipIfNotExist.Checked);
			};
			rbIgnore.Click += (s, e) =>
			{
				UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = Utility.SetOrClearUpdateMethodFlag(UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod, UpdateMethod.Ignore, rbIgnore.Checked);
			};
			rbOnlyNotExist.Click += (s, e) =>
			{
				UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = Utility.SetOrClearUpdateMethodFlag(UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod, UpdateMethod.SkipIfExists, rbOnlyNotExist.Checked);
			};
			rbVersionCheck.Click += (s, e) =>
			{
				UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = Utility.SetOrClearUpdateMethodFlag(UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod, UpdateMethod.VersionCompare, rbVersionCheck.Checked);

				if (rbVersionCheck.Checked)
				{
					using (var dlg = new SelectVerificationLevel())
					{
						if (dlg.ShowDialog() == DialogResult.OK)
							UpdatePackageBuilder.Instance.AuProject.DefaultFileVerificationLevel = dlg.FileVerificationLevel;
					}
				}
			};
			nudTimeoutSucceed.AddDataBinding(chkAutoCloseSucceed, s => s.Enabled, s => s.Checked);
			nudTimeoutFailed.AddDataBinding(chkAutoCloseFailed, s => s.Enabled, s => s.Checked);
			nudTimeoutFailed.ValueChanged += (sender, args) =>
			{
				if (_project == null)
					return;
				_project.UpdateInfo.AutoCloseFailedTimeout = (int)nudTimeoutFailed.Value;
			};
			nudTimeoutSucceed.ValueChanged += (sender, args) =>
			{
				if (_project == null)
					return;
				_project.UpdateInfo.AutoCloseSucceedTimeout = (int)nudTimeoutSucceed.Value;
			};
			Load += OptionTab_Load;
		}

		void OptionTab_Load(object sender, EventArgs e)
		{
			var upb = UpdatePackageBuilder.Instance;
			upb.ProjectLoaded += upb_ProjectLoaded;
			upb.ProjectClosed += upb_ProjectClosed;

			if (upb.AuProject != null)
				BindProject(upb.AuProject);
		}

		void upb_ProjectClosed(object sender, PackageEventArgs e)
		{
			chkUseIncreaseUpdate.DataBindings.Clear();
			chkCreateCompatiblePackage.DataBindings.Clear();
			chkCompressUpdateInfo.DataBindings.Clear();
			txtPackagePassword.DataBindings.Clear();
			requiredMinVersion.DataBindings.Clear();
			chkAutoCloseSucceed.DataBindings.Clear();
			chkAutoCloseFailed.DataBindings.Clear();
			chkParallelBuilding.DataBindings.Clear();
			chkUseSha1.DataBindings.Clear();

			_project = null;
		}

		void upb_ProjectLoaded(object sender, PackageEventArgs e)
		{
			BindProject(e.AuProject);
		}

		void BindProject(AuProject project)
		{
			var ui = project.UpdateInfo;

			_project = project;
			chkUseIncreaseUpdate.AddDataBinding(project, s => s.Checked, s => s.EnableIncreaseUpdate);
			chkCompressUpdateInfo.AddDataBinding(project, s => s.Checked, s => s.CompressPackage);
			chkCreateCompatiblePackage.AddDataBinding(project, s => s.Checked, s => s.CreateCompatiblePackage);
			chkAutoCloseSucceed.AddDataBinding(ui, s => s.Checked, s => s.AutoCloseSucceedWindow);
			chkAutoCloseFailed.AddDataBinding(ui, s => s.Checked, s => s.AutoCloseFailedDialog);
			chkParallelBuilding.AddDataBinding(_project, s => s.Checked, s => s.UseParallelBuilding);
			chkUseSha1.AddDataBinding(_project, s => s.Checked, auProject => auProject.UsingSha1);
			nudTimeoutFailed.Value = ui.AutoCloseFailedTimeout;
			nudTimeoutSucceed.Value = ui.AutoCloseSucceedTimeout;


			this.deletePreviousFileMode.SelectedIndex = (int)ui.DeleteMethod;
			this.deleteRules.Text = ui.DeleteFileLimits.IsEmpty() ? "" : string.Join(Environment.NewLine, ui.DeleteFileLimits);

			txtPackagePassword.AddDataBinding(ui, s => s.Text, s => s.PackagePassword);
			requiredMinVersion.AddDataBinding(ui, s => s.Text, s => s.RequiredMinVersion);
			project.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "DefaultUpdateMethod" || e.PropertyName == "DefaultFileVerificationLevel")
					RebindDefaultUpdateMethodInfo(s as AuProject);
			};

			RebindDefaultUpdateMethodInfo(project);
		}

		/// <summary>
		/// 重新绑定数据
		/// </summary>
		/// <param name="project"></param>
		void RebindDefaultUpdateMethodInfo(AuProject project)
		{
			rbAlways.Checked = Utility.HasMethod(project.DefaultUpdateMethod, UpdateMethod.Always);
			rbVersionCheck.Checked = Utility.HasMethod(project.DefaultUpdateMethod, UpdateMethod.VersionCompare);
			rbOnlyNotExist.Checked = Utility.HasMethod(project.DefaultUpdateMethod, UpdateMethod.SkipIfExists);
			rbIgnore.Checked = Utility.HasMethod(project.DefaultUpdateMethod, UpdateMethod.Ignore);
			chkSkipIfNotExist.Checked = Utility.HasMethod(project.DefaultUpdateMethod, UpdateMethod.SkipIfNotExist);

			lblCheckTypeDesc.Text = Utility.HasMethod(project.DefaultUpdateMethod, UpdateMethod.VersionCompare) ? project.DefaultFileVerificationLevel.ToDisplayString() : "点击选项时选择比较类型";
		}

	}
}
