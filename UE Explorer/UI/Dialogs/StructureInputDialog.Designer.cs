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
			this.TextBoxName = new System.Windows.Forms.TextBox();
			this.Define = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point( 13, 13 );
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size( 35, 13 );
			label1.TabIndex = 1;
			label1.Text = "Name";
			// 
			// TextBoxName
			// 
			this.TextBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxName.Location = new System.Drawing.Point( 54, 13 );
			this.TextBoxName.Name = "TextBoxName";
			this.TextBoxName.Size = new System.Drawing.Size( 218, 20 );
			this.TextBoxName.TabIndex = 0;
			// 
			// Define
			// 
			this.Define.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Define.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Define.Location = new System.Drawing.Point( 197, 42 );
			this.Define.Name = "Define";
			this.Define.Size = new System.Drawing.Size( 75, 23 );
			this.Define.TabIndex = 2;
			this.Define.Text = "Define";
			this.Define.UseVisualStyleBackColor = true;
			// 
			// StructureInputDialog
			// 
			this.AcceptButton = this.Define;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 284, 77 );
			this.Controls.Add( this.Define );
			this.Controls.Add( label1 );
			this.Controls.Add( this.TextBoxName );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StructureInputDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Structure Input";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox TextBoxName;
		private System.Windows.Forms.Button Define;
	}
}