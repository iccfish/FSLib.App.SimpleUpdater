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
		void LogInformation(string message, object state = null);

		/// <summary>
		/// 记录警告信息
		/// </summary>
		/// <param name="message"></param>
		void LogWarning(string message, object state = null);

		/// <summary>
		/// 记录调试信息
		/// </summary>
		/// <param name="message"></param>
		void LogDebug(string message, object state = null);

		/// <summary>
		/// 记录错误信息
		/// </summary>
		/// <param name="message">信息</param>
		/// <param name="state">状态信息</param>
		void LogError(string message, object state = null);

		/// <summary>
		/// 记录日志项
		/// </summary>
		/// <param name="entry"></param>
		void Log(LogEntry entry);

		/// <summary>
		/// 请求记录日志项
		/// </summary>
		event EventHandler<RequestLogEventArgs> RequestLog;
	}

	class RequestLogEventArgs : EventArgs
	{
		public RequestLogEventArgs(LogEntry entry) { Entry = entry; }

		public LogEntry Entry { get; }
	}
}
