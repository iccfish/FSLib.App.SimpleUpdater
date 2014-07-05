using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FSLib.App.SimpleUpdater.Generator
{
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Windows.Forms;

	static class ExtensionMethods
	{
		/// <summary>
		/// 判断当前字符串是否为空或长度为零
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>true为空或长度为零</returns>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// 添加一个数据源绑定
		/// </summary>
		/// <typeparam name="TControl"></typeparam>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="control"></param>
		/// <param name="source"></param>
		/// <param name="controlExpression"></param>
		/// <param name="propertyExpression"></param>
		public static void AddDataBinding<TControl, TSource, TValue>(this TControl control, TSource source, Expression<Func<TControl, TValue>> controlExpression, Expression<Func<TSource, TValue>> propertyExpression) where TControl : Control
		{
			if (control == null || controlExpression == null || propertyExpression == null)
				return;

			if (controlExpression.Body.NodeType != ExpressionType.MemberAccess || !((controlExpression.Body as MemberExpression).Member is PropertyInfo))
				return;
			if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess || !((propertyExpression.Body as MemberExpression).Member is PropertyInfo))
				return;

			var controlPropertyName = ((controlExpression.Body as MemberExpression).Member as PropertyInfo).Name;
			var sourcePropertyName = ((propertyExpression.Body as MemberExpression).Member as PropertyInfo).Name;

			control.DataBindings.Add(controlPropertyName, source, sourcePropertyName);
		}


		/// <summary>
		/// 压缩指定文件
		/// </summary>
		public static byte[] CompressBuffer(byte[] data)
		{
			using (var ms = new System.IO.MemoryStream())
			using (var zs = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress))
			{
				zs.Write(data, 0, data.Length);
				zs.Close();
				ms.Close();

				return ms.ToArray();
			}
		}


		/// <summary>
		/// 将字符串转换为Int值
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.Int32"/></returns>
		public static int ToInt32(this string value, int defaultValue)
		{
			int temp;
			return int.TryParse(value, out temp) ? temp : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为Int值
		/// </summary>
		/// <param name="value">字符串</param>
		/// <returns>转换后的 <see cref="System.Int32"/></returns>
		public static int ToInt32(this string value)
		{
			int temp;
			return int.TryParse(value, out temp) ? temp : 0;
		}
		/// <summary>
		/// 序列化对象到文件
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <param name="FileName">保存到的目标文件</param>
		public static void XmlSerilizeToFile(this object objectToSerialize, string FileName)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(FileName));

			using (FileStream stream = new FileStream(FileName, FileMode.Create))
			{
				objectToSerialize.XmlSerializeToStream(stream);
				stream.Close();
			}
		}

		/// <summary>
		/// 序列化指定对象到指定流中
		/// </summary>
		/// <param name="ObjectToSerialize">要序列化的对象</param>
		/// <param name="stream">目标流</param>
		public static void XmlSerializeToStream(this object ObjectToSerialize, Stream stream)
		{
			if (ObjectToSerialize == null || stream == null)
				return;

			XmlSerializer xso = new XmlSerializer(ObjectToSerialize.GetType());
			xso.Serialize(stream, ObjectToSerialize);
		}
		/// <summary>
		/// 从指定的文件中反序列化对象
		/// </summary>
		/// <param name="type">目标类型</param>
		/// <param name="fileName">文件路径</param>
		/// <returns>反序列化的结果</returns>
		public static object XmlDeserializeFile(this Type type, string fileName)
		{
			return XmlDeserializeFromFile(fileName, type);
		}

		/// <summary>
		/// 从文件中反序列化指定类型的对象
		/// </summary>
		/// <param name="objType">反序列化的对象类型</param>
		/// <param name="FileName">文件名</param>
		/// <returns>对象</returns>
		public static object XmlDeserializeFromFile(string FileName, System.Type objType)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				object res = stream.XmlDeserializeFromStream(objType);
				stream.Close();
				return res;
			}
		}

		/// <summary>
		/// 从流中反序列化对象
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="stream">流对象</param>
		/// <returns>反序列化结果</returns>
		public static T XmlDeserializeFromStream<T>(this Stream stream) where T : class
		{
			T res = stream.XmlDeserializeFromStream(typeof(T)) as T;
			return res;
		}

		/// <summary>
		/// 从流中反序列化出指定对象类型的对象
		/// </summary>
		/// <param name="objType">对象类型</param>
		/// <param name="stream">流对象</param>
		/// <returns>反序列结果</returns>
		public static object XmlDeserializeFromStream(this Stream stream, System.Type objType)
		{
			var xso = new XmlSerializer(objType);
			var res = xso.Deserialize(stream);

			return res;
		}

		/// <summary>
		/// 确认字符串是以指定字符串结尾的
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="ending">结尾</param>
		/// <returns></returns>
		public static string EnsureEndWith(this string value, string ending)
		{
			if (value == null || value.EndsWith(ending)) return value;
			return value + ending;
		}

		/// <summary>
		/// 对可遍历对象进行遍历并进行指定操作
		/// </summary>
		/// <typeparam name="T">遍历的类型</typeparam>
		/// <param name="enumberable">对象</param>
		/// <param name="predicate">函数委托</param>
		/// <exception cref="System.ArgumentNullException">predicate</exception>
		public static void ForEach<T>(this IEnumerable<T> enumberable, Action<T> predicate)
		{
			if (enumberable == null)
				throw new ArgumentNullException("enumberable", "enumberable is null.");
			if (predicate == null)
				throw new ArgumentNullException("predicate", "predicate is null.");

			foreach (T item in enumberable)
			{
				predicate(item);
			}
		}

		/// <summary>
		/// 获得指定数组是否为空
		/// </summary>
		/// <typeparam name="T">数组类型</typeparam>
		/// <param name="array">要检测的数组</param>
		/// <returns>如果为空或长度为零的数组，则返回true</returns>
		public static bool IsEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}

		/// <summary>
		/// 判断一个版本号是否是无效的
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public static bool IsIllegal(this Version version)
		{
			return version == null || version.Major == 0;
		}

		/// <summary>
		/// 测试一个文件是否是压缩的XML
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static bool IsCompressedXmlFile(string fileName)
		{
			using (var fs=new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				var buffer = new byte[4];
				if (fs.Read(buffer, 0, buffer.Length) != buffer.Length) return false;

				return BitConverter.ToInt32(buffer, 0) != 0x6D783F3C;
			}
		}

		/// <summary>
		/// 对一个流进行解压缩
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static System.IO.Stream Decompress(this Stream stream)
		{
			return new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
		}

		/// <summary>
		/// 解压缩一个文件为文件流
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static Stream DecompressFile(string fileName)
		{
			return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read).Decompress();
		}
	}
}
