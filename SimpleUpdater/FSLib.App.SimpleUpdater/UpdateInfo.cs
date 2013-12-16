using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FSLib.App.SimpleUpdater.Wrapper;
using SimpleUpdater.Attributes;

namespace FSLib.App.SimpleUpdater
{
	/// <summary>
	/// 升级信息的具体包装
	/// </summary>
	[Serializable]
	[DoNotObfuscate, DoNotObfuscateControlFlow, DoNotObfuscateType, DoNotPrune, DoNotPruneType]
	[DoNotCaptureFields, DoNotCaptureVariables, DoNotEncodeStrings]	//防止SmartAssembly处理
	public class UpdateInfo
	{
		/// <summary>
		/// 应用程序名
		/// </summary>
		public string AppName { get; set; }

		/// <summary>
		/// 应用程序版本
		/// </summary>
		public string AppVersion { get; set; }

		/// <summary>
		/// 发布页面地址
		/// </summary>
		public string PublishUrl { get; set; }

		/// <summary>
		/// 更新前执行的程序
		/// </summary>
		public string FileExecuteBefore { get; set; }

		/// <summary>
		/// 更新前执行的程序参数
		/// </summary>
		public string ExecuteArgumentBefore { get; set; }

		/// <summary>
		/// 更新后执行的程序
		/// </summary>
		public string FileExecuteAfter { get; set; }

		/// <summary>
		/// 更新后执行的程序参数
		/// </summary>
		public string ExecuteArgumentAfter { get; set; }

		/// <summary>
		/// 程序执行超时
		/// </summary>
		public int ExecuteTimeout { get; set; }

		private string _desc;
		private List<PackageInfo> _packages;

		/// <summary>
		/// 更新描述
		/// </summary>
		public string Desc
		{
			get
			{
				return _desc;
			}
			set
			{
				_desc = string.Join(Environment.NewLine, value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
			}
		}

		/// <summary>
		/// 安装包文件名
		/// </summary>
		public string Package { get; set; }

		/// <summary>
		/// 校验的HASH
		/// </summary>
		public string MD5 { get; set; }

		/// <summary>
		/// 要删除或要保留的文件
		/// </summary>
		public string[] DeleteFileLimits { get; set; }

		/// <summary>
		/// 获得删除规则的正则表达式形式
		/// </summary>
		/// <returns></returns>
		internal List<Regex> GetDeleteFileLimitRuleSet()
		{
			if (this.DeleteFileLimits == null) return new List<Regex>();
			return ExtensionMethod.ToList(Wrapper.ExtensionMethod.Select(DeleteFileLimits, s => new Regex(s, RegexOptions.IgnoreCase)));
		}

		/// <summary>
		/// 删除方式
		/// </summary>
		public DeletePreviousProgramMethod DeleteMethod { get; set; }

		#region new property in 1.3.0.0

		/// <summary>
		/// 升级需要的最低版本
		/// </summary>
		public string RequiredMinVersion { get; set; }

		/// <summary>
		/// 包大小
		/// </summary>
		public long PackageSize { get; set; }

		#endregion

		#region new property in 1.5.0.0

		/// <summary>
		/// 升级程序版本
		/// </summary>
		public Version UpdaterVersion { get; set; }

		/// <summary>
		/// 升级包密码
		/// </summary>
		public string PackagePassword { get; set; }

		#endregion

		#region new property in 2.0.0.0

		/// <summary>获得当前更新文件的版本</summary>
		/// <value></value>
		/// <remarks></remarks>
		public Version UpdateManifestVersion { get; set; }

		/// <summary> 是否不提示用户便强制升级 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public bool ForceUpdate { get; set; }

		/// <summary> 是否不提示用户便强制结束进程 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public bool ForceKillProcesses { get; set; }

		/// <summary> 获得或设置更新包集合 </summary>
		/// <value></value>
		/// <remarks></remarks>
		public List<PackageInfo> Packages
		{
			get { return _packages ?? (_packages = new List<PackageInfo>()); }
			set { _packages = value; }
		}

		/// <summary>
		/// 隐藏更新前执行进程的窗口
		/// </summary>
		public bool HideBeforeExecuteWindow { get; set; }

		/// <summary>
		/// 隐藏更新后执行的进程窗口
		/// </summary>
		public bool HideAfterExecuteWindow { get; set; }

		#endregion

		#region new property in 2.2.0.0

		/// <summary>
		/// 获得或设置升级说明的网页路径
		/// </summary>
		public string WebUpdateNote { get; set; }

		/// <summary>
		/// 获得或设置RTF格式的升级说明
		/// </summary>
		public string RtfUpdateNote { get; set; }
		#endregion

		#region new property in 2.3.0.0

		/// <summary>
		/// 获得或设置是否必须进行升级,否则拒绝运行
		/// </summary>
		public bool MustUpdate { get; set; }


		/// <summary>
		/// 获得可用于更新的服务器集群列表
		/// </summary>
		public string[] ServerCluster { get; set; }

		#endregion
	}

	/// <summary> 更新模式 </summary>
	/// <remarks></remarks>
	public enum UpdateMethod
	{
		/// <summary> 总是更新 </summary>
		/// <remarks></remarks>
		Always = 0,
		/// <summary> 版本控制 </summary>
		/// <remarks></remarks>
		VersionCompare = 1,
		/// <summary> 如果不存在则更新 </summary>
		/// <remarks></remarks>
		SkipIfExists = 2
	}
}
