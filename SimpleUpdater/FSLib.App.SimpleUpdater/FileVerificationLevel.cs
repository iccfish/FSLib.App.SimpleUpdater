using System;

namespace FSLib.App.SimpleUpdater
{
	/// <summary> 文件验证等级 </summary>
	/// <remarks></remarks>
	[Flags]
	public enum FileVerificationLevel
	{
		/// <summary>
		/// 没有
		/// </summary>
		None = 0,
		/// <summary> 验证大小 </summary>
		/// <remarks></remarks>
		Size = 1,
		/// <summary> 验证版本 </summary>
		/// <remarks></remarks>
		Version = 2,
		/// <summary> 验证Hash </summary>
		/// <remarks></remarks>
		Hash = 4
	}
}