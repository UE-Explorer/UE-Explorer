namespace UEExplorer.UI.Dialogs
{
	partial class FindDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindDialog));
			this.Find = new System.Windows.Forms.Button();
			this.FindInput = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Find
			// 
			resources.ApplyResources(this.Find, "Find");
			this.Find.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Find.Name = "Find";
			this.Find.UseVisualStyleBackColor = true;
			this.Find.Click += new System.EventHandler(this.Find_Click);
			// 
			// FindInput
			// 
			resources.ApplyResources(this.FindInput, "FindInput");
			this.FindInput.Name = "FindInput";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// FindDialog
			// 
			this.AcceptButton = this.Find;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.FindInput);
			this.Controls.Add(this.Find);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindDialog";
			this.ShowIcon = false;
			this.TopMost = true;
			this.Shown += new System.EventHandler(this.FindDialog_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button Find;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox FindInput;
	}
}