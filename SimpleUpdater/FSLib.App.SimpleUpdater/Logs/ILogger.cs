namespace FSLib.App.SimpleUpdater.Logs
{
	using System;

	/// <summary>
	/// 日志记录
	/// </summary>
	interface ILogger
	{
		/// <summary>
		/// 获得当前日志记录器的名字
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 记录信息
		/// </summary>
		/// <param name="message"></param>
		void LogInformation(string message);

		/// <summary>
		/// 记录警告信息
		/// </summary>
		/// <param name="message"></param>
		void LogWarning(string message);

		/// <summary>
		/// 记录调试信息
		/// </summary>
		/// <param name="message"></param>
		void LogDebug(string message);

		/// <summary>
		/// 记录错误信息
		/// </summary>
		/// <param name="message">信息</param>
		/// <param name="ex">关联的异常</param>
		void LogError(string message, Exception ex);
	}
}
