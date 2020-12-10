namespace FSLib.App.SimpleUpdater.Logs
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	abstract class LogTarget : ILogTarget
	{
		protected Queue<LogEntry> LogEntries { get; private set; } = new Queue<LogEntry>();

		private readonly object _entryLock = new object();

		protected LogTarget()
		{
		}

		/// <inheritdoc />
		public void Append(LogEntry entry)
		{
			entry.Message = $"[{DateTime.Now:O}][{Thread.CurrentThread.ManagedThreadId}][{entry.Level.ToString().ToUpper()}] {entry.Message}";

			lock (_entryLock)
			{
				LogEntries.Enqueue(entry);
			}

			if (_isWritingEntries != 0)
				WriteEntries();
		}

		private int _isWritingEntries;

		void WriteEntries()
		{
			if (Interlocked.CompareExchange(ref _isWritingEntries, 1, 0) != 0)
				return;

			while (true)
			{
				LogEntry[] entries;
				lock (_entryLock)
				{
					entries = LogEntries.ToArray();
				}

				try
				{
					WriteEntries(entries);

					lock (_entryLock)
					{
						for (int i = 0; i < entries.Length; i++)
						{
							LogEntries.Dequeue();
						}
					}
				}
				catch (Exception e)
				{
				}
				Thread.Sleep(100);
				if (LogEntries.Count == 0)
					break;
			}

			_isWritingEntries = 0;
		}

		protected abstract void WriteEntries(IEnumerable<LogEntry> entries);
	}
}
