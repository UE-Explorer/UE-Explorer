namespace UEExplorer.UI.Tabs
{
	partial class UC_PackageExplorer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_PackageExplorer));
            this.kryptonDockableWorkspaceMain = new Krypton.Docking.KryptonDockableWorkspace();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditorFindTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrevButton = new System.Windows.Forms.ToolStripMenuItem();
            this.NextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ActiveObjectPath = new System.Windows.Forms.ToolStripTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.kryptonDockingManagerMain = new Krypton.Docking.KryptonDockingManager();
            this.kryptonPage2 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage4 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage1 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage5 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage6 = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceSequence1 = new Krypton.Workspace.KryptonWorkspaceSequence();
            this.dockingMenu = new System.Windows.Forms.MenuStrip();
            this.packageToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockingPanel = new System.Windows.Forms.Panel();
            this.contextService = new UEExplorer.UI.ContextProvider();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage6)).BeginInit();
            this.dockingMenu.SuspendLayout();
            this.dockingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonDockableWorkspaceMain
            // 
            this.kryptonDockableWorkspaceMain.ActivePage = null;
            this.kryptonDockableWorkspaceMain.AutoHiddenHost = false;
            this.kryptonDockableWorkspaceMain.CompactFlags = ((Krypton.Workspace.CompactFlags)(((Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | Krypton.Workspace.CompactFlags.PromoteLeafs)));
            this.kryptonDockableWorkspaceMain.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            resources.ApplyResources(this.kryptonDockableWorkspaceMain, "kryptonDockableWorkspaceMain");
            this.kryptonDockableWorkspaceMain.Name = "kryptonDockableWorkspaceMain";
            // 
            // 
            // 
            this.kryptonDockableWorkspaceMain.Root.UniqueName = "4d261f73ae8a4564b6d37aefef894f8a";
            this.kryptonDockableWorkspaceMain.Root.WorkspaceControl = this.kryptonDockableWorkspaceMain;
            this.kryptonDockableWorkspaceMain.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonDockableWorkspaceMain.ShowMaximizeButton = false;
            this.kryptonDockableWorkspaceMain.TabStop = true;
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.copyToolStripButton, "copyToolStripButton");
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
            // 
            // EditorFindTextBox
            // 
            this.EditorFindTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.EditorFindTextBox, "EditorFindTextBox");
            this.EditorFindTextBox.Name = "EditorFindTextBox";
            this.EditorFindTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditorFindTextBox_KeyPress);
            // 
            // findNextToolStripMenuItem
            // 
            this.findNextToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.findNextToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.findNextToolStripMenuItem, "findNextToolStripMenuItem");
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.Click += new System.EventHandler(this.findNextToolStripMenuItem_Click_1);
            // 
            // PrevButton
            // 
            this.PrevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.PrevButton, "PrevButton");
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Click += new System.EventHandler(this.ToolStripButton_Backward_Click);
            // 
            // NextButton
            // 
            this.NextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.NextButton, "NextButton");
            this.NextButton.Name = "NextButton";
            this.NextButton.Click += new System.EventHandler(this.ToolStripButton_Forward_Click);
            // 
            // ActiveObjectPath
            // 
            resources.ApplyResources(this.ActiveObjectPath, "ActiveObjectPath");
            this.ActiveObjectPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActiveObjectPath.Name = "ActiveObjectPath";
            this.ActiveObjectPath.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActiveObjectPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchObjectTextBox_KeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            // 
            // kryptonDockingManagerMain
            // 
            this.kryptonDockingManagerMain.PageCloseRequest += new System.EventHandler<Krypton.Docking.CloseRequestEventArgs>(this.kryptonDockingManagerMain_PageCloseRequest);
            this.kryptonDockingManagerMain.PageFloatingRequest += new System.EventHandler<Krypton.Docking.CancelUniqueNameEventArgs>(this.kryptonDockingManagerMain_PageFloatingRequest);
            // 
            // kryptonPage2
            // 
            this.kryptonPage2.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage2.Flags = 65534;
            this.kryptonPage2.LastVisibleSet = true;
            resources.ApplyResources(this.kryptonPage2, "kryptonPage2");
            this.kryptonPage2.Name = "kryptonPage2";
            this.kryptonPage2.UniqueName = "cbf677f4a6c048798211d4dab3b22477";
            // 
            // kryptonPage4
            // 
            this.kryptonPage4.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage4.Flags = 65534;
            this.kryptonPage4.LastVisibleSet = true;
            resources.ApplyResources(this.kryptonPage4, "kryptonPage4");
            this.kryptonPage4.Name = "kryptonPage4";
            this.kryptonPage4.UniqueName = "47549a94a3624ac9ac2da8569297d817";
            // 
            // kryptonPage1
            // 
            this.kryptonPage1.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage1.Flags = 65534;
            this.kryptonPage1.LastVisibleSet = true;
            resources.ApplyResources(this.kryptonPage1, "kryptonPage1");
            this.kryptonPage1.Name = "kryptonPage1";
            this.kryptonPage1.UniqueName = "76f8f762a0804b988fb145d4229fbd09";
            // 
            // kryptonPage5
            // 
            this.kryptonPage5.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage5.Flags = 65534;
            this.kryptonPage5.LastVisibleSet = true;
            resources.ApplyResources(this.kryptonPage5, "kryptonPage5");
            this.kryptonPage5.Name = "kryptonPage5";
            this.kryptonPage5.UniqueName = "ead514722d0245339b9bb6e1f22b4812";
            // 
            // kryptonPage6
            // 
            this.kryptonPage6.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage6.Flags = 65534;
            this.kryptonPage6.LastVisibleSet = true;
            resources.ApplyResources(this.kryptonPage6, "kryptonPage6");
            this.kryptonPage6.Name = "kryptonPage6";
            this.kryptonPage6.UniqueName = "3c1f10898ae444798edf4d6af56c6a35";
            // 
            // kryptonWorkspaceSequence1
            // 
            this.kryptonWorkspaceSequence1.UniqueName = "acee924747424e13b6bf72a4df70f46f";
            this.kryptonWorkspaceSequence1.WorkspaceControl = null;
            // 
            // dockingMenu
            // 
            this.dockingMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrevButton,
            this.NextButton,
            this.ActiveObjectPath,
            this.findNextToolStripMenuItem,
            this.EditorFindTextBox,
            this.copyToolStripButton});
            resources.ApplyResources(this.dockingMenu, "dockingMenu");
            this.dockingMenu.Name = "dockingMenu";
            this.dockingMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // packageToolToolStripMenuItem
            // 
            this.packageToolToolStripMenuItem.Name = "packageToolToolStripMenuItem";
            resources.ApplyResources(this.packageToolToolStripMenuItem, "packageToolToolStripMenuItem");
            // 
            // dockingPanel
            // 
            resources.ApplyResources(this.dockingPanel, "dockingPanel");
            this.dockingPanel.Controls.Add(this.kryptonDockableWorkspaceMain);
            this.dockingPanel.Name = "dockingPanel";
            // 
            // UC_PackageExplorer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dockingMenu);
            this.Controls.Add(this.dockingPanel);
            this.Name = "UC_PackageExplorer";
            this.Load += new System.EventHandler(this.UC_PackageExplorer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage6)).EndInit();
            this.dockingMenu.ResumeLayout(false);
            this.dockingMenu.PerformLayout();
            this.dockingPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem PrevButton;
        private System.Windows.Forms.ToolStripMenuItem NextButton;
        private System.Windows.Forms.ToolStripTextBox ActiveObjectPath;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripButton;
        private System.Windows.Forms.ToolStripTextBox EditorFindTextBox;
        private System.Windows.Forms.ToolStripMenuItem findNextToolStripMenuItem;
        private Krypton.Docking.KryptonDockableWorkspace kryptonDockableWorkspaceMain;
        private Krypton.Docking.KryptonDockingManager kryptonDockingManagerMain;
        private Krypton.Navigator.KryptonPage kryptonPage2;
        private Krypton.Navigator.KryptonPage kryptonPage4;
        private Krypton.Navigator.KryptonPage kryptonPage1;
        private Krypton.Navigator.KryptonPage kryptonPage5;
        private Krypton.Navigator.KryptonPage kryptonPage6;
        private Krypton.Workspace.KryptonWorkspaceSequence kryptonWorkspaceSequence1;
        private System.Windows.Forms.MenuStrip dockingMenu;
        private System.Windows.Forms.ToolStripMenuItem packageToolToolStripMenuItem;
        private System.Windows.Forms.Panel dockingPanel;
        private ContextProvider contextService;
    }
}
