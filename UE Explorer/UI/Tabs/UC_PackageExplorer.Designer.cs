namespace UEExplorer.UI.Tabs
{
	partial class UC_PackageExplorer
	{
		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_PackageExplorer));
            this.documentsNavigator = new Krypton.Docking.KryptonDockableNavigator();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.quickFindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prevButton = new System.Windows.Forms.ToolStripMenuItem();
            this.nextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.dockingManager = new Krypton.Docking.KryptonDockingManager();
            this.dockingMenu = new System.Windows.Forms.MenuStrip();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockingPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.documentsNavigator)).BeginInit();
            this.documentsNavigator.SuspendLayout();
            this.dockingMenu.SuspendLayout();
            this.dockingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentsNavigator
            // 
            this.documentsNavigator.Bar.BarMapExtraText = Krypton.Navigator.MapKryptonPageText.None;
            this.documentsNavigator.Bar.BarMapImage = ((Krypton.Navigator.MapKryptonPageImage)(resources.GetObject("documentsNavigator.Bar.BarMapImage")));
            this.documentsNavigator.Bar.BarMapText = Krypton.Navigator.MapKryptonPageText.TextTitle;
            this.documentsNavigator.Bar.BarMultiline = Krypton.Navigator.BarMultiline.Singleline;
            this.documentsNavigator.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Top;
            this.documentsNavigator.Bar.CheckButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.documentsNavigator.Bar.ItemAlignment = Krypton.Toolkit.RelativePositionAlign.Near;
            this.documentsNavigator.Bar.ItemMaximumSize = new System.Drawing.Size(400, 200);
            this.documentsNavigator.Bar.ItemMinimumSize = new System.Drawing.Size(20, 20);
            this.documentsNavigator.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.Auto;
            this.documentsNavigator.Bar.ItemSizing = Krypton.Navigator.BarItemSizing.SameHeight;
            this.documentsNavigator.Bar.TabBorderStyle = Krypton.Toolkit.TabBorderStyle.SquareOutsizeSmall;
            this.documentsNavigator.Bar.TabStyle = Krypton.Toolkit.TabStyle.HighProfile;
            this.documentsNavigator.ControlKryptonFormFeatures = false;
            resources.ApplyResources(this.documentsNavigator, "documentsNavigator");
            this.documentsNavigator.Name = "documentsNavigator";
            this.documentsNavigator.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.documentsNavigator.Owner = null;
            this.documentsNavigator.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.copyToolStripButton, "copyToolStripButton");
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            // 
            // quickFindToolStripMenuItem
            // 
            this.quickFindToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.quickFindToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.quickFindToolStripMenuItem, "quickFindToolStripMenuItem");
            this.quickFindToolStripMenuItem.Name = "quickFindToolStripMenuItem";
            this.quickFindToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            // 
            // prevButton
            // 
            this.prevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.prevButton, "prevButton");
            this.prevButton.Name = "prevButton";
            this.prevButton.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.prevButton.Click += new System.EventHandler(this.ToolStripButton_Backward_Click);
            // 
            // nextButton
            // 
            this.nextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.nextButton, "nextButton");
            this.nextButton.Name = "nextButton";
            this.nextButton.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.nextButton.Click += new System.EventHandler(this.ToolStripButton_Forward_Click);
            // 
            // dockingManager
            // 
            this.dockingManager.PageCloseRequest += new System.EventHandler<Krypton.Docking.CloseRequestEventArgs>(this.kryptonDockingManagerMain_PageCloseRequest);
            this.dockingManager.PageFloatingRequest += new System.EventHandler<Krypton.Docking.CancelUniqueNameEventArgs>(this.kryptonDockingManagerMain_PageFloatingRequest);
            // 
            // dockingMenu
            // 
            this.dockingMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prevButton,
            this.nextButton,
            this.quickFindToolStripMenuItem,
            this.copyToolStripButton,
            this.openFileToolStripMenuItem,
            this.findInStripMenuItem});
            resources.ApplyResources(this.dockingMenu, "dockingMenu");
            this.dockingMenu.Name = "dockingMenu";
            this.dockingMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openFileToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.OpenfileDialog;
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            resources.ApplyResources(this.openFileToolStripMenuItem, "openFileToolStripMenuItem");
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // findInStripMenuItem
            // 
            this.findInStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findInStripMenuItem.Image = global::UEExplorer.Properties.Resources.FindInFile;
            this.findInStripMenuItem.Name = "findInStripMenuItem";
            this.findInStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            resources.ApplyResources(this.findInStripMenuItem, "findInStripMenuItem");
            this.findInStripMenuItem.Click += new System.EventHandler(this.findInStripMenuItem_Click);
            // 
            // packageToolToolStripMenuItem
            // 
            this.packageToolToolStripMenuItem.Name = "packageToolToolStripMenuItem";
            resources.ApplyResources(this.packageToolToolStripMenuItem, "packageToolToolStripMenuItem");
            // 
            // dockingPanel
            // 
            resources.ApplyResources(this.dockingPanel, "dockingPanel");
            this.dockingPanel.Controls.Add(this.documentsNavigator);
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
            ((System.ComponentModel.ISupportInitialize)(this.documentsNavigator)).EndInit();
            this.documentsNavigator.ResumeLayout(false);
            this.dockingMenu.ResumeLayout(false);
            this.dockingMenu.PerformLayout();
            this.dockingPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private System.Windows.Forms.ToolStripMenuItem prevButton;
        private System.Windows.Forms.ToolStripMenuItem nextButton;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem quickFindToolStripMenuItem;
        private Krypton.Docking.KryptonDockableNavigator documentsNavigator;
        private Krypton.Docking.KryptonDockingManager dockingManager;
        private System.Windows.Forms.MenuStrip dockingMenu;
        private System.Windows.Forms.ToolStripMenuItem packageToolToolStripMenuItem;
        private System.Windows.Forms.Panel dockingPanel;
        private System.Windows.Forms.ToolStripMenuItem findInStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
    }
}
