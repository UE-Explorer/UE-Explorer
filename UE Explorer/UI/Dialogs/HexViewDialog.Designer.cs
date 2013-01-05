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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexViewDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel_Position = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportBinaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBinaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewASCIIItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewByteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HexPanel = new UEExplorer.UI.Dialogs.UserControl_HexView();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Controls.Add(this.HexPanel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SizeLabel,
            this.ToolStripStatusLabel_Position});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // SizeLabel
            // 
            this.SizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.SizeLabel.Name = "SizeLabel";
            resources.ApplyResources(this.SizeLabel, "SizeLabel");
            // 
            // ToolStripStatusLabel_Position
            // 
            this.ToolStripStatusLabel_Position.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripStatusLabel_Position.Name = "ToolStripStatusLabel_Position";
            resources.ApplyResources(this.ToolStripStatusLabel_Position, "ToolStripStatusLabel_Position");
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyBytesToolStripMenuItem,
            this.copyViewToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportBinaryFileToolStripMenuItem,
            this.importBinaryFileToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // copyBytesToolStripMenuItem
            // 
            this.copyBytesToolStripMenuItem.Name = "copyBytesToolStripMenuItem";
            resources.ApplyResources(this.copyBytesToolStripMenuItem, "copyBytesToolStripMenuItem");
            this.copyBytesToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // copyViewToolStripMenuItem
            // 
            this.copyViewToolStripMenuItem.Name = "copyViewToolStripMenuItem";
            resources.ApplyResources(this.copyViewToolStripMenuItem, "copyViewToolStripMenuItem");
            this.copyViewToolStripMenuItem.Click += new System.EventHandler(this.CopyAsViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exportBinaryFileToolStripMenuItem
            // 
            this.exportBinaryFileToolStripMenuItem.Name = "exportBinaryFileToolStripMenuItem";
            resources.ApplyResources(this.exportBinaryFileToolStripMenuItem, "exportBinaryFileToolStripMenuItem");
            this.exportBinaryFileToolStripMenuItem.Click += new System.EventHandler(this.ExportBinaryFileToolStripMenuItem_Click);
            // 
            // importBinaryFileToolStripMenuItem
            // 
            this.importBinaryFileToolStripMenuItem.Name = "importBinaryFileToolStripMenuItem";
            resources.ApplyResources(this.importBinaryFileToolStripMenuItem, "importBinaryFileToolStripMenuItem");
            this.importBinaryFileToolStripMenuItem.Click += new System.EventHandler(this.ImportBinaryFileToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewASCIIItem,
            this.ViewByteItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // ViewASCIIItem
            // 
            this.ViewASCIIItem.Checked = true;
            this.ViewASCIIItem.CheckOnClick = true;
            this.ViewASCIIItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewASCIIItem.Name = "ViewASCIIItem";
            resources.ApplyResources(this.ViewASCIIItem, "ViewASCIIItem");
            this.ViewASCIIItem.CheckedChanged += new System.EventHandler(this.ViewASCIIToolStripMenuItem_CheckedChanged);
            // 
            // ViewByteItem
            // 
            this.ViewByteItem.Checked = true;
            this.ViewByteItem.CheckOnClick = true;
            this.ViewByteItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewByteItem.Name = "ViewByteItem";
            resources.ApplyResources(this.ViewByteItem, "ViewByteItem");
            this.ViewByteItem.CheckedChanged += new System.EventHandler(this.ViewByteToolStripMenuItem_CheckedChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem});
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // infoToolStripMenuItem
            // 
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            // 
            // HexPanel
            // 
            resources.ApplyResources(this.HexPanel, "HexPanel");
            this.HexPanel.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.HexPanel.BackColor = System.Drawing.SystemColors.Window;
            this.HexPanel.Buffer = null;
            this.HexPanel.DrawASCII = true;
            this.HexPanel.DrawByte = true;
            this.HexPanel.Name = "HexPanel";
            // 
            // HexViewDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HexViewDialog";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HexViewDialog_FormClosing);
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
		private UserControl_HexView HexPanel;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel SizeLabel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ViewASCIIItem;
		private System.Windows.Forms.ToolStripMenuItem ViewByteItem;
		internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_Position;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyBytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportBinaryFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importBinaryFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

	}
}