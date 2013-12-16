using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 删除旧的程序文件方式
	/// </summary>
	public enum DeletePreviousProgramMethod
	{
		/// <summary>
		/// 不主动删除仅覆盖
		/// </summary>
		None = 0,
		/// <summary>
		/// 删除除明确要求保留之外的文件和目录
		/// </summary>
		AllExceptSpecified = 1,
		/// <summary>
		/// 仅删除明确要求删除的文件和目录
		/// </summary>
		NoneButSpecified = 2
	}
}
