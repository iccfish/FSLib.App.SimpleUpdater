namespace FSLib.App.SimpleUpdater.Generator.Defination
{
	using System.ComponentModel;
	using System.Linq;

	using Annotations;

	/// <summary>
	/// 自动升级项目
	/// </summary>
	public class AuProject : INotifyPropertyChanged
	{
		bool _createCompatiblePackage;
		bool _compressPackage;
		bool _enableIncreaseUpdate;

		/// <summary>
		/// 应用程序路径
		/// </summary>
		public string ApplicationDirectory { get; set; }

		/// <summary>
		/// 升级包路径
		/// </summary>
		public string DestinationDirectory { get; set; }

		/// <summary>
		/// 发布路径
		/// </summary>
		public string PublishUri { get; set; }

		/// <summary>
		/// 发布用户名
		/// </summary>
		public string PublishUserName { get; set; }

		/// <summary>
		/// 发布密码
		/// </summary>
		public string PublishPassword { get; set; }

		/// <summary>
		/// 更新说明的RTF格式文件路径
		/// </summary>
		public string UpdateRtfNotePath { get; set; }

		#region 新属性

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

		#endregion

		public string VersionUpdateSrc { get; set; }

		public string UpdateContentSrc { get; set; }

		public string UpdateRtfSrc { get; set; }


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
				return xso.Deserialize(fs) as AuProject;
			}
		}

		/// <summary>
		/// 保存至指定的文件中
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path)
		{
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			{
				var xso = new System.Xml.Serialization.XmlSerializer(typeof(AuProject));
				xso.Serialize(fs, this);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
