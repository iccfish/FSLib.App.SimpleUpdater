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
	public partial class SelectVerificationLevel : Form
	{
		public SelectVerificationLevel()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		public FileVerificationLevel FileVerificationLevel
		{
			get
			{
				var m = FileVerificationLevel.None;
				if (chkContent.Checked) m |= SimpleUpdater.FileVerificationLevel.Hash;
				if (chkSize.Checked) m |= SimpleUpdater.FileVerificationLevel.Size;
				if (chkVersion.Checked) m |= SimpleUpdater.FileVerificationLevel.Version;

				return m;
			}
			set
			{
				chkContent.Checked = (value & SimpleUpdater.FileVerificationLevel.Hash) != SimpleUpdater.FileVerificationLevel.None;
				chkSize.Checked = (value & SimpleUpdater.FileVerificationLevel.Size) != SimpleUpdater.FileVerificationLevel.None;
				chkVersion.Checked = (value & SimpleUpdater.FileVerificationLevel.Version) != SimpleUpdater.FileVerificationLevel.None;
			}
		}
	}
}
