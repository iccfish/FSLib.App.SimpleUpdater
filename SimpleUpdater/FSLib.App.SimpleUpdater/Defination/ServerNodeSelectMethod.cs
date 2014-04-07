using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary>
	/// 服务器节点确认方式
	/// </summary>
	public enum ServerNodeSelectMethod
	{
		/// <summary>
		/// 备用模式，仅在下载失败后才使用其它服务器节点
		/// </summary>
		Fallback = 0,
		/// <summary>
		/// 测速模式，先根据测速网址确定最佳节点
		/// </summary>
		DetectSpeedFirst = 1
	}
}
