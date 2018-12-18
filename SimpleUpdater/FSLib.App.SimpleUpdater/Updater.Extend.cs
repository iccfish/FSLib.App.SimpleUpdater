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

			Trace.TraceInformation($"{nameof(CheckUpdateSync)} Start with threadid={Thread.CurrentThread.ManagedThreadId}");

			var evt = new ManualResetEvent(false);
			EventHandler eh = null;
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

			Trace.TraceInformation($"{nameof(CheckUpdateSync)} End with threadid={Thread.CurrentThread.ManagedThreadId}");

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

			EventHandler eh = null;
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
		/// <param name="updateFoundAction">发现更新的委托。如果此委托为null或返回null，则显示内置的更新对话框。如果此委托返回true，则启动更新后自动退出；如果此委托返回false，则忽略更新并按照正常的操作流程继续。</param>
		/// <param name="errorHandler">检查更新发生错误的委托</param>
		public void EnsureNoUpdate(Func<bool?> updateFoundAction = null, Action<Exception> errorHandler = null)
		{
			EnsureNoUpdate<Form>(null, null, null);
		}

		/// <summary>
		/// 确认是否有更新，再继续后面的操作。
		/// </summary>
		/// <typeparam name="T">要显示的UI界面</typeparam>
		/// <param name="updateFoundAction">发现更新的委托。如果此委托为null或返回null，则显示内置的更新对话框。如果此委托返回true，则启动更新后自动退出；如果此委托返回false，则忽略更新并按照正常的操作流程继续。</param>
		/// <param name="errorHandler">检查更新发生错误的委托</param>
		/// <param name="updateUi">用于显示状态的UI界面</param>
		public void EnsureNoUpdate<T>(Func<bool?> updateFoundAction = null, Func<Exception, bool> errorHandler = null, T updateUi = null, bool noUi = false) where T : Form
		{
			Application.EnableVisualStyles();
			var ui = noUi ? null : (Form)updateUi ?? new EnsureUpdate();
			var evt = new ManualResetEvent(false);

			var continueProcess = false;
			Context.EnableEmbedDialog = false;

			if (ui != null)
				ui.Shown += (s, e) => BeginCheckUpdateInProcess();

			EventHandler updateFound = null, versionErrorHandler = null, ueEventHandler = null, noupdateFoundHandler = null;

			var unscribeAllEvents = new Action<Updater, bool, bool>((s, cont, close) =>
			{
				Delegate.RemoveAll(s.UpdatesFound, updateFound);
				Delegate.RemoveAll(s.MinmumVersionRequired, versionErrorHandler);
				Delegate.RemoveAll(s.Error, ueEventHandler);
				Delegate.RemoveAll(s.NoUpdatesFound, noupdateFoundHandler);

				continueProcess = cont;
				if (close)
				{
					ui?.Close();
					evt.Set();
				}
			});
			noupdateFoundHandler = (s, e) => unscribeAllEvents(s as Updater, true, true);
			updateFound = (s, e) =>
			{
				var client = s as Updater;
				var context = client.Context;

				if (context.MustUpdate || context.ForceUpdate)
				{
					Instance_UpdatesFound(s, e);
					unscribeAllEvents(client, false, true);
				}
				else
				{
					if (updateFoundAction != null)
					{
						var result = updateFoundAction();
						if (result == true)
						{
							client.StartExternalUpdater();
							unscribeAllEvents(s as Updater, false, true);

							return;
						}
						if (result == false)
						{
							unscribeAllEvents(s as Updater, true, true);

							return;
						}
					}
					context.EnableEmbedDialog = true;
					Instance_UpdatesFound(s, e);
					unscribeAllEvents(s as Updater, true, true);
				}
			};
			versionErrorHandler = (s, e) =>
			{
				var result = false;
				if (errorHandler != null)
				{
					result = errorHandler(new Exception(string.Format(SR.MinmumVersionRequired_Desc, (s as Updater).Context.UpdateInfo.RequiredMinVersion)));
				}
				else
				{
					MessageBox.Show(string.Format(SR.MinmumVersionRequired_Desc, (s as Updater).Context.UpdateInfo.RequiredMinVersion, (s as Updater).Context.CurrentVersion), SR.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				unscribeAllEvents(s as Updater, result, true);
			};
			ueEventHandler = (s, e) =>
			{
				var err = (s as Updater).Context.Exception;
				var result = Context.TreatErrorAsNotUpdated;
				if (errorHandler != null)
				{
					result = errorHandler(err);
				}
				else
				{
					MessageBox.Show(String.Format(SR.Updater_UnableToCheckUpdate, err.Message), SR.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				unscribeAllEvents(s as Updater, result, true);
			};
			UpdatesFound += updateFound;
			Error += ueEventHandler;
			MinmumVersionRequired += versionErrorHandler;
			NoUpdatesFound += noupdateFoundHandler;

			if (ui != null)
				ui.ShowDialog();
			else
			{
				BeginCheckUpdateInProcess();
				evt.WaitOne();
			}

			if (!continueProcess)
			{
				Environment.Exit(0);
			}
		}


		#endregion
	}
}

