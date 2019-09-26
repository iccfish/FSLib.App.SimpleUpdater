using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	using System.Windows.Forms;

	using Defination;

	class ProjectEditControl : UserControl
	{
		public ProjectEditControl()
		{
			if (!Program.Running)
				return;

			var builder = UpdatePackageBuilder.Instance;
			builder.ProjectClosed += Builder_ProjectClosed;
			builder.ProjectLoaded += Builder_ProjectLoaded;
			this.Disposed += (s, e) =>
			{
				Builder_ProjectClosed(null, null);
				builder.ProjectClosed -= Builder_ProjectClosed;
				builder.ProjectLoaded -= Builder_ProjectLoaded;
			};
		}

		private void Builder_ProjectLoaded(object sender, PackageEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Builder_ProjectClosed(object sender, PackageEventArgs e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获得当前项目
		/// </summary>
		protected AuProject Project => UpdatePackageBuilder.Instance.AuProject;

		/// <summary>
		/// 正在加载项目
		/// </summary>
		protected event EventHandler ProjectLoading;

		/// <summary>
		/// 已加载项目
		/// </summary>
		protected event EventHandler ProjectLoaded;

		/// <summary>
		/// 正在关闭项目
		/// </summary>
		protected event EventHandler ProjectClosing;

		/// <summary>
		/// 已关闭项目
		/// </summary>
		protected event EventHandler ProjectClosed;

		/// <summary>
		/// 正在保存项目
		/// </summary>
		protected event EventHandler ProjectSaving;

		/// <summary>
		/// 已保存项目
		/// </summary>
		protected event EventHandler ProjectSaved;

		protected virtual void OnProjectClosed() { ProjectClosed?.Invoke(this, EventArgs.Empty); }

		protected virtual void OnProjectLoading() { ProjectLoading?.Invoke(this, EventArgs.Empty); }

		protected virtual void OnProjectLoaded() { ProjectLoaded?.Invoke(this, EventArgs.Empty); }

		protected virtual void OnProjectClosing() { ProjectClosing?.Invoke(this, EventArgs.Empty); }

		protected virtual void OnProjectSaving() { ProjectSaving?.Invoke(this, EventArgs.Empty); }

		protected virtual void OnProjectSaved() { ProjectSaved?.Invoke(this, EventArgs.Empty); }
	}
}
