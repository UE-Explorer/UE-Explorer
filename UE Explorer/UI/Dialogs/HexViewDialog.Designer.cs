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
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportBinaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importBinaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.userControl_HexView1 = new UEExplorer.UI.Dialogs.UserControl_HexView();
			this.panel1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.statusStrip1);
			this.panel1.Controls.Add(this.menuStrip1);
			this.panel1.Controls.Add(this.userControl_HexView1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(983, 445);
			this.panel1.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ToolStripStatusLabel_Position});
			this.statusStrip1.Location = new System.Drawing.Point(0, 423);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(983, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(27, 17);
			this.toolStripStatusLabel1.Text = "Size";
			// 
			// ToolStripStatusLabel_Position
			// 
			this.ToolStripStatusLabel_Position.BackColor = System.Drawing.Color.Transparent;
			this.ToolStripStatusLabel_Position.Name = "ToolStripStatusLabel_Position";
			this.ToolStripStatusLabel_Position.Size = new System.Drawing.Size(50, 17);
			this.ToolStripStatusLabel_Position.Text = "Position";
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.Color.White;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(983, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewASCIIToolStripMenuItem,
            this.viewByteToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// viewASCIIToolStripMenuItem
			// 
			this.viewASCIIToolStripMenuItem.Checked = true;
			this.viewASCIIToolStripMenuItem.CheckOnClick = true;
			this.viewASCIIToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.viewASCIIToolStripMenuItem.Name = "viewASCIIToolStripMenuItem";
			this.viewASCIIToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.viewASCIIToolStripMenuItem.Text = "View ASCII";
			this.viewASCIIToolStripMenuItem.CheckedChanged += new System.EventHandler(this.viewASCIIToolStripMenuItem_CheckedChanged);
			// 
			// viewByteToolStripMenuItem
			// 
			this.viewByteToolStripMenuItem.Checked = true;
			this.viewByteToolStripMenuItem.CheckOnClick = true;
			this.viewByteToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.viewByteToolStripMenuItem.Name = "viewByteToolStripMenuItem";
			this.viewByteToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.viewByteToolStripMenuItem.Text = "View Byte";
			this.viewByteToolStripMenuItem.CheckedChanged += new System.EventHandler(this.viewByteToolStripMenuItem_CheckedChanged);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyBytesToolStripMenuItem,
            this.copyViewToolStripMenuItem,
            this.exportBinaryFileToolStripMenuItem,
            this.importBinaryFileToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// copyBytesToolStripMenuItem
			// 
			this.copyBytesToolStripMenuItem.Name = "copyBytesToolStripMenuItem";
			this.copyBytesToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
			this.copyBytesToolStripMenuItem.Text = "Copy Bytes";
			this.copyBytesToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// copyViewToolStripMenuItem
			// 
			this.copyViewToolStripMenuItem.Name = "copyViewToolStripMenuItem";
			this.copyViewToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
			this.copyViewToolStripMenuItem.Text = "Copy View";
			this.copyViewToolStripMenuItem.Click += new System.EventHandler(this.copyAsViewToolStripMenuItem_Click);
			// 
			// exportBinaryFileToolStripMenuItem
			// 
			this.exportBinaryFileToolStripMenuItem.Name = "exportBinaryFileToolStripMenuItem";
			this.exportBinaryFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
			this.exportBinaryFileToolStripMenuItem.Text = "Export Binary File";
			this.exportBinaryFileToolStripMenuItem.Click += new System.EventHandler(this.exportBinaryFileToolStripMenuItem_Click);
			// 
			// importBinaryFileToolStripMenuItem
			// 
			this.importBinaryFileToolStripMenuItem.Name = "importBinaryFileToolStripMenuItem";
			this.importBinaryFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
			this.importBinaryFileToolStripMenuItem.Text = "Import Binary File";
			this.importBinaryFileToolStripMenuItem.Click += new System.EventHandler(this.importBinaryFileToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// infoToolStripMenuItem
			// 
			this.infoToolStripMenuItem.Enabled = false;
			this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
			this.infoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.infoToolStripMenuItem.Text = "Info";
			this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
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
			this.userControl_HexView1.Font = new System.Drawing.Font("Arial", 9.25F);
			this.userControl_HexView1.Location = new System.Drawing.Point(8, 30);
			this.userControl_HexView1.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.userControl_HexView1.Name = "userControl_HexView1";
			this.userControl_HexView1.Size = new System.Drawing.Size(967, 387);
			this.userControl_HexView1.TabIndex = 0;
			// 
			// HexViewDialog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(983, 445);
			this.Controls.Add(this.panel1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new System.Drawing.Size(380, 0);
			this.Name = "HexViewDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Hex Viewer";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);

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
		internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_Position;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyBytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportBinaryFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importBinaryFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;

	}
}