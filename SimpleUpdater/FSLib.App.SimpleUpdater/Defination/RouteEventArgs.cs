using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	public class RouteEventArgs : EventArgs
	{
		/// <summary>
		/// 获得或设置是否停止执行。如果设置为 <code>true</code>，则后续执行不再继续
		/// </summary>
		public bool StopExecute { get; set; } = false;
	}
}
