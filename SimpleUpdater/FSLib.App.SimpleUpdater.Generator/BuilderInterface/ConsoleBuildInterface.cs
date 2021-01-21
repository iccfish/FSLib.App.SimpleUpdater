using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.BuilderInterface
{
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Windows.Forms;

	using Defination;

	using SimpleUpdater.Defination;

	using Wrapper;

	using ZipBuilder;

	using Timer = System.Threading.Timer;

	class ConsoleBuildInterface : BuilderInterfaceBase
	{
		/// <summary>
		/// 创建 <see cref="FormBuildInterface" />  的新实例(FormBuildInterface)
		/// </summary>
		public ConsoleBuildInterface()
		{
			Console.WriteLine("update project building engine by iFish(iccfish@qq.com)");
			Console.WriteLine("==============================");
			Console.WriteLine();
		}

		protected override void BuildSuccess(AuProject project, Dictionary<string, string> packages, UpdateInfo resultUpdateInfo)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("update project build complete.");
			Console.WriteLine("===============================");

			Console.ForegroundColor = ConsoleColor.Yellow;
			var index = 0;
			foreach (var package in packages)
			{
				Console.WriteLine("[{0:#0000}] {1}		{2}", ++index, package.Key, package.Value);
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("===============================");
			Console.WriteLine("built {0} packages", packages.Count);
			Console.ResetColor();

			base.BuildSuccess(project, packages, resultUpdateInfo);
		}

		protected override void BuilderFailed(AuProject project, Exception exception, UpdateInfo resultUpdateInfo)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("update project building failed: " + exception.Message);
			Console.ResetColor();
			base.BuilderFailed(project, exception, resultUpdateInfo);
		}

		protected override void UpdateProgress(RunworkEventArgs.ProgressIdentify progress)
		{
			base.UpdateProgress(progress);

			if (!string.IsNullOrEmpty(progress.StateMessage))
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + progress.StateMessage);
				Console.ResetColor();
			}
		}

		private Timer _timer;
		private DateTime _packagingStartTime;


		/// <inheritdoc />
		protected override void OnFilePackingBegin()
		{
			var tasks = ZipTasks.ToArray();
			var random = new Random();

			_packagingStartTime = DateTime.Now;
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine($"building {ZipTasks.Count()} update packages(using paraell building: {Project.UseParallelBuilding.ToString().ToUpper()})...");
			Console.ForegroundColor = ConsoleColor.Yellow;
			_timer = new Timer(_ =>
				{
					var runningTasks = new List<ZipTask>();
					tasks.Where(s => s.State != ZipTaskState.Done && s.State != ZipTaskState.Queue).ForEach(s => runningTasks.Add(s));

					var running = runningTasks.Count;
					var done = tasks.Count(s => s.State == ZipTaskState.Done);
					var showIndex = random.Next(running);
					var task = running == 0 ? null : runningTasks[showIndex];

					var str = task == null ? $"[{done}/{tasks.Length}] waiting..." : $"[{done}/{tasks.Length}] {running} running -> {task.State.ToString().ToLower()}/{(task.Percentage == -1 ? "" : task.Percentage)}% @ {task.PackageDescription}".GetSubString(Console.WindowWidth - 1, "..");
					var widthCount = str.Sum(s => s > 255 ? 2 : 1);
					if (widthCount < Console.WindowWidth - 1)
						str = str.PadRight(Console.WindowWidth - 1 - (str.Length - widthCount), ' ');
					Console.Write("\r");
					Console.Write(str);
				},
				null,
				0,
				200);

			base.OnFilePackingBegin();
		}

		/// <inheritdoc />
		protected override void OnFilePackingEnd()
		{
			_timer.Change(-1, -1);
			Console.Write("\r");
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine($"done building {ZipTasks.Count()} update packages in {(DateTime.Now - _packagingStartTime).TotalSeconds:F3} seconds.".PadRight(Console.WindowWidth - 1, ' '));
			base.OnFilePackingEnd();
		}
	}
}
