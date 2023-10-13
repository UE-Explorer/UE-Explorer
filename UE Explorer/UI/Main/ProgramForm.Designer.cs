﻿using System.Windows.Forms;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramForm));
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.loadingProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.nativeTableDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
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
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.navigateBackwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigateForwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extentionsMenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinUsOnSocialMediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliotsForumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sendFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportAProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPalette = new Krypton.Toolkit.KryptonCustomPaletteBase(this.components);
            this.dockSpace = new UEExplorer.UI.Tabs.UC_PackageExplorer();
            this.taskProcessor = new System.Windows.Forms.Timer(this.components);
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brandPictureBox)).BeginInit();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadingProgressBar,
            this.progressStatusLabel,
            this.nativeTableDropDownButton});
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
            this.viewToolStripMenuItem,
            this.toolsMenuItem,
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
            this.menuItem12.Image = global::UEExplorer.Properties.Resources.Save;
            this.menuItem12.Name = "menuItem12";
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
            this.findMenuItem});
            resources.ApplyResources(this.editMenuItem, "editMenuItem");
            this.editMenuItem.Name = "editMenuItem";
            // 
            // findMenuItem
            // 
            this.findMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem22});
            this.findMenuItem.Name = "findMenuItem";
            resources.ApplyResources(this.findMenuItem, "findMenuItem");
            // 
            // menuItem22
            // 
            this.menuItem22.Name = "menuItem22";
            resources.ApplyResources(this.menuItem22, "menuItem22");
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.packageExplorerToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.navigateBackwardToolStripMenuItem,
            this.navigateForwardToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // packageExplorerToolStripMenuItem
            // 
            this.packageExplorerToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Package;
            this.packageExplorerToolStripMenuItem.Name = "packageExplorerToolStripMenuItem";
            resources.ApplyResources(this.packageExplorerToolStripMenuItem, "packageExplorerToolStripMenuItem");
            this.packageExplorerToolStripMenuItem.Click += new System.EventHandler(this.packageExplorerToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // navigateBackwardToolStripMenuItem
            // 
            this.navigateBackwardToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Backwards;
            this.navigateBackwardToolStripMenuItem.Name = "navigateBackwardToolStripMenuItem";
            resources.ApplyResources(this.navigateBackwardToolStripMenuItem, "navigateBackwardToolStripMenuItem");
            this.navigateBackwardToolStripMenuItem.Click += new System.EventHandler(this.navigateBackwardToolStripMenuItem_Click);
            // 
            // navigateForwardToolStripMenuItem
            // 
            this.navigateForwardToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Forwards;
            this.navigateForwardToolStripMenuItem.Name = "navigateForwardToolStripMenuItem";
            resources.ApplyResources(this.navigateForwardToolStripMenuItem, "navigateForwardToolStripMenuItem");
            this.navigateForwardToolStripMenuItem.Click += new System.EventHandler(this.navigateForwardToolStripMenuItem_Click);
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extentionsMenuitem,
            this.separator3,
            this.menuItem17,
            this.separator5,
            this.menuItem20});
            this.toolsMenuItem.Name = "toolsMenuItem";
            resources.ApplyResources(this.toolsMenuItem, "toolsMenuItem");
            this.toolsMenuItem.DropDownOpening += new System.EventHandler(this.ToolsToolStripMenuItem_DropDownOpening);
            // 
            // extentionsMenuitem
            // 
            resources.ApplyResources(this.extentionsMenuitem, "extentionsMenuitem");
            this.extentionsMenuitem.Name = "extentionsMenuitem";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            resources.ApplyResources(this.separator3, "separator3");
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
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesMenuItem,
            this.joinUsOnSocialMediaToolStripMenuItem,
            this.toolStripSeparator2,
            this.sendFeedbackToolStripMenuItem,
            this.separator6,
            this.menuItem5});
            this.helpMenuItem.Name = "helpMenuItem";
            resources.ApplyResources(this.helpMenuItem, "helpMenuItem");
            // 
            // checkForUpdatesMenuItem
            // 
            this.checkForUpdatesMenuItem.Name = "checkForUpdatesMenuItem";
            resources.ApplyResources(this.checkForUpdatesMenuItem, "checkForUpdatesMenuItem");
            this.checkForUpdatesMenuItem.Click += new System.EventHandler(this.CheckForUpdatesMenuItem_Click);
            // 
            // joinUsOnSocialMediaToolStripMenuItem
            // 
            this.joinUsOnSocialMediaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facebookToolStripMenuItem,
            this.eliotsForumToolStripMenuItem});
            this.joinUsOnSocialMediaToolStripMenuItem.Name = "joinUsOnSocialMediaToolStripMenuItem";
            resources.ApplyResources(this.joinUsOnSocialMediaToolStripMenuItem, "joinUsOnSocialMediaToolStripMenuItem");
            // 
            // facebookToolStripMenuItem
            // 
            this.facebookToolStripMenuItem.Name = "facebookToolStripMenuItem";
            resources.ApplyResources(this.facebookToolStripMenuItem, "facebookToolStripMenuItem");
            this.facebookToolStripMenuItem.Click += new System.EventHandler(this.SocialMenuItem_Click);
            // 
            // eliotsForumToolStripMenuItem
            // 
            this.eliotsForumToolStripMenuItem.Name = "eliotsForumToolStripMenuItem";
            resources.ApplyResources(this.eliotsForumToolStripMenuItem, "eliotsForumToolStripMenuItem");
            this.eliotsForumToolStripMenuItem.Click += new System.EventHandler(this.forumToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // sendFeedbackToolStripMenuItem
            // 
            this.sendFeedbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportAProblemToolStripMenuItem});
            this.sendFeedbackToolStripMenuItem.Name = "sendFeedbackToolStripMenuItem";
            resources.ApplyResources(this.sendFeedbackToolStripMenuItem, "sendFeedbackToolStripMenuItem");
            // 
            // reportAProblemToolStripMenuItem
            // 
            this.reportAProblemToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.InfoTipInline_11_11;
            this.reportAProblemToolStripMenuItem.Name = "reportAProblemToolStripMenuItem";
            resources.ApplyResources(this.reportAProblemToolStripMenuItem, "reportAProblemToolStripMenuItem");
            this.reportAProblemToolStripMenuItem.Click += new System.EventHandler(this.ReportAnIssue);
            // 
            // separator6
            // 
            this.separator6.Name = "separator6";
            resources.ApplyResources(this.separator6, "separator6");
            // 
            // menuItem5
            // 
            this.menuItem5.Image = global::UEExplorer.Properties.Resources.AboutBox;
            this.menuItem5.Name = "menuItem5";
            resources.ApplyResources(this.menuItem5, "menuItem5");
            this.menuItem5.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPalette = this.kryptonPalette;
            this.kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Custom;
            // 
            // kryptonPalette
            // 
            this.kryptonPalette.BasePaletteMode = Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonPalette.BaseRenderMode = Krypton.Toolkit.RendererMode.Standard;
            this.kryptonPalette.ButtonSpecs.Close.Image = global::UEExplorer.Properties.Resources.Close;
            this.kryptonPalette.ButtonSpecs.FormClose.Image = global::UEExplorer.Properties.Resources.Close;
            this.kryptonPalette.ButtonSpecs.PinHorizontal.Image = global::UEExplorer.Properties.Resources.Pin;
            this.kryptonPalette.ButtonSpecs.PinVertical.Image = global::UEExplorer.Properties.Resources.Pin;
            this.kryptonPalette.Common.StateCommon.Border.Color1 = System.Drawing.Color.LightGray;
            this.kryptonPalette.Common.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.ControlStyles.ControlClient.StateCommon.Border.Color1 = System.Drawing.Color.LightGray;
            this.kryptonPalette.ControlStyles.ControlClient.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.HeaderStyles.HeaderDockActive.StateCommon.Back.Color1 = System.Drawing.SystemColors.MenuHighlight;
            this.kryptonPalette.HeaderStyles.HeaderDockActive.StateCommon.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.HeaderStyles.HeaderDockActive.StateCommon.Border.Draw = Krypton.Toolkit.InheritBool.False;
            this.kryptonPalette.HeaderStyles.HeaderDockActive.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.HeaderStyles.HeaderDockActive.StateCommon.Content.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.HeaderStyles.HeaderDockActive.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.HeaderStyles.HeaderDockInactive.StateCommon.Back.Color1 = System.Drawing.Color.LightGray;
            this.kryptonPalette.HeaderStyles.HeaderDockInactive.StateCommon.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.HeaderStyles.HeaderDockInactive.StateCommon.Border.Draw = Krypton.Toolkit.InheritBool.False;
            this.kryptonPalette.HeaderStyles.HeaderDockInactive.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.HeaderStyles.HeaderForm.StateCommon.Back.Color1 = System.Drawing.Color.DarkGray;
            this.kryptonPalette.HeaderStyles.HeaderForm.StateCommon.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.HeaderStyles.HeaderForm.StateCommon.Border.Color1 = System.Drawing.Color.LightGray;
            this.kryptonPalette.HeaderStyles.HeaderForm.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.HeaderStyles.HeaderForm.StateCommon.Content.LongText.Color1 = System.Drawing.Color.DimGray;
            this.kryptonPalette.HeaderStyles.HeaderForm.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DimGray;
            this.kryptonPalette.Images.DropDownButton.Common = global::UEExplorer.Properties.Resources.ExpandDown;
            this.kryptonPalette.Images.TreeView.Minus = global::UEExplorer.Properties.Resources.Collapse;
            this.kryptonPalette.Images.TreeView.Plus = global::UEExplorer.Properties.Resources.Expand;
            this.kryptonPalette.TabStyles.TabCommon.StateCommon.Border.Draw = Krypton.Toolkit.InheritBool.False;
            this.kryptonPalette.TabStyles.TabCommon.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.TabStyles.TabCommon.StateCommon.Border.Width = 1;
            this.kryptonPalette.TabStyles.TabCommon.StateNormal.Back.Color1 = System.Drawing.Color.Silver;
            this.kryptonPalette.TabStyles.TabCommon.StateNormal.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.TabStyles.TabCommon.StateNormal.Content.LongText.Color1 = System.Drawing.Color.DimGray;
            this.kryptonPalette.TabStyles.TabCommon.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.DimGray;
            this.kryptonPalette.TabStyles.TabCommon.StateSelected.Back.Color1 = System.Drawing.SystemColors.MenuHighlight;
            this.kryptonPalette.TabStyles.TabCommon.StateSelected.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.SolidTopLine;
            this.kryptonPalette.TabStyles.TabCommon.StateSelected.Content.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.TabStyles.TabCommon.StateSelected.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.TabStyles.TabCommon.StateTracking.Back.Color1 = System.Drawing.SystemColors.MenuHighlight;
            this.kryptonPalette.TabStyles.TabCommon.StateTracking.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.TabStyles.TabCommon.StateTracking.Content.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.TabStyles.TabCommon.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Back.Color1 = System.Drawing.Color.DarkGray;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Border.Color1 = System.Drawing.Color.Gray;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Border.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Border.Draw = Krypton.Toolkit.InheritBool.False;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Content.LongText.Color1 = System.Drawing.Color.DimGray;
            this.kryptonPalette.TabStyles.TabDock.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.kryptonPalette.TabStyles.TabDock.StateNormal.Back.Color1 = System.Drawing.Color.Silver;
            this.kryptonPalette.TabStyles.TabDock.StateNormal.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.TabStyles.TabDock.StateNormal.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.TabStyles.TabDock.StateSelected.Back.Color1 = System.Drawing.SystemColors.MenuHighlight;
            this.kryptonPalette.TabStyles.TabDock.StateSelected.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonPalette.TabStyles.TabDock.StateSelected.Content.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.TabStyles.TabDock.StateSelected.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonPalette.ToolMenuStatus.UseRoundedEdges = Krypton.Toolkit.InheritBool.False;
            // 
            // dockSpace
            // 
            resources.ApplyResources(this.dockSpace, "dockSpace");
            this.dockSpace.FilePath = null;
            this.dockSpace.Name = "dockSpace";
            // 
            // taskProcessor
            // 
            this.taskProcessor.Enabled = true;
            this.taskProcessor.Tick += new System.EventHandler(this.taskProcessor_Tick);
            // 
            // ProgramForm
            // 
            this.AllowDrop = true;
            this.AllowFormChrome = false;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.dockSpace);
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
            this.Load += new System.EventHandler(this.ProgramForm_Load);
            this.Shown += new System.EventHandler(this.ProgramForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.ProgramForm_DragOver);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brandPictureBox)).EndInit();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem extentionsMenuitem;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripMenuItem menuItem17;
        private System.Windows.Forms.ToolStripSeparator separator5;
        private System.Windows.Forms.ToolStripMenuItem menuItem20;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem22;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator6;
        private System.Windows.Forms.ToolStripMenuItem mostRecentMenuItem;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private Krypton.Toolkit.KryptonManager kryptonManager;
        private Krypton.Toolkit.KryptonCustomPaletteBase kryptonPalette;
        private ToolStripProgressBar loadingProgressBar;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem packageExplorerToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem navigateBackwardToolStripMenuItem;
        private ToolStripMenuItem navigateForwardToolStripMenuItem;
        private ToolStripMenuItem sendFeedbackToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem joinUsOnSocialMediaToolStripMenuItem;
        private ToolStripMenuItem facebookToolStripMenuItem;
        private ToolStripMenuItem reportAProblemToolStripMenuItem;
        private ToolStripMenuItem eliotsForumToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Tabs.UC_PackageExplorer dockSpace;
        private Timer taskProcessor;


        // OnClick events!

    }
}

