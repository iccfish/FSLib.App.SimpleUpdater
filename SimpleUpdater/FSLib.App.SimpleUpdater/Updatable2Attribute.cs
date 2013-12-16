using System;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 表示第二种替换模式的自动更新标记
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class Updatable2Attribute : Attribute
	{
		/// <summary> 获得下载链接模板 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string UrlTemplate { get; private set; }

		/// <summary> 获得下载信息文件名 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string InfoFileName { get; private set; }


		/// <summary>
		/// 创建 <see cref="Updatable2Attribute" />  的新实例(Updatable2Attribute)
		/// </summary>
		/// <param name="urlTemplate">下载文件的URL模板，以 {0} 为占位符</param>
		/// <param name="infoFileName">下载升级信息的文件名</param>
		public Updatable2Attribute(string urlTemplate, string infoFileName)
		{
			UrlTemplate = urlTemplate;
			InfoFileName = infoFileName;
		}
	}
}