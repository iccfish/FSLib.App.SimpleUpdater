using System;
using System.Collections.Generic;
using System.Threading;
using FSLib.App.SimpleUpdater.Wrapper;
using System.Diagnostics;

namespace FSLib.App.SimpleUpdater
{
	using Defination;

	public class FileInstaller
	{
		/// <summary>
		/// 文件操作事件类
		/// </summary>
		public class InstallFileEventArgs : EventArgs
		{
			/// <summary>
			/// 获得来源文件
			/// </summary>
			public string Source { get; private set; }

			/// <summary>
			/// 获得目标文件
			/// </summary>
			public string Destination { get; private set; }

			/// <summary> 总数 </summary>
			/// <value></value>
			/// <remarks></remarks>
			public int TotalCount { get; private set; }

			/// <summary> 当前的序号 </summary>
			/// <value></value>
			/// <remarks></remarks>
			public int CurrentCount { get; private set; }

			/// <summary>
			/// 创建 <see cref="InstallFileEventArgs" /> 的新实例
			/// </summary>
			public InstallFileEventArgs(string source, string destination, int totalCount, int currentCount)
			{
				CurrentCount = currentCount;
				TotalCount = totalCount;
				Source = source;
				Destination = destination;
			}
		}

		/// <summary>
		/// 创建一个 <see cref="FileInstaller"/> 的新对象
		/// </summary>
		public FileInstaller()
		{
			PreservedFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		#region 属性

		/// <summary>
		/// 获得或设置更新的信息
		/// </summary>
		public UpdateInfo UpdateInfo { get; set; }

		/// <summary>
		/// 获得或设置当前更新程序所工作的目录
		/// </summary>
		public string WorkingRoot { get; set; }

		private string _applicationRoot;
		/// <summary>
		/// 获得或设置应用程序目录
		/// </summary>
		public string ApplicationRoot
		{
			get
			{
				return _applicationRoot;
			}
			set
			{
				_applicationRoot = value;
				if (!_applicationRoot.EndsWith(@"\")) _applicationRoot += @"\";
			}
		}

		private string _sourceFolder;
		/// <summary>
		/// 安装的源文件夹
		/// </summary>
		public string SourceFolder
		{
			get
			{
				return _sourceFolder;
			}
			set
			{
				_sourceFolder = value;
				if (!_sourceFolder.EndsWith(@"\")) _sourceFolder += @"\";
			}
		}

		/// <summary>
		/// 获得备份路径
		/// </summary>
		string BackupPath
		{
			get
			{
				return System.IO.Path.Combine(WorkingRoot, "backup");
			}
		}

		/// <summary>
		/// 获得还原路径
		/// </summary>
		string RollbackPath
		{
			get
			{
				return System.IO.Path.Combine(WorkingRoot, "rollback");
			}
		}


		/// <summary>
		/// 获得在安装过程中要保留的文件
		/// </summary>
		public Dictionary<string, string> PreservedFiles { get; private set; }


		#endregion

		#region 私有变量

		/// <summary>
		/// 备份文件
		/// </summary>
		readonly List<string> bakList = new List<string>();
		readonly List<string> installedFile = new List<string>();


		#endregion

		#region 事件

		/// <summary> 获得安装过程中发生的错误 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public Exception Exception { get; private set; }

		/// <summary>
		/// 开始删除文件
		/// </summary>
		public event EventHandler DeleteFileStart;

		/// <summary>
		/// 引发 <see cref="DeleteFileStart" /> 事件
		/// </summary>
		protected virtual void OnDeleteFileStart()
		{
			if (DeleteFileStart != null)
			{
				DeleteFileStart(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// 删除文件完成事件
		/// </summary>
		public event EventHandler DeleteFileFinished;

		/// <summary>
		/// 引发 <see cref="DeleteFileFinished" /> 事件
		/// </summary>
		protected virtual void OnDeleteFileFinished()
		{
			if (DeleteFileFinished != null)
				DeleteFileFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 删除文件事件
		/// </summary>
		public event EventHandler<InstallFileEventArgs> DeleteFile;

		/// <summary>
		/// 引发 <see cref="DeleteFile" /> 事件
		/// </summary>
		protected virtual void OnDeleteFile(InstallFileEventArgs ea)
		{
			if (DeleteFile == null)
				return;

			DeleteFile(this, ea);
		}



		/// <summary>
		/// 开始安装文件事件
		/// </summary>
		public event EventHandler InstallFileStart;

		/// <summary>
		/// 引发 <see cref="InstallFileStart" /> 事件
		/// </summary>
		protected virtual void OnInstallFileStart()
		{
			if (InstallFileStart == null)
				return;

			InstallFileStart(this, EventArgs.Empty);
		}

		/// <summary>
		/// 完成安装文件事件
		/// </summary>
		public event EventHandler InstallFileFinished;

		/// <summary>
		/// 引发 <see cref="InstallFileFinished" /> 事件
		/// </summary>
		protected virtual void OnInstallFileFinished()
		{
			if (InstallFileFinished == null)
				return;

			InstallFileFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 安装文件事件
		/// </summary>
		public event EventHandler<InstallFileEventArgs> InstallFile;

		/// <summary>
		/// 引发 <see cref="InstallFile" /> 事件
		/// </summary>
		protected virtual void OnInstallFile(InstallFileEventArgs ea)
		{
			if (InstallFile == null)
				return;

			InstallFile(this, ea);
		}

		/// <summary>
		/// 回滚文件开始事件
		/// </summary>
		public event EventHandler RollbackStart;

		/// <summary>
		/// 引发 <see cref="RollbackStart" /> 事件
		/// </summary>
		protected virtual void OnRollbackStart()
		{
			if (RollbackStart == null)
				return;

			RollbackStart(this, EventArgs.Empty);
		}

		/// <summary>
		/// 回滚文件结束事件
		/// </summary>
		public event EventHandler RollbackFinished;

		/// <summary>
		/// 引发 <see cref="RollbackFinished" /> 事件
		/// </summary>
		protected virtual void OnRollbackFinished()
		{
			if (RollbackFinished == null)
				return;

			RollbackFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 回滚文件事件
		/// </summary>
		public event EventHandler<InstallFileEventArgs> RollbackFile;

		/// <summary>
		/// 引发 <see cref="RollbackFile" /> 事件
		/// </summary>
		protected virtual void OnRollbackFile(InstallFileEventArgs ea)
		{
			if (RollbackFile == null)
				return;

			RollbackFile(this, ea);
		}


		#endregion

		/// <summary>
		/// 安装文件
		/// </summary>
		public bool Install(RunworkEventArgs e)
		{
			if (!DeletePreviousFile(e))
			{
				RollbackFiles(e);
				return false;
			}
			DeleteEmptyDirectories();

			if (!InstallFiles(e))
			{
				DeleteInstalledFiles(e);
				RollbackFiles(e);

				return false;
			}

			return true;
		}

		#region 工作函数

		/// <summary>
		/// 删除空目录
		/// </summary>
		void DeleteEmptyDirectories()
		{
			DeleteEmptyDirectories(ApplicationRoot, false);
		}

		/// <summary>
		/// 删除空目录
		/// </summary>
		void DeleteEmptyDirectories(string path, bool deleteSelf)
		{
			try
			{
				var list = System.IO.Directory.GetDirectories(path);
				foreach (var item in list)
				{
					DeleteEmptyDirectories(item, true);
				}

				if (deleteSelf && System.IO.Directory.GetFileSystemEntries(path).Length == 0)
				{
					Trace.TraceInformation("正在删除空目录 {0}", path);
					System.IO.Directory.Delete(path);
				}
			}
			catch (Exception ex)
			{
				Trace.TraceInformation("删除空目录时发生错误：{0}", ex.Message);
			}
		}



		/// <summary>
		/// 删除原始安装文件
		/// </summary>
		bool DeletePreviousFile(RunworkEventArgs e)
		{
			if (this.UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.None) return true;

			e.PostEvent(OnDeleteFileStart);

			var bakPath = RollbackPath;
			var rules = UpdateInfo.GetDeleteFileLimitRuleSet();

			//找到所有文件
			var allOldFiles = System.IO.Directory.GetFiles(ApplicationRoot, "*.*", System.IO.SearchOption.AllDirectories);

			//备份
			var index = 0;
			foreach (var file in allOldFiles)
			{
				var rPath = file.Remove(0, ApplicationRoot.Length).TrimEnd('\\');
				//保留的文件
				if (PreservedFiles.ContainsKey(rPath))
				{
					Trace.TraceInformation("文件 {0} 在保持文件列表中，跳过删除", file);
					continue;
				}

				var dPath = System.IO.Path.Combine(bakPath, rPath);

				if ((UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.AllExceptSpecified && rules.FindIndex(s => s.IsMatch(rPath)) == -1)
						||
					(UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.NoneButSpecified && rules.FindIndex(s => s.IsMatch(rPath)) != -1)
					)
				{
					e.PostEvent(() => OnDeleteFile(new InstallFileEventArgs(file, dPath, allOldFiles.Length, ++index)));

					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dPath));
					Trace.TraceInformation("备份并删除文件: {0}  ->  {1}", file, dPath);
					System.IO.File.Copy(file, dPath);

					var tryCount = 0;
					while (true)
					{
						++tryCount;

						try
						{
							System.IO.File.Delete(file);
							break;
						}
						catch (Exception ex)
						{
							this.Exception = ex;
							Trace.TraceWarning("第[" + tryCount + "]次删除失败：" + ex.Message);
						}
						//如果删除失败，则等待1秒后重试
						if (tryCount < 10)
							Thread.Sleep(1000);
						else return false;

					}
					bakList.Add(rPath);
				}
			}
			e.PostEvent(OnDeleteFileFinished);

			return true;
		}



		/// <summary>
		/// 安装文件
		/// </summary>
		bool InstallFiles(RunworkEventArgs e)
		{
			e.PostEvent(OnInstallFileStart);

			string[] filelist = CreateNewFileList();
			string OriginalPath, newVersionFile, backupPath;
			OriginalPath = newVersionFile = "";

			var tryCount = 0;
			try
			{
				var index = 0;
				foreach (var file in filelist)
				{
					OriginalPath = System.IO.Path.Combine(ApplicationRoot, file);
					newVersionFile = System.IO.Path.Combine(SourceFolder, file);
					backupPath = System.IO.Path.Combine(BackupPath, file);

					e.PostEvent(() => OnInstallFile(new InstallFileEventArgs(newVersionFile, OriginalPath, filelist.Length, ++index)));

					if (System.IO.File.Exists(OriginalPath))
					{
						System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(backupPath));
						tryCount = 0;

						while (true)
						{
							++tryCount;
							try
							{
								Trace.TraceInformation("第[" + tryCount + "]次尝试备份文件: " + OriginalPath + "  ->  " + backupPath);
								System.IO.File.Move(OriginalPath, backupPath);
								Trace.TraceInformation("备份成功。");
								break;
							}
							catch (Exception ex)
							{
								Trace.TraceWarning("第[" + tryCount + "]次尝试失败： " + ex.Message);

								if (tryCount < 10)
									Thread.Sleep(1000);
								else throw ex;
							}
						}
						bakList.Add(file);
					}
					tryCount = 0;
					while (true)
					{
						++tryCount;
						try
						{
							Trace.TraceInformation("正在复制新版本文件: " + newVersionFile + "  ->  " + OriginalPath);
							System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(OriginalPath));
							System.IO.File.Copy(newVersionFile, OriginalPath);
							Trace.TraceInformation("安装成功");
							break;
						}
						catch (Exception ex)
						{
							Trace.TraceWarning("第[" + tryCount + "]次尝试失败： " + ex.Message);

							if (tryCount < 10)
								Thread.Sleep(1000);
							else throw ex;
						}
					}
					//尝试删除已安装文件
					tryCount = 0;
					while (true)
					{
						++tryCount;
						try
						{
							Trace.TraceInformation("正在尝试删除已安装文件: " + newVersionFile);
							System.IO.File.Delete(newVersionFile);
							Trace.TraceInformation("删除成功");
							break;
						}
						catch (Exception ex)
						{
							Trace.TraceWarning("第[" + tryCount + "]次尝试失败： " + ex.Message);

							if (index < 10)
								Thread.Sleep(1000);
							else break;
						}

					}
					installedFile.Add(file);
					Trace.TraceInformation("安装文件: " + newVersionFile + "  ->  " + OriginalPath);
				}
			}
			catch (Exception ex)
			{
				this.Exception = new Exception(string.Format(SR.Updater_InstallFileError, OriginalPath, newVersionFile, ex.Message));
				Trace.Fail("安装文件时发生错误：" + ex.Message, ex.ToString());
				return false;
			}

			e.PostEvent(OnInstallFileFinished);

			return true;
		}

		/// <summary>
		/// 删除已安装的文件, 并还原原始文件
		/// </summary>
		void DeleteInstalledFiles(RunworkEventArgs e)
		{
			foreach (var filepath in installedFile)
			{
				var originalFile = System.IO.Path.Combine(ApplicationRoot, filepath);

				if (System.IO.File.Exists(originalFile)) System.IO.File.Delete(originalFile);

				Trace.TraceInformation("删除已安装文件: " + originalFile);
			}
		}

		/// <summary>
		/// 回滚备份的文件
		/// </summary>
		void RollbackFiles(RunworkEventArgs e)
		{
			e.PostEvent(OnRollbackStart);
			var rootPath = RollbackPath;

			var index = 0;
			foreach (string file in bakList)
			{
				string newPath = System.IO.Path.Combine(ApplicationRoot, file);
				string oldPath = System.IO.Path.Combine(rootPath, file);

				OnRollbackFile(new InstallFileEventArgs(oldPath, newPath, bakList.Count, ++index));

				Trace.TraceInformation("还原原始文件: " + oldPath + "  ->  " + newPath);
				System.IO.File.Move(oldPath, newPath);
			}

			e.PostEvent(OnRollbackFinished);
		}

		/// <summary>
		/// 创建要安装的新文件列表
		/// </summary>
		/// <returns></returns>
		string[] CreateNewFileList()
		{
			var source = SourceFolder;

			var files = System.IO.Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
			for (var i = 0; i < files.Length; i++)
			{
				files[i] = files[i].Remove(0, source.Length).Trim(new[] { '\\', '/' });
			}

			return files;
		}


		#endregion
	}
}
