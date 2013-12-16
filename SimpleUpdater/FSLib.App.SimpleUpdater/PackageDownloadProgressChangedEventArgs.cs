namespace FSLib.App.SimpleUpdater
{
	/// <summary> 表示包文件下载进度 </summary>
	/// <remarks></remarks>
	public class PackageDownloadProgressChangedEventArgs : System.ComponentModel.ProgressChangedEventArgs
	{
		/// <summary> 获得当前的包 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public PackageInfo Package { get; private set; }

		/// <summary> 表示要接受的总长度 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public long TotalBytesToReceive { get; private set; }

		/// <summary> 表示当前已经接收到的长度 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public long BytesReceived { get; private set; }

		/// <summary>
		/// 创建 <see cref="PackageDownloadProgressChangedEventArgs" />  的新实例(PackageDownloadProgressChangedEventArgs)
		/// </summary>
		public PackageDownloadProgressChangedEventArgs(PackageInfo package, long totalBytesToReceive, long bytesReceived, int progressPercentage)
			: base(progressPercentage, package)
		{
			Package = package;
			TotalBytesToReceive = totalBytesToReceive;
			BytesReceived = bytesReceived;
		}
	}
}