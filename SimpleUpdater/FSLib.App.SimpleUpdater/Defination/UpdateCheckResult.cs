using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary>
	/// 表示检查更新的结果
	/// </summary>
	public class UpdateCheckResult
	{
		/// <summary>
		/// 获得是否成功
		/// </summary>
		public bool Success { get; private set; }

		/// <summary>
		/// 获得是否发生了错误
		/// </summary>
		public bool Error { get; private set; }

		/// <summary>
		/// 获得是否有更新
		/// </summary>
		public bool HasUpdate { get; private set; }

		/// <summary>
		/// 获得找到的新版本
		/// </summary>
		public Version NewVersion { get; private set; }


		/// <summary>
		/// 获得发生的错误
		/// </summary>
		public Exception Exception { get; private set; }

		/// <summary>
		/// 构造一个新的 <see cref="UpdateCheckResult"/> 对象
		/// </summary>
		internal UpdateCheckResult(bool success, bool error, bool hasUpdate, Version newVersion, Exception exception)
		{
			Success = success;
			Error = error;
			HasUpdate = hasUpdate;
			NewVersion = newVersion;
			Exception = exception;
		}
	}
}
