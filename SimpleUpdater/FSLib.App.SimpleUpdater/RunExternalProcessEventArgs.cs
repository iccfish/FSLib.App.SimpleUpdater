using System;
using System.Diagnostics;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 外部进程启动的事件参数
	/// </summary>
	public class RunExternalProcessEventArgs : EventArgs
	{
		/// <summary> 获得将要启动的进程启动参数 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public ProcessStartInfo ProcessStartInfo { get; private set; }

		/// <summary>
		/// 创建 <see cref="RunExternalProcessEventArgs" />  的新实例(RunExternalProcessEventArgs)
		/// </summary>
		public RunExternalProcessEventArgs(ProcessStartInfo processStartInfo) { ProcessStartInfo = processStartInfo; }
	}
}