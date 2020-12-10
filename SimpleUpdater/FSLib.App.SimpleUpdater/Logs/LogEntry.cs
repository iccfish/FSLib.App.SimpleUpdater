namespace FSLib.App.SimpleUpdater.Logs
{
	class LogEntry
	{
		/// <summary>
		/// 获得或设置记录等级
		/// </summary>
		public LogLevel Level { get; set; }

		/// <summary>
		/// 获得或记录信息
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 获得或设置状态对象
		/// </summary>
		public object State { get; set; }

		/// <summary>
		/// 记录器的名字
		/// </summary>
		public string LoggerName { get; set; }

		public LogEntry()
		{
		}

		public LogEntry(LogLevel level, string message, object state, string loggerName)
		{
			Level = level;
			Message = message;
			State = state;
			LoggerName = loggerName;
		}
	}
}
