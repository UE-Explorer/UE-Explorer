using System.Windows.Forms;
using Krypton.Toolkit;
using UEExplorer.Framework.Plugin;
using UEExplorer.Framework.UI;
using UEExplorer.UI.Controls;

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
            this.objectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.VSIcons = new System.Windows.Forms.ImageList(this.components);
            this.explorerToolsMenuStrip = new Krypton.Toolkit.KryptonToolStrip();
            this.packageToolsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sortByToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.sortByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isTrackingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.filterTreeDelayTimer = new System.Windows.Forms.Timer(this.components);
            this.treeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.filterTextBox = new UEExplorer.UI.Controls.FilterTextBox();
            this.explorerTableLayoutPanel = new Krypton.Toolkit.KryptonTableLayoutPanel();
            this.searchPanel = new Krypton.Toolkit.KryptonPanel();
            this.packagesTreeView = new UEExplorer.UI.Controls.TreeViewExt();
            this.explorerToolsMenuStrip.SuspendLayout();
            this.explorerTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchPanel)).BeginInit();
            this.searchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectContextMenu
            // 
            this.objectContextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.objectContextMenu.Name = "objectContextMenu";
            this.objectContextMenu.Size = new System.Drawing.Size(61, 4);
            this.objectContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.objectContextMenu_Opening);
            this.objectContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.objectContextMenu_ItemClicked);
            // 
            // VSIcons
            // 
            this.VSIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("VSIcons.ImageStream")));
            this.VSIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.VSIcons.Images.SetKeyName(0, "UConst");
            this.VSIcons.Images.SetKeyName(1, "UObject");
            this.VSIcons.Images.SetKeyName(2, "UState");
            this.VSIcons.Images.SetKeyName(3, "Interface");
            this.VSIcons.Images.SetKeyName(4, "Operator");
            this.VSIcons.Images.SetKeyName(5, "Operater-Protected");
            this.VSIcons.Images.SetKeyName(6, "Operator-Private");
            this.VSIcons.Images.SetKeyName(7, "TreeView");
            this.VSIcons.Images.SetKeyName(8, "OutParameter");
            this.VSIcons.Images.SetKeyName(9, "Map");
            this.VSIcons.Images.SetKeyName(10, "UPackage");
            this.VSIcons.Images.SetKeyName(11, "ReturnValue");
            this.VSIcons.Images.SetKeyName(12, "Extend");
            this.VSIcons.Images.SetKeyName(13, "UFunction");
            this.VSIcons.Images.SetKeyName(14, "UFunction-Protected");
            this.VSIcons.Images.SetKeyName(15, "UFunction-Private");
            this.VSIcons.Images.SetKeyName(16, "UProperty");
            this.VSIcons.Images.SetKeyName(17, "UProperty-Protected");
            this.VSIcons.Images.SetKeyName(18, "UProperty-Private");
            this.VSIcons.Images.SetKeyName(19, "Diagram");
            this.VSIcons.Images.SetKeyName(20, "Chunks");
            this.VSIcons.Images.SetKeyName(21, "UEnum");
            this.VSIcons.Images.SetKeyName(22, "UEnum-Protected");
            this.VSIcons.Images.SetKeyName(23, "UEnum-Private");
            this.VSIcons.Images.SetKeyName(24, "Delegate");
            this.VSIcons.Images.SetKeyName(25, "Delegate-Protected");
            this.VSIcons.Images.SetKeyName(26, "Delegate-Private");
            this.VSIcons.Images.SetKeyName(27, "Event");
            this.VSIcons.Images.SetKeyName(28, "Event-Protected");
            this.VSIcons.Images.SetKeyName(29, "Event-Private");
            this.VSIcons.Images.SetKeyName(30, "UStruct");
            this.VSIcons.Images.SetKeyName(31, "UStruct-Protected");
            this.VSIcons.Images.SetKeyName(32, "UStruct-Private");
            this.VSIcons.Images.SetKeyName(33, "UClass");
            this.VSIcons.Images.SetKeyName(34, "UClass-Abstract");
            this.VSIcons.Images.SetKeyName(35, "UClass-Within");
            this.VSIcons.Images.SetKeyName(36, "USound");
            this.VSIcons.Images.SetKeyName(37, "UTexture");
            this.VSIcons.Images.SetKeyName(38, "USoundGroup");
            this.VSIcons.Images.SetKeyName(39, "UFont");
            this.VSIcons.Images.SetKeyName(40, "UPalette");
            this.VSIcons.Images.SetKeyName(41, "AActor");
            this.VSIcons.Images.SetKeyName(42, "UComponent");
            this.VSIcons.Images.SetKeyName(43, "UTextBuffer");
            this.VSIcons.Images.SetKeyName(44, "UnrealPackageFile");
            this.VSIcons.Images.SetKeyName(45, "Media");
            this.VSIcons.Images.SetKeyName(46, "Imports");
            this.VSIcons.Images.SetKeyName(47, "UPolys");
            this.VSIcons.Images.SetKeyName(48, "UModel");
            // 
            // explorerToolsMenuStrip
            // 
            this.explorerToolsMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.explorerToolsMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.explorerToolsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.packageToolsStripMenuItem,
            this.collapseAllToolStripButton,
            this.toolStripSeparator1,
            this.sortByToolStripSplitButton,
            this.isTrackingToolStripButton});
            this.explorerToolsMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.explorerToolsMenuStrip.Name = "explorerToolsMenuStrip";
            this.explorerToolsMenuStrip.Padding = new System.Windows.Forms.Padding(4);
            this.explorerToolsMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.explorerToolsMenuStrip.Size = new System.Drawing.Size(320, 31);
            this.explorerToolsMenuStrip.Stretch = true;
            this.explorerToolsMenuStrip.TabIndex = 22;
            // 
            // packageToolsStripMenuItem
            // 
            this.packageToolsStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.packageToolsStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportClassesToolStripMenuItem});
            this.packageToolsStripMenuItem.Enabled = false;
            this.packageToolsStripMenuItem.Image = global::UEExplorer.Properties.Resources.Settings;
            this.packageToolsStripMenuItem.Name = "packageToolsStripMenuItem";
            this.packageToolsStripMenuItem.Size = new System.Drawing.Size(28, 23);
            this.packageToolsStripMenuItem.DropDownOpening += new System.EventHandler(this.packageToolsStripMenuItem_DropDownOpening);
            // 
            // exportClassesToolStripMenuItem
            // 
            this.exportClassesToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.ExportData;
            this.exportClassesToolStripMenuItem.Name = "exportClassesToolStripMenuItem";
            this.exportClassesToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.exportClassesToolStripMenuItem.Text = "&Export";
            // 
            // collapseAllToolStripButton
            // 
            this.collapseAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collapseAllToolStripButton.Image = global::UEExplorer.Properties.Resources.CollapseAll;
            this.collapseAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collapseAllToolStripButton.Name = "collapseAllToolStripButton";
            this.collapseAllToolStripButton.Size = new System.Drawing.Size(23, 20);
            this.collapseAllToolStripButton.Text = "Collapse All";
            this.collapseAllToolStripButton.Click += new System.EventHandler(this.collapseAllToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // sortByToolStripSplitButton
            // 
            this.sortByToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sortByToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByNameToolStripMenuItem,
            this.sortByTypeToolStripMenuItem,
            this.sortByOffsetToolStripMenuItem});
            this.sortByToolStripSplitButton.Image = global::UEExplorer.Properties.Resources.SortAscending;
            this.sortByToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sortByToolStripSplitButton.Name = "sortByToolStripSplitButton";
            this.sortByToolStripSplitButton.Size = new System.Drawing.Size(32, 20);
            this.sortByToolStripSplitButton.ToolTipText = "Sort Nodes By";
            this.sortByToolStripSplitButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sortByToolStripSplitButton_DropDownItemClicked);
            // 
            // sortByNameToolStripMenuItem
            // 
            this.sortByNameToolStripMenuItem.Checked = global::UEExplorer.Properties.Settings.Default.SortByName_Checked;
            this.sortByNameToolStripMenuItem.CheckOnClick = true;
            this.sortByNameToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.SortAscending;
            this.sortByNameToolStripMenuItem.Name = "sortByNameToolStripMenuItem";
            this.sortByNameToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.sortByNameToolStripMenuItem.Text = "Sort by Name";
            // 
            // sortByTypeToolStripMenuItem
            // 
            this.sortByTypeToolStripMenuItem.CheckOnClick = true;
            this.sortByTypeToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.SortByType;
            this.sortByTypeToolStripMenuItem.Name = "sortByTypeToolStripMenuItem";
            this.sortByTypeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.sortByTypeToolStripMenuItem.Text = "Sort by Type";
            // 
            // sortByOffsetToolStripMenuItem
            // 
            this.sortByOffsetToolStripMenuItem.CheckOnClick = true;
            this.sortByOffsetToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.SortByColumn;
            this.sortByOffsetToolStripMenuItem.Name = "sortByOffsetToolStripMenuItem";
            this.sortByOffsetToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.sortByOffsetToolStripMenuItem.Text = "Sort by Offset";
            // 
            // isTrackingToolStripButton
            // 
            this.isTrackingToolStripButton.Checked = global::UEExplorer.Properties.Settings.Default.SyncNodeWithActiveContext;
            this.isTrackingToolStripButton.CheckOnClick = true;
            this.isTrackingToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isTrackingToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.isTrackingToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("isTrackingToolStripButton.Image")));
            this.isTrackingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.isTrackingToolStripButton.Name = "isTrackingToolStripButton";
            this.isTrackingToolStripButton.Size = new System.Drawing.Size(23, 20);
            this.isTrackingToolStripButton.Text = "Sync Active Context";
            this.isTrackingToolStripButton.CheckStateChanged += new System.EventHandler(this.isTrackingToolStripButton_CheckStateChanged);
            // 
            // filterTreeDelayTimer
            // 
            this.filterTreeDelayTimer.Interval = 350;
            this.filterTreeDelayTimer.Tick += new System.EventHandler(this.filterTreeDelayTimer_Tick);
            // 
            // treeToolTip
            // 
            this.treeToolTip.AutomaticDelay = 0;
            this.treeToolTip.AutoPopDelay = 500;
            this.treeToolTip.InitialDelay = 0;
            this.treeToolTip.ReshowDelay = 0;
            this.treeToolTip.UseFading = false;
            // 
            // filterTextBox
            // 
            this.filterTextBox.AllowDrop = true;
            this.filterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTextBox.CueHint.Color1 = System.Drawing.Color.Gray;
            this.filterTextBox.CueHint.CueHintText = "Filter";
            this.filterTextBox.CueHint.Padding = new System.Windows.Forms.Padding(0);
            this.filterTextBox.Enabled = false;
            this.filterTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.filterTextBox.Location = new System.Drawing.Point(0, 0);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(314, 23);
            this.filterTextBox.TabIndex = 0;
            this.treeToolTip.SetToolTip(this.filterTextBox, "Search for objects by name");
            this.filterTextBox.TextChanged += new System.EventHandler(this.toolStripTextBoxFilter_TextChanged);
            this.filterTextBox.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripTextBoxFilter_Paint);
            // 
            // explorerTableLayoutPanel
            // 
            this.explorerTableLayoutPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("explorerTableLayoutPanel.BackgroundImage")));
            this.explorerTableLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.explorerTableLayoutPanel.ColumnCount = 1;
            this.explorerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.explorerTableLayoutPanel.Controls.Add(this.explorerToolsMenuStrip, 0, 0);
            this.explorerTableLayoutPanel.Controls.Add(this.searchPanel, 0, 1);
            this.explorerTableLayoutPanel.Controls.Add(this.packagesTreeView, 0, 2);
            this.explorerTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explorerTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.explorerTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.explorerTableLayoutPanel.Name = "explorerTableLayoutPanel";
            this.explorerTableLayoutPanel.RowCount = 3;
            this.explorerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.explorerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.explorerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.explorerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.explorerTableLayoutPanel.Size = new System.Drawing.Size(320, 500);
            this.explorerTableLayoutPanel.TabIndex = 23;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.filterTextBox);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchPanel.Location = new System.Drawing.Point(3, 35);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(4);
            this.searchPanel.Size = new System.Drawing.Size(314, 26);
            this.searchPanel.TabIndex = 23;
            // 
            // packagesTreeView
            // 
            this.packagesTreeView.AllowDrop = true;
            this.packagesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.packagesTreeView.ContextMenuStrip = this.objectContextMenu;
            this.packagesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packagesTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.packagesTreeView.FullRowSelect = true;
            this.packagesTreeView.HideSelection = false;
            this.packagesTreeView.ImageIndex = 0;
            this.packagesTreeView.ImageList = this.VSIcons;
            this.packagesTreeView.Indent = 6;
            this.packagesTreeView.Location = new System.Drawing.Point(0, 64);
            this.packagesTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.packagesTreeView.Name = "packagesTreeView";
            this.packagesTreeView.SelectedImageIndex = 0;
            this.packagesTreeView.ShowLines = false;
            this.packagesTreeView.Size = new System.Drawing.Size(320, 436);
            this.packagesTreeView.TabIndex = 21;
            this.packagesTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewPackages_BeforeExpand);
            this.packagesTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.packagesTreeView_DrawNode);
            this.packagesTreeView.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.TreeViewPackages_NodeMouseHover);
            this.packagesTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewPackages_BeforeSelect);
            this.packagesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewPackages_AfterSelect);
            this.packagesTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewPackages_NodeMouseClick);
            this.packagesTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeViewPackages_DragDrop);
            this.packagesTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeViewPackages_DragOver);
            this.packagesTreeView.DoubleClick += new System.EventHandler(this.packagesTreeView_DoubleClick);
            // 
            // PackageExplorerPanel
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.explorerTableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(280, 0);
            this.Name = "PackageExplorerPanel";
            this.Size = new System.Drawing.Size(320, 500);
            this.Load += new System.EventHandler(this.PackageExplorerPanel_Load);
            this.explorerToolsMenuStrip.ResumeLayout(false);
            this.explorerToolsMenuStrip.PerformLayout();
            this.explorerTableLayoutPanel.ResumeLayout(false);
            this.explorerTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchPanel)).EndInit();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewExt packagesTreeView;
        private System.Windows.Forms.ImageList VSIcons;
        private ContextMenuStrip objectContextMenu;
        private Krypton.Toolkit.KryptonToolStrip explorerToolsMenuStrip;
        private FilterTextBox filterTextBox;
        private ToolStripMenuItem packageToolsStripMenuItem;
        private ToolStripMenuItem exportClassesToolStripMenuItem;
        private Timer filterTreeDelayTimer;
        private ToolTip treeToolTip;
        private Krypton.Toolkit.KryptonTableLayoutPanel explorerTableLayoutPanel;
        private KryptonPanel searchPanel;
        private ToolStripButton collapseAllToolStripButton;
        private ToolStripSplitButton sortByToolStripSplitButton;
        private ToolStripMenuItem sortByOffsetToolStripMenuItem;
        private ToolStripMenuItem sortByNameToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem sortByTypeToolStripMenuItem;
        private ToolStripButton isTrackingToolStripButton;
    }
}
