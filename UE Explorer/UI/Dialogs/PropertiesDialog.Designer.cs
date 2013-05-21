namespace UEExplorer.UI.Dialogs
{
	partial class PropertiesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesDialog));
            this.ObjectPropertiesGrid = new System.Windows.Forms.PropertyGrid();
            this.ObjectLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ObjectPropertiesGrid
            // 
            resources.ApplyResources(this.ObjectPropertiesGrid, "ObjectPropertiesGrid");
            this.ObjectPropertiesGrid.Name = "ObjectPropertiesGrid";
            this.ObjectPropertiesGrid.ToolbarVisible = false;
            // 
            // ObjectLabel
            // 
            resources.ApplyResources(this.ObjectLabel, "ObjectLabel");
            this.ObjectLabel.Name = "ObjectLabel";
            // 
            // PropertiesDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ObjectLabel);
            this.Controls.Add(this.ObjectPropertiesGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Label ObjectLabel;
		public System.Windows.Forms.PropertyGrid ObjectPropertiesGrid;
	}
}