using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator
{
	/// <summary>
	/// 自动升级项目
	/// </summary>
	public class AuProject
	{
		/// <summary>
		/// 应用程序路径
		/// </summary>
		public string ApplicationDirectory { get; set; }

		/// <summary>
		/// 升级包路径
		/// </summary>
		public string DestinationDirectory { get; set; }

		/// <summary>
		/// 发布路径
		/// </summary>
		public string PublishUri { get; set; }

		/// <summary>
		/// 发布用户名
		/// </summary>
		public string PublishUserName { get; set; }

		/// <summary>
		/// 发布密码
		/// </summary>
		public string PublishPassword { get; set; }

		/// <summary>
		/// 更新说明的RTF格式文件路径
		/// </summary>
		public string UpdateRtfNotePath { get; set; }

		/// <summary>
		/// 从指定的文件中加载项目
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static AuProject LoadFile(string path)
		{
			using (var fs=new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
			{
				var xso = new System.Xml.Serialization.XmlSerializer(typeof (AuProject));
				return xso.Deserialize(fs) as AuProject;
			}
		}

		/// <summary>
		/// 保存至指定的文件中
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path)
		{
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			{
				var xso = new System.Xml.Serialization.XmlSerializer(typeof(AuProject));
				xso.Serialize(fs, this);
			}
		}
	}
}
