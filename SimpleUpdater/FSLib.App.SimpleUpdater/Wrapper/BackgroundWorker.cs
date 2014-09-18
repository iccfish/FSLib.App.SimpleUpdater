using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace FSLib.App.SimpleUpdater.Wrapper
{
	/// <summary>
	/// 异步任务提供类
	/// </summary>
	public class BackgroundWorker : Component
	{

		#region 构造函数


		/// <summary>
		/// Initializes a new instance of the BackgroundWorker class.
		/// </summary>
		public BackgroundWorker()
			: this(false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the BackgroundWorker class.
		/// </summary>
		public BackgroundWorker(bool wokerSupportForceAbort)
		{
			WokerSupportForceAbort = wokerSupportForceAbort;
			this.WorkerSupportReportProgress = true;
			this.WokerSupportCancellation = true;
			this.WorkerPriority = ThreadPriority.Normal;
		}
		#endregion

		#region 属性

		/// <summary>
		/// 获得或设置一个值，表明当前的Worker是否支持报告进度
		/// </summary>
		public bool WorkerSupportReportProgress { get; set; }

		/// <summary>
		/// 获得或设置一个值，表明当前的Worker是否支持取消
		/// </summary>
		public bool WokerSupportCancellation { get; set; }

		/// <summary>
		/// 获得或设置一个值，表明当前的Worker是否支持强行终止任务
		/// </summary>
		public bool WokerSupportForceAbort { get; set; }

		/// <summary>
		/// 获得或设置一个值，表明当前worker忙碌的时候是否将光标设置为忙碌状态
		/// </summary>
		public bool UseWaitCursor { get; set; }

		/// <summary>
		/// 获得一个值，表明当前的任务是否正在运行中
		/// </summary>
		[Browsable(false)]
		public bool IsBusy
		{
			get
			{
				return _operation != null && _runworkEventArgs != null && _runworkEventArgs.Thread != null && _runworkEventArgs.Thread.IsAlive;
			}
		}

		/// <summary>
		/// 获得运行时数据
		/// </summary>
		[Browsable(false)]
		public RunworkEventArgs RuntimeData
		{
			get
			{
				return _runworkEventArgs;
			}
		}

		/// <summary>
		/// 获得或设置当前任务是否需要以管理员模式运行
		/// </summary>
		public bool IsAdminRequired { get; set; }

		/// <summary>
		/// 获得或设置一个值，表明当前的任务是否正在取消中
		/// </summary>
		public bool CancellationPending
		{
			get
			{
				if (this._runworkEventArgs == null) throw new InvalidOperationException("TaskNotRunning");
				else return this._runworkEventArgs.CancellationPending;
			}
			set
			{
				if (this._runworkEventArgs == null) throw new InvalidOperationException("TaskNotRunning");
				if (!this.WokerSupportCancellation) throw new InvalidOperationException("OperationNotSupported");
				if (this._runworkEventArgs.CancellationPending) throw new InvalidOperationException("TaskAlreadyCancelPending");

				this._runworkEventArgs.CancellationPending = value;
			}
		}

		/// <summary>
		/// 获得或设置线程的优先级
		/// </summary>
		public ThreadPriority WorkerPriority { get; set; }

		#endregion

		#region 私有变量
		/// <summary>
		/// 当前的事件操作参数
		/// </summary>
		RunworkEventArgs _runworkEventArgs;

		/// <summary>
		/// 异步操作
		/// </summary>
		AsyncOperation _operation;

		#endregion

		#region 事件

		/// <summary>
		/// 当工作线程的进度发生变化时触发
		/// </summary>
		public event EventHandler<RunworkEventArgs> WorkerProgressChanged;

		/// <summary>
		/// 引发 <see cref="WorkerProgressChanged"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="RunworkEventArgs"/> 的事件参数</param>
		protected virtual void OnWorkerProgressChanged(RunworkEventArgs e)
		{
			if (!IsBusy)
				return;

			var handler = WorkerProgressChanged;
			if (handler == null)
				return;

			_operation.Post(_ => handler(this, e), null);
		}

		/// <summary>
		/// 当工作线程被强行终止时触发
		/// </summary>
		public event EventHandler<RunworkEventArgs> WorkerThreadAborted;

		/// <summary>
		/// 引发 <see cref="WorkerThreadAborted"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="RunworkEventArgs"/> 的事件参数</param>
		protected virtual void OnWorkerThreadAborted(RunworkEventArgs e)
		{
			if (!IsBusy)
				return;

			var handler = WorkerThreadAborted;
			if (handler == null)
				return;

			_operation.Post(_ => handler(this, e), null);
		}

		/// <summary>
		/// 当工作线程失败时触发
		/// </summary>
		public event EventHandler<RunworkEventArgs> WorkFailed;

		/// <summary>
		/// 引发 <see cref="WorkFailed"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="RunworkEventArgs"/> 的事件参数</param>
		protected virtual void OnWorkFailed(RunworkEventArgs e)
		{
			if (!IsBusy)
				return;

			var handler = WorkFailed;
			if (handler == null)
				return;

			_operation.PostOperationCompleted(_ => handler(this, e), null);
			_operation = null;
		}

		/// <summary>
		/// 当工作线程完成时触发
		/// </summary>
		public event EventHandler<RunworkEventArgs> WorkCompleted;

		/// <summary>
		/// 引发 <see cref="WorkCompleted"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="RunworkEventArgs"/> 的事件参数</param>
		protected void OnWorkCompleted(RunworkEventArgs e)
		{
			if (!IsBusy)
				return;

			var handler = WorkCompleted;
			if (handler == null)
				return;

			_operation.PostOperationCompleted(_ => handler(this, e), null);
			_operation = null;
		}

		/// <summary>
		/// 当工作任务取消时触发
		/// </summary>
		public event EventHandler<RunworkEventArgs> WorkCancelled;

		/// <summary>
		/// 引发 <see cref="WorkCancelled"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="RunworkEventArgs"/> 的事件参数</param>
		protected void OnWorkCancelled(RunworkEventArgs e)
		{
			if (!IsBusy)
				return;

			var handler = WorkCancelled;
			if (handler == null)
				return;

			_operation.PostOperationCompleted(_ => handler(this, e), null);
			_operation = null;
		}

		/// <summary>
		/// 当工作线程开始执行任务时，触发
		/// </summary>
		public event EventHandler<RunworkEventArgs> DoWork;

		/// <summary>
		/// 引发 <see cref="DoWork"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="RunworkEventArgs"/> 的事件参数</param>
		protected virtual void OnDoWork(RunworkEventArgs e)
		{
			if (IsBusy)
				return;

			DoWork(this, e);
		}

		/// <summary>
		/// 报告进度已经变化
		/// </summary>
		internal void ReportProgress()
		{
			this.OnWorkerProgressChanged(this._runworkEventArgs);
		}

		#endregion

		/// <summary>
		/// 启动任务
		/// </summary>
		public void RunWorkASync()
		{
			RunWorkASync(null);
		}

		/// <summary>
		/// 使用指定参数来启动任务
		/// </summary>
		/// <param name="arguments"></param>
		public void RunWorkASync(object arguments)
		{
			_operation = AsyncOperationManager.CreateOperation(null);
			var ts = new Thread(RunWorkAsyncInternal)
			{
				IsBackground = true,
				Priority = WorkerPriority
			};
			_runworkEventArgs = new RunworkEventArgs(this) { Argument = arguments, Thread = ts };

			if (UseWaitCursor) System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			ts.Start(_runworkEventArgs);
		}

		/// <summary>
		/// 强行中止任务
		/// <para>本方法不通过设置 <see cref="CancellationPending"/> 来取消任务，而是通过直接中止对应线程来完成操作，因此不推荐使用本方法。</para>
		/// </summary>
		public void KillTask()
		{
			if (IsBusy)
			{
				try
				{
					_runworkEventArgs.Thread.Abort();
				}
				catch (ThreadAbortException ex)
				{
					_runworkEventArgs.Exception = ex;
					OnWorkerThreadAborted(_runworkEventArgs);
				}
			}
		}

		/// <summary>
		/// 如果任务正在运行，则等待任务完成，直到等待超时
		/// </summary>
		/// <param name="timeout">等待时间，超过等待时间则返回。如果设置为0，则会一直等待</param>
		public void WaitForTask(int timeout)
		{
			if (IsBusy) _runworkEventArgs.Thread.Join(timeout);
		}

		/// <summary>
		/// 启动异步执行方法
		/// </summary>
		void RunWorkAsyncInternal(object e)
		{
			var args = e as RunworkEventArgs;
			try
			{
				OnDoWork(this._runworkEventArgs);
				if (!this.CancellationPending) this._runworkEventArgs.Succeed = true;
			}
			catch (Exception ex)
			{
				this._runworkEventArgs.Exception = ex;
			}

			//重设光标
			if (UseWaitCursor)
			{
				SendOrPostCallback _callback = _ =>
				{
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
				};

				if (this._operation != null)
				{
					this._operation.Post(_callback, null);
				}
				else
				{
					_callback(null);
				}
			}

			if (args.Succeed) OnWorkCompleted(args);// this._operation.PostOperationCompleted(this.callWorkCompleted, e);
			else if (this.CancellationPending) OnWorkCancelled(args);
			else OnWorkFailed(args);
		}



		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="action"></param>
		public void PostEvent(Action action)
		{
			if (!IsBusy)
				return;

			_operation.Post(_ => action(), null);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="callback">回调</param>
		public void PostEvent(SendOrPostCallback callback)
		{
			PostEvent(callback, null);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="handler">事件回调</param>
		/// <param name="target">指向引发事件的源对象</param>
		public void PostEvent(EventHandler handler, object target)
		{
			if (!IsBusy || handler == null)
				return;

			PostEvent(_ => handler(target, EventArgs.Empty), null);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="target">指向引发事件的源对象</param>
		/// <param name="eventArg">包含事件数据</param>
		/// <param name="handler">回调函数</param>
		/// <typeparam name="T">事件数据类型</typeparam>
		public void PostEvent<T>(EventHandler<T> handler, object target, T eventArg) where T : EventArgs
		{
			if (!IsBusy || handler == null)
				return;

			PostEvent(_ => handler(target, eventArg), null);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="callback">回调</param>
		/// <param name="arg">参数</param>
		public void PostEvent(SendOrPostCallback callback, object arg)
		{
			if (!IsBusy || callback == null)
				return;

			_operation.Post(callback, arg);
		}
	}
}
