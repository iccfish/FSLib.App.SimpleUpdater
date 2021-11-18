using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.ZipBuilder
{
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Security.Cryptography;
	using System.Threading;
	using System.Windows.Forms;

	using Defination;

	using SimpleUpdater.Defination;

	using Utils;

	class ZipTask
	{

		/// <summary>
		/// 获得项目
		/// </summary>
		public AuProject Project { get; }

		/// <summary>
		/// 包描述
		/// </summary>
		public string PackageDescription { get; }

		/// <summary>
		/// 包名
		/// </summary>
		public string PackageName { get; }

		/// <summary>
		/// 目标文件
		/// </summary>
		public Dictionary<string, FileInfo> Files { get; }

		/// <summary>
		/// 文件哈希，仅一个文件有效
		/// </summary>
		public string FileHash { get; private set; }

		/// <summary>
		/// 文件长度，仅1个文件有效
		/// </summary>
		public long FileLength { get; private set; }

		/// <summary>
		/// 验证方式
		/// </summary>
		public UpdateMethod UpdateMethod { get; }

		/// <summary>
		/// 获得验证级别
		/// </summary>
		public FileVerificationLevel VerificationLevel { get; }

		/// <summary>
		/// 包Hash
		/// </summary>
		public string PackageHash { get; private set; }

		/// <summary>
		/// 包长度
		/// </summary>
		public long PackageLength { get; private set; }

		private ZipTaskState _state;

		/// <summary>
		/// 当前状态
		/// </summary>
		public ZipTaskState State
		{
			get => _state;
			set
			{
				if (value == _state)
					return;

				_state = value;
				OnProgressChanged();
			}
		}

		/// <summary>
		/// 获得或设置操作完的回调
		/// </summary>
		public Action OnDone { get; set; }

		public ZipTask(string packageName, Dictionary<string, FileInfo> files, UpdateMethod updateMethod, FileVerificationLevel verificationLevel, AuProject project, string packageDescription)
		{
			PackageName = packageName;
			Files = files;
			UpdateMethod = updateMethod;
			VerificationLevel = verificationLevel;
			Project = project;
			PackageDescription = packageDescription;
		}

		private long _totalLength, _processedLength;

		public int Percentage
		{
			get
			{
				if (State == ZipTaskState.Done || State == ZipTaskState.Queue || _totalLength <= 0)
					return -1;

				var stepCount = _skipFileHashing ? 2 : 3;
				return (int)Math.Round(_processedLength * 1.0d / _totalLength * 100.0 / stepCount + 100.0 / stepCount * ((int)State - 1 - (_skipFileHashing ? 1 : 0)), 2);
			}
		}

		private bool _skipFileHashing = false;

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler ProgressChanged;

		/// <summary>
		/// 引发 <see cref="ProgressChanged"/> 事件
		/// </summary>

		protected virtual void OnProgressChanged()
		{
			ProgressChanged?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// 构建
		/// </summary>
		public void Build()
		{
			if (Files.Count == 1 && (VerificationLevel & FileVerificationLevel.Hash) == FileVerificationLevel.Hash)
			{
				State = ZipTaskState.FileHashing;

				var file = Files.First().Value;
				var hash = HashFile(file.FullName);
				FileLength = file.Length;
				FileHash = hash.Key;
			}
			else
			{
				_skipFileHashing = true;
			}

			State = ZipTaskState.PackageBuilding;
			var zipFile = Path.Combine(Project.ParseFullPath(Project.DestinationDirectory), PackageName);
			BuildPackage(zipFile);

			State = ZipTaskState.PackageHashing;
			var data = HashFile(zipFile);
			PackageLength = data.Value;
			PackageHash = data.Key;

			OnDone?.Invoke();
			State = ZipTaskState.Done;
		}

		void BuildPackage(string zipFile)
		{
			using var fs = new FileStream(zipFile, FileMode.Create, FileAccess.Write, FileShare.None);
			using var zip = new ICCEmbedded.SharpZipLib.Zip.ZipOutputStream(fs)
			{
				Password = Project.PublishPassword
			};
			zip.SetLevel(9);

			var entryFactory = new ICCEmbedded.SharpZipLib.Zip.ZipEntryFactory();

			//合并路径
			var fileGroups = Files.GroupBy(s => Path.GetDirectoryName(s.Key)).ToDictionary(s => s.Key, s => s.ToArray());
			var folders = fileGroups.Keys.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToArray();
			var prevProcessed = 0L;

			_totalLength = Files.Sum(s => s.Value.Length);
			_processedLength = 0L;
			OnProgressChanged();

			folders.ForEach(s =>
			{
				if (!string.IsNullOrEmpty(s))
				{
					var fe = entryFactory.MakeDirectoryEntry(s);
					fe.IsUnicodeText = true;
					fe.DateTime = DateTime.Now;
					zip.PutNextEntry(fe);
					zip.CloseEntry();
				}

				foreach (var f in fileGroups[s])
				{

					var ent = entryFactory.MakeFileEntry(f.Key);
					ent.IsUnicodeText = true;
					ent.DateTime = f.Value.LastWriteTime;

					//复制文件内容。简单起见，这里不返回进度显示。。。
					using var sou = f.Value.OpenRead();
					using var mirrorStream = new ProgressStream(sou);

					mirrorStream.PositionChanged += (sender, args) => { _processedLength = prevProcessed + mirrorStream.Position; OnProgressChanged(); };

					ent.Size = sou.Length;
					zip.PutNextEntry(ent);
					ICCEmbedded.SharpZipLib.Core.StreamUtils.Copy(mirrorStream, zip, new byte[0x400]);
					prevProcessed += mirrorStream.Length;

					mirrorStream.Close();
					zip.CloseEntry();
				}
			});
			zip.Flush();
			zip.Close();
			fs.Close();
		}

		KeyValuePair<string, long> HashFile(string file)
		{
			using var hashFile = File.OpenRead(file);
			using var ps = new ProgressStream(hashFile);
			var hash = Project.UsingSha1 ? (HashAlgorithm)SHA1.Create() : MD5.Create();


			_totalLength = hashFile.Length;
			_processedLength = 0L;
			OnProgressChanged();

			ps.PositionChanged += (sender, args) => { _processedLength = ps.Position; OnProgressChanged(); };
			OnProgressChanged();

			return new KeyValuePair<string, long>(BitConverter.ToString(hash.ComputeHash(ps)).Replace("-", "").ToUpper(), _totalLength);

		}
	}
}
