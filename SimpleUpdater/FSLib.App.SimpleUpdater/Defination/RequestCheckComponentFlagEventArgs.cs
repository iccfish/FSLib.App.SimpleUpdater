using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary>
	/// 要求检测对应组件状态事件的参数
	/// </summary>
	public class RequestCheckComponentFlagEventArgs : EventArgs
	{
		/// <summary>
		/// 组件ID
		/// </summary>
		public string ComponentId { get; private set; }

		/// <summary>
		/// 组件状态
		/// </summary>
		public bool Valid { get; set; }

		/// <summary>
		/// 创建 <see cref="RequestCheckComponentFlagEventArgs" />  的新实例(RequestCheckComponentFlagEventArgs)
		/// </summary>
		/// <param name="compid"></param>
		public RequestCheckComponentFlagEventArgs(string compid)
		{
			ComponentId = compid;
		}
	}
}
