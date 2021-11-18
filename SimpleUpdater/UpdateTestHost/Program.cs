using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UpdateTestHost
{
	using System.IO;
	using System.Runtime.CompilerServices;

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

			var updater = Updater.CreateUpdaterInstance("https://www.fishlee.net/service/update2/69/78/update_c.xml");
			var context = updater.Context;
			context.LogFile = Path.GetFullPath(@".\log.txt");
			//var updater = Updater.CreateUpdaterInstance("https://www.fishlee.net/service/update2/56/40/{0}", "update_c.xml");

			updater.Error += (sender, args) =>
			{
				MessageBox.Show(updater.Context.Exception.Message);
			};
			updater.MinmumVersionRequired += (sender, args) =>
			{
				MessageBox.Show(updater.Context.Exception?.Message);
			};

			updater.BeginCheckUpdateInProcess();

			Application.Run(new Form());
		}
	}
}
