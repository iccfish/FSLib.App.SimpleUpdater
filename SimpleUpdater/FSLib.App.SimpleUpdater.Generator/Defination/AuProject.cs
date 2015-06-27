namespace FSLib.App.SimpleUpdater.Generator.Defination
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Xml.Serialization;

	using Annotations;

	using SimpleUpdater.Defination;

	/// <summary>
	/// 自动升级项目
	/// </summary>
	public class AuProject : INotifyPropertyChanged
	{
		bool _createCompatiblePackage;
		bool _compressPackage;
		bool _enableIncreaseUpdate;

		/// <summary>
		/// 创建 <see cref="AuProject" />  的新实例(AuProject)
		/// </summary>
		public AuProject()
		{
			EnableIncreaseUpdate = true;
			CreateCompatiblePackage = true;
			CompressPackage = true;
			DefaultFileVerificationLevel = FileVerificationLevel.Hash | FileVerificationLevel.Size;
			DefaultUpdateMethod = UpdateMethod.VersionCompare;
			UseRandomPackageNaming = true;
		}

		#region 事件区域

		/// <summary>
		/// 请求合成信息
		/// </summary>
		public event EventHandler RequireGatherInfo;

		/// <summary>
		/// 引发 <see cref="RequireGatherInfo" /> 事件
		/// </summary>
		protected virtual void OnRequireGatherInfo()
		{
			var handler = RequireGatherInfo;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion


		/// <summary>
		/// 获得更新信息
		/// </summary>
		public UpdateInfo UpdateInfo
		{
			get { return _updateInfo ?? (UpdateInfo = new UpdateInfo()); }
			set
			{
				if (Equals(value, _updateInfo))
					return;
				_updateInfo = value;
				OnPropertyChanged("UpdateInfo");
			}
		}

		/// <summary>
		/// 获得或设置默认的文件校验等级
		/// </summary>
		public FileVerificationLevel DefaultFileVerificationLevel
		{
			get { return _defaultFileVerificationLevel; }
			set
			{
				if (value == _defaultFileVerificationLevel)
					return;
				_defaultFileVerificationLevel = value;
				OnPropertyChanged("DefaultFileVerificationLevel");
			}
		}

		/// <summary>
		/// 获得或设置默认的更新模式
		/// </summary>
		public FSLib.App.SimpleUpdater.Defination.UpdateMethod DefaultUpdateMethod
		{
			get { return _defaultUpdateMethod; }
			set
			{
				if (value == _defaultUpdateMethod)
					return;
				_defaultUpdateMethod = value;
				OnPropertyChanged("DefaultUpdateMethod");
			}
		}

		/// <summary>
		/// 获得活或设置文件列表
		/// </summary>
		public List<ProjectItem> Files
		{
			get { return _files ?? (_files = new List<ProjectItem>()); }
			set { _files = value; }
		}

		/// <summary>
		/// 获得项目文件路径
		/// </summary>
		[XmlIgnore]
		public string ProjectFilePath
		{
			get { return _pojectFilePath; }
			private set
			{
				_pojectFilePath = value;
				ProjectFolder = string.IsNullOrEmpty(value) ? "" : Path.GetDirectoryName(value);
			}
		}

		/// <summary>
		/// 获得项目文件目录
		/// </summary>
		[XmlIgnore]
		public string ProjectFolder { get; private set; }

		/// <summary>
		/// 将路径转换为完成的路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string ParseFullPath(string path)
		{
			if (string.IsNullOrEmpty(path) || Path.IsPathRooted(path) || string.IsNullOrEmpty(ProjectFolder))
				return path;

			return Path.GetFullPath(Path.Combine(ProjectFolder, path));
		}

		/// <summary>
		/// 将路径转换为相对于项目的路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string ToRelativePath(string path)
		{
			if (string.IsNullOrEmpty(path) || !Path.IsPathRooted(path))
				return path;

			var ps1 = ProjectFolder.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			var ps2 = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			var upperBound = Math.Min(ps1.Length, ps2.Length);
			var startIndex = upperBound;
			for (int i = 0; i < upperBound; i++)
			{
				if (string.Compare(ps1[i], ps2[i], true) != 0)
				{
					startIndex = i;
					break;
				}
			}
			if (startIndex == 0)
				return path;

			if (ps1.Length == startIndex && ps2.Length <= startIndex)
				return ".\\";

			return string.Join(Path.DirectorySeparatorChar.ToString(), Enumerable.Repeat("..", ps1.Length - startIndex).Concat(ps2.Skip(startIndex)).ToArray());
		}

		/// <summary>
		/// 将所有需要转换为相对地址的路径均进行转换
		/// </summary>
		void AllToRelativePath()
		{
			ApplicationDirectory = ToRelativePath(ApplicationDirectory);
			DestinationDirectory = ToRelativePath(DestinationDirectory);
			UpdateContentSrc = ToRelativePath(UpdateContentSrc);
			UpdateRtfNotePath = ToRelativePath(UpdateRtfNotePath);
			VersionUpdateSrc = ToRelativePath(VersionUpdateSrc);
		}

		/// <summary>
		/// 应用程序路径
		/// </summary>
		public string ApplicationDirectory
		{
			get { return _applicationDirectory; }
			set
			{
				if (value == _applicationDirectory)
					return;
				_applicationDirectory = value;
				OnPropertyChanged("ApplicationDirectory");
			}
		}

		/// <summary>
		/// 升级包路径
		/// </summary>
		public string DestinationDirectory
		{
			get { return _destinationDirectory; }
			set
			{
				if (value == _destinationDirectory)
					return;
				_destinationDirectory = value;
				OnPropertyChanged("DestinationDirectory");
			}
		}

		/// <summary>
		/// 发布路径
		/// </summary>
		public string PublishUri
		{
			get { return _publishUri; }
			set
			{
				if (value == _publishUri)
					return;
				_publishUri = value;
				OnPropertyChanged("PublishUri");
			}
		}

		/// <summary>
		/// 发布用户名
		/// </summary>
		public string PublishUserName
		{
			get { return _publishUserName; }
			set
			{
				if (value == _publishUserName)
					return;
				_publishUserName = value;
				OnPropertyChanged("PublishUserName");
			}
		}

		/// <summary>
		/// 发布密码
		/// </summary>
		public string PublishPassword
		{
			get { return _publishPassword; }
			set
			{
				if (value == _publishPassword)
					return;
				_publishPassword = value;
				OnPropertyChanged("PublishPassword");
			}
		}



		/// <summary>
		/// 更新说明的RTF格式文件路径
		/// </summary>
		public string UpdateRtfNotePath
		{
			get { return _updateRtfNotePath; }
			set
			{
				if (value == _updateRtfNotePath)
					return;
				_updateRtfNotePath = value;
				OnPropertyChanged("UpdateRtfNotePath");
			}
		}

		/// <summary>
		/// 获得或设置是否创建兼容的程序包
		/// </summary>
		public bool CreateCompatiblePackage
		{
			get { return _createCompatiblePackage; }
			set
			{
				if (value.Equals(_createCompatiblePackage))
					return;
				_createCompatiblePackage = value;
				OnPropertyChanged("CreateCompatiblePackage");
			}
		}

		/// <summary>
		/// 获得或设置是否创建压缩版程序包
		/// </summary>
		public bool CompressPackage
		{
			get { return _compressPackage; }
			set
			{
				if (value.Equals(_compressPackage))
					return;
				_compressPackage = value;
				OnPropertyChanged("CompressPackage");
			}
		}

		/// <summary>
		/// 获得或设置是否启用增量更新
		/// </summary>
		public bool EnableIncreaseUpdate
		{
			get { return _enableIncreaseUpdate; }
			set
			{
				if (value.Equals(_enableIncreaseUpdate))
					return;
				_enableIncreaseUpdate = value;
				OnPropertyChanged("EnableIncreaseUpdate");
			}
		}

		/// <summary>
		/// 当前版本的更新源文件
		/// </summary>
		public string VersionUpdateSrc
		{
			get { return _versionUpdateSrc; }
			set
			{
				if (value == _versionUpdateSrc)
					return;
				_versionUpdateSrc = value;
				OnPropertyChanged("VersionUpdateSrc");
			}
		}

		/// <summary>
		/// 获得或设置更新的内容源地址
		/// </summary>
		public string UpdateContentSrc
		{
			get { return _updateContentSrc; }
			set
			{
				if (value == _updateContentSrc)
					return;
				_updateContentSrc = value;
				OnPropertyChanged("UpdateContentSrc");
			}
		}

		/// <summary>
		/// 获得或设置升级文件的扩展名
		/// </summary>
		public string PackageExtension
		{
			get
			{
				if (string.IsNullOrEmpty(_packageExtension))
					return "zip";
				return _packageExtension;
			}
			set
			{
				if (value == _packageExtension)
					return;
				_packageExtension = value;
				OnPropertyChanged("PackageExtension");
			}
		}

		/// <summary>
		/// 从指定的文件中加载项目
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static AuProject LoadFile(string path)
		{
			using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
			{
				var xso = new System.Xml.Serialization.XmlSerializer(typeof(AuProject));
				var obj = xso.Deserialize(fs) as AuProject;
				obj.ProjectFilePath = path;
				if (obj._updateInfo == null)
					obj.TryLoadUpdateInfo();
				TranformOldProjectInfo(obj);

				return obj;
			}
		}

		public static AuProject LoadFromOldProject(string xml)
		{
			var project = new AuProject();
			project.DestinationDirectory = Path.GetDirectoryName(xml);
			project.TryLoadUpdateInfo();

			if (project._updateInfo == null)
				return null;    //加载失败
			TranformOldProjectInfo(project);

			//返回
			return project;
		}

		static void TranformOldProjectInfo(AuProject project)
		{
			//兼容性处理
			var ui = project._updateInfo;
			if (ui.Packages != null && ui.Packages.Count > 0)
			{
				var files = ui.Packages == null || ui.Packages.Count == 0 ? null : ui.Packages.SelectMany(s =>
					s.Files.Select(y => new ProjectItem() { Path = y, FileVerificationLevel = s.VerificationLevel, UpdateMethod = s.Method })
					).ToList();
				project._files = files;
				ui.Packages = null;
			}
		}

		/// <summary>
		/// 保存至指定的文件中
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path = null)
		{
			OnRequireGatherInfo();

			if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(ProjectFilePath))
				return;

			ProjectFilePath = path;
			AllToRelativePath();

			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			{
				var xso = new System.Xml.Serialization.XmlSerializer(typeof(AuProject));
				xso.Serialize(fs, this);
			}
		}

		string _lastLoadPath;
		UpdateInfo _updateInfo;
		string _pojectFilePath;
		string _updateRtfNotePath;
		string _publishPassword;
		string _publishUserName;
		string _publishUri;
		string _versionUpdateSrc;
		string _destinationDirectory;
		string _applicationDirectory;
		List<ProjectItem> _files;
		string _pkgPwd;
		string _updateContentSrc;
		FileVerificationLevel _defaultFileVerificationLevel;
		UpdateMethod _defaultUpdateMethod;
		string _packageExtension;

		/// <summary>
		/// 尝试加载升级包信息
		/// </summary>
		protected void TryLoadUpdateInfo()
		{
			//尝试加载更新信息
			var fullpath = ParseFullPath(DestinationDirectory);
			if (fullpath == _lastLoadPath || string.IsNullOrEmpty(fullpath) || !System.IO.Directory.Exists(fullpath)) return;
			_lastLoadPath = fullpath;

			var fileNames = new[] { "update_c.xml", "update.xml" };
			foreach (var fn in fileNames)
			{
				var updateFile = System.IO.Path.Combine(fullpath, fn);
				if (System.IO.File.Exists(updateFile))
				{
					if (OpenXML(updateFile))
						break;
				}
			}
		}

		/// <summary>
		/// 打开配置文件
		/// </summary>
		/// <param name="path"></param>
		private bool OpenXML(string path)
		{
			UpdateInfo ui;
			if (!ExtensionMethods.IsCompressedXmlFile(path))
			{
				ui = typeof(UpdateInfo).XmlDeserializeFile(path) as UpdateInfo;
			}
			else
			{
				ui = ExtensionMethods.DecompressFile(path).XmlDeserializeFromStream<UpdateInfo>();
			}

			if (ui == null)
				return false;
			UpdateInfo = ui;
			return true;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		bool _useRandomPackageNaming;

		/// <summary>
		/// 升级包随机命名
		/// </summary>
		public bool UseRandomPackageNaming
		{
			get { return _useRandomPackageNaming; }
			set
			{
				if (value == _useRandomPackageNaming) return;
				_useRandomPackageNaming = value;
				OnPropertyChanged("UseRandomPackageNaming");
			}
		}

		bool _cleanBeforeBuild;

		/// <summary>
		/// 构建之前先清理
		/// </summary>
		public bool CleanBeforeBuild
		{
			get { return _cleanBeforeBuild; }
			set
			{
				if (value == _cleanBeforeBuild) return;
				_cleanBeforeBuild = value;
				OnPropertyChanged("CleanBeforeBuild");
			}
		}

		public ProjectItem FindProjectItem(string path)
		{
			return Files.FirstOrDefault(s => string.Compare(s.Path, path, StringComparison.OrdinalIgnoreCase) == 0);
		}

		string _componentFlags;

		/// <summary>
		/// 获得或设置组件标记
		/// </summary>
		public string ComponentFlags
		{
			get { return _componentFlags ?? ""; }
			set
			{
				if (value == _componentFlags) return;
				_componentFlags = value;
				OnPropertyChanged("ComponentFlags");
			}
		}
	}
}
