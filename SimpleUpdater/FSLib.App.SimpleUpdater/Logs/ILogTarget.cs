namespace FSLib.App.SimpleUpdater.Logs
{
	using System;

	interface ILogTarget : IDisposable
	{
		/// <summary>
		/// 添加信息记录
		/// </summary>
		void Append(LogEntry entry);
	}
}
