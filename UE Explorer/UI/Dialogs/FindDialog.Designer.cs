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
			this.Find = new System.Windows.Forms.Button();
			this.FindInput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// Find
			// 
			this.Find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Find.Location = new System.Drawing.Point( 221, 42 );
			this.Find.Name = "Find";
			this.Find.Size = new System.Drawing.Size( 75, 23 );
			this.Find.TabIndex = 0;
			this.Find.Text = "Find";
			this.Find.UseVisualStyleBackColor = true;
			this.Find.Click += new System.EventHandler( this.Find_Click );
			// 
			// FindInput
			// 
			this.FindInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.FindInput.Location = new System.Drawing.Point( 13, 13 );
			this.FindInput.Name = "FindInput";
			this.FindInput.Size = new System.Drawing.Size( 283, 20 );
			this.FindInput.TabIndex = 1;
			// 
			// FindDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 308, 77 );
			this.Controls.Add( this.FindInput );
			this.Controls.Add( this.Find );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FindDialog";
			this.Text = "Find";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button Find;
		private System.Windows.Forms.TextBox FindInput;
	}
}