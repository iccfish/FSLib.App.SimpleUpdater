namespace FSLib.App.SimpleUpdater.UpdateControl
{
	partial class UpdateError
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateError));
			this.SuspendLayout();
			// 
			// UpdateError
			// 
			resources.ApplyResources(this, "$this");
			this.Image = global::FSLib.App.SimpleUpdater.Properties.Resources.cou_16_warning;
			this.Name = "UpdateError";
			this.ShowCloseButton = true;
			this.ShowProgress = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

	}
}
