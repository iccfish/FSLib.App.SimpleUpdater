using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater
{
	using System.Diagnostics;
	using System.Drawing;
	using System.Threading;

#if !NET20 && !NET35
	using System.Threading.Tasks;
#endif

	using System.Windows.Forms;

	using FSLib.App.SimpleUpdater.Defination;
	using FSLib.App.SimpleUpdater.Dialogs;
	using FSLib.App.SimpleUpdater.Wrapper;

	partial class Updater
	{
		#region 同步检测更新方式

		/// <summary>
		/// 同步检查更新
		/// </summary>
		/// <param name="enableEmbedDialog">是否启用内置对话框，默认为 <code>true</code>，也就是允许当发现新版本时先弹出提示或进行相关操作。</param>
		/// <returns></returns>
		public UpdateCheckResult CheckUpdateSync(bool enableEmbedDialog = true)
		{
			Context.EnableEmbedDialog = enableEmbedDialog;

			_logger.LogInformation($"{nameof(CheckUpdateSync)} Start with threadid={Thread.CurrentThread.ManagedThreadId}");

			var               evt    = new ManualResetEvent(false);
			EventHandler      eh     = null;
			UpdateCheckResult result = null;

			eh = (_, __) =>
			{
				Delegate.RemoveAll(CheckUpdateComplete, eh);
				if (Context.Exception != null)
					result = new UpdateCheckResult(false, true, false, null, Context.Exception);
				else if (Context.HasUpdate)
					result = new UpdateCheckResult(true, false, true, new Version(Context.UpdateInfo.AppVersion), null);
				else
					result = new UpdateCheckResult(true, false, false, null, null);

				evt.Set();
			};
			CheckUpdateComplete += eh;

			//强制从非UI线程启动，否则如果当前线程是UI线程，会冻结整个线程造成死锁。
			ThreadPool.QueueUserWorkItem(_ => BeginCheckUpdateInProcess());
			evt.WaitOne();

			_logger.LogInformation($"{nameof(CheckUpdateSync)} End with threadid={Thread.CurrentThread.ManagedThreadId}");

			return result;
		}

#if !NET20 && !NET35
		/// <summary>
		/// 任务模式检查更新。任务将会返回新版本号，如果返回null，则意味着没找到新版本。
		/// </summary>
		/// <param name="enableEmbedDialog">是否启用内置对话框，默认为 <code>true</code>，也就是允许当发现新版本时先弹出提示或进行相关操作。</param>
		/// <returns></returns>
		public Task<Version> CheckUpdateTask(bool enableEmbedDialog = true)
		{
			Context.EnableEmbedDialog = enableEmbedDialog;

			var tcs = new TaskCompletionSource<Version>();

			EventHandler      eh = null;
			UpdateCheckResult result = null;

			eh = (_, __) =>
			{
				Delegate.RemoveAll(CheckUpdateComplete, eh);
				if (Context.Exception != null)
					tcs.SetException(Context.Exception);
				else if (Context.HasUpdate)
					tcs.SetResult(new Version(Context.UpdateInfo.AppVersion));
				else
					tcs.SetResult(null);
			};
			CheckUpdateComplete += eh;
			BeginCheckUpdateInProcess();

			return tcs.Task;
		}

#endif

		#endregion

		#region 确认没有更新才启动

		/// <summary>
		/// 确认是否有更新，再继续后面的操作。
		/// </summary>
		public void EnsureNoUpdate()
		{
			EnsureNoUpdate<Form>(null, false);
		}

		/// <summary>
		/// 确认是否有更新，再继续后面的操作。
		/// </summary>
		/// <typeparam name="T">要显示的UI界面</typeparam>
		/// <param name="updateUi">用于显示状态的UI界面</param>
		public void EnsureNoUpdate<T>(T updateUi = null, bool noUi = false) where T : Form
		{
			Application.EnableVisualStyles();
			var ui  = noUi ? null : (Form)updateUi ?? new EnsureUpdate();
			var evt = new ManualResetEvent(false);

			Context.MustUpdate             = true;
			Context.ForceUpdate            = true;
			Context.EnableEmbedDialog      = true;
			Context.AutoExitCurrentProcess = true;

			void SubscribeStateChange(object sender, EventArgs e)
			{
				switch (Context.CheckState)
				{
					case UpdateCheckState.Error:
					case UpdateCheckState.NoUpdate:
						break;
					default: return;
				}

				Context.CheckStateChanged -= SubscribeStateChange;

				ui?.Close();
				evt.Set();
			}

			Context.CheckStateChanged += SubscribeStateChange;


			if (ui != null)
			{
				ui.Shown += (s, e) => BeginCheckUpdateInProcess();
				ui.ShowDialog();
			}
			else
			{
				BeginCheckUpdateInProcess();
				evt.WaitOne();
			}
		}

		#endregion
	}
}
