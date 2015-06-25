using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Defination
{
	using System.IO;
	using FSLib.App.SimpleUpdater.Defination;

	/// <summary>
	/// 用于编辑更新方式中的文件信息
	/// </summary>
	class FileTreeItem
	{
		public FileTreeItem(FileInfo fileInfo, UpdateMethod method, FileVerificationLevel level, string relativePath, string flag)
		{
			FileInfo = fileInfo;
			UpdateMethod = method;
			VerificationLevel = level;
			RelativePath = relativePath;
			Flag = flag;
		}

		/// <summary>
		/// 获得相对路径
		/// </summary>
		public string RelativePath { get; private set; }

		public FileInfo FileInfo { get; private set; }

		UpdateMethod _updateMethod;

		public UpdateMethod UpdateMethod
		{
			get { return _updateMethod; }
			set
			{
				if (_updateMethod == value)
					return;

				_updateMethod = value;
				ChangeState(value, _verificationLevel);
				OnUpdateMethodChagned();
			}
		}

		FileVerificationLevel _verificationLevel;

		public FileVerificationLevel VerificationLevel
		{
			get { return _verificationLevel; }
			set
			{
				if (_verificationLevel == value)
					return;

				_verificationLevel = value;
				ChangeState(_updateMethod, value);
				OnVerificationLevelChanged();
			}
		}

		string _flag;

		public string Flag
		{
			get { return _flag; }
			set
			{
				if (string.Compare(value, _flag, StringComparison.OrdinalIgnoreCase) == 0)
					return;

				_flag = value;

				var proj = UpdatePackageBuilder.Instance.AuProject;
				if (proj != null)
				{
					var pi = proj.FindProjectItem(RelativePath);
					if (pi == null)
					{
						pi = new ProjectItem()
						{
							Path = RelativePath,
							FileVerificationLevel = VerificationLevel,
							UpdateMethod = UpdateMethod
						};
						proj.Files.Add(pi);
					}

					pi.Flag = value;
				}

				OnFlagChanged();
			}
		}

		void ChangeState(UpdateMethod method, FileVerificationLevel level)
		{
			var proj = UpdatePackageBuilder.Instance.AuProject;
			if (proj == null)
				return;

			if (method == UpdateMethod.AsProject)
			{
				//跟随项目，则删除
				var pi = proj.FindProjectItem(RelativePath);
				if (pi != null)
					proj.Files.Remove(pi);
			}
			else
			{
				var pi = proj.FindProjectItem(RelativePath);
				if (pi == null)
				{
					pi = new ProjectItem()
					{
						Path = RelativePath
					};
					proj.Files.Add(pi);
				}

				pi.FileVerificationLevel = level;
				pi.UpdateMethod = method;
			}
		}

		public event EventHandler UpdateMethodChagned;
		public event EventHandler VerificationLevelChanged;
		public event EventHandler FlagChanged;

		protected virtual void OnUpdateMethodChagned()
		{
			if (UpdateMethodChagned != null)
				UpdateMethodChagned(this, EventArgs.Empty);
		}

		protected virtual void OnVerificationLevelChanged()
		{
			if (VerificationLevelChanged != null)
				VerificationLevelChanged(this, EventArgs.Empty);
		}

		protected virtual void OnFlagChanged()
		{
			if (FlagChanged != null)
				FlagChanged(this, EventArgs.Empty);
		}
	}
}
