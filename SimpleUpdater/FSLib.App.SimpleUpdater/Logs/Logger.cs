namespace FSLib.App.SimpleUpdater.Logs
{
	using System;

	class Logger : ILogger
	{
		public Logger(string loggerName) { Name = loggerName; }

		/// <summary>
		/// 日志记录器的名字
		/// </summary>
		public string Name { get; }

		/// <inheritdoc />
		public void LogInformation(string message, object state = null) => Log(new LogEntry(LogLevel.Info, message, state, Name));

		/// <inheritdoc />
		public void LogWarning(string message, object state = null) => Log(new LogEntry(LogLevel.Warning, message, state, Name));

		/// <inheritdoc />
		public void LogDebug(string message, object state = null) => Log(new LogEntry(LogLevel.Debug, message, state, Name));

		/// <inheritdoc />
		public void LogError(string message, object state = null) => Log(new LogEntry(LogLevel.Error, message, state, Name));

		/// <inheritdoc />
		public void Log(LogEntry entry)
		{
			entry.LoggerName = Name;
			OnRequestLog(new RequestLogEventArgs(entry));
		}


		/// <inheritdoc />
		public event EventHandler<RequestLogEventArgs> RequestLog;

		/// <summary>
		/// 引发 <see cref="RequestLog"/> 事件
		/// </summary>

		protected virtual void OnRequestLog(RequestLogEventArgs e)
		{
			RequestLog?.Invoke(this, e);
		}
	}
}
