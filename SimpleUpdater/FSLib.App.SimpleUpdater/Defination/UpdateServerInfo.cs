using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	/// <summary>
	/// 升级服务器信息
	/// </summary>
	public class UpdateServerInfo
	{
		/// <summary>
		/// 服务器地址模板
		/// </summary>
		public string Url { get; private set; }

		/// <summary>
		/// 文件名模板
		/// </summary>
		public string InfoFileName { get; private set; }

		/// <summary>
		/// 创建 <see cref="UpdateServerInfo" />  的新实例(UpdateServerInfo)
		/// </summary>
		/// <param name="url"></param>
		/// <param name="infoFileName"></param>
		public UpdateServerInfo(string url, string infoFileName = null)
		{
			Url = url;
			InfoFileName = infoFileName;
		}
	}
}
