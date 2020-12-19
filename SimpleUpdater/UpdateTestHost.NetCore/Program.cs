using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateTestHost.NetCore
{
	using System.IO;

	using FSLib.App.SimpleUpdater;

	static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
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
