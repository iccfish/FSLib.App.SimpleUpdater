using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSLib.App.SimpleUpdater.Defination;
using FSLib.App.SimpleUpdater.Generator.Defination;

namespace FSLib.App.SimpleUpdater.Generator.BuilderInterface
{
	using Wrapper;

	abstract class BuilderInterfaceBase
	{
		/// <summary>
		/// 构建已经完成（成功或失败）
		/// </summary>
		public event EventHandler WorkerShutdown;

		/// <summary>
		/// 构建已经初始化
		/// </summary>
		public event EventHandler WorkerInitialized;

		/// <summary>
		/// 引发 <see cref="WorkerShutdown" /> 事件
		/// </summary>
		protected virtual void OnWorkerShutdown()
		{
			var handler = WorkerShutdown;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 引发 <see cref="WorkerInitialized" /> 事件
		/// </summary>
		protected virtual void OnWorkerInitialized()
		{
			var handler = WorkerInitialized;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 构建项目
		/// </summary>
		/// <param name="filepath"></param>
		public virtual void Build(string filepath)
		{
			var builder = UpdatePackageBuilder.Instance;
			var project = AuProject.LoadFile(filepath);

			if (project == null)
			{
				BuilderFailed(null, new ApplicationException(SR.UnableOpenProject), null);
				return;
			}

			builder.AuProject = project;

			var bgw = new BackgroundWorker();
			bgw.DoWork += (s, e) => builder.Build(e);
			bgw.WorkFailed += (s, e) => BuilderFailed(project, e.Exception, builder.BuiltUpdateInfo);
			bgw.WorkCompleted += (s, e) => BuildSuccess(project, builder.Result, builder.BuiltUpdateInfo);
			bgw.WorkerProgressChanged += (s, e) => UpdateProgress(e.Progress);

			OnWorkerInitialized();
			bgw.RunWorkASync();
		}

		/// <summary>
		/// 构建进度发生变化
		/// </summary>
		/// <param name="progress"></param>
		protected virtual void UpdateProgress(RunworkEventArgs.ProgressIdentify progress)
		{

		}

		/// <summary>
		/// 构建成功
		/// </summary>
		/// <param name="project"></param>
		/// <param name="packages"></param>
		/// <param name="resultUpdateInfo"></param>
		protected virtual void BuildSuccess(AuProject project, Dictionary<string, string> packages, UpdateInfo resultUpdateInfo)
		{
			OnWorkerShutdown();
		}

		/// <summary>
		/// 构建失败
		/// </summary>
		/// <param name="project"></param>
		/// <param name="exception"></param>
		/// <param name="resultUpdateInfo"></param>
		protected virtual void BuilderFailed(AuProject project, Exception exception, UpdateInfo resultUpdateInfo)
		{
			OnWorkerShutdown();
		}
	}
}
