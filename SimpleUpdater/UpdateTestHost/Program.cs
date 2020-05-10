using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UpdateTestHost
{
	using FSLib.App.SimpleUpdater;

	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var updater = Updater.CreateUpdaterInstance("https://www.fishlee.net/service/update2/69/78/{0}", "update_c.xml");
			updater.BeginCheckUpdateInProcess();

			Application.Run(new Form());
		}
	}
}
