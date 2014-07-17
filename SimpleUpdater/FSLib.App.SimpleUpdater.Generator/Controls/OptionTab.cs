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
			rbAlways.CheckedChanged += (s, e) =>
			{
				if (rbAlways.Checked)
					UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = UpdateMethod.Always;
			};
			rbIgnore.CheckedChanged += (s, e) =>
			{
				if (rbIgnore.Checked)
					UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = UpdateMethod.Ignore;
			};
			rbOnlyNotExist.CheckedChanged += (s, e) =>
			{
				if (rbOnlyNotExist.Checked)
					UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = UpdateMethod.SkipIfExists;
			};
			rbVersionCheck.CheckedChanged += (s, e) =>
			{
				if (rbVersionCheck.Checked)
				{
					UpdatePackageBuilder.Instance.AuProject.DefaultUpdateMethod = UpdateMethod.VersionCompare;

					using (var dlg = new SelectVerificationLevel())
					{
						if (dlg.ShowDialog() == DialogResult.OK)
							UpdatePackageBuilder.Instance.AuProject.DefaultFileVerificationLevel = dlg.FileVerificationLevel;
					}
				}
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
		}

		void upb_ProjectLoaded(object sender, PackageEventArgs e)
		{
			BindProject(e.AuProject);
		}

		void BindProject(AuProject project)
		{
			var ui = project.UpdateInfo;

			chkUseIncreaseUpdate.AddDataBinding(project, s => s.Checked, s => s.EnableIncreaseUpdate);
			chkCompressUpdateInfo.AddDataBinding(project, s => s.Checked, s => s.CompressPackage);
			chkCreateCompatiblePackage.AddDataBinding(project, s => s.Checked, s => s.CreateCompatiblePackage);

			this.deletePreviousFileMode.SelectedIndex = (int)ui.DeleteMethod;
			this.deleteRules.Text = ui.DeleteFileLimits.IsEmpty() ? "" : string.Join(Environment.NewLine, ui.DeleteFileLimits);

			txtPackagePassword.AddDataBinding(ui, s => s.Text, s => s.PackagePassword);
			requiredMinVersion.AddDataBinding(ui, s => s.Text, s => s.RequiredMinVersion);
			ui.PropertyChanged += (s, e) =>
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

			switch (project.DefaultUpdateMethod)
			{
				case UpdateMethod.Always:
					rbAlways.Checked = true;
					break;
				case UpdateMethod.VersionCompare:
					rbVersionCheck.Checked = true;
					break;
				case UpdateMethod.SkipIfExists:
					rbOnlyNotExist.Checked = true;
					break;
				case UpdateMethod.Ignore:
					rbIgnore.Checked = true;
					break;
				default:
					break;
			}
			lblCheckTypeDesc.Text = project.DefaultUpdateMethod == UpdateMethod.VersionCompare ? project.DefaultFileVerificationLevel.ToDisplayString() : "点击选项时选择比较类型";
		}

	}
}
