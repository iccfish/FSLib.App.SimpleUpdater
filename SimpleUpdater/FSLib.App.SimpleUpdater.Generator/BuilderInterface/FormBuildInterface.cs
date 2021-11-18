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

	using ZipBuilder;

	using Timer = System.Threading.Timer;

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

			_form.SetProgress(progress.TaskCount <= 0, progress.TaskPercentage, progress.StateMessage);
		}

		/// <summary>
		/// 引发 <see cref="BuilderInterfaceBase.WorkerShutdown" /> 事件
		/// </summary>
		protected override void OnWorkerShutdown()
		{
			base.OnWorkerShutdown();
			_form.Close();
		}


		private System.Threading.Timer _timer;


		/// <inheritdoc />
		protected override void OnFilePackingBegin()
		{
			var tasks = ZipTasks.ToArray();
			var random = new Random();

			_timer = new Timer(_ =>
				{
					var runningTasks = new List<ZipTask>();
					tasks.Where(s => s.State != ZipTaskState.Done && s.State != ZipTaskState.Queue).ForEach(s => runningTasks.Add(s));

					var running = runningTasks.Count;
					var done = tasks.Count(s => s.State == ZipTaskState.Done);
					var showIndex = random.Next(running);
					var task = running == 0 ? null : runningTasks[showIndex];

					var str = task == null ? $"[parallel:{Project.UseParallelBuilding}/{done}/{tasks.Length}] waiting..." : $"[{done}/{tasks.Length}] {running} running -> {task.State.ToString().ToLower()}/{(task.Percentage == -1 ? "" : task.Percentage)}% @ {task.PackageDescription}";
					var totalCount = tasks.Length * 100;
					var percentage = done * 100 + runningTasks.Sum(x => x.Percentage);

					_form.SetProgress(false, (int)Math.Round(percentage * 1.0 / totalCount * 100), str);
				},
				null,
				0,
				200);

			base.OnFilePackingBegin();
		}

		/// <inheritdoc />
		protected override void OnFilePackingEnd()
		{
			_timer.Change(-1, -1);
			base.OnFilePackingEnd();
		}

	}
}
