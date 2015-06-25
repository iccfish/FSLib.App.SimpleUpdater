using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Defination
{
	using SimpleUpdater.Defination;

	/// <summary>
	/// 项目中的文件定义
	/// </summary>
	public class ProjectItem
	{
		/// <summary>
		/// 获得或设置更新的路径
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 获得或设置更新模式
		/// </summary>
		public UpdateMethod UpdateMethod { get; set; }

		/// <summary>
		/// 获得或设置更新的级别
		/// </summary>
		public FileVerificationLevel FileVerificationLevel { get; set; }

		public string Flag { get; set; }
	}
}
