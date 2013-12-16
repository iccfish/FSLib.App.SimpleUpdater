namespace FSLib.App.SimpleUpdater.Dialogs
{
	partial class MinmumVersionRequired
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinmumVersionRequired));
			this.label1 = new System.Windows.Forms.Label();
			this.lblDesc = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// lblDesc
			// 
			resources.ApplyResources(this.lblDesc, "lblDesc");
			this.lblDesc.Name = "lblDesc";
			// 
			// btnGo
			// 
			resources.ApplyResources(this.btnGo, "btnGo");
			this.btnGo.Name = "btnGo";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// MinmumVersionRequired
			// 
			this.AcceptButton = this.btnGo;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MinmumVersionRequired";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MinmumVersionRequired_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnClose;
	}
}