using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace UEExplorer.UI
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
            System.Windows.Forms.ToolStripMenuItem webMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramForm));
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.loadingProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.nativeTableDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.platformMenuItem = new System.Windows.Forms.ToolStripDropDownButton();
            this.platformPCMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.platformConsoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabComponentsStrip = new Storm.TabControl.TabStrip();
            this.brandPictureBox = new System.Windows.Forms.PictureBox();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mostRecentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cacheExtractorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.colorGeneratorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleFilesAssociationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportAnIssueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator6 = new System.Windows.Forms.ToolStripSeparator();
            this.forumLinkMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.donateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contactMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.socialMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator7 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openHomeButton = new System.Windows.Forms.Button();
            this.containerPanel = new System.Windows.Forms.Panel();
            webMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabComponentsStrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brandPictureBox)).BeginInit();
            this.mainMenuStrip.SuspendLayout();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItem26
            // 
            webMenuItem.Name = "webMenuItem";
            resources.ApplyResources(webMenuItem, "webMenuItem");
            webMenuItem.Click += new System.EventHandler(this.WebMenuItem_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadingProgressBar,
            this.progressStatusLabel,
            this.nativeTableDropDownButton,
            this.platformMenuItem});
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
            this.nativeTableDropDownButton.DropDownOpening += new System.EventHandler(this.NativeTableDropDownButton_DropDownOpening);
            this.nativeTableDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.NativeTableDropDownButton_DropDownItemClicked);
            // 
            // Platform
            // 
            this.platformMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.platformMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.platformPCMenuItem,
            this.platformConsoleMenuItem});
            resources.ApplyResources(this.platformMenuItem, "platformConsoleMenuItem");
            this.platformMenuItem.Name = "platformConsoleMenuItem";
            this.platformMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.platformMenuItem_DropDownItemClicked);
            // 
            // pCToolStripMenuItem
            // 
            this.platformPCMenuItem.Name = "platformPCMenuItem";
            resources.ApplyResources(this.platformPCMenuItem, "platformPCMenuItem");
            // 
            // consoleToolStripMenuItem
            // 
            this.platformConsoleMenuItem.Name = "platformConsoleMenuItem";
            resources.ApplyResources(this.platformConsoleMenuItem, "platformConsoleMenuItem");
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
            this.TabComponentsStrip.TabStripItemClosed += new EventHandler(TabComponentsStrip_TabStripItemClosed);
            this.TabComponentsStrip.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragDrop);
            this.TabComponentsStrip.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragEnter);
            // 
            // brandPictureBox
            // 
            resources.ApplyResources(this.brandPictureBox, "brandPictureBox");
            this.brandPictureBox.Name = "brandPictureBox";
            this.brandPictureBox.TabStop = false;
            this.brandPictureBox.Image = global::UEExplorer.Properties.Resources.UE_ProgramLogo;
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
            this.saveFileMenuItem,
            this.separator1,
            this.mostRecentMenuItem,
            this.separator2,
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            resources.ApplyResources(this.fileMenuItem, "fileMenuItem");
            this.fileMenuItem.DropDownOpening += new System.EventHandler(this.fileMenuItem_DropDownOpening);
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            resources.ApplyResources(this.openFileMenuItem, "openFileMenuItem");
            this.openFileMenuItem.Click += new System.EventHandler(this.OpenFileMenuItem_Click);
            // 
            // menuItem12
            // 
            resources.ApplyResources(this.saveFileMenuItem, "saveFileMenuItem");
            this.saveFileMenuItem.Name = "saveFileMenuItem";
            this.saveFileMenuItem.Click += new System.EventHandler(this.SaveFileToolMenu);
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
            this.exitMenuItem.Name = "exitMenuItem";
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolMenuItem});
            resources.ApplyResources(this.editMenuItem, "editMenuItem");
            this.editMenuItem.Name = "editMenuItem";
            // 
            // menuItem21
            // 
            this.findToolMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem22});
            this.findToolMenuItem.Name = "findToolMenuItem";
            resources.ApplyResources(this.findToolMenuItem, "findToolMenuItem");
            this.findToolMenuItem.Click += new System.EventHandler(this.FindMenuItem_Click);
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
            this.colorGeneratorMenuItem,
            this.separator5,
            this.toggleFilesAssociationMenuItem});
            this.toolsMenuItem.Name = "toolsMenuItem";
            resources.ApplyResources(this.toolsMenuItem, "toolsMenuItem");
            this.toolsMenuItem.DropDownOpening += new System.EventHandler(this.ToolsMenuItem_DropDownOpening);
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
            this.cacheExtractorMenuItem.Click += new System.EventHandler(this.CacheExtractorMenuItem_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            resources.ApplyResources(this.separator4, "separator4");
            // 
            // menuItem17
            // 
            this.colorGeneratorMenuItem.Name = "colorGeneratorMenuItem";
            resources.ApplyResources(this.colorGeneratorMenuItem, "colorGeneratorMenuItem");
            this.colorGeneratorMenuItem.Click += new System.EventHandler(this.ColorGeneratorMenuItem_Click);
            // 
            // separator5
            // 
            this.separator5.Name = "separator5";
            resources.ApplyResources(this.separator5, "separator5");
            // 
            // menuItem20
            // 
            this.toggleFilesAssociationMenuItem.Name = "toggleFilesAssociationMenuItem";
            resources.ApplyResources(this.toggleFilesAssociationMenuItem, "toggleFilesAssociationMenuItem");
            this.toggleFilesAssociationMenuItem.Click += new System.EventHandler(this.ToggleFilesAssociationMenuItem_Click);
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Name = "optionsMenuItem";
            resources.ApplyResources(this.optionsMenuItem, "optionsMenuItem");
            this.optionsMenuItem.Click += new System.EventHandler(this.OptionsMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportAnIssueMenuItem,
            this.checkForUpdatesMenuItem,
            this.separator6,
            webMenuItem,
            this.forumLinkMenu,
            this.donateMenuItem,
            this.contactMenuItem,
            this.socialMenuItem,
            this.separator7,
            this.aboutMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            resources.ApplyResources(this.helpMenuItem, "helpMenuItem");
            // 
            // menuItem15
            // 
            this.reportAnIssueMenuItem.Name = "reportAnIssueMenuItem";
            resources.ApplyResources(this.reportAnIssueMenuItem, "reportAnIssueMenuItem");
            this.reportAnIssueMenuItem.Click += new System.EventHandler(this.ReportAnIssue);
            // 
            // menuItem23
            // 
            this.checkForUpdatesMenuItem.Name = "checkForUpdatesMenuItem";
            resources.ApplyResources(this.checkForUpdatesMenuItem, "checkForUpdatesMenuItem");
            this.checkForUpdatesMenuItem.Click += new System.EventHandler(this.CheckForUpdatesMenuItem_Click);
            // 
            // separator6
            // 
            this.separator6.Name = "separator6";
            resources.ApplyResources(this.separator6, "separator6");
            // 
            // menuItem24
            // 
            this.forumLinkMenu.Name = "forumLinkMenu";
            resources.ApplyResources(this.forumLinkMenu, "forumLinkMenu");
            this.forumLinkMenu.Click += new System.EventHandler(this.ForumMenuItem_Click);
            // 
            // menuItem6
            // 
            this.donateMenuItem.Name = "donateMenuItem";
            resources.ApplyResources(this.donateMenuItem, "donateMenuItem");
            this.donateMenuItem.Click += new System.EventHandler(this.DonateMenuItem_Click);
            // 
            // menuItem4
            // 
            this.contactMenuItem.Name = "contactMenuItem";
            resources.ApplyResources(this.contactMenuItem, "contactMenuItem");
            this.contactMenuItem.Click += new System.EventHandler(this.ContactMenuItem_Click);
            // 
            // SocialMenuItem
            // 
            this.socialMenuItem.Name = "socialMenuItem";
            resources.ApplyResources(this.socialMenuItem, "socialMenuItem");
            this.socialMenuItem.Click += new System.EventHandler(this.SocialMenuItem_Click);
            // 
            // separator7
            // 
            this.separator7.Name = "separator7";
            resources.ApplyResources(this.separator7, "separator7");
            // 
            // menuItem5
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            resources.ApplyResources(this.aboutMenuItem, "aboutMenuItem");
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // openHomeButton
            // 
            resources.ApplyResources(this.openHomeButton, "openHomeButton");
            this.openHomeButton.Name = "openHomeButton";
            this.openHomeButton.Click += new System.EventHandler(this.OpenHomeButton_Click);
            this.openHomeButton.Location = new Point(0, 0);
            this.openHomeButton.Visible = false;
            // 
            // containerPanel
            // 
            resources.ApplyResources(this.containerPanel, "containerPanel");
            //this.containerPanel.Controls.Add(this.openHomeButton);
            this.containerPanel.Controls.Add(this.TabComponentsStrip);
            this.containerPanel.Name = "containerPanel";
            // 
            // ProgramForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.mainStatusStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "ProgramForm";
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
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripMenuItem menuItem13;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripMenuItem cacheExtractorMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator4;
        private System.Windows.Forms.ToolStripMenuItem colorGeneratorMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator5;
        private System.Windows.Forms.ToolStripMenuItem toggleFilesAssociationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem22;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forumLinkMenu;
        private System.Windows.Forms.ToolStripSeparator separator6;
        private System.Windows.Forms.ToolStripSeparator separator7;
        private System.Windows.Forms.ToolStripMenuItem platformPCMenuItem;
        private System.Windows.Forms.ToolStripMenuItem platformConsoleMenuItem;
        internal System.Windows.Forms.ToolStripDropDownButton platformMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostRecentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contactMenuItem;
        private System.Windows.Forms.Button openHomeButton;
        private Storm.TabControl.TabStrip TabComponentsStrip;
        private System.Windows.Forms.ToolStripMenuItem socialMenuItem;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem reportAnIssueMenuItem;
        private System.Windows.Forms.Panel containerPanel;
        internal System.Windows.Forms.ToolStripProgressBar loadingProgressBar;

        // OnClick events!

    }
}

