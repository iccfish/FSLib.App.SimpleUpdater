using System;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 文件安装事件
	/// </summary>
	public class InstallFileEventArgs : EventArgs
	{
		/// <summary>
		/// 获得来源文件
		/// </summary>
		public string Source { get; private set; }

		/// <summary>
		/// 获得目标文件
		/// </summary>
		public string Destination { get; private set; }

		/// <summary>
		/// 文件包中的名字
		/// </summary>
		public string NameInPackage { get; set; }

		/// <summary> 总数 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public int TotalCount { get; private set; }

		/// <summary> 当前的序号 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public int CurrentCount { get; private set; }

		/// <summary>
		/// 创建 <see cref="InstallFileEventArgs" /> 的新实例
		/// </summary>
		public InstallFileEventArgs(string source, string destination, int totalCount, int currentCount, string nameInPackage)
		{
			CurrentCount = currentCount;
			NameInPackage = nameInPackage;
			TotalCount = totalCount;
			Source = source;
			Destination = destination;
		}
	}
}
