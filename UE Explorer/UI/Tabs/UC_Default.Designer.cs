namespace UEExplorer.UI.Tabs
{
	partial class UC_Default
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.DefaultPage = new System.Windows.Forms.WebBrowser();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// DefaultPage
			// 
			this.DefaultPage.AllowWebBrowserDrop = false;
			this.DefaultPage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DefaultPage.Location = new System.Drawing.Point( 0, 0 );
			this.DefaultPage.Margin = new System.Windows.Forms.Padding( 0 );
			this.DefaultPage.Name = "DefaultPage";
			this.DefaultPage.ScriptErrorsSuppressed = true;
			this.DefaultPage.ScrollBarsEnabled = false;
			this.DefaultPage.Size = new System.Drawing.Size( 1308, 584 );
			this.DefaultPage.TabIndex = 2;
			this.DefaultPage.Url = new System.Uri( "", System.UriKind.Relative );
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.DefaultPage );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 0, 0 );
			this.panel1.Margin = new System.Windows.Forms.Padding( 0 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 1308, 584 );
			this.panel1.TabIndex = 3;
			// 
			// UC_Default
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add( this.panel1 );
			this.Name = "UC_Default";
			this.Size = new System.Drawing.Size( 1308, 584 );
			this.panel1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.WebBrowser DefaultPage;
		private System.Windows.Forms.Panel panel1;

	}
}
