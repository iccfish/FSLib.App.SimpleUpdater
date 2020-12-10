namespace FSLib.App.SimpleUpdater.Logs
{
	interface ILogTarget
	{
		/// <summary>
		/// 添加信息记录
		/// </summary>
		void Append(LogEntry entry);
	}
}
