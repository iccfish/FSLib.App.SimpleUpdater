using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	using System.Text.RegularExpressions;
	using FSLib.App.SimpleUpdater.Defination;

	static class Utility
	{
		/// <summary>
		/// 对字符串路径进行转移，以便于正确地在命令行中传递
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string SafeQuotePathInCommandLine(string path)
		{
			if (string.IsNullOrEmpty(path) || !Regex.IsMatch(path, @"(?<!\\)\\$"))
				return path;

			return path + @"\";
		}

		/// <summary>
		/// 测试是否具有指定的更新方式
		/// </summary>
		/// <param name="method"></param>
		/// <param name="targetMethod"></param>
		/// <returns></returns>
		public static bool HasMethod(UpdateMethod method, UpdateMethod targetMethod)
		{
			return (method & targetMethod) > 0;
		}

		/// <summary>
		/// 测试是否具有指定的更新方式
		/// </summary>
		/// <param name="method"></param>
		/// <param name="targetMethod"></param>
		/// <returns></returns>
		public static bool HasMethod(FileVerificationLevel method, FileVerificationLevel targetMethod)
		{
			return (method & targetMethod) > 0;
		}

		/// <summary>
		/// 清除指定的标记位
		/// </summary>
		/// <param name="method"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static UpdateMethod ClearUpdateMethodFlag(UpdateMethod method, UpdateMethod flag)
		{
			method &= (~flag);
			return method;
		}

		/// <summary>
		/// 清除指定的标记位
		/// </summary>
		/// <param name="method"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static UpdateMethod SetUpdateMethodFlag(UpdateMethod method, UpdateMethod flag)
		{
			method |= flag;
			return method;
		}

		public static UpdateMethod SetOrClearUpdateMethodFlag(UpdateMethod method, UpdateMethod flag, bool add)
		{
			if (add)
				return SetUpdateMethodFlag(method, flag);
			return ClearUpdateMethodFlag(method, flag);
		}

		/// <summary>
		/// 清除指定的标记位
		/// </summary>
		/// <param name="method"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static FileVerificationLevel ClearUpdateMethodFlag(FileVerificationLevel method, FileVerificationLevel flag)
		{
			method &= (~flag);
			return method;
		}

		/// <summary>
		/// 清除指定的标记位
		/// </summary>
		/// <param name="method"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static FileVerificationLevel SetUpdateMethodFlag(FileVerificationLevel method, FileVerificationLevel flag)
		{
			method |= flag;
			return method;
		}

		public static FileVerificationLevel SetOrClearUpdateMethodFlag(FileVerificationLevel method, FileVerificationLevel flag, bool add)
		{
			if (add)
				return SetUpdateMethodFlag(method, flag);
			return ClearUpdateMethodFlag(method, flag);
		}
	}
}
