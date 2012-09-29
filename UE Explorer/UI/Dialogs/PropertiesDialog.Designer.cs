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
			this.ObjectPropertiesGrid = new System.Windows.Forms.PropertyGrid();
			this.ObjectLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ObjectPropertiesGrid
			// 
			this.ObjectPropertiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ObjectPropertiesGrid.HelpVisible = false;
			this.ObjectPropertiesGrid.Location = new System.Drawing.Point(12, 35);
			this.ObjectPropertiesGrid.Name = "ObjectPropertiesGrid";
			this.ObjectPropertiesGrid.Size = new System.Drawing.Size(364, 370);
			this.ObjectPropertiesGrid.TabIndex = 0;
			this.ObjectPropertiesGrid.ToolbarVisible = false;
			// 
			// ObjectLabel
			// 
			this.ObjectLabel.AutoSize = true;
			this.ObjectLabel.Location = new System.Drawing.Point(13, 13);
			this.ObjectLabel.Name = "ObjectLabel";
			this.ObjectLabel.Size = new System.Drawing.Size(79, 13);
			this.ObjectLabel.TabIndex = 1;
			this.ObjectLabel.Text = "OBJECTNAME";
			// 
			// PropertiesDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(388, 417);
			this.Controls.Add(this.ObjectLabel);
			this.Controls.Add(this.ObjectPropertiesGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PropertiesDialog";
			this.ShowIcon = false;
			this.Text = "Object Properties";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Label ObjectLabel;
		public System.Windows.Forms.PropertyGrid ObjectPropertiesGrid;
	}
}