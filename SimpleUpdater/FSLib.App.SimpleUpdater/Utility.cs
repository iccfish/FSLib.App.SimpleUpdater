using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	using System.Text.RegularExpressions;

	class Utility
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
	}
}
