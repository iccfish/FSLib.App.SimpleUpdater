namespace FSLib.App.SimpleUpdater.Dialogs
{
	partial class ProgramExecuteTimeout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramExecuteTimeout));
			this.label1 = new System.Windows.Forms.Label();
			this.btnKill = new System.Windows.Forms.Button();
			this.btnWait = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// btnKill
			// 
			resources.ApplyResources(this.btnKill, "btnKill");
			this.btnKill.Name = "btnKill";
			this.btnKill.UseVisualStyleBackColor = true;
			this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
			// 
			// btnWait
			// 
			resources.ApplyResources(this.btnWait, "btnWait");
			this.btnWait.Name = "btnWait";
			this.btnWait.UseVisualStyleBackColor = true;
			this.btnWait.Click += new System.EventHandler(this.btnWait_Click);
			// 
			// ProgramExecuteTimeout
			// 
			this.AcceptButton = this.btnWait;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnWait);
			this.Controls.Add(this.btnKill);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgramExecuteTimeout";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnKill;
		private System.Windows.Forms.Button btnWait;
	}
}