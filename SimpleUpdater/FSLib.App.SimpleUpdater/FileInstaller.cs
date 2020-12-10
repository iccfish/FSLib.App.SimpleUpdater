using System;
using System.Collections.Generic;
using System.Threading;
using FSLib.App.SimpleUpdater.Wrapper;
using System.Diagnostics;

namespace FSLib.App.SimpleUpdater
{
	using System.IO;
	using Defination;

	using Logs;

	/// <summary>
	/// 更新文件安装工作类
	/// </summary>
	public class FileInstaller
	{
		private static ILogger _logger = LogManager.Instance.GetLogger<FileInstaller>();

		/// <summary>
		/// 文件操作事件类
		/// </summary>

		/// <summary>
		/// 创建一个 <see cref="FileInstaller"/> 的新对象
		/// </summary>
		public FileInstaller()
		{
			PreservedFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

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

		#region 属性

		/// <summary>
		/// 获得或设置更新的信息
		/// </summary>
		public UpdateInfo UpdateInfo { get; set; }

		/// <summary>
		/// 获得或设置当前更新程序所工作的目录
		/// </summary>
		public string WorkingRoot { get; set; }

		string _applicationRoot;
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

		string _sourceFolder;
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
		/// 获得还原路径
		/// </summary>
		string RollbackPath
		{
			get
			{
				return Path.Combine(WorkingRoot, "rollback");
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
		readonly List<string> _bakList = new List<string>();
		readonly List<string> _installedFile = new List<string>();


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
			var handler = DeleteFileStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
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
			var handler = DeleteFileFinished;
			if (handler != null)
				handler(this, EventArgs.Empty);
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
			var handler = DeleteFile;
			if (handler != null)
				handler(this, ea);
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
			var handler = InstallFileStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
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
			var handler = InstallFileFinished;
			if (handler != null)
				handler(this, EventArgs.Empty);
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
			var handler = InstallFile;
			if (handler != null)
				handler(this, ea);
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
			var handler = RollbackStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
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
			var handler = RollbackFinished;
			if (handler != null)
				handler(this, EventArgs.Empty);
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
			var handler = RollbackFile;
			if (handler != null)
				handler(this, ea);
		}


		#endregion


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
				var list = Directory.GetDirectories(path);
				foreach (var item in list)
				{
					DeleteEmptyDirectories(item, true);
				}

				if (deleteSelf && Directory.GetFileSystemEntries(path).Length == 0)
				{
					_logger.LogInformation($"Removing empty directory '{path}'");
					Directory.Delete(path);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error remove empty directory: {ex.Message}", ex);
			}
		}



		/// <summary>
		/// 删除原始安装文件
		/// </summary>
		bool DeletePreviousFile(RunworkEventArgs e)
		{
			if (UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.None) return true;

			e.PostEvent(OnDeleteFileStart);

			var bakPath = RollbackPath;
			var rules = UpdateInfo.GetDeleteFileLimitRuleSet();

			//找到所有文件
			var allOldFiles = Directory.GetFiles(ApplicationRoot, "*.*", SearchOption.AllDirectories);

			//备份
			var index = 0;
			foreach (var file in allOldFiles)
			{
				e.ReportProgress(allOldFiles.Length, ++index, file);

				var rPath = file.Remove(0, ApplicationRoot.Length).TrimEnd('\\');
				//保留的文件
				if (PreservedFiles.ContainsKey(rPath))
				{
					_logger.LogInformation($"File '{file}' not touched as it was in reserve list.");
					continue;
				}

				var dPath = Path.Combine(bakPath, rPath);

				if ((UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.AllExceptSpecified && rules.FindIndex(s => s.IsMatch(rPath)) == -1)
						||
					(UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.NoneButSpecified && rules.FindIndex(s => s.IsMatch(rPath)) != -1)
					)
				{
					e.PostEvent(() => OnDeleteFile(new InstallFileEventArgs(file, dPath, allOldFiles.Length, index, rPath)));
					Directory.CreateDirectory(Path.GetDirectoryName(dPath));
					_logger.LogInformation($"Backup and remove file: '{file}'  -> '{dPath}'");
					File.Copy(file, dPath);

					var tryCount = 0;
					while (true)
					{
						++tryCount;

						try
						{
							File.Delete(file);
							break;
						}
						catch (Exception ex)
						{
							Exception = ex;
							_logger.LogError($"No.{tryCount} attempt to remove file failed: {ex.Message}", ex);
						}
						//如果删除失败，则等待1秒后重试
						if (tryCount < 10)
							Thread.Sleep(1000);
						else return false;

					}
					_bakList.Add(rPath);
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
					e.ReportProgress(filelist.Length, ++index, file);

					OriginalPath = Path.Combine(ApplicationRoot, file);
					newVersionFile = Path.Combine(SourceFolder, file);
					backupPath = Path.Combine(RollbackPath, file);

					e.PostEvent(() => OnInstallFile(new InstallFileEventArgs(newVersionFile, OriginalPath, filelist.Length, index, file)));

					if (File.Exists(OriginalPath))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(backupPath));
						tryCount = 0;

						while (true)
						{
							++tryCount;
							try
							{
								if (File.Exists(OriginalPath))
								{
									_logger.LogInformation("No." + tryCount + " attempt to backup file: '" + OriginalPath + "' -> '" + backupPath + "'");
									File.Copy(OriginalPath, backupPath, true);
									_logger.LogInformation("No." + tryCount + "attempt to delete file: '" + OriginalPath);
									File.Delete(OriginalPath);
									_logger.LogInformation("Backup succeed.");
								}

								break;
							}
							catch (Exception ex)
							{
								_logger.LogError($"No.{tryCount} attempt failed:{ex.Message}", ex);

								if (tryCount < 20)
									Thread.Sleep(1000);
								else throw ex;
							}
						}
						_bakList.Add(file);
					}
					tryCount = 0;
					while (true)
					{
						++tryCount;
						try
						{
							_logger.LogInformation("Copying new file: '" + newVersionFile + "' -> '" + OriginalPath + "'");
							Directory.CreateDirectory(Path.GetDirectoryName(OriginalPath));
							File.Copy(newVersionFile, OriginalPath);
							_logger.LogInformation("Installation succeed.");
							break;
						}
						catch (Exception ex)
						{
							_logger.LogWarning($"No.{tryCount}] attempt failed: {ex.Message}");

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
							_logger.LogInformation("Trying removing file: " + newVersionFile);
							File.Delete(newVersionFile);
							_logger.LogInformation("Delete succeed.");
							break;
						}
						catch (Exception ex)
						{
							_logger.LogWarning("No." + tryCount + " attempt failed: " + ex.Message);

							if (tryCount < 10)
								Thread.Sleep(1000);
							else break;
						}

					}
					_installedFile.Add(file);
					_logger.LogInformation("Install file: '" + newVersionFile + "' -> '" + OriginalPath + "'");
				}
			}
			catch (Exception ex)
			{
				Exception = new Exception(string.Format(SR.Updater_InstallFileError, OriginalPath, newVersionFile, ex.Message));
				_logger.LogError("Install file failed: " + ex.Message, ex);
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
			foreach (var filepath in _installedFile)
			{
				var originalFile = Path.Combine(ApplicationRoot, filepath);

				if (File.Exists(originalFile))
					File.Delete(originalFile);

				_logger.LogInformation("Removing installed file: " + originalFile);
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
			foreach (string file in _bakList)
			{
				e.ReportProgress(_bakList.Count, ++index, file);

				var newPath = Path.Combine(ApplicationRoot, file);
				var oldPath = Path.Combine(rootPath, file);

				OnRollbackFile(new InstallFileEventArgs(oldPath, newPath, _bakList.Count, index, file));

				_logger.LogInformation("Restore original file: '" + oldPath + "' -> '" + newPath + "'");
				File.Move(oldPath, newPath);
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

			var files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
			for (var i = 0; i < files.Length; i++)
			{
				files[i] = files[i].Remove(0, source.Length).Trim(new[] { '\\', '/' });
			}

			return files;
		}


		#endregion
	}
}
