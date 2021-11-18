namespace FSLib.App.SimpleUpdater.Logs
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;

	/// <summary>
	/// 基于文件的日志目标
	/// </summary>
	class FileLogTarget : LogTarget
	{
		/// <summary>
		/// 新建 <see cref="FileLogTarget"/> 对象
		/// </summary>
		/// <param name="logFilePath">目标文件路径</param>
		public FileLogTarget(string logFilePath) { LogFilePath = logFilePath; }

		/// <summary>
		/// 获得当前日志文件路径
		/// </summary>
		public string LogFilePath { get; private set; }

		/// <inheritdoc />
		protected override void WriteEntries(IEnumerable<LogEntry> entries)
		{
			var sb = new StringBuilder();
			foreach (var entry in entries)
			{
				sb.AppendLine(entry.Message);
				if (entry.State != null)
					sb.AppendLine(entry.State.ToString());
			}

			File.AppendAllText(LogFilePath, sb.ToString());
		}
	}
}
