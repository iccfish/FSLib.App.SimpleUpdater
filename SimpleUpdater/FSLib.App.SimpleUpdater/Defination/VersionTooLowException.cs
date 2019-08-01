using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary>
	/// 版本过低无法更新异常
	/// </summary>
	public class VersionTooLowException : Exception
	{
		/// <summary>
		/// 需要的最低版本
		/// </summary>
		public Version MinimumVersion { get; private set; }

		/// <summary>
		/// 当前版本
		/// </summary>
		public Version CurrentVersion { get; private set; }

		/// <summary>
		/// 创建 <see cref="VersionTooLowException"/> 的新对象
		/// </summary>
		/// <param name="currentVersion"></param>
		/// <param name="minimumVersion"></param>
		public VersionTooLowException(Version currentVersion, Version minimumVersion) :
			base(string.Format(SR.Ex_VersionTooLow, minimumVersion, currentVersion))
		{
			CurrentVersion = currentVersion;
			MinimumVersion = minimumVersion;
		}
	}
}
