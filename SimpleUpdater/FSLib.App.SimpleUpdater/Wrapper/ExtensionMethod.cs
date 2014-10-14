using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FSLib.App.SimpleUpdater.Wrapper
{
#if !NET4
	public delegate TR Func<TR>();
	public delegate TR Func<TS, TR>(TS ele);
	public delegate void Action<T1, T2>(T1 t1, T2 t2);
#endif

	public static class ExtensionMethod
	{
		/// <summary>
		/// 为字符串设定默认值
		/// </summary>
		/// <param name="value">要设置的值</param>
		/// <param name="defaultValue">如果要设定的值为空，则返回此默认值</param>
		/// <returns>设定后的结果</returns>
		public static string DefaultForEmpty(string value, string defaultValue)
		{
			return string.IsNullOrEmpty(value) ? defaultValue : value;
		}



		readonly static string[] SizeDefinitions = new[] {
		"字节",
		"KB",
		"MB",
		"GB",
		"TB"
		};

		/// <summary>
		/// 控制尺寸显示转换上限
		/// </summary>
		private const double SizeLevel = 0x400 * 0.9;

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(double size)
		{
			return ToSizeDescription(size, 2);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(double size, int digits)
		{
			var sizeDefine = 0;


			while (sizeDefine < SizeDefinitions.Length && size > SizeLevel)
			{
				size /= 0x400;
				sizeDefine++;
			}


			if (sizeDefine == 0) return size.ToString("#0") + SizeDefinitions[sizeDefine];
			return size.ToString("#0." + string.Empty.PadLeft(digits, '0')) + SizeDefinitions[sizeDefine];
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(ulong size)
		{
			return ToSizeDescription((double)size);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(ulong size, int digits)
		{
			return ToSizeDescription((double)size, digits);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(long size)
		{
			return ToSizeDescription((double)size);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(long size, int digits)
		{
			return ToSizeDescription((double)size, digits);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(int size)
		{
			return ToSizeDescription((double)size);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(int size, int digits)
		{
			return ToSizeDescription((double)size, digits);
		}


		/// <summary>
		/// 同步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Send(SynchronizationContext context, SendOrPostCallback callback)
		{
			context.Send(callback, null);
		}

		public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> func)
		{
			foreach (var item in source)
			{
				yield return func(item);
			}
		}

		/// <summary> 将指定的序列转换为强类型的List独享 </summary>
		/// <param name="source" type="System.Collections.Generic.IEnumerable`1">类型为 <see>System.Collections.Generic.IEnumerable`1</see> 的参数</param>
		/// <returns></returns>
		public static List<T> ToList<T>(IEnumerable<T> source)
		{
			var list = new List<T>();
			foreach (var item in source)
			{
				list.Add(item);
			}
			return list;
		}

		/// <summary>
		/// 解压缩一个字节流
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		internal static byte[] Decompress(byte[] buffer)
		{
			using (var source = new System.IO.MemoryStream(buffer))
			{
				source.Seek(0, System.IO.SeekOrigin.Begin);

				using (var dest = new System.IO.MemoryStream())
				using (var gz = new System.IO.Compression.GZipStream(source, System.IO.Compression.CompressionMode.Decompress))
				{
					var buf = new byte[0x400];
					var count = 0;
					while ((count = gz.Read(buf, 0, buf.Length)) > 0)
					{
						dest.Write(buf, 0, count);
					}

					dest.Close();
					return dest.ToArray();
				}
			}
		}

		/// <summary> 获得指定文件的Hash值 </summary>
		/// <param name="filePath" type="string">文件路径</param>
		/// <returns></returns>
		public static string GetFileHash(string filePath)
		{
			var cpter = System.Security.Cryptography.MD5.Create();
			return BitConverter.ToString(cpter.ComputeHash(System.IO.File.ReadAllBytes(filePath))).Replace("-", "").ToUpper();
		}

		/// <summary> 计算一个序列中符合指定要求的元素的个数 </summary>
		/// <param name="source" type="System.Collections.Generic.IEnumerable`1">类型为 <see>System.Collections.Generic.IEnumerable`1</see> 的参数</param>
		/// <param name="predicate" type="FSLib.App.SimpleUpdater.Wrapper.Func`2">类型为 <see>FSLib.App.SimpleUpdater.Wrapper.Func`2</see> 的参数</param>
		/// <returns></returns>
		public static int Count<T>(IEnumerable<T> source, Func<T, bool> predicate)
		{
			var c = 0;
			foreach (var item in source)
			{
				if (predicate(item)) c++;
			}
			return c;
		}

		/// <summary> 对序列进行过滤 </summary>
		/// <param name="source" type="System.Collections.Generic.IEnumerable`1">类型为 <see>System.Collections.Generic.IEnumerable<T></see> 的参数</param>
		/// <param name="predicate" type="FSLib.App.SimpleUpdater.Wrapper.Func`2">类型为 <see>FSLib.App.SimpleUpdater.Wrapper.Func<T,bool></see> 的参数</param>
		/// <returns></returns>
		public static IEnumerable<T> Where<T>(IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (var item in source)
			{
				if (predicate(item)) yield return item;
			}
		}

		/// <summary> 对序列进行转换 </summary>
		/// <param name="source" type="System.Collections.Generic.IEnumerable`1">类型为 <see>System.Collections.Generic.IEnumerable<T></see> 的参数</param>
		/// <param name="predicate" type="FSLib.App.SimpleUpdater.Wrapper.Func`2">类型为 <see>FSLib.App.SimpleUpdater.Wrapper.Func<T,bool></see> 的参数</param>
		/// <returns></returns>
		public static IEnumerable<R> Where<T, R>(IEnumerable<T> source, Func<T, R> predicate)
		{
			foreach (var item in source)
			{
				yield return predicate(item);
			}
		}

		/// <summary> 计算一个序列中指定属性之和 </summary>
		/// <param name="source" type="System.Collections.Generic.IEnumerable`1">类型为 <see>System.Collections.Generic.IEnumerable`1</see> 的参数</param>
		/// <param name="predicate" type="FSLib.App.SimpleUpdater.Wrapper.Func`2">类型为 <see>FSLib.App.SimpleUpdater.Wrapper.Func`2</see> 的参数</param>
		/// <returns></returns>
		public static long Sum<T>(IEnumerable<T> source, Func<T, long> predicate)
		{
			var c = 0L;
			foreach (var item in source)
			{
				c += predicate(item);
			}
			return c;
		}

		/// <summary> 比较文件的版本和指定的版本。如果文件版本低于指定版本则返回true </summary>
		/// <param name="filePath" type="string"></param>
		/// <param name="version" type="System.Version"></param>
		/// <returns> bool </returns>
		public static bool CompareVersion(string filePath, string version)
		{
			var fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath);
			if (fv == null) throw new ApplicationException("无法获得文件 " + filePath + " 的版本信息");

			return version != ConvertVersionInfo(fv).ToString();
		}


		/// <summary> 将文件版本信息转换为本地版本信息 </summary>
		/// <param name="fvi" type="System.Diagnostics.FileVersionInfo">类型为 <see>System.Diagnostics.FileVersionInfo</see> 的参数</param>
		/// <returns></returns>
		public static Version ConvertVersionInfo(System.Diagnostics.FileVersionInfo fvi)
		{
			return new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
		}
	}
}
