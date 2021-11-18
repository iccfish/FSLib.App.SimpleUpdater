namespace FSLib.App.SimpleUpdater.Logs
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	class LogManager
	{
		#region 单例模式

		static LogManager _instance;
		static readonly object _lockObject = new object();

		/// <summary>
		/// 获得 <see cref="LogManager"/> 的单例对象
		/// </summary>
		public static LogManager Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lockObject)
					{
						if (_instance == null)
						{
							_instance = new LogManager();
						}
					}
				}

				return _instance;
			}
		}

		#endregion

		#region 私有构造函数

		private LogManager()
		{
		}

		#endregion

		#region 私有构造函数

		private LogManager(string targetFile)
		{
		}

		#endregion

		private ReaderWriterLock _rwLock = new ReaderWriterLock();

		IList<ILogTarget> LogTargets { get; } = new List<ILogTarget>();

		public IEnumerable<ILogTarget> GetAllLogTargets()
		{
			_rwLock.AcquireReaderLock(0);

			var arr = new ILogTarget[LogTargets.Count];
			arr.CopyTo(arr, 0);

			_rwLock.ReleaseReaderLock();
			
			return arr;
		}

		/// <summary>
		/// 添加新的日志记录目标
		/// </summary>
		/// <param name="target"></param>
		public void AddLogTarget(ILogTarget target)
		{
			_rwLock.AcquireWriterLock(0);
			LogTargets.Add(target);
			_rwLock.ReleaseWriterLock();
		}

		/// <summary>
		/// 移除指定的日志记录目标
		/// </summary>
		/// <param name="target"></param>
		public void RemoveLogTarget(ILogTarget target)
		{
			_rwLock.AcquireWriterLock(0);
			LogTargets.Remove(target);
			_rwLock.ReleaseWriterLock();
		}
		
		/// <summary>
		/// 清空所有日志记录目标
		/// </summary>
		public void RemoveAllLogTargets()
		{
			_rwLock.AcquireWriterLock(0);
			LogTargets.Clear();
			_rwLock.ReleaseWriterLock();
		}

		public ILogger GetLogger<T>()
		{
			var name = typeof(T).FullName;

			var logger = new Logger(name);
			logger.RequestLog += Logger_OnRequestLog;

			return logger;
		}


		private void Logger_OnRequestLog(object sender, RequestLogEventArgs e)
		{
			_rwLock.AcquireReaderLock(0);
			foreach (var logTarget in LogTargets)
			{
				logTarget.Append(e.Entry);
			}
			_rwLock.ReleaseReaderLock();
		}
	}
}
