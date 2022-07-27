using System;

namespace FSLib.App.SimpleUpdater
{
	using Defination;

	/// <summary> ��ʾ���ļ������¼����� </summary>
	/// <remarks></remarks>
	public class PackageEventArgs : EventArgs
	{
		/// <summary> ��õ�ǰ���ڲ����İ� </summary>
		/// <value></value>
		/// <remarks></remarks>
		public PackageInfo Package { get; private set; }

		/// <summary>
		/// ���� <see cref="PackageEventArgs" />  ����ʵ��(PackageDownloadEventArgs)
		/// </summary>
		public PackageEventArgs(PackageInfo package) { Package = package; }
	}
}
