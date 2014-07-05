using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator
{
	using System.IO;
	using System.Threading;

	using BuilderInterface;

	static class Program
	{
		internal static bool Running;
		[DllImport("kernel32.dll")]
		static extern bool FreeConsole();
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Running = true;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var file = string.Empty;
			var build = false;
			var noui = false;
			var miniui = false;

			if (args.Length > 0)
			{
				miniui = args.Any(s => string.Compare(s, "/miniui", true) == 0);
				noui = args.Any(s => string.Compare(s, "/noui", true) == 0);
				build = args.Any(s => string.Compare(s, "/build", true) == 0);
				file = args.Last();

				try
				{
					if (!System.IO.File.Exists(file))
						file = null;
				}
				catch (Exception ex)
				{
					file = null;
				}
			}
			else
			{
				FreeConsole();
			}

			if (build)
			{
				if (string.IsNullOrEmpty(file) || !File.Exists(file))
				{
					Console.WriteLine(SR.Program_Main_NoProjectFileSpecified);
					return;
				}

				if (noui)
				{
					RunBuild(file, false);
				}
				else if (miniui)
				{
					FreeConsole();
					RunBuild(file, true);
				}
				else
				{
					FreeConsole();
					Application.Run(new Main() { PreloadFile = file, AutoBuild = true });
				}
			}
			else
			{
				Application.Run(new Main() { PreloadFile = file });
			}
		}

		static void RunBuild(string file, bool miniui)
		{
			var builder = miniui ? (BuilderInterfaceBase)new FormBuildInterface() : new ConsoleBuildInterface();
			var running = true;
			builder.WorkerShutdown += (s, e) => running = false;
			builder.Build(file);

			while (running)
			{
				Thread.Sleep(50);
			}
		}
	}
}
