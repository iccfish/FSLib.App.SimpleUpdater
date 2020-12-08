using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Logs
{
	using System;

	class LogManager
	{
		#region 单例模式

		static LogManager _instance;

		/// <summary>
		/// 获得 <see cref="LogManager"/> 的单例对象
		/// </summary>
		public static LogManager Instance { get; private set; }

		public static void Init(string targetFile)
		{
			if (_instance != null)
				throw new InvalidOperationException();

			_instance = new LogManager(targetFile);
		}

		#endregion

		#region 私有构造函数

		private LogManager(string targetFile)
		{
		}

		#endregion
		
		public ILogger GetLogger<T>()
		{

		}
	}

	class Logger
	{

	}

	interface ILogTarget
	{
		
	}

	class LogTarget
	{

	}
}
