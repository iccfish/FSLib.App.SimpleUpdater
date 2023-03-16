using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	public enum UpdateCheckState
	{
		/// <summary>
		/// 初始状态，尚未检查
		/// </summary>
		Initial = 0,

		/// <summary>
		/// 检查中
		/// </summary>
		Checking = 1,

		/// <summary>
		/// 无更新
		/// </summary>
		NoUpdate = 2,

		/// <summary>
		/// 有更新
		/// </summary>
		UpdatesAvailable = 3,

		/// <summary>
		/// 错误
		/// </summary>
		Error = 4
	}
}
