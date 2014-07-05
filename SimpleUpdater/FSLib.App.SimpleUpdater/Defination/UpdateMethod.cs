namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary> 更新模式 </summary>
	/// <remarks></remarks>
	public enum UpdateMethod
	{
		/// <summary> 总是更新 </summary>
		/// <remarks></remarks>
		Always = 0,
		/// <summary> 版本控制 </summary>
		/// <remarks></remarks>
		VersionCompare = 1,
		/// <summary> 如果不存在则更新 </summary>
		/// <remarks></remarks>
		SkipIfExists = 2,
		/// <summary>
		/// 忽略
		/// </summary>
		Ignore = 3
	}
}