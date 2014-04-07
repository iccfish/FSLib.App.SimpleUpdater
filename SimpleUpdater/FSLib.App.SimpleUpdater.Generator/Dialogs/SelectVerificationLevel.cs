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
				if (chkContent.Checked) m |= FileVerificationLevel.Hash;
				if (chkSize.Checked) m |= FileVerificationLevel.Size;
				if (chkVersion.Checked) m |= FileVerificationLevel.Version;

				return m;
			}
			set
			{
				chkContent.Checked = (value & FileVerificationLevel.Hash) != FileVerificationLevel.None;
				chkSize.Checked = (value & FileVerificationLevel.Size) != FileVerificationLevel.None;
				chkVersion.Checked = (value & FileVerificationLevel.Version) != FileVerificationLevel.None;
			}
		}
	}
}
