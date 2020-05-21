using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using FSLib.App.SimpleUpdater.Dialogs;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
	using System.ComponentModel;

	public class ControlBase : UserControl
	{
		private ProgressBar progressBar1;
		private Label lblDesc;
		private Label lblProgDesc;
		private Button btnOk;
		private PictureBox pictureBox1;

		public ControlBase()
		{
			InitializeComponent();
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Program.IsRunning)
			{
				this.Location = new Point(12, 30);
				this.BackColor = Color.Transparent;
				this.Visible = false;
				Size = new Size(216, 88);

				foreach (Control ctl in this.Controls)
				{
					if (ctl is Button) (ctl as Button).ForeColor = SystemColors.ControlText;
				}
			}
		}

		public bool ShowProgress
		{
			get { return progressBar1.Visible; }
			set
			{
				progressBar1.Visible = value;
				if (value) SetProgress(0);
			}
		}

		public void SetProgress(int current, int max = 0)
		{
			if (max <= 0)
				progressBar1.Style = ProgressBarStyle.Marquee;
			else
			{
				progressBar1.Style = ProgressBarStyle.Blocks;
				if (max > 0)
				{
					if (progressBar1.Value > max)
						progressBar1.Value = 0;
					progressBar1.Maximum = max;
				}
				progressBar1.Value = current;
			}
		}

		public Image Image
		{
			get { return pictureBox1.Image; }
			set { pictureBox1.Image = value; }
		}

		public void SetIcon(Image img)
		{
			pictureBox1.Image = img ?? Properties.Resources.Info;
		}

		[Localizable(true)]
		public string StepTitle { get { return lblDesc.Text; } set { lblDesc.Text = value; } }

		[Localizable(true)]
		public string StepDesc { get { return lblProgDesc.Text; } set { lblProgDesc.Text = value; } }

		public bool ShowCloseButton { get { return btnOk.Visible; } set { btnOk.Visible = value; } }

		/// <summary>
		/// 请求主窗体隐藏所有控件
		/// </summary>
		protected void HideControls()
		{
			var mw = this.FindForm() as MainWindow;
			mw.HideAllControls();
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlBase));
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblDesc = new System.Windows.Forms.Label();
			this.lblProgDesc = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			resources.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// lblDesc
			// 
			resources.ApplyResources(this.lblDesc, "lblDesc");
			this.lblDesc.Name = "lblDesc";
			// 
			// lblProgDesc
			// 
			resources.ApplyResources(this.lblProgDesc, "lblProgDesc");
			this.lblProgDesc.Name = "lblProgDesc";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.Info;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// ControlBase
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblProgDesc);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.pictureBox1);
			this.Name = "ControlBase";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!Visible) return;

			FindForm().Close();
		}

		protected void AutoClose(int timeout)
		{
			var t = new Timer
			{
				Interval = timeout
			};
			t.Tick += (s, x) =>
			{
				t.Stop();
				FindForm().Close();
			};
			t.Start();
		}
	}
}
