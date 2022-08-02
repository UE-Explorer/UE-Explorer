namespace UEExplorer.UI.ActionPanels
{
    partial class PackageExplorerPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageExplorerPanel));
            this.TreeViewPackages = new Krypton.Toolkit.KryptonTreeView();
            this.VSIcons = new System.Windows.Forms.ImageList(this.components);
            this.objectContextMenu = new Krypton.Toolkit.KryptonContextMenu();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripTextBoxFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeViewPackages
            // 
            this.TreeViewPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewPackages.HideSelection = false;
            this.TreeViewPackages.ImageIndex = 0;
            this.TreeViewPackages.ImageList = this.VSIcons;
            this.TreeViewPackages.KryptonContextMenu = this.objectContextMenu;
            this.TreeViewPackages.Location = new System.Drawing.Point(0, 30);
            this.TreeViewPackages.Name = "TreeViewPackages";
            this.TreeViewPackages.PaletteMode = Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.TreeViewPackages.SelectedImageIndex = 0;
            this.TreeViewPackages.ShowNodeToolTips = true;
            this.TreeViewPackages.Size = new System.Drawing.Size(502, 432);
            this.TreeViewPackages.TabIndex = 21;
            this.TreeViewPackages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewPackages_AfterSelect);
            this.TreeViewPackages.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewPackages_BeforeExpand);
            // 
            // VSIcons
            // 
            this.VSIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("VSIcons.ImageStream")));
            this.VSIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.VSIcons.Images.SetKeyName(0, "text-left");
            this.VSIcons.Images.SetKeyName(1, "UConst");
            this.VSIcons.Images.SetKeyName(2, "UObject");
            this.VSIcons.Images.SetKeyName(3, "UState");
            this.VSIcons.Images.SetKeyName(4, "Interface");
            this.VSIcons.Images.SetKeyName(5, "Operator");
            this.VSIcons.Images.SetKeyName(6, "Operater-Protected");
            this.VSIcons.Images.SetKeyName(7, "Operator-Private");
            this.VSIcons.Images.SetKeyName(8, "TreeView");
            this.VSIcons.Images.SetKeyName(9, "Info");
            this.VSIcons.Images.SetKeyName(10, "Actor");
            this.VSIcons.Images.SetKeyName(11, "OutParameter");
            this.VSIcons.Images.SetKeyName(12, "Map");
            this.VSIcons.Images.SetKeyName(13, "Namespace");
            this.VSIcons.Images.SetKeyName(14, "ReturnValue");
            this.VSIcons.Images.SetKeyName(15, "Extend");
            this.VSIcons.Images.SetKeyName(16, "UFunction");
            this.VSIcons.Images.SetKeyName(17, "UFunction-Protected");
            this.VSIcons.Images.SetKeyName(18, "UFunction-Private");
            this.VSIcons.Images.SetKeyName(19, "Library");
            this.VSIcons.Images.SetKeyName(20, "Content");
            this.VSIcons.Images.SetKeyName(21, "Table");
            this.VSIcons.Images.SetKeyName(22, "UProperty");
            this.VSIcons.Images.SetKeyName(23, "UProperty-Protected");
            this.VSIcons.Images.SetKeyName(24, "UProperty-Private");
            this.VSIcons.Images.SetKeyName(25, "Diagram");
            this.VSIcons.Images.SetKeyName(26, "Chunks");
            this.VSIcons.Images.SetKeyName(27, "UDefaultProperty");
            this.VSIcons.Images.SetKeyName(28, "ImportCatalogPart");
            this.VSIcons.Images.SetKeyName(29, "UEnum");
            this.VSIcons.Images.SetKeyName(30, "UEnum-Protected");
            this.VSIcons.Images.SetKeyName(31, "UEnum-Private");
            this.VSIcons.Images.SetKeyName(32, "Delegate");
            this.VSIcons.Images.SetKeyName(33, "Delegate-Protected");
            this.VSIcons.Images.SetKeyName(34, "Delegate-Private");
            this.VSIcons.Images.SetKeyName(35, "Event");
            this.VSIcons.Images.SetKeyName(36, "Event-Protected");
            this.VSIcons.Images.SetKeyName(37, "Event-Private");
            this.VSIcons.Images.SetKeyName(38, "UStruct");
            this.VSIcons.Images.SetKeyName(39, "UStruct-Protected");
            this.VSIcons.Images.SetKeyName(40, "UStruct-Private");
            this.VSIcons.Images.SetKeyName(41, "UClass");
            this.VSIcons.Images.SetKeyName(42, "UClass-Abstract");
            this.VSIcons.Images.SetKeyName(43, "UClass-Within");
            // 
            // objectContextMenu
            // 
            this.objectContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.objectContextMenu_Opening);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpen,
            this.toolStripTextBoxFilter});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(502, 27);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripTextBoxFilter
            // 
            this.toolStripTextBoxFilter.Name = "toolStripTextBoxFilter";
            this.toolStripTextBoxFilter.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBoxFilter.TextChanged += new System.EventHandler(this.toolStripTextBoxFilter_TextChanged);
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuItemOpen.Image = global::UEExplorer.Properties.Resources.OpenfileDialog;
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(28, 23);
            // 
            // PackageExplorerPanel
            // 
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.TreeViewPackages);
            this.Name = "PackageExplorerPanel";
            this.Size = new System.Drawing.Size(502, 462);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonTreeView TreeViewPackages;
        private System.Windows.Forms.ImageList VSIcons;
        private Krypton.Toolkit.KryptonContextMenu objectContextMenu;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxFilter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
    }
}
