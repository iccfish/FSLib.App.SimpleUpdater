using System.Diagnostics;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;

namespace FSLib.App.SimpleUpdater.Wrapper
{
	/// <summary>
	/// XML序列化支持类
	/// </summary>
	public static class XMLSerializeHelper
	{
		/// <summary>
		/// 序列化对象为文本
		/// </summary>
		/// <returns>保存信息的 <see cref="T:System.String"/></returns>
		public static T XmlDeserializeFromString<T>(string content) where T : class
		{
			if (String.IsNullOrEmpty(content))
				return null;

			using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
			{
				try
				{
					var xso = new XmlSerializer(typeof(T));
					return (T)xso.Deserialize(ms);
				}
				catch (Exception ex)
				{
					Trace.TraceInformation("执行反序列化时发生错误 ----> \r\n" + ex.ToString());
					return default(T);
				}
			}
		}

		/// <summary>
		/// 序列化对象到文件
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <param name="fileName">保存到的目标文件</param>
		public static void XmlSerilizeToFile(object objectToSerialize, string fileName)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName));

			using (var stream = new FileStream(fileName, FileMode.Create))
			{
				var xso = new XmlSerializer(objectToSerialize.GetType());
				xso.Serialize(stream, objectToSerialize);
				stream.Close();
			}
		}

	}
}
