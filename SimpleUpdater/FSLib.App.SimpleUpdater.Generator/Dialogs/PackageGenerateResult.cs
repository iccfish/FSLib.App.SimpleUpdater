using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Generator.Dialogs
{
	using SimpleUpdater.Defination;

	public partial class PackageGenerateResult : Form
	{
		public PackageGenerateResult()
		{
			InitializeComponent();
		}

		public UpdateInfo UpdateInfo { get; set; }

		public Dictionary<string, string> PackageResult { get; set; }

		private void PackageGenerateResult_Load(object sender, EventArgs e)
		{
			LoadPackgesIntoList();

			var et = txtMd5.Text;
			var vt = txtVersion.Text;
			list.SelectedIndexChanged += (s, ex) =>
			{
				if (list.SelectedItems.Count == 0)
				{
					txtMd5.Text = et;
					txtVersion.Text = vt;
					return;
				}

				var item = list.SelectedItems[0];
				var pkg = item.Tag as PackageInfo;

				if (pkg == null)
				{
					txtMd5.Text = "选定的包没有相关的MD5信息";
					txtVersion.Text = "选定的包没有相关的版本信息";
				}
				else
				{
					txtMd5.Text = pkg.PackageHash;
					txtVersion.Text = pkg.Version;
				}
			};
		}

		void LoadPackgesIntoList()
		{
			var dic = UpdateInfo.Packages == null ? new Dictionary<string, PackageInfo>() : UpdateInfo.Packages.ToDictionary(s => s.PackageName);
			if (!UpdateInfo.Package.IsNullOrEmpty())
			{
				dic.Add(UpdateInfo.Package, new PackageInfo()
					{
						PackageSize = UpdateInfo.PackageSize,
						PackageHash = UpdateInfo.MD5,
						PackageName = UpdateInfo.Package
					});
			}

			foreach (var pr in PackageResult)
			{
				var pkg = dic.ContainsKey(pr.Key) ? dic[pr.Key] : null;

				var lv = new ListViewItem(pr.Key, 0);
				lv.SubItems.Add(pkg == null ? "-" : Wrapper.ExtensionMethod.ToSizeDescription(pkg.PackageSize));
				lv.SubItems.Add(pr.Value);
				lv.Tag = pkg;

				list.Items.Add(lv);
			}

			list.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}
	}
}
