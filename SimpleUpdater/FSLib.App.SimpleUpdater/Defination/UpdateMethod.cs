namespace FSLib.App.SimpleUpdater.Defination
{
	using System;

	/// <summary> 更新模式 </summary>
	/// <remarks></remarks>
	[Flags]
	public enum UpdateMethod
	{
		/// <summary>
		/// 同项目定义
		/// </summary>
		AsProject = 0,
		/// <summary>总是更新</summary>
		/// <remarks></remarks>
		Always = 16,
		/// <summary>版本控制</summary>
		/// <remarks></remarks>
		VersionCompare = 1,
		/// <summary> 如果存在则跳过</summary>
		/// <remarks></remarks>
		SkipIfExists = 2,
		/// <summary>
		/// 忽略
		/// </summary>
		Ignore = 4,
		/// <summary>
		/// 如果不存在则跳过
		/// </summary>
		SkipIfNotExist = 8
	}
}