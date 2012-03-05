namespace UEExplorer.UI.Dialogs
{
	partial class HexViewDialog
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.ToolStripStatusLabel_Position = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewASCIIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewByteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewDecimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewIntegerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.userControl_HexView1 = new UEExplorer.UI.Dialogs.UserControl_HexView();
			this.panel1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.statusStrip1 );
			this.panel1.Controls.Add( this.menuStrip1 );
			this.panel1.Controls.Add( this.userControl_HexView1 );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 0, 0 );
			this.panel1.Margin = new System.Windows.Forms.Padding( 4, 5, 4, 5 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 1024, 445 );
			this.panel1.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ToolStripStatusLabel_Position} );
			this.statusStrip1.Location = new System.Drawing.Point( 0, 423 );
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size( 1024, 22 );
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size( 62, 17 );
			this.toolStripStatusLabel1.Text = "Buffer Size";
			// 
			// ToolStripStatusLabel_Position
			// 
			this.ToolStripStatusLabel_Position.BackColor = System.Drawing.Color.Transparent;
			this.ToolStripStatusLabel_Position.Name = "ToolStripStatusLabel_Position";
			this.ToolStripStatusLabel_Position.Size = new System.Drawing.Size( 50, 17 );
			this.ToolStripStatusLabel_Position.Text = "Position";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 1024, 24 );
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.viewASCIIToolStripMenuItem,
            this.viewByteToolStripMenuItem,
            this.viewDecimalToolStripMenuItem,
            this.viewIntegerToolStripMenuItem} );
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size( 44, 20 );
			this.viewToolStripMenuItem.Text = "View";
			// 
			// viewASCIIToolStripMenuItem
			// 
			this.viewASCIIToolStripMenuItem.Checked = true;
			this.viewASCIIToolStripMenuItem.CheckOnClick = true;
			this.viewASCIIToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.viewASCIIToolStripMenuItem.Name = "viewASCIIToolStripMenuItem";
			this.viewASCIIToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.viewASCIIToolStripMenuItem.Text = "View ASCII";
			this.viewASCIIToolStripMenuItem.CheckedChanged += new System.EventHandler( this.viewASCIIToolStripMenuItem_CheckedChanged );
			// 
			// viewByteToolStripMenuItem
			// 
			this.viewByteToolStripMenuItem.Checked = true;
			this.viewByteToolStripMenuItem.CheckOnClick = true;
			this.viewByteToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.viewByteToolStripMenuItem.Name = "viewByteToolStripMenuItem";
			this.viewByteToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.viewByteToolStripMenuItem.Text = "View Byte";
			this.viewByteToolStripMenuItem.CheckedChanged += new System.EventHandler( this.viewByteToolStripMenuItem_CheckedChanged );
			// 
			// viewDecimalToolStripMenuItem
			// 
			this.viewDecimalToolStripMenuItem.CheckOnClick = true;
			this.viewDecimalToolStripMenuItem.Name = "viewDecimalToolStripMenuItem";
			this.viewDecimalToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.viewDecimalToolStripMenuItem.Text = "View Decimal";
			this.viewDecimalToolStripMenuItem.Visible = false;
			// 
			// viewIntegerToolStripMenuItem
			// 
			this.viewIntegerToolStripMenuItem.CheckOnClick = true;
			this.viewIntegerToolStripMenuItem.Name = "viewIntegerToolStripMenuItem";
			this.viewIntegerToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.viewIntegerToolStripMenuItem.Text = "View Integer";
			this.viewIntegerToolStripMenuItem.Visible = false;
			// 
			// userControl_HexView1
			// 
			this.userControl_HexView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.userControl_HexView1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.userControl_HexView1.BackColor = System.Drawing.SystemColors.Window;
			this.userControl_HexView1.Buffer = null;
			this.userControl_HexView1.DrawASCII = true;
			this.userControl_HexView1.DrawByte = true;
			this.userControl_HexView1.DrawDecimal = false;
			this.userControl_HexView1.DrawInteger = false;
			this.userControl_HexView1.Font = new System.Drawing.Font( "Arial", 9.25F );
			this.userControl_HexView1.Location = new System.Drawing.Point( 0, 24 );
			this.userControl_HexView1.Margin = new System.Windows.Forms.Padding( 8, 6, 8, 6 );
			this.userControl_HexView1.Name = "userControl_HexView1";
			this.userControl_HexView1.Size = new System.Drawing.Size( 1024, 399 );
			this.userControl_HexView1.TabIndex = 0;
			// 
			// HexViewDialog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size( 1024, 445 );
			this.Controls.Add( this.panel1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding( 4, 5, 4, 5 );
			this.MinimumSize = new System.Drawing.Size( 380, 0 );
			this.Name = "HexViewDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Hex Viewer";
			this.panel1.ResumeLayout( false );
			this.panel1.PerformLayout();
			this.statusStrip1.ResumeLayout( false );
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private UserControl_HexView userControl_HexView1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewASCIIToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewByteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewDecimalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewIntegerToolStripMenuItem;
		internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_Position;

	}
}