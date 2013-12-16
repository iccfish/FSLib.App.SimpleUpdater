namespace FSLib.App.SimpleUpdater.Dialogs
{
	partial class CloseApp
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloseApp));
			this.label1 = new System.Windows.Forms.Label();
			this.processList = new System.Windows.Forms.ListBox();
			this.btnAutoClose = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// processList
			// 
			this.processList.FormattingEnabled = true;
			resources.ApplyResources(this.processList, "processList");
			this.processList.Name = "processList";
			// 
			// btnAutoClose
			// 
			resources.ApplyResources(this.btnAutoClose, "btnAutoClose");
			this.btnAutoClose.Name = "btnAutoClose";
			this.btnAutoClose.UseVisualStyleBackColor = true;
			this.btnAutoClose.Click += new System.EventHandler(this.btnAutoClose_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// CloseApp
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAutoClose);
			this.Controls.Add(this.processList);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "CloseApp";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox processList;
		private System.Windows.Forms.Button btnAutoClose;
		private System.Windows.Forms.Button btnCancel;
	}
}