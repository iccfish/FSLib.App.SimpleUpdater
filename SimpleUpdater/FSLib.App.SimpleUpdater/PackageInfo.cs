using System;
using System.Xml.Serialization;
using SimpleUpdater.Attributes;

namespace FSLib.App.SimpleUpdater
{
	/// <summary> 表示单个文件信息 </summary>
	/// <remarks></remarks>
	[Serializable]
	[DoNotObfuscate, DoNotObfuscateControlFlow, DoNotObfuscateType, DoNotPrune, DoNotPruneType]
	[DoNotCaptureFields, DoNotCaptureVariables, DoNotEncodeStrings]	//防止SmartAssembly处理
	public class PackageInfo
	{
		#region 包信息-需要保存的持久化信息

		/// <summary> 文件路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string FilePath { get; set; }

		/// <summary> 文件大小 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public long FileSize { get; set; }

		/// <summary> 版本 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string Version { get; set; }

		/// <summary> Hash </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string PackageHash { get; set; }

		/// <summary> 包名 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string PackageName { get; set; }

		/// <summary> 压缩包文件大小 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public long PackageSize { get; set; }

		/// <summary> 更新模式 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public UpdateMethod Method { get; set; }

		/// <summary> 获得或设置当前文件验证等级 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public FileVerificationLevel VerificationLevel { get; set; }

		/// <summary> 获得或设置本地文件的哈希值 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public string FileHash { get; set; }

		/// <summary>
		/// 获得或设置关联的文件
		/// </summary>
		public string[] Files { get; set; }

		#endregion


		#region 包本身的公开方法

		/// <summary> 解压包 </summary>
		public void Extract()
		{
		}

		/// <summary> 增加失败计数 </summary>
		public void IncreaseFailureCounter()
		{
			RetryCount = (RetryCount ?? 0) + 1;
		}

		#endregion


		#region 扩展属性-为了运行时而引入，非固化在升级包中的属性

		/// <summary> 获得或设置处理用的上下文环境 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[System.Xml.Serialization.XmlIgnore]
		public UpdateContext Context { get; set; }


		/// <summary> 获得当前包是否正在下载 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public bool IsDownloading { get; internal set; }

		/// <summary> 获得当前包是否已经下载 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public bool IsDownloaded { get; internal set; }

		/// <summary> 获得处理过程中最后发生的错误 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public Exception LastError { get; internal set; }

		/// <summary> 获得重试次数计数 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public int? RetryCount { get; internal set; }

		/// <summary> 获得本地保存路径 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[System.Xml.Serialization.XmlIgnore]
		public string LocalSavePath
		{
			get
			{
				if (Context == null) throw new InvalidOperationException("尚未附加到上下文中");
				return System.IO.Path.Combine(Context.UpdatePackagePath, PackageName);
			}
		}

		string _sourceUri;

		/// <summary> 获得下载的源URL </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public string SourceUri
		{
			get
			{
				if (Context == null) throw new InvalidOperationException("尚未附加到上下文中");

				return _sourceUri ?? (_sourceUri = Context.GetUpdatePackageFullUrl(PackageName));
			}
		}

		bool? _hashResult;

		/// <summary> 获得本地的包文件是否有效 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public bool? IsLocalFileValid
		{
			get
			{
				var path = LocalSavePath;
				if (!System.IO.File.Exists(path)) return null;
				return _hashResult ?? (_hashResult = Wrapper.ExtensionMethod.GetFileHash(path) == PackageHash);
			}
		}

		/// <summary> 获得已下载的长度 </summary>
		/// <value></value>
		/// <remarks></remarks>
		[XmlIgnore]
		public long DownloadedSize { get; internal set; }

		#endregion
	}
}