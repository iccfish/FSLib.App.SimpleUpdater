using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FSLib.App.Utilities
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args.Length == 0)
			{
				return;
			}

			//内部命令支持
			switch (args[0].ToLower())
			{
				case "deletetmp": DelayDeleteFile(int.Parse(args[1]), args[2]); return;
				case "deletetarget": DeleteTarget(args[1], args[2]); return;
				case "zipfile": ZipFile(args[1]); return;
				case "unzipfile": UnZipFile(args[1]); return;
				default:
					break;
			}

			//检查参数
			PassArgumentsToUpdater(args);
		}

		#region 加载自动更新

		static void PassArgumentsToUpdater(string[] args)
		{
			InvokeUpdateMethod(args);
		}

		static Assembly LoadUpdaterAssembly()
		{
			var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			var files = System.IO.Directory.GetFiles(path, "SimpleUpdater.*");

			if (files.Length == 0) return null;
			try
			{
				return Assembly.Load(System.IO.Path.GetFileNameWithoutExtension(files[0]));
			}
			catch (Exception)
			{
				return null;
			}
		}

		static Type GetUpdaterType()
		{
			var a = LoadUpdaterAssembly();
			return a?.GetType("FSLib.App.SimpleUpdater.Program");
		}


		static void InvokeUpdateMethod(string[] args)
		{
			var type = GetUpdaterType();
			if (type == null) return;

			var m = type.GetMethod("Main");
			m.Invoke(null, new object[] { args });
		}

		#endregion

		#region ZIP文件

		static void ZipFile(string file)
		{
			var target = file + ".gz";
			using (var ts = System.IO.File.OpenWrite(target))
			using (var gz = new System.IO.Compression.GZipStream(ts, System.IO.Compression.CompressionMode.Compress))
			{
				var buffer = System.IO.File.ReadAllBytes(file);
				gz.Write(buffer, 0, buffer.Length);
			}
		}


		static void UnZipFile(string file)
		{
			var target = file.Substring(0, file.LastIndexOf("."));
			using (var fs = System.IO.File.OpenRead(file))
			using (var gz = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress))
			using (var ts = System.IO.File.OpenWrite(target))
			{
				var buffer = new byte[0x400];
				var count = 0;
				while ((count = gz.Read(buffer, 0, buffer.Length)) > 0)
				{
					ts.Write(buffer, 0, count);
				}
			}
		}
		#endregion

		#region 根据正则表达式脚本删除文件或目录
		/// <summary>
		/// 删除目标
		/// </summary>
		/// <param name="scriptFile">脚本文件</param>
		/// <param name="targetDirectory">目标目录</param>
		static void DeleteTarget(string scriptFile, string targetDirectory)
		{
			if (!System.IO.File.Exists(scriptFile) || !System.IO.Directory.Exists(targetDirectory)) return;
			var lines = System.IO.File.ReadAllLines(scriptFile);
			var regs = new Regex[lines.Length];
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i] != null) regs[i] = new Regex(lines[i], RegexOptions.IgnoreCase);
			}

			//search all files
			SearchDirectory(targetDirectory, regs);
		}

		/// <summary>
		/// 搜索目录并删除文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="regs"></param>
		static void SearchDirectory(string path, Regex[] regs)
		{
			if (ValidateFileName(path, regs))
			{
				try
				{
					System.IO.Directory.Delete(path, true);
				}
				catch (Exception) { return; }

				return;
			}

			string[] directories, files;
			try
			{
				directories = System.IO.Directory.GetDirectories(path);
				files = System.IO.Directory.GetFiles(path);
			}
			catch (Exception)
			{
				return;
			}

			foreach (var d in directories)
			{
				SearchDirectory(d, regs);
			}

			foreach (var f in files)
			{
				if (!ValidateFileName(f, regs)) continue;
				try
				{
					System.IO.File.Delete(f);
				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>
		/// 确定一个路径是否符合规则
		/// </summary>
		/// <param name="path"></param>
		/// <param name="regs"></param>
		/// <returns></returns>
		static bool ValidateFileName(string path, Regex[] regs)
		{
			foreach (var r in regs)
			{
				if (r.IsMatch(path)) return true;
			}
			return false;
		}
		#endregion


		/// <summary>
		/// 延迟删除程序
		/// </summary>
		static void DelayDeleteFile(int pid, string path)
		{
			var plist = new List<Process>();
			try
			{
				var process = System.Diagnostics.Process.GetProcessById(pid);
				if (process != null && !process.HasExited) plist.Add(process);
			}
			catch (Exception)
			{
			}
			//查找所有指定目录中运行的进程
			var allproc = System.Diagnostics.Process.GetProcesses();
			foreach (var p in allproc)
			{
				try
				{
					var mpath = p.MainModule.FileName;
					if (mpath.IndexOf(path, StringComparison.OrdinalIgnoreCase) != -1) plist.Add(p);
				}
				catch (Exception)
				{

				}
			}
			//等待结束
			foreach (var p in plist)
			{
				if (!p.HasExited) p.WaitForExit();
			}

			try
			{
				if (System.IO.Directory.Exists(path))
					System.IO.Directory.Delete(path, true);
			}
			catch (Exception)
			{
			}
		}
	}
}
