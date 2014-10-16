using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary>
	/// 表示一个可取消的操作事件参数
	/// </summary>
	public class CancelableEventArgs:EventArgs
	{
		/// <summary>
		/// 获得或设置一个值，指示当前的操作是否被取消
		/// </summary>
		public bool IsCancelled { get; set; }

		/// <summary>
		/// 创建 <see cref="CancelableEventArgs" />  的新实例(CancelableEventArgs)
		/// </summary>
		public CancelableEventArgs() : this(false)
		{
		}

		/// <summary>
		/// 创建 <see cref="CancelableEventArgs" />  的新实例(CancelableEventArgs)
		/// </summary>
		/// <param name="isCancelled"></param>
		public CancelableEventArgs(bool isCancelled)
		{
			IsCancelled = isCancelled;
		}
	}
}
