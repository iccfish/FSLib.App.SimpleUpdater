using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace FSLib.App.SimpleUpdater.Wrapper
{
	public delegate void Action();
	
	/// <summary>
	/// 任务运行事件数据
	/// </summary>
	public class RunworkEventArgs : EventArgs
	{
		/// <summary>
		/// 运行参数
		/// </summary>
		public object Argument { get; set; }

		/// <summary>
		/// 运行结果
		/// </summary>
		public object Result { get; set; }

		/// <summary>
		/// 运行中出现的错误
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// 正在运行任务的线程
		/// </summary>
		public Thread Thread { get; set; }

		/// <summary>
		/// 当前任务运行的进度
		/// </summary>
		public ProgressIdentify Progress { get; private set; }

		/// <summary>
		/// 获得或取得一个值，表明当前任务是否已经声明取消
		/// </summary>
		public bool CancellationPending { get; set; }

		/// <summary>
		/// 获得或设置一个值，表明当前正在执行的任务是否成功完成
		/// </summary>
		public bool Succeed { get; set; }

		/// <summary>
		/// 当前的任务管理对象
		/// </summary>
		BackgroundWorker _worker;

		/// <summary>
		/// Initializes a new instance of the RunworkEventArgs class.
		/// </summary>
		internal RunworkEventArgs(BackgroundWorker worker)
		{
			this.Progress = new ProgressIdentify();
			this._worker = worker;
			this.Succeed = false;
		}


		#region 内联类型
		/// <summary>
		/// 进度声明类
		/// </summary>
		public class ProgressIdentify
		{
			/// <summary>
			/// 任务总数
			/// </summary>
			public int TaskCount { get; set; }

			/// <summary>
			/// 任务进度
			/// </summary>
			public int TaskProgress { get; set; }

			/// <summary>
			/// 获得或设置任务进度的百分比
			/// </summary>
			public int TaskPercentage
			{
				get
				{
					if (TaskCount == 0)
					{
						return TaskProgress >= 0 && TaskProgress <= 100 ? TaskProgress : 0;
					}
					return (int)Math.Floor(TaskProgress * 100.0 / TaskCount);
				}
				set
				{
					if (value < 0 || value > TaskCount) throw new ArgumentOutOfRangeException("PercentageRangeLimit");
					TaskCount = 0;
					TaskProgress = value;
				}
			}

			/// <summary>
			/// 进度信息
			/// </summary>
			public string StateMessage { get; set; }

			/// <summary>
			/// 用户自定义进度对象
			/// </summary>
			public object UserState { get; set; }

			/// <summary>
			/// 重置当前进度的状态
			/// </summary>
			public void Reset()
			{
				TaskCount = TaskProgress = 0;
				UserState = StateMessage = null;
			}
		}
		#endregion

		/// <summary>
		/// 报告当前进度变化
		/// </summary>
		/// <param name="currentTaskIndex">当前任务索引</param>
		public void ReportProgress(int currentTaskIndex)
		{
			ReportProgress(0, currentTaskIndex);
		}

		/// <summary>
		/// 报告当前进度变化
		/// </summary>
		/// <param name="taskCount">任务总数</param>
		/// <param name="currentTaskIndex">当前任务索引</param>
		public void ReportProgress(int taskCount, int currentTaskIndex)
		{
			ReportProgress(taskCount, currentTaskIndex, String.Empty);
		}

		/// <summary>
		/// 报告当前进度变化
		/// </summary>
		/// <param name="taskCount">任务总数</param>
		/// <param name="currentTaskIndex">当前任务索引</param>
		/// <param name="stateMessage">状态信息</param>
		public void ReportProgress(int taskCount, int currentTaskIndex, string stateMessage)
		{
			ReportProgress(taskCount, currentTaskIndex, stateMessage, null);
		}

		/// <summary>
		/// 报告当前进度变化
		/// </summary>
		/// <param name="taskCount">任务总数</param>
		/// <param name="currentTaskIndex">当前任务索引</param>
		/// <param name="stateMessage">状态信息</param>
		/// <param name="userState">自定义进度数据</param>
		public void ReportProgress(int taskCount, int currentTaskIndex, string stateMessage, object userState)
		{
			if (!_worker.WorkerSupportReportProgress) throw new InvalidOperationException("ReportProgressNotSupported");

			this.Progress.TaskCount = taskCount;
			this.Progress.TaskProgress = currentTaskIndex;
			this.Progress.StateMessage = stateMessage;
			Progress.UserState = userState;

			this._worker.ReportProgress();
		}


		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="action"></param>
		public void PostEvent(Action action)
		{
			_worker.PostEvent(action);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="callback">回调</param>
		public void PostEvent(SendOrPostCallback callback)
		{
			_worker.PostEvent(callback);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="handler">事件回调</param>
		/// <param name="target">指向引发事件的源对象</param>
		public void PostEvent(EventHandler handler, object target)
		{
			_worker.PostEvent(handler, target);
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
			_worker.PostEvent<T>(handler, target, eventArg);
		}

		/// <summary>
		/// 向异步操作提交事件
		/// </summary>
		/// <param name="callback">回调</param>
		/// <param name="arg">参数</param>
		public void PostEvent(SendOrPostCallback callback, object arg)
		{
			_worker.PostEvent(callback, arg);
		}
	}
}
