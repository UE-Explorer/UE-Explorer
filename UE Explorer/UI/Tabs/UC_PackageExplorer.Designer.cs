using UEExplorer.Framework;

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
            this.kryptonWorkspaceCell1 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCell2 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditorFindTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrevButton = new System.Windows.Forms.ToolStripMenuItem();
            this.NextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ActiveObjectPath = new System.Windows.Forms.ToolStripTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.kryptonDockingManagerMain = new Krypton.Docking.KryptonDockingManager();
            this.kryptonWorkspaceSequence1 = new Krypton.Workspace.KryptonWorkspaceSequence();
            this.dockingMenu = new System.Windows.Forms.MenuStrip();
            this.packageToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockingPanel = new System.Windows.Forms.Panel();
            this.contextService = new UEExplorer.UI.ContextProvider();
            this.packageManager = new UEExplorer.Framework.PackageManager();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceMain)).BeginInit();
            this.kryptonDockableWorkspaceMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell1)).BeginInit();
            this.kryptonWorkspaceCell1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell2)).BeginInit();
            this.kryptonWorkspaceCell2.SuspendLayout();
            this.dockingMenu.SuspendLayout();
            this.dockingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonDockableWorkspaceMain
            // 
            this.kryptonDockableWorkspaceMain.ActivePage = null;
            this.kryptonDockableWorkspaceMain.AutoHiddenHost = false;
            this.kryptonDockableWorkspaceMain.CompactFlags = ((Krypton.Workspace.CompactFlags)((((Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | Krypton.Workspace.CompactFlags.PromoteLeafs) 
            | Krypton.Workspace.CompactFlags.AtLeastOneVisibleCell)));
            this.kryptonDockableWorkspaceMain.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            resources.ApplyResources(this.kryptonDockableWorkspaceMain, "kryptonDockableWorkspaceMain");
            this.kryptonDockableWorkspaceMain.Name = "kryptonDockableWorkspaceMain";
            // 
            // 
            // 
            this.kryptonDockableWorkspaceMain.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell1,
            this.kryptonWorkspaceCell2});
            this.kryptonDockableWorkspaceMain.Root.UniqueName = "4d261f73ae8a4564b6d37aefef894f8a";
            this.kryptonDockableWorkspaceMain.Root.WorkspaceControl = this.kryptonDockableWorkspaceMain;
            this.kryptonDockableWorkspaceMain.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonDockableWorkspaceMain.ShowMaximizeButton = false;
            this.kryptonDockableWorkspaceMain.TabStop = true;
            this.kryptonDockableWorkspaceMain.ActivePageChanged += new System.EventHandler<Krypton.Workspace.ActivePageChangedEventArgs>(this.kryptonDockableWorkspaceMain_ActivePageChanged);
            // 
            // kryptonWorkspaceCell1
            // 
            this.kryptonWorkspaceCell1.AllowPageDrag = true;
            this.kryptonWorkspaceCell1.AllowTabFocus = false;
            resources.ApplyResources(this.kryptonWorkspaceCell1, "kryptonWorkspaceCell1");
            this.kryptonWorkspaceCell1.Name = "kryptonWorkspaceCell1";
            this.kryptonWorkspaceCell1.StarSize = "25*,50*";
            this.kryptonWorkspaceCell1.Tag = "Nav";
            this.kryptonWorkspaceCell1.UniqueName = "6a6743310c954b4997a711ce824b8c41";
            // 
            // kryptonWorkspaceCell2
            // 
            this.kryptonWorkspaceCell2.AllowPageDrag = true;
            this.kryptonWorkspaceCell2.AllowTabFocus = false;
            this.kryptonWorkspaceCell2.Bar.TabStyle = Krypton.Toolkit.TabStyle.StandardProfile;
            this.kryptonWorkspaceCell2.Name = "kryptonWorkspaceCell2";
            this.kryptonWorkspaceCell2.StarSize = "75*,50*";
            this.kryptonWorkspaceCell2.Tag = "Docs";
            this.kryptonWorkspaceCell2.UniqueName = "a5940d6ad76d4f1ba2317f0cf0d129d0";
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
            // kryptonWorkspaceSequence1
            // 
            this.kryptonWorkspaceSequence1.UniqueName = "acee924747424e13b6bf72a4df70f46f";
            this.kryptonWorkspaceSequence1.WorkspaceControl = null;
            // 
            // dockingMenu
            // 
            resources.ApplyResources(this.dockingMenu, "dockingMenu");
            this.dockingMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrevButton,
            this.NextButton,
            this.ActiveObjectPath,
            this.findNextToolStripMenuItem,
            this.EditorFindTextBox,
            this.copyToolStripButton});
            this.dockingMenu.Name = "dockingMenu";
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
            this.kryptonDockableWorkspaceMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell1)).EndInit();
            this.kryptonWorkspaceCell1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell2)).EndInit();
            this.kryptonWorkspaceCell2.ResumeLayout(false);
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
        private Krypton.Workspace.KryptonWorkspaceSequence kryptonWorkspaceSequence1;
        private System.Windows.Forms.MenuStrip dockingMenu;
        private System.Windows.Forms.ToolStripMenuItem packageToolToolStripMenuItem;
        private System.Windows.Forms.Panel dockingPanel;
        private ContextProvider contextService;
        private PackageManager packageManager;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell1;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell2;
    }
}
