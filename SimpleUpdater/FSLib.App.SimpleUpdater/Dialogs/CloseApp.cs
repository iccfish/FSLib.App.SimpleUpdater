using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using FSLib.App.SimpleUpdater.Wrapper;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	public partial class CloseApp : Form
	{
		public CloseApp()
		{
			InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnAutoClose_Click(object sender, EventArgs e)
		{
			Process[] plist = new Process[processList.Items.Count];
			for (int i = 0; i < plist.Length; i++)
			{
				plist[i] = (processList.Items[i] as ProcessWrapper);
			}

			foreach (Process p in plist)
			{
				p.Kill();
			}
		}

		public void AttachProcessList(IEnumerable<Process> list)
		{
			foreach (Process p in list)
			{
				if (p == null || p.HasExited) continue;
				p.EnableRaisingEvents = true;
				p.Exited += ProcessExited;

				processList.Items.Add(new ProcessWrapper(p));
			}
		}

		void ProcessExited(object sender, EventArgs e)
		{
			Process p = sender as Process;

			foreach (ProcessWrapper pw in processList.Items)
			{
				Process ip = pw;
				if (p == ip)
				{
					processList.Items.Remove(pw);
					break;
				}
			}

			if (processList.Items.Count == 0)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}



		class ProcessWrapper
		{
			/// <summary>
			/// 关联的进程
			/// </summary>
			public Process Process { get; set; }

			/// <summary>
			/// 创建 ProcessWrapper class 的新实例
			/// </summary>
			public ProcessWrapper(Process process)
			{
				Process = process;
			}

			public override string ToString()
			{
				return string.Format("[ID: {0:0000}] {1}", Process.Id, ExtensionMethod.DefaultForEmpty(Process.MainWindowTitle, "(无标题)"));
			}

			public static implicit operator Process(ProcessWrapper pw)
			{
				return pw.Process;
			}

			public static implicit operator ProcessWrapper(Process p)
			{
				return new ProcessWrapper(p);
			}
		}
	}
}
