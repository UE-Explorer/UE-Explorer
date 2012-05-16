namespace UEExplorer.UI.Dialogs
{
	partial class SendDialog
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
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.LinkLabel linkLabel1;
			this.InfoText = new System.Windows.Forms.TextBox();
			this.OKButton = new System.Windows.Forms.Button();
			this.MyCancelButton = new System.Windows.Forms.Button();
			this.Email = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point( 10, 45 );
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size( 80, 13 );
			label1.TabIndex = 4;
			label1.Text = "Additional Info?";
			// 
			// label2
			// 
			label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point( 12, 331 );
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size( 32, 13 );
			label2.TabIndex = 6;
			label2.Text = "Email";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point( 10, 9 );
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size( 303, 13 );
			label4.TabIndex = 8;
			label4.Text = "This will send a report to eliotvu.com with the exception details!";
			// 
			// linkLabel1
			// 
			linkLabel1.AutoSize = true;
			linkLabel1.Location = new System.Drawing.Point( 12, 22 );
			linkLabel1.Name = "linkLabel1";
			linkLabel1.Size = new System.Drawing.Size( 128, 13 );
			linkLabel1.TabIndex = 10;
			linkLabel1.TabStop = true;
			linkLabel1.Text = "http://eliotvu.com/forum/";
			// 
			// InfoText
			// 
			this.InfoText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.InfoText.Location = new System.Drawing.Point( 13, 61 );
			this.InfoText.Multiline = true;
			this.InfoText.Name = "InfoText";
			this.InfoText.Size = new System.Drawing.Size( 348, 264 );
			this.InfoText.TabIndex = 0;
			// 
			// OKButton
			// 
			this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point( 13, 357 );
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size( 75, 23 );
			this.OKButton.TabIndex = 1;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			// 
			// MyCancelButton
			// 
			this.MyCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.MyCancelButton.Location = new System.Drawing.Point( 285, 357 );
			this.MyCancelButton.Name = "MyCancelButton";
			this.MyCancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.MyCancelButton.TabIndex = 2;
			this.MyCancelButton.Text = "Cancel";
			this.MyCancelButton.UseVisualStyleBackColor = true;
			// 
			// Email
			// 
			this.Email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Email.Location = new System.Drawing.Point( 49, 331 );
			this.Email.Name = "Email";
			this.Email.Size = new System.Drawing.Size( 312, 20 );
			this.Email.TabIndex = 3;
			this.Email.Text = "Anonymous";
			// 
			// SendDialog
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.MyCancelButton;
			this.ClientSize = new System.Drawing.Size( 373, 392 );
			this.Controls.Add( linkLabel1 );
			this.Controls.Add( label4 );
			this.Controls.Add( label2 );
			this.Controls.Add( label1 );
			this.Controls.Add( this.Email );
			this.Controls.Add( this.MyCancelButton );
			this.Controls.Add( this.OKButton );
			this.Controls.Add( this.InfoText );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SendDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Send Email";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox InfoText;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button MyCancelButton;
		public System.Windows.Forms.TextBox Email;
	}
}