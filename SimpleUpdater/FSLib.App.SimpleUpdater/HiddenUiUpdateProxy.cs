using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 用于隐藏所有UI进行更新的回调类
	/// </summary>
	class HiddenUiUpdateProxy
	{
		public void RunUpdate()
		{
			var u = Updater.Instance;
			var waitHandler = new System.Threading.ManualResetEvent(false);


			u.NoUpdatesFound += (s, e) => waitHandler.Set();
			u.Error += (s, e) => waitHandler.Set();
			u.ExternalUpdateStarted += (s, e) => waitHandler.Set();
			u.UpdateCancelled += (s, e) => waitHandler.Set();

			Updater.CheckUpdateSimple();

			waitHandler.WaitOne();
		}
	}
}
