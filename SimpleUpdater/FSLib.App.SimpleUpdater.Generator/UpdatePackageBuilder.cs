using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator
{
	using SimpleUpdater.Defination;

	class UpdatePackageBuilder
	{
		/// <summary>
		/// 获得或设置当前的升级基础信息
		/// </summary>
		public UpdateInfo UpdateInfo { get; set; }

		/// <summary>
		/// 升级包的路径
		/// </summary>
		public string PackagePath { get; set; }

		/// <summary>
		/// 获得或设置是否生成压缩版的XML信息文件
		/// </summary>
		public bool CompressXmlFile { get; set; }

		/// <summary>
		/// 获得或设置是否生成兼容版的升级包
		/// </summary>
		public bool CreateCompatiblePackage { get; set; }

		/// <summary>
		/// 获得或设置是否允许增量升级包
		/// </summary>
		public bool EnableIncreaseUpdate { get; set; }

		/// <summary>
		/// 获得或设置当前更新的文件
		/// </summary>
		public KeyValuePair<string, FileInfo>[] AllFiles { get; set; }

		public Func<string, string, Version> GetVersionHandler;

		public Func<string, UpdateMethod> GetUpdateMethodHandler;

		public Func<string, FileVerificationLevel> GetVerificationLevelHandler;

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
			return Path.Combine(PackagePath, "update" + (compressed ? "_c" : "") + ".xml");
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
		public void CreateZip(string title, string zipFile, Wrapper.RunworkEventArgs e, KeyValuePair<string, FileInfo>[] files)
		{
			using (var fs = new System.IO.FileStream(zipFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			using (var zip = new ICCEmbedded.SharpZipLib.Zip.ZipOutputStream(fs))
			{
				zip.Password = UpdateInfo.PackagePassword;
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
						e.ReportProgress(e.Progress.TaskCount, e.Progress.TaskProgress + 1, string.Format(title, f.Key));

						var ent = entryFactory.MakeFileEntry(f.Key);
						ent.DateTime = f.Value.LastWriteTime;
						zip.PutNextEntry(ent);

						//复制文件内容。简单起见，这里不返回进度显示。。。
						using (var sou = f.Value.OpenRead())
						{
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
		/// 创建指定包
		/// </summary>
		/// <param name="e"></param>
		public void Build(Wrapper.RunworkEventArgs e)
		{
			Result = new Dictionary<string, string>();
			BuildPackages(e);

			//保存信息文件
			var xmlPath = GetXmlFilePath(false);
			UpdateInfo.XmlSerilizeToFile(xmlPath);
			if (!CompressXmlFile || CreateCompatiblePackage)
			{
				Result.Add(Path.GetFileName(xmlPath), "兼容升级模式（或未开启增量更新时）的升级信息文件");
			}

			//压缩？
			if (CompressXmlFile)
			{
				var xmlCompressPath = GetXmlFilePath(true);
				CompressFile(xmlPath, xmlCompressPath);
				Result.Add(Path.GetFileName(xmlCompressPath), "已压缩的升级信息文件");

				if (!CreateCompatiblePackage)
				{
					File.Delete(xmlPath);
				}
			}
		}

		/// <summary>
		/// 创建指定包
		/// </summary>
		/// <param name="e"></param>
		public void BuildPackages(Wrapper.RunworkEventArgs e)
		{
			e.Progress.TaskCount = AllFiles.Length;

			//古典版的安装包？
			if (!EnableIncreaseUpdate || CreateCompatiblePackage)
			{
				var mainPkgId = GetPackageName("main") + ".zip";
				Result.Add(mainPkgId, "兼容升级模式（或未开启增量更新时）的升级包文件");
				var file = System.IO.Path.Combine(PackagePath, mainPkgId);
				CreateZip("正在生成兼容版升级包，正在压缩 {0}", file, e, AllFiles);

				var fileInfo = new System.IO.FileInfo(file);
				UpdateInfo.PackageSize = fileInfo.Length;
				e.ReportProgress(0, 0, "正在计算包文件Hash...");
				UpdateInfo.MD5 = Wrapper.ExtensionMethod.GetFileHash(file);
				UpdateInfo.Package = mainPkgId;
			}
			if (!EnableIncreaseUpdate) return;

			//生成主文件包
			e.ReportProgress(AllFiles.Length, 0, "");
			UpdateInfo.Packages = new List<PackageInfo>();
			var mainFiles = AllFiles.Where(s => GetUpdateMethodHandler(s.Key) == UpdateMethod.Always).ToArray();
			if (mainFiles.Length > 0)
			{
				var mainPkgId = GetPackageName("alwaysintall") + ".zip";

				var pkgName = Path.Combine(PackagePath, mainPkgId);
				CreateZip("正在生成全局升级包，正在压缩 {0}", pkgName, e, mainFiles);
				Result.Add(mainPkgId, "全局升级包，包含必须更新的文件");


				var fileInfo = new System.IO.FileInfo(pkgName);
				UpdateInfo.Packages.Add(new PackageInfo()
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
			foreach (var file in AllFiles)
			{
				var method = GetUpdateMethodHandler(file.Key);
				if (method == UpdateMethod.Always) continue;

				var vm = GetVerificationLevelHandler(file.Key);
				var ver = GetVersionHandler(file.Key, file.Value.FullName);
				//var pkgFileName = file.Key.Replace("\\", "_").Replace(".", "_") + ".zip";
				var pkgFileName = GetPackageName(file.Key) + ".zip";

				var pkg = Path.Combine(PackagePath, pkgFileName);
				CreateZip("正在生成增量包 " + file.Key + ", 正在压缩....", pkg, e, new[] { file });
				Result.Add(pkgFileName, "文件【" + file.Key + "】的增量升级包");

				var pkgInfo = new System.IO.FileInfo(pkg);
				UpdateInfo.Packages.Add(new PackageInfo()
				{
					Version = ver.ToString(),
					VerificationLevel = vm,
					FilePath = file.Key,
					FileSize = new FileInfo(file.Value.FullName).Length,
					FileHash = Wrapper.ExtensionMethod.GetFileHash(file.Value.FullName),
					PackageHash = Wrapper.ExtensionMethod.GetFileHash(pkg),
					PackageName = pkgFileName,
					PackageSize = pkgInfo.Length,
					Method = method,
					Files = new[] { file.Key }
				});
			}
		}


		static MD5 _md5 = MD5.Create();

		static string GetPackageName(string path)
		{
			return BitConverter.ToString(_md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(path.ToLower()))).Replace("-", "").ToUpper();
		}
	}
}
