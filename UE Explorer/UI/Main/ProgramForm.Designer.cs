using System.Windows.Forms;

namespace UEExplorer.UI.Main
{
    partial class ProgramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem menuItem26;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramForm));
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.loadingProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.nativeTableDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.Platform = new System.Windows.Forms.ToolStripDropDownButton();
            this.pCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabComponentsStrip = new Storm.TabControl.TabStrip();
            this.brandPictureBox = new System.Windows.Forms.PictureBox();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mostRecentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cacheExtractorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem23 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem24 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.SocialMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPalette = new Krypton.Toolkit.KryptonPalette(this.components);
            this.containerPanel = new Krypton.Toolkit.KryptonPanel();
            menuItem26 = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabComponentsStrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brandPictureBox)).BeginInit();
            this.mainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.containerPanel)).BeginInit();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItem26
            // 
            menuItem26.Name = "menuItem26";
            resources.ApplyResources(menuItem26, "menuItem26");
            menuItem26.Click += new System.EventHandler(this.MenuItem26_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadingProgressBar,
            this.progressStatusLabel,
            this.nativeTableDropDownButton,
            this.Platform});
            resources.ApplyResources(this.mainStatusStrip, "mainStatusStrip");
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // loadingProgressBar
            // 
            this.loadingProgressBar.CausesValidation = false;
            resources.ApplyResources(this.loadingProgressBar, "loadingProgressBar");
            this.loadingProgressBar.Name = "loadingProgressBar";
            this.loadingProgressBar.Step = 1;
            // 
            // progressStatusLabel
            // 
            this.progressStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.progressStatusLabel.Name = "progressStatusLabel";
            resources.ApplyResources(this.progressStatusLabel, "progressStatusLabel");
            this.progressStatusLabel.Spring = true;
            // 
            // nativeTableDropDownButton
            // 
            this.nativeTableDropDownButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.nativeTableDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.nativeTableDropDownButton, "nativeTableDropDownButton");
            this.nativeTableDropDownButton.Name = "nativeTableDropDownButton";
            this.nativeTableDropDownButton.DropDownOpening += new System.EventHandler(this.SelectedNativeTable_DropDownOpening);
            this.nativeTableDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SelectedNativeTable_DropDownItemClicked);
            // 
            // Platform
            // 
            this.Platform.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Platform.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pCToolStripMenuItem,
            this.consoleToolStripMenuItem});
            resources.ApplyResources(this.Platform, "Platform");
            this.Platform.Name = "Platform";
            this.Platform.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Platform_DropDownItemClicked);
            // 
            // pCToolStripMenuItem
            // 
            this.pCToolStripMenuItem.Name = "pCToolStripMenuItem";
            resources.ApplyResources(this.pCToolStripMenuItem, "pCToolStripMenuItem");
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            resources.ApplyResources(this.consoleToolStripMenuItem, "consoleToolStripMenuItem");
            // 
            // TabComponentsStrip
            // 
            this.TabComponentsStrip.AllowDrop = true;
            resources.ApplyResources(this.TabComponentsStrip, "TabComponentsStrip");
            this.TabComponentsStrip.Name = "TabComponentsStrip";
            this.TabComponentsStrip.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.TabComponentsStrip.NormalColorEnd = System.Drawing.Color.White;
            this.TabComponentsStrip.NormalColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.TabComponentsStrip.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(116)))), ((int)(((byte)(116)))));
            this.TabComponentsStrip.RightClickMenu = null;
            this.TabComponentsStrip.SelectedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.TabComponentsStrip.SelectedColorEnd = System.Drawing.Color.White;
            this.TabComponentsStrip.SelectedColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.TabComponentsStrip.SelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.TabComponentsStrip.TabStripItemClosing += new Storm.TabControl.TabStripItemClosingHandler(this.TabComponentsStrip_TabStripItemClosing);
            this.TabComponentsStrip.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragDrop);
            this.TabComponentsStrip.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragEnter);
            // 
            // brandPictureBox
            // 
            resources.ApplyResources(this.brandPictureBox, "brandPictureBox");
            this.brandPictureBox.Name = "brandPictureBox";
            this.brandPictureBox.TabStop = false;
            // 
            // mainMenuStrip
            // 
            resources.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.toolsMenuItem,
            this.optionsMenuItem,
            this.helpMenuItem});
            this.mainMenuStrip.Name = "mainMenuStrip";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.menuItem12,
            this.separator1,
            this.mostRecentMenuItem,
            this.separator2,
            this.menuItem10});
            this.fileMenuItem.Name = "fileMenuItem";
            resources.ApplyResources(this.fileMenuItem, "fileMenuItem");
            this.fileMenuItem.DropDownOpening += new System.EventHandler(this.fileMenuItem_DropDownOpening);
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Image = global::UEExplorer.Properties.Resources.OpenfileDialog;
            this.openFileMenuItem.Name = "openFileMenuItem";
            resources.ApplyResources(this.openFileMenuItem, "openFileMenuItem");
            this.openFileMenuItem.Click += new System.EventHandler(this.OpenFileToolStripMenuItem_Click);
            // 
            // menuItem12
            // 
            resources.ApplyResources(this.menuItem12, "menuItem12");
            this.menuItem12.Name = "menuItem12";
            this.menuItem12.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            resources.ApplyResources(this.separator1, "separator1");
            // 
            // mostRecentMenuItem
            // 
            resources.ApplyResources(this.mostRecentMenuItem, "mostRecentMenuItem");
            this.mostRecentMenuItem.Name = "mostRecentMenuItem";
            this.mostRecentMenuItem.DropDownOpening += new System.EventHandler(this.mostRecentMenuItem_DropDownOpening);
            this.mostRecentMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mostRecentMenuItem_DropDownItemClicked);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            resources.ApplyResources(this.separator2, "separator2");
            // 
            // menuItem10
            // 
            this.menuItem10.Name = "menuItem10";
            resources.ApplyResources(this.menuItem10, "menuItem10");
            this.menuItem10.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem21});
            resources.ApplyResources(this.editMenuItem, "editMenuItem");
            this.editMenuItem.Name = "editMenuItem";
            // 
            // menuItem21
            // 
            this.menuItem21.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem22});
            this.menuItem21.Name = "menuItem21";
            resources.ApplyResources(this.menuItem21, "menuItem21");
            this.menuItem21.Click += new System.EventHandler(this.FindToolStripMenuItem_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Name = "menuItem22";
            resources.ApplyResources(this.menuItem22, "menuItem22");
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem13,
            this.separator3,
            this.cacheExtractorMenuItem,
            this.separator4,
            this.menuItem17,
            this.separator5,
            this.menuItem20});
            this.toolsMenuItem.Name = "toolsMenuItem";
            resources.ApplyResources(this.toolsMenuItem, "toolsMenuItem");
            this.toolsMenuItem.DropDownOpening += new System.EventHandler(this.ToolsToolStripMenuItem_DropDownOpening);
            // 
            // menuItem13
            // 
            resources.ApplyResources(this.menuItem13, "menuItem13");
            this.menuItem13.Name = "menuItem13";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            resources.ApplyResources(this.separator3, "separator3");
            // 
            // cacheExtractorMenuItem
            // 
            resources.ApplyResources(this.cacheExtractorMenuItem, "cacheExtractorMenuItem");
            this.cacheExtractorMenuItem.Name = "cacheExtractorMenuItem";
            this.cacheExtractorMenuItem.Click += new System.EventHandler(this.UnrealCacheExtractorToolStripMenuItem_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            resources.ApplyResources(this.separator4, "separator4");
            // 
            // menuItem17
            // 
            this.menuItem17.Name = "menuItem17";
            resources.ApplyResources(this.menuItem17, "menuItem17");
            this.menuItem17.Click += new System.EventHandler(this.UnrealColorGeneratorToolStripMenuItem_Click);
            // 
            // separator5
            // 
            this.separator5.Name = "separator5";
            resources.ApplyResources(this.separator5, "separator5");
            // 
            // menuItem20
            // 
            this.menuItem20.Name = "menuItem20";
            resources.ApplyResources(this.menuItem20, "menuItem20");
            this.menuItem20.Click += new System.EventHandler(this.ToggleUEExplorerFileIconsToolStripMenuItem_Click);
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Name = "optionsMenuItem";
            resources.ApplyResources(this.optionsMenuItem, "optionsMenuItem");
            this.optionsMenuItem.Click += new System.EventHandler(this.MenuItem7_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem15,
            this.menuItem23,
            this.separator6,
            menuItem26,
            this.menuItem24,
            this.menuItem6,
            this.menuItem4,
            this.SocialMenuItem,
            this.separator7,
            this.menuItem5});
            this.helpMenuItem.Name = "helpMenuItem";
            resources.ApplyResources(this.helpMenuItem, "helpMenuItem");
            // 
            // menuItem15
            // 
            this.menuItem15.Name = "menuItem15";
            resources.ApplyResources(this.menuItem15, "menuItem15");
            this.menuItem15.Click += new System.EventHandler(this.ReportAnIssue);
            // 
            // menuItem23
            // 
            this.menuItem23.Name = "menuItem23";
            resources.ApplyResources(this.menuItem23, "menuItem23");
            this.menuItem23.Click += new System.EventHandler(this.OnCheckForUpdates);
            // 
            // separator6
            // 
            this.separator6.Name = "separator6";
            resources.ApplyResources(this.separator6, "separator6");
            // 
            // menuItem24
            // 
            this.menuItem24.Name = "menuItem24";
            resources.ApplyResources(this.menuItem24, "menuItem24");
            this.menuItem24.Click += new System.EventHandler(this.MenuItem24_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Name = "menuItem6";
            resources.ApplyResources(this.menuItem6, "menuItem6");
            this.menuItem6.Click += new System.EventHandler(this.DonateToolStripMenuItem1_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Name = "menuItem4";
            resources.ApplyResources(this.menuItem4, "menuItem4");
            this.menuItem4.Click += new System.EventHandler(this.MenuItem4_Click);
            // 
            // SocialMenuItem
            // 
            this.SocialMenuItem.Name = "SocialMenuItem";
            resources.ApplyResources(this.SocialMenuItem, "SocialMenuItem");
            this.SocialMenuItem.Click += new System.EventHandler(this.SocialMenuItem_Click);
            // 
            // separator7
            // 
            this.separator7.Name = "separator7";
            resources.ApplyResources(this.separator7, "separator7");
            // 
            // menuItem5
            // 
            this.menuItem5.Name = "menuItem5";
            resources.ApplyResources(this.menuItem5, "menuItem5");
            this.menuItem5.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPalette = this.kryptonPalette;
            this.kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteModeManager.Custom;
            // 
            // kryptonPalette
            // 
            this.kryptonPalette.BasePaletteMode = Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonPalette.ButtonSpecs.FormClose.Image = global::UEExplorer.Properties.Resources.Close;
            this.kryptonPalette.ButtonSpecs.PinHorizontal.Image = global::UEExplorer.Properties.Resources.Pin;
            this.kryptonPalette.ButtonSpecs.PinVertical.Image = global::UEExplorer.Properties.Resources.Pin;
            this.kryptonPalette.FormStyles.FormMain.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.TabStyles.TabCommon.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonPalette.TabStyles.TabDock.StateNormal.Border.Color1 = System.Drawing.Color.Gray;
            this.kryptonPalette.TabStyles.TabDock.StateNormal.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonPalette.TabStyles.TabDock.StateNormal.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            // 
            // containerPanel
            // 
            resources.ApplyResources(this.containerPanel, "containerPanel");
            this.containerPanel.Controls.Add(this.TabComponentsStrip);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Palette = this.kryptonPalette;
            this.containerPanel.PaletteMode = Krypton.Toolkit.PaletteMode.Custom;
            // 
            // ProgramForm
            // 
            this.AllowDrop = true;
            this.AllowFormChrome = false;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.mainStatusStrip);
            this.DoubleBuffered = true;
            this.HeaderStyle = Krypton.Toolkit.HeaderStyle.Custom1;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "ProgramForm";
            this.Palette = this.kryptonPalette;
            this.PaletteMode = Krypton.Toolkit.PaletteMode.Custom;
            this.StateCommon.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.StateCommon.Back.Draw = Krypton.Toolkit.InheritBool.True;
            this.StateCommon.Border.Color1 = System.Drawing.Color.Navy;
            this.StateCommon.Border.Draw = Krypton.Toolkit.InheritBool.True;
            this.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.StateCommon.Border.Width = 0;
            this.StateCommon.Header.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(50)))));
            this.StateCommon.Header.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.StateCommon.Header.Border.Draw = Krypton.Toolkit.InheritBool.False;
            this.StateCommon.Header.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.Load += new System.EventHandler(this.ProgramForm_Load);
            this.Shown += new System.EventHandler(this.ProgramForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragOver);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabComponentsStrip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brandPictureBox)).EndInit();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.containerPanel)).EndInit();
            this.containerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.StatusStrip mainStatusStrip;
        internal System.Windows.Forms.ToolStripStatusLabel progressStatusLabel;
        private System.Windows.Forms.PictureBox brandPictureBox;
        internal System.Windows.Forms.ToolStripDropDownButton nativeTableDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem5;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripMenuItem menuItem10;
        private System.Windows.Forms.ToolStripMenuItem menuItem12;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripMenuItem menuItem13;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripMenuItem cacheExtractorMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator4;
        private System.Windows.Forms.ToolStripMenuItem menuItem17;
        private System.Windows.Forms.ToolStripSeparator separator5;
        private System.Windows.Forms.ToolStripMenuItem menuItem20;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem21;
        private System.Windows.Forms.ToolStripMenuItem menuItem22;
        private System.Windows.Forms.ToolStripMenuItem menuItem23;
        private System.Windows.Forms.ToolStripMenuItem optionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem24;
        private System.Windows.Forms.ToolStripSeparator separator6;
        private System.Windows.Forms.ToolStripSeparator separator7;
        private System.Windows.Forms.ToolStripMenuItem pCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        internal System.Windows.Forms.ToolStripDropDownButton Platform;
        private System.Windows.Forms.ToolStripMenuItem menuItem6;
        private System.Windows.Forms.ToolStripMenuItem mostRecentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem4;
        private Storm.TabControl.TabStrip TabComponentsStrip;
        private System.Windows.Forms.ToolStripMenuItem SocialMenuItem;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItem15;
        private Krypton.Toolkit.KryptonManager kryptonManager;
        private Krypton.Toolkit.KryptonPalette kryptonPalette;
        private Krypton.Toolkit.KryptonPanel containerPanel;
        private ToolStripProgressBar loadingProgressBar;


        // OnClick events!

    }
}

