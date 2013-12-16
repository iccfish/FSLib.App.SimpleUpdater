using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator
{
	static class Program
	{
		internal static bool Running;

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Running = true;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}
	}
}
