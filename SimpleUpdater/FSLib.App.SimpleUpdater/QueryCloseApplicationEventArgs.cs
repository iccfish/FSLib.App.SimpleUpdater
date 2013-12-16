using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 请求关闭进程对话框
	/// </summary>
	public class QueryCloseApplicationEventArgs : EventArgs
	{
		/// <summary>
		/// 获得需要关闭的进程
		/// </summary>
		public IEnumerable<Process> Processes { get; private set; }

		/// <summary>
		/// 调用内建的处理机制
		/// </summary>
		Action<QueryCloseApplicationEventArgs> _defaultBehavior;

		/// <summary>
		/// 是否取消当前操作
		/// </summary>
		public bool? IsCancelled { get; set; }


		/// <summary>
		/// 创建 <see cref="QueryCloseApplicationEventArgs" /> 的新实例
		/// </summary>
		public QueryCloseApplicationEventArgs(IEnumerable<Process> processes, Action<QueryCloseApplicationEventArgs> defaultBehavior)
		{
			Processes = processes;
			_defaultBehavior = defaultBehavior;
			IsCancelled = null;
		}

		/// <summary>
		/// 使用内建的处理模型
		/// </summary>
		public void CallDefaultBeihavior()
		{
			_defaultBehavior(this);
		}
	}
}
