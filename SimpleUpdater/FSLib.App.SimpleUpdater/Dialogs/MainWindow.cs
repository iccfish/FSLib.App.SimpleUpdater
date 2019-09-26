using System;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	public partial class MainWindow : AbstractUpdateBase
	{
		public MainWindow()
		{
			InitializeComponent();

			Load += MainWindow_Load;
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			HideAllControls();
		}

		/// <summary>
		/// 隐藏所有控件
		/// </summary>
		internal void HideAllControls()
		{
			foreach (Control item in panMain.Controls)
			{
				if (item is UpdateControl.ControlBase) item.Visible = false;
			}
		}

		/// <summary>
		/// 应用主题
		/// </summary>
		/// <param name="style"></param>
		protected override void ApplyTheme(DialogStyle style)
		{
			base.ApplyTheme(style);

			panel1.BackColor = style.TitleBackColor;
			panel1.ForeColor = style.TitleForeColor;
			pictureBox1.Image = style.Icon;
		}
	}
}
