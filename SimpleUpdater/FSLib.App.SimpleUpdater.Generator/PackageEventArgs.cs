using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator
{
	using Defination;

	/// <summary>
	/// 包事件对象
	/// </summary>
	class PackageEventArgs:EventArgs
	{

		/// <summary>
		/// 获得相关联的包
		/// </summary>
		public AuProject AuProject { get; private set; }

		/// <summary>
		/// 创建 <see cref="PackageEventArgs" />  的新实例(PackageEventArgs)
		/// </summary>
		/// <param name="auProject"></param>
		public PackageEventArgs(AuProject auProject)
		{
			AuProject = auProject;
		}
	}
}
