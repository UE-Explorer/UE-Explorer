namespace UEExplorer.UI.Dialogs
{
	partial class StructureInputDialog
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
			System.Windows.Forms.Label label1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StructureInputDialog));
			this.TextBoxName = new System.Windows.Forms.TextBox();
			this.Define = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// TextBoxName
			// 
			resources.ApplyResources(this.TextBoxName, "TextBoxName");
			this.TextBoxName.Name = "TextBoxName";
			// 
			// Define
			// 
			resources.ApplyResources(this.Define, "Define");
			this.Define.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Define.Name = "Define";
			this.Define.UseVisualStyleBackColor = true;
			// 
			// StructureInputDialog
			// 
			this.AcceptButton = this.Define;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Define);
			this.Controls.Add(label1);
			this.Controls.Add(this.TextBoxName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StructureInputDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Shown += new System.EventHandler(this.StructureInputDialog_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox TextBoxName;
		private System.Windows.Forms.Button Define;
	}
}