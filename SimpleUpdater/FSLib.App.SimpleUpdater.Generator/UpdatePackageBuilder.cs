using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator
{
	using System.Diagnostics;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

	using Defination;

	using SimpleUpdater.Defination;

	using Wrapper;

	using ZipBuilder;

	class UpdatePackageBuilder
	{
		/// <summary>
		/// 获得唯一实例
		/// </summary>
		public static UpdatePackageBuilder Instance => _instance ??= new UpdatePackageBuilder();

		/// <summary>
		/// 实例构造函数
		/// </summary>
		private UpdatePackageBuilder()
		{
		}

		#region 公共属性


		/// <summary>
		/// 获得或设置当前的升级基础信息
		/// </summary>
		public UpdateInfo UpdateInfo => AuProject.UpdateInfo;

		/// <summary>
		/// 获得已创建的更新项目
		/// </summary>
		public UpdateInfo BuiltUpdateInfo { get; private set; }

		/// <summary>
		/// 获得所有的构建任务信息
		/// </summary>
		public IEnumerable<ZipTask> AllPackageBuildTasks { get; private set; }

		/// <summary>
		/// 获得当前的项目
		/// </summary>
		public AuProject AuProject
		{
			get => _auProject ?? (AuProject = new AuProject());
			set
			{
				if (_auProject != null)
				{
					OnProjectClosed(new PackageEventArgs(_auProject));
				}
				_auProject = value;
				if (_auProject != null)
					OnProjectLoaded(new PackageEventArgs(_auProject));
			}
		}
		#endregion

		#region 事件区域

		/// <summary>
		/// 项目已加载
		/// </summary>
		public event EventHandler<PackageEventArgs> ProjectLoaded;

		/// <summary>
		/// 引发 <see cref="ProjectLoaded" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnProjectLoaded(PackageEventArgs ea)
		{
			var handler = ProjectLoaded;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary>
		/// 项目正在卸载
		/// </summary>
		public event EventHandler<PackageEventArgs> ProjectClosing;

		/// <summary>
		/// 项目已卸载
		/// </summary>
		public event EventHandler<PackageEventArgs> ProjectClosed;
		/// <summary>
		/// 引发 <see cref="ProjectClosed" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnProjectClosed(PackageEventArgs ea)
		{
			var handler = ProjectClosed;
			if (handler != null)
				handler(this, ea);
		}

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler FilePackagingBegin;

		/// <summary>
		/// 引发 <see cref="FilePackagingBegin"/> 事件
		/// </summary>

		protected virtual void OnFilePackagingBegin()
		{
			FilePackagingBegin?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler FilePackagingEnd;

		/// <summary>
		/// 引发 <see cref="FilePackagingEnd"/> 事件
		/// </summary>

		protected virtual void OnFilePackagingEnd()
		{
			FilePackagingEnd?.Invoke(this, EventArgs.Empty);
		}

		#endregion


		AuProject _auProject;

		/// <summary>
		/// 获得或设置所有的结果
		/// </summary>
		public Dictionary<string, string> Result { get; set; }

		/// <summary>
		/// 获得XML文件路径
		/// </summary>
		/// <param name="compressed"></param>
		/// <returns></returns>
		public string GetXmlFilePath(bool compressed)
		{
			return Path.Combine(AuProject.ParseFullPath(AuProject.DestinationDirectory), "update" + (compressed ? "_c" : "") + ".xml");
		}

		/// <summary>
		/// 压缩指定文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="destFile"></param>
		public void CompressFile(string path, string destFile)
		{
			using var ms = new MemoryStream();
			using var zs = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);

			var buffer = File.ReadAllBytes(path);
			zs.Write(buffer, 0, buffer.Length);
			zs.Close();
			ms.Close();

			File.WriteAllBytes(destFile, ms.ToArray());
		}

		/// <summary>
		/// 自动从绑定的信息中绑定信息
		/// </summary>
		public void AutoLoadInformations()
		{
			if (AuProject == null)
				return;

			//自动取版本号
			if (!string.IsNullOrEmpty(AuProject.VersionUpdateSrc))
			{
				var path = AuProject.ParseFullPath(AuProject.VersionUpdateSrc);
				if (File.Exists(path))
				{
					UpdateInfo.AppVersion = ExtensionMethod.ConvertVersionInfo(System.Diagnostics.FileVersionInfo.GetVersionInfo(path)).ToString();
				}
			}

			//自动读取更新记录
			if (!string.IsNullOrEmpty(AuProject.UpdateContentSrc))
			{
				var path = AuProject.ParseFullPath(AuProject.UpdateContentSrc);
				if (File.Exists(path))
				{
					UpdateInfo.Desc = File.ReadAllText(path);
				}
			}

			//准备UpdateInfo
			if (!string.IsNullOrEmpty(AuProject.UpdateRtfNotePath))
			{
				var path = AuProject.ParseFullPath(AuProject.UpdateRtfNotePath);
				if (File.Exists(path))
					UpdateInfo.RtfUpdateNote = Convert.ToBase64String(ExtensionMethods.CompressBuffer(File.ReadAllBytes(path)));
			}
		}

		/// <summary>
		/// 清理目标目录
		/// </summary>
		void CleanTargetDirectory()
		{
			var directory = AuProject.ParseFullPath(AuProject.DestinationDirectory);
			//删除 update*.xml
			foreach (var file in Directory.GetFiles(directory, "update*.xml"))
			{
				File.Delete(file);
			}
			//删除package
			foreach (var file in Directory.GetFiles(directory, "*." + AuProject.PackageExtension))
			{
				File.Delete(file);
			}
		}

		/// <summary>
		/// 创建指定包
		/// </summary>
		/// <param name="e"></param>
		public void Build(RunworkEventArgs e)
		{
			e.ReportProgress(0, 0, "正在准备信息...");
			AutoLoadInformations();

			UpdateInfo ui = null;
			using (var ms = new MemoryStream())
			{
				UpdateInfo.XmlSerializeToStream(ms);
				ms.Flush();
				ms.Seek(0, SeekOrigin.Begin);
				ui = ms.XmlDeserializeFromStream<UpdateInfo>();
			}
			if (ui == null)
				throw new ApplicationException("error occoured while preparing package.");

			if (AuProject.CleanBeforeBuild)
			{
				e.ReportProgress(0, 0, "cleanup directory...");
				CleanTargetDirectory();
			}

			Result = new Dictionary<string, string>();
			PreparePackageBuildTasks(e, ui);

			//生成压缩包信息
			OnFilePackagingBegin();
			if (AuProject.UseParallelBuilding)
			{
				Parallel.ForEach(AllPackageBuildTasks, task => task.Build());
			}
			else
			{
				foreach (var task in AllPackageBuildTasks)
				{
					task.Build();
				}
			}
			OnFilePackagingEnd();

			//保存信息文件
			var xmlPath = GetXmlFilePath(false);
			ui.XmlSerilizeToFile(xmlPath);
			if (!AuProject.CompressPackage || AuProject.CreateCompatiblePackage)
			{
				Result.Add(Path.GetFileName(xmlPath), "update information without compression.");
			}

			//压缩？
			if (AuProject.CompressPackage)
			{
				var xmlCompressPath = GetXmlFilePath(true);
				CompressFile(xmlPath, xmlCompressPath);
				Result.Add(Path.GetFileName(xmlCompressPath), "compressed update information file");

				if (!AuProject.CreateCompatiblePackage)
				{
					File.Delete(xmlPath);
				}
			}

			BuiltUpdateInfo = ui;
		}

		/// <summary>
		/// 创建指定包
		/// </summary>
		/// <param name="e"></param>
		public void PreparePackageBuildTasks(RunworkEventArgs e, UpdateInfo ui)
		{
			var targetDir = AuProject.ParseFullPath(AuProject.DestinationDirectory);
			var appDir = AuProject.ParseFullPath(AuProject.ApplicationDirectory);

			if (!Directory.Exists(appDir))
				throw new ApplicationException("invalid application directory");
			if (!Directory.Exists(targetDir))
			{
				try
				{
					Directory.CreateDirectory(targetDir);
				}
				catch (Exception ex)
				{
					throw new ApplicationException("unable to create destination director.", ex);
				}
			}

			e.ReportProgress(0, 0, "scanning files...");
			FileInfo[] allfiles;

			try
			{
				allfiles = new DirectoryInfo(appDir).GetFiles("*.*", SearchOption.AllDirectories);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("unable to scan source directory.", ex);
			}

			//生成映射，排除忽略列表
			e.ReportProgress(0, 0, "preparing file list...");
			var projectItems = AuProject.Files.ToDictionary(s => s.Path, StringComparer.OrdinalIgnoreCase);
			var targetfiles = allfiles.Select(s => new KeyValuePair<string, FileInfo>(s.FullName.Remove(0, appDir.Length).Trim(Path.DirectorySeparatorChar), s)).Where(s => (!projectItems.ContainsKey(s.Key) && AuProject.DefaultUpdateMethod != UpdateMethod.Ignore) || (projectItems.ContainsKey(s.Key) && projectItems[s.Key].UpdateMethod != UpdateMethod.Ignore)).ToDictionary(s => s.Key, s => s.Value);

			//任务
			var tasks = new List<ZipTask>();

			//古典版的安装包？
			if (!AuProject.EnableIncreaseUpdate || AuProject.CreateCompatiblePackage)
			{
				var mainPkgId = GetPackageName("main") + "." + AuProject.PackageExtension;
				var task = new ZipTask(mainPkgId, targetfiles, UpdateMethod.Always, FileVerificationLevel.None, _auProject, "Full package");
				task.OnDone = () =>
				{
					ui.Package = mainPkgId;
					ui.PackageSize = task.PackageLength;
					ui.MD5 = task.PackageHash;
				};
				tasks.Add(task);
				Result.Add(mainPkgId, task.PackageDescription);
				e.ReportProgress(0, 0, $"building task: full package ({targetfiles.Count} files)");
			}

			var updaterClientIncluded = false;
			if (AuProject.EnableIncreaseUpdate)
			{
				ui.Packages = new List<PackageInfo>();

				var mainFiles = targetfiles.Where(s => (!projectItems.ContainsKey(s.Key) && AuProject.DefaultUpdateMethod == UpdateMethod.Always) || (projectItems.ContainsKey(s.Key) && projectItems[s.Key].UpdateMethod == UpdateMethod.Always)).ToDictionary(s => s.Key, s => s.Value);

				if (mainFiles.Count > 0)
				{
					var pkgId = GetPackageName("alwaysintall") + "." + AuProject.PackageExtension;

					var task = new ZipTask(pkgId, mainFiles, UpdateMethod.Always, FileVerificationLevel.None, _auProject, "global package");
					task.OnDone = () =>
					{
						ui.Packages.Add(new PackageInfo()
						{
							Version = "0.0.0.0",
							VerificationLevel = FileVerificationLevel.None,
							FilePath = "",
							FileSize = 0L,
							FileHash = "",
							PackageHash = task.PackageHash,
							PackageName = pkgId,
							PackageSize = task.PackageLength,
							Method = UpdateMethod.Always,
							Files = mainFiles.Select(s => s.Key).ToArray()
						});
					};
					tasks.Add(task);
					Result.Add(pkgId, task.PackageDescription);

				}

				//针对单个文件生成包
				foreach (var file in targetfiles)
				{
					ProjectItem config;
					if (!projectItems.ContainsKey(file.Key))
					{
						if (AuProject.DefaultUpdateMethod == UpdateMethod.Always || AuProject.DefaultUpdateMethod == UpdateMethod.Ignore)
							continue;
						config = new ProjectItem()
						{
							UpdateMethod = AuProject.DefaultUpdateMethod,
							FileVerificationLevel = AuProject.DefaultFileVerificationLevel
						};
					}
					else
					{
						config = projectItems[file.Key];
						//fix always pack issue
						if (config.UpdateMethod == UpdateMethod.Always)
							continue;
					}

					//file info
					var fdi = FileVersionInfo.GetVersionInfo(file.Value.FullName);
					var pkgFileName = GetPackageName(file.Key) + "." + AuProject.PackageExtension;

					var task = new ZipTask(pkgFileName, new Dictionary<string, FileInfo>() { [file.Key] = file.Value }, config.UpdateMethod, config.FileVerificationLevel, _auProject, $"{file.Key}");
					var isSimpleUpdateClient = !updaterClientIncluded && Regex.IsMatch(file.Key, @"(^|[\\/]?)SimpleUpdater\.dll$");
					task.OnDone = () =>
					{
						var detail = new PackageInfo()
						{
							Version = string.IsNullOrEmpty(fdi.FileVersion) ? "0.0.0.0" : ExtensionMethod.ConvertVersionInfo(fdi).ToString(),
							VerificationLevel = config.FileVerificationLevel,
							FilePath = file.Key,
							FileSize = file.Value.Length,
							FileHash = task.FileHash,
							PackageHash = task.PackageHash,
							PackageName = pkgFileName,
							PackageSize = task.PackageLength,
							Method = config.UpdateMethod,
							Files = new[] { file.Key },
							ComponentId = config.Flag
						};
						ui.Packages.Add(detail);

						//检测是否是自动更新的
						if (isSimpleUpdateClient)
						{
							ui.UpdaterClient = detail;
						}
					};
					updaterClientIncluded |= isSimpleUpdateClient;
					tasks.Add(task);
					Result.Add(pkgFileName, task.PackageDescription);
				}
			}

			if (!updaterClientIncluded)
			{
				var updaterClient = typeof(Updater).Assembly.Location;
				var version = ExtensionMethod.ConvertVersionInfo(FileVersionInfo.GetVersionInfo(updaterClient));
				var pkgFileName = GetPackageName(Path.GetFileName(updaterClient)) + "." + AuProject.PackageExtension;
				var fileInfo = new FileInfo(updaterClient);

				var task = new ZipTask(pkgFileName, new Dictionary<string, FileInfo>() { [Path.GetFileName(updaterClient)] = fileInfo }, UpdateMethod.VersionCompare, FileVerificationLevel.Hash | FileVerificationLevel.Size | FileVerificationLevel.Version, _auProject, "Asset package required for updater client.");
				task.OnDone = () =>
				{
					var detail = new PackageInfo()
					{
						Version = version.ToString(),
						VerificationLevel = task.VerificationLevel,
						FilePath = task.Files.First().Key,
						FileSize = task.FileLength,
						FileHash = task.FileHash,
						PackageHash = task.PackageHash,
						PackageName = pkgFileName,
						PackageSize = task.PackageLength,
						Method = task.UpdateMethod,
						Files = new[] { task.Files.First().Key },
						ComponentId = null
					};

					ui.UpdaterClient = detail;
				};
				Result.Add(pkgFileName, task.PackageDescription);
			}

			AllPackageBuildTasks = tasks;
			e.ReportProgress(0, 0, $"task generation finished, {tasks.Count} packages needs to be built.");
		}


		static SHA1 _sha1 = SHA1.Create();
		static UpdatePackageBuilder _instance;
		Dictionary<string, string> _nameCache = new(StringComparer.OrdinalIgnoreCase);

		string GetPackageName(string path)
		{
			if (_nameCache.ContainsKey(path))
				return _nameCache[path];

			if (AuProject.UseRandomPackageNaming)
			{
				var name = Guid.NewGuid().ToString("N");
				_nameCache[path] = name;
				return name;
			}

			return BitConverter.ToString(_sha1.ComputeHash(Encoding.Unicode.GetBytes(path.ToLower()))).Replace("-", "").ToUpper();
		}
	}
}
