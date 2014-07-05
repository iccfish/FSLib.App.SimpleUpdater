using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.BuilderInterface
{
	using System.Windows.Forms;

	using Defination;

	using SimpleUpdater.Defination;

	using Wrapper;

	class ConsoleBuildInterface : BuilderInterfaceBase
	{
		/// <summary>
		/// 创建 <see cref="FormBuildInterface" />  的新实例(FormBuildInterface)
		/// </summary>
		public ConsoleBuildInterface()
		{
			Console.WriteLine("自动更新包构建工具 by 木鱼");
			Console.WriteLine("==============================");
			Console.WriteLine();
		}

		protected override void BuildSuccess(AuProject project, Dictionary<string, string> packages, UpdateInfo resultUpdateInfo)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("更新项目构建成功");
			Console.WriteLine("===============================");

			Console.ForegroundColor = ConsoleColor.Yellow;
			var index = 0;
			foreach (var package in packages)
			{
				Console.WriteLine("[{0:#0000}] {1}		{2}", ++index, package.Key, package.Value);
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("===============================");
			Console.WriteLine("已构建 {0} 个包。", packages.Count);
			Console.ResetColor();

			base.BuildSuccess(project, packages, resultUpdateInfo);
		}

		protected override void BuilderFailed(AuProject project, Exception exception, UpdateInfo resultUpdateInfo)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("更新项目构建失败：" + exception.Message);
			Console.ResetColor();
			base.BuilderFailed(project, exception, resultUpdateInfo);
		}

		protected override void UpdateProgress(RunworkEventArgs.ProgressIdentify progress)
		{
			base.UpdateProgress(progress);

			if (!string.IsNullOrEmpty(progress.StateMessage))
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "] " + progress.StateMessage);
				Console.ResetColor();
			}
		}

	}
}
