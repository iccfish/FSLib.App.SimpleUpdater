using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.BuilderInterface
{
	using System.Windows.Forms;

	using Defination;

	using FormBuildUi;

	using SimpleUpdater.Defination;

	using Wrapper;

	class FormBuildInterface : BuilderInterfaceBase
	{
		MiniBuildUi _form;

		/// <summary>
		/// 创建 <see cref="FormBuildInterface" />  的新实例(FormBuildInterface)
		/// </summary>
		public FormBuildInterface()
		{
			_form = new MiniBuildUi();
		}

		/// <summary>
		/// 构建项目
		/// </summary>
		/// <param name="filepath"></param>
		public override void Build(string filepath)
		{
			base.Build(filepath);
			_form.ShowDialog();
		}

		protected override void BuildSuccess(AuProject project, Dictionary<string, string> packages, UpdateInfo resultUpdateInfo)
		{
			_form.SetSuccess();
			System.Windows.Forms.MessageBox.Show("更新项目构建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			base.BuildSuccess(project, packages, resultUpdateInfo);
		}

		protected override void BuilderFailed(AuProject project, Exception exception, UpdateInfo resultUpdateInfo)
		{
			_form.SetFailed();
			System.Windows.Forms.MessageBox.Show("更新项目构建失败：" + exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			base.BuilderFailed(project, exception, resultUpdateInfo);
		}

		protected override void UpdateProgress(RunworkEventArgs.ProgressIdentify progress)
		{
			base.UpdateProgress(progress);

			_form.SetProgress(progress);
		}

		/// <summary>
		/// 引发 <see cref="BuilderInterfaceBase.WorkerShutdown" /> 事件
		/// </summary>
		protected override void OnWorkerShutdown()
		{
			base.OnWorkerShutdown();
			_form.Close();
		}
	}
}
