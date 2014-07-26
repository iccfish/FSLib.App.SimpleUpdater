using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator
{
	using Defination;

	using SimpleUpdater.Defination;

	class UpdatePackageBuilder
	{
		/// <summary>
		/// 获得唯一实例
		/// </summary>
		public static UpdatePackageBuilder Instance
		{
			get { return _instance ?? new UpdatePackageBuilder(); }
			private set { _instance = value; }
		}

		/// <summary>
		/// 实例构造函数
		/// </summary>
		private UpdatePackageBuilder()
		{
			Instance = this;
		}

		#region 公共属性


		/// <summary>
		/// 获得或设置当前的升级基础信息
		/// </summary>
		public UpdateInfo UpdateInfo { get { return AuProject.UpdateInfo; } }

		/// <summary>
		/// 获得已创建的更新项目
		/// </summary>
		public UpdateInfo BuiltUpdateInfo { get; private set; }

		/// <summary>
		/// 获得当前的项目
		/// </summary>
		public AuProject AuProject
		{
			get
			{
				return _auProject ?? (AuProject = new AuProject());
			}
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
			using (var ms = new System.IO.MemoryStream())
			using (var zs = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress))
			{
				var buffer = System.IO.File.ReadAllBytes(path);
				zs.Write(buffer, 0, buffer.Length);
				zs.Close();
				ms.Close();

				System.IO.File.WriteAllBytes(destFile, ms.ToArray());
			}
		}

		/// <summary>
		/// 创建压缩包
		/// </summary>
		/// <param name="zipFile"></param>
		/// <param name="e"></param>
		public void CreateZip(string title, string zipFile, string pwd, Wrapper.RunworkEventArgs e, KeyValuePair<string, FileInfo>[] files)
		{
			using (var fs = new System.IO.FileStream(zipFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			using (var zip = new ICCEmbedded.SharpZipLib.Zip.ZipOutputStream(fs))
			{
				zip.Password = pwd;
				zip.SetLevel(9);

				var entryFactory = new ICCEmbedded.SharpZipLib.Zip.ZipEntryFactory();

				//合并路径
				var fileGroups = files.GroupBy(s => System.IO.Path.GetDirectoryName(s.Key)).ToDictionary(s => s.Key, s => s.ToArray());
				var folders = fileGroups.Keys.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToArray();

				folders.ForEach(s =>
				{
					if (!string.IsNullOrEmpty(s))
					{
						var fe = entryFactory.MakeDirectoryEntry(s);
						fe.DateTime = DateTime.Now;
						zip.PutNextEntry(fe);
						zip.CloseEntry();
					}

					foreach (var f in fileGroups[s])
					{
						if (e != null)
							e.ReportProgress(e.Progress.TaskCount, e.Progress.TaskProgress + 1, string.Format(title, f.Key));

						var ent = entryFactory.MakeFileEntry(f.Key);
						ent.DateTime = f.Value.LastWriteTime;

						//复制文件内容。简单起见，这里不返回进度显示。。。
						using (var sou = f.Value.OpenRead())
						{
							ent.Size = sou.Length;
							zip.PutNextEntry(ent);
							ICCEmbedded.SharpZipLib.Core.StreamUtils.Copy(sou, zip, new byte[0x400]);
						}
						zip.CloseEntry();
					}
				});
				zip.Flush();
				zip.Close();
			}
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
					var fdi = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
					UpdateInfo.AppVersion = fdi.ProductVersion;
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
					UpdateInfo.RtfUpdateNote = Convert.ToBase64String(ExtensionMethods.CompressBuffer(System.IO.File.ReadAllBytes(path)));
			}
		}

		/// <summary>
		/// 创建指定包
		/// </summary>
		/// <param name="e"></param>
		public void Build(Wrapper.RunworkEventArgs e)
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
				throw new ApplicationException("准备升级信息时发生异常");

			Result = new Dictionary<string, string>();
			BuildPackages(e, ui);


			//保存信息文件
			var xmlPath = GetXmlFilePath(false);
			ui.XmlSerilizeToFile(xmlPath);
			if (!AuProject.CompressPackage || AuProject.CreateCompatiblePackage)
			{
				Result.Add(Path.GetFileName(xmlPath), "兼容升级模式（或未开启增量更新时）的升级信息文件");
			}

			//压缩？
			if (AuProject.CompressPackage)
			{
				var xmlCompressPath = GetXmlFilePath(true);
				CompressFile(xmlPath, xmlCompressPath);
				Result.Add(Path.GetFileName(xmlCompressPath), "已压缩的升级信息文件");

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
		public void BuildPackages(Wrapper.RunworkEventArgs e, UpdateInfo ui)
		{
			var targetDir = AuProject.ParseFullPath(AuProject.DestinationDirectory);
			var appDir = AuProject.ParseFullPath(AuProject.ApplicationDirectory);

			if (!Directory.Exists(appDir))
				throw new ApplicationException("无效的应用程序目录");
			if (!Directory.Exists(targetDir))
			{
				try
				{
					Directory.CreateDirectory(targetDir);
				}
				catch (Exception ex)
				{
					throw new ApplicationException("无法创建目标目录", ex);
				}
			}

			e.ReportProgress(0, 0, "正在扫描文件列表...");
			FileInfo[] allfiles;

			try
			{
				allfiles = new DirectoryInfo(appDir).GetFiles("*.*", SearchOption.AllDirectories);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("无法扫描来源目录", ex);
			}

			//生成映射，排除忽略列表
			e.ReportProgress(0, 0, "正在准备文件列表...");
			var projectItems = AuProject.Files.ToDictionary(s => s.Path, StringComparer.OrdinalIgnoreCase);
			var targetfiles = allfiles.Select(s => new KeyValuePair<string, FileInfo>(s.FullName.Remove(0, appDir.Length).Trim(Path.DirectorySeparatorChar), s))
				.Where(s => (!projectItems.ContainsKey(s.Key) && AuProject.DefaultUpdateMethod != UpdateMethod.Ignore) || (projectItems.ContainsKey(s.Key) && projectItems[s.Key].UpdateMethod != UpdateMethod.Ignore))
				.ToArray();

			//古典版的安装包？
			if (!AuProject.EnableIncreaseUpdate || AuProject.CreateCompatiblePackage)
			{
				var mainPkgId = GetPackageName("main") + ".zip";
				var file = System.IO.Path.Combine(targetDir, mainPkgId);
				Result.Add(mainPkgId, "兼容升级模式（或未开启增量更新时）的升级包文件");
				e.Progress.TaskCount = targetfiles.Length;
				CreateZip("正在生成兼容版升级包，正在压缩 {0}", file, ui.PackagePassword, e, targetfiles);

				var fileInfo = new System.IO.FileInfo(file);
				ui.PackageSize = fileInfo.Length;
				e.ReportProgress(0, 0, "正在计算包文件Hash...");
				ui.MD5 = Wrapper.ExtensionMethod.GetFileHash(file);
				ui.Package = mainPkgId;
			}
			if (!AuProject.EnableIncreaseUpdate) return;

			//生成主文件包
			e.ReportProgress(targetfiles.Length, 0, "");
			ui.Packages = new List<PackageInfo>();
			var mainFiles = targetfiles
				.Where(s => (!projectItems.ContainsKey(s.Key) && AuProject.DefaultUpdateMethod == UpdateMethod.Always) || (projectItems.ContainsKey(s.Key) && projectItems[s.Key].UpdateMethod == UpdateMethod.Always))
				.ToArray();
			if (mainFiles.Length > 0)
			{
				var mainPkgId = GetPackageName("alwaysintall") + ".zip";
				var pkgName = Path.Combine(targetDir, mainPkgId);
				e.Progress.TaskCount = mainFiles.Length;
				CreateZip("正在生成全局升级包，正在压缩 {0}", pkgName, ui.PackagePassword, e, mainFiles);
				Result.Add(mainPkgId, "全局升级包，包含必须更新的文件");


				var fileInfo = new System.IO.FileInfo(pkgName);
				ui.Packages.Add(new PackageInfo()
				{
					Version = "0.0.0.0",
					VerificationLevel = FileVerificationLevel.None,
					FilePath = "",
					FileSize = 0L,
					FileHash = "",
					PackageHash = Wrapper.ExtensionMethod.GetFileHash(pkgName),
					PackageName = mainPkgId,
					PackageSize = fileInfo.Length,
					Method = UpdateMethod.Always,
					Files = mainFiles.Select(s => s.Key).ToArray()
				});
			}

			//针对单个文件生成包
			e.Progress.TaskCount = targetfiles.Length;
			e.Progress.TaskProgress = 0;
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
				}

				//file info
				var fdi = System.Diagnostics.FileVersionInfo.GetVersionInfo(file.Value.FullName);
				//var pkgFileName = file.Key.Replace("\\", "_").Replace(".", "_") + ".zip";
				var pkgFileName = GetPackageName(file.Key) + ".zip";

				var pkg = Path.Combine(targetDir, pkgFileName);
				e.ReportProgress(e.Progress.TaskCount, ++e.Progress.TaskProgress, "正在生成增量包 " + file.Key + ", 正在压缩....");
				CreateZip(null, pkg, ui.PackagePassword, null, new[] { file });
				Result.Add(pkgFileName, "文件【" + file.Key + "】的增量升级包");

				var pkgInfo = new System.IO.FileInfo(pkg);
				ui.Packages.Add(new PackageInfo()
				{
					Version = string.IsNullOrEmpty(fdi.ProductVersion) ? "0.0.0.0" :  Wrapper.ExtensionMethod.ConvertVersionInfo(fdi).ToString(),
					VerificationLevel = config.FileVerificationLevel,
					FilePath = file.Key,
					FileSize = new FileInfo(file.Value.FullName).Length,
					FileHash = Wrapper.ExtensionMethod.GetFileHash(file.Value.FullName),
					PackageHash = Wrapper.ExtensionMethod.GetFileHash(pkg),
					PackageName = pkgFileName,
					PackageSize = pkgInfo.Length,
					Method = config.UpdateMethod,
					Files = new[] { file.Key }
				});
			}
		}


		static MD5 _md5 = MD5.Create();
		static UpdatePackageBuilder _instance;

		static string GetPackageName(string path)
		{
			return BitConverter.ToString(_md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(path.ToLower()))).Replace("-", "").ToUpper();
		}
	}
}
