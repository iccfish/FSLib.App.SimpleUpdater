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
			CheckDisposed();
			entry.Message = $"[{DateTime.Now:O}][{Thread.CurrentThread.ManagedThreadId}][{entry.LoggerName}][{entry.Level.ToString().ToUpper()}] {entry.Message}";

			lock (_entryLock)
			{
				LogEntries.Enqueue(entry);
			}

			if (_isWritingEntries == 0)
				ThreadPool.QueueUserWorkItem(_ => WriteEntries(), null);
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
				if (LogEntries.Count == 0)
					break;
			}

			_isWritingEntries = 0;
		}

		protected abstract void WriteEntries(IEnumerable<LogEntry> entries);

		#region Dispose方法实现

		/// <summary>
		/// 当前对象已经被释放
		/// </summary>
		public event EventHandler Disposed;

		protected virtual void OnDisposed()
		{
			Disposed?.Invoke(this, EventArgs.Empty);
		}

		bool _disposed;

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			_disposed = true;

			WriteEntries();

			OnDisposed();

			//挂起终结器
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 检查是否已经被销毁。如果被销毁，则抛出异常
		/// </summary>
		/// <exception cref="ObjectDisposedException">对象已被销毁</exception>
		protected void CheckDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException(this.GetType().Name);
		}


		#endregion


	}
}
