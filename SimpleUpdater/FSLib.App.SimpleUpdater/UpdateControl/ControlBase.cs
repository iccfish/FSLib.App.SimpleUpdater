using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using FSLib.App.SimpleUpdater.Dialogs;

namespace FSLib.App.SimpleUpdater.UpdateControl
{
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
				if (max > 0) progressBar1.Maximum = max;
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

		public string StepTitle { get { return lblDesc.Text; } set { lblDesc.Text = value; } }

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
			this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar1.Location = new System.Drawing.Point(0, 85);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(238, 17);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 7;
			// 
			// lblDesc
			// 
			this.lblDesc.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.lblDesc.Location = new System.Drawing.Point(37, 19);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(186, 32);
			this.lblDesc.TabIndex = 6;
			// 
			// lblProgDesc
			// 
			this.lblProgDesc.Location = new System.Drawing.Point(10, 51);
			this.lblProgDesc.Name = "lblProgDesc";
			this.lblProgDesc.Size = new System.Drawing.Size(213, 31);
			this.lblProgDesc.TabIndex = 8;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.Info;
			this.pictureBox1.Location = new System.Drawing.Point(12, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(20, 20);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(73, 79);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "确定(&C)";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Visible = false;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// ControlBase
			// 
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblProgDesc);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.pictureBox1);
			this.Name = "ControlBase";
			this.Size = new System.Drawing.Size(238, 102);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!Visible) return;

			FindForm().Close();
		}
	}
}
