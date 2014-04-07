using System;

namespace FSLib.App.SimpleUpdater
{
	using Defination;

	/// <summary>
	///     更新包下载错误异常
	/// </summary>
	[Serializable]
	public class PackageDownloadException : System.ApplicationException, System.Runtime.Serialization.ISerializable
	{

		/// <summary>
		///     Parameterless (default) constructor
		/// </summary>
		public PackageDownloadException(params PackageInfo[] packages)
			: base("升级包下载失败")
		{
		}


		/// <summary> 获得出错的文件包 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public PackageInfo[] ErrorPackages { get; private set; }
	}
}