using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Defination
{
	/// <summary>
	/// 服务器节点
	/// </summary>
	[Serializable]
	public class ServerNode
	{
		/// <summary>
		/// 获得或设置服务器节点名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 获得或设置下载文件地址模板
		/// </summary>
		public string UrlTemplate { get; set; }

		/// <summary>
		/// 获得或设置服务器速度测试地址
		/// </summary>
		public string TestUrl { get; set; }
	}
}
