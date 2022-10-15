namespace UEExplorer.UI.Forms
{
	partial class HexViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexViewerForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.FAQControl = new UEExplorer.UI.Forms.HexViewerFAQ();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel_Position = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexWorkshopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportBinaryItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportBinaryItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyBytesItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyHexViewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyAddressItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopySizeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ReloadBufferItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadPackageItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewByteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewASCIIItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HexPanel = new UEExplorer.UI.Forms.HexViewerControl();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.FAQControl);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Controls.Add(this.HexPanel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // FAQControl
            // 
            resources.ApplyResources(this.FAQControl, "FAQControl");
            this.FAQControl.Name = "FAQControl";
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
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openInToolStripMenuItem
            // 
            this.openInToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexWorkshopToolStripMenuItem});
            this.openInToolStripMenuItem.Name = "openInToolStripMenuItem";
            resources.ApplyResources(this.openInToolStripMenuItem, "openInToolStripMenuItem");
            // 
            // hexWorkshopToolStripMenuItem
            // 
            this.hexWorkshopToolStripMenuItem.Name = "hexWorkshopToolStripMenuItem";
            resources.ApplyResources(this.hexWorkshopToolStripMenuItem, "hexWorkshopToolStripMenuItem");
            this.hexWorkshopToolStripMenuItem.Click += new System.EventHandler(this.HEXWorkshopToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveItem,
            this.toolStripSeparator2,
            this.ExportBinaryItem,
            this.ImportBinaryItem,
            this.toolStripSeparator1,
            this.CopyBytesItem,
            this.CopyHexViewItem,
            this.toolStripSeparator4,
            this.CopyAddressItem,
            this.CopySizeItem,
            this.toolStripSeparator3,
            this.ReloadBufferItem,
            this.ReloadPackageItem});
            this.editToolStripMenuItem.ForeColor = System.Drawing.Color.DarkRed;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // SaveItem
            // 
            resources.ApplyResources(this.SaveItem, "SaveItem");
            this.SaveItem.Name = "SaveItem";
            this.SaveItem.Click += new System.EventHandler(this.SaveModificationsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ExportBinaryItem
            // 
            this.ExportBinaryItem.Name = "ExportBinaryItem";
            resources.ApplyResources(this.ExportBinaryItem, "ExportBinaryItem");
            this.ExportBinaryItem.Click += new System.EventHandler(this.ExportBinaryFileToolStripMenuItem_Click);
            // 
            // ImportBinaryItem
            // 
            this.ImportBinaryItem.Name = "ImportBinaryItem";
            resources.ApplyResources(this.ImportBinaryItem, "ImportBinaryItem");
            this.ImportBinaryItem.Click += new System.EventHandler(this.ImportBinaryFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // CopyBytesItem
            // 
            this.CopyBytesItem.Name = "CopyBytesItem";
            resources.ApplyResources(this.CopyBytesItem, "CopyBytesItem");
            this.CopyBytesItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // CopyHexViewItem
            // 
            this.CopyHexViewItem.Name = "CopyHexViewItem";
            resources.ApplyResources(this.CopyHexViewItem, "CopyHexViewItem");
            this.CopyHexViewItem.Click += new System.EventHandler(this.CopyAsViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // CopyAddressItem
            // 
            this.CopyAddressItem.Name = "CopyAddressItem";
            resources.ApplyResources(this.CopyAddressItem, "CopyAddressItem");
            this.CopyAddressItem.Click += new System.EventHandler(this.CopyAddressToolStripMenuItem_Click);
            // 
            // CopySizeItem
            // 
            this.CopySizeItem.Name = "CopySizeItem";
            resources.ApplyResources(this.CopySizeItem, "CopySizeItem");
            this.CopySizeItem.Click += new System.EventHandler(this.CopySizeInHexToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // ReloadBufferItem
            // 
            this.ReloadBufferItem.AutoToolTip = true;
            this.ReloadBufferItem.Name = "ReloadBufferItem";
            resources.ApplyResources(this.ReloadBufferItem, "ReloadBufferItem");
            this.ReloadBufferItem.Click += new System.EventHandler(this.RedeserializeObjectOnlyUseAtOwnRiskToolStripMenuItem_Click);
            // 
            // ReloadPackageItem
            // 
            this.ReloadPackageItem.Name = "ReloadPackageItem";
            resources.ApplyResources(this.ReloadPackageItem, "ReloadPackageItem");
            this.ReloadPackageItem.Click += new System.EventHandler(this.ReloadPackageToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewByteItem,
            this.ViewASCIIItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
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
            // ViewASCIIItem
            // 
            this.ViewASCIIItem.Checked = true;
            this.ViewASCIIItem.CheckOnClick = true;
            this.ViewASCIIItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewASCIIItem.Name = "ViewASCIIItem";
            resources.ApplyResources(this.ViewASCIIItem, "ViewASCIIItem");
            this.ViewASCIIItem.CheckedChanged += new System.EventHandler(this.ViewASCIIToolStripMenuItem_CheckedChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
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
            // HexViewerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HexViewerForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HexViewDialog_FormClosing);
            this.Load += new System.EventHandler(this.HexViewerForm_Load);
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
		private HexViewerControl HexPanel;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel SizeLabel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ViewASCIIItem;
		private System.Windows.Forms.ToolStripMenuItem ViewByteItem;
		internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_Position;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CopyBytesItem;
		private System.Windows.Forms.ToolStripMenuItem CopyHexViewItem;
		private System.Windows.Forms.ToolStripMenuItem ExportBinaryItem;
		private System.Windows.Forms.ToolStripMenuItem ImportBinaryItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem SaveItem;
        private System.Windows.Forms.ToolStripMenuItem ReloadPackageItem;
        private HexViewerFAQ FAQControl;
        private System.Windows.Forms.ToolStripMenuItem CopyAddressItem;
        private System.Windows.Forms.ToolStripMenuItem CopySizeItem;
        private System.Windows.Forms.ToolStripMenuItem ReloadBufferItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexWorkshopToolStripMenuItem;

	}
}