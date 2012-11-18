namespace UEExplorer.UI
{
    partial class ProgramForm
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.MenuItem menuItem26;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramForm));
			this.UEStatusStrip = new System.Windows.Forms.StatusStrip();
			this.LoadingProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.SelectedNativeTable = new System.Windows.Forms.ToolStripDropDownButton();
			this.Platform = new System.Windows.Forms.ToolStripDropDownButton();
			this.pCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TabComponentsStrip = new Storm.TabControl.TabStrip();
			this.tabStripItem2 = new Storm.TabControl.TabStripItem();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this._ROF = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this._CacheExtractorItem = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuItem25 = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem27 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.tabStripItem1 = new Storm.TabControl.TabStripItem();
			this.tabStripItem3 = new Storm.TabControl.TabStripItem();
			menuItem26 = new System.Windows.Forms.MenuItem();
			this.UEStatusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TabComponentsStrip)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// menuItem26
			// 
			menuItem26.Index = 2;
			menuItem26.Text = "> &Home";
			menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
			// 
			// UEStatusStrip
			// 
			this.UEStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadingProgress,
            this.ProgressLabel,
            this.SelectedNativeTable,
            this.Platform});
			this.UEStatusStrip.Location = new System.Drawing.Point(0, 540);
			this.UEStatusStrip.Name = "UEStatusStrip";
			this.UEStatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.UEStatusStrip.Size = new System.Drawing.Size(929, 22);
			this.UEStatusStrip.TabIndex = 7;
			this.UEStatusStrip.Text = "statusStrip1";
			// 
			// LoadingProgress
			// 
			this.LoadingProgress.CausesValidation = false;
			this.LoadingProgress.Enabled = false;
			this.LoadingProgress.Name = "LoadingProgress";
			this.LoadingProgress.Size = new System.Drawing.Size(100, 16);
			this.LoadingProgress.Step = 1;
			this.LoadingProgress.ToolTipText = "Loading Progress";
			this.LoadingProgress.Visible = false;
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(806, 17);
			this.ProgressLabel.Spring = true;
			this.ProgressLabel.Text = "Ready";
			this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ProgressLabel.ToolTipText = "Loading Status";
			// 
			// SelectedNativeTable
			// 
			this.SelectedNativeTable.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.SelectedNativeTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.SelectedNativeTable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SelectedNativeTable.Name = "SelectedNativeTable";
			this.SelectedNativeTable.Size = new System.Drawing.Size(42, 20);
			this.SelectedNativeTable.Text = "NTL";
			this.SelectedNativeTable.DropDownOpening += new System.EventHandler(this.SelectedNativeTable_DropDownOpening);
			this.SelectedNativeTable.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SelectedNativeTable_DropDownItemClicked);
			// 
			// Platform
			// 
			this.Platform.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.Platform.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pCToolStripMenuItem,
            this.consoleToolStripMenuItem});
			this.Platform.Image = ((System.Drawing.Image)(resources.GetObject("Platform.Image")));
			this.Platform.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Platform.Name = "Platform";
			this.Platform.Size = new System.Drawing.Size(66, 20);
			this.Platform.Text = "Platform";
			this.Platform.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Platform_DropDownItemClicked);
			// 
			// pCToolStripMenuItem
			// 
			this.pCToolStripMenuItem.Name = "pCToolStripMenuItem";
			this.pCToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.pCToolStripMenuItem.Text = "PC";
			// 
			// consoleToolStripMenuItem
			// 
			this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
			this.consoleToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.consoleToolStripMenuItem.Text = "Console";
			// 
			// TabComponentsStrip
			// 
			this.TabComponentsStrip.AllowDrop = true;
			this.TabComponentsStrip.AlwaysShowClose = false;
			this.TabComponentsStrip.AlwaysShowMenuGlyph = false;
			this.TabComponentsStrip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TabComponentsStrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.TabComponentsStrip.Location = new System.Drawing.Point(0, 0);
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
			this.TabComponentsStrip.SelectedItem = this.tabStripItem2;
			this.TabComponentsStrip.Size = new System.Drawing.Size(929, 540);
			this.TabComponentsStrip.TabIndex = 1;
			this.TabComponentsStrip.Text = "TabComponents";
			this.TabComponentsStrip.Visible = false;
			this.TabComponentsStrip.TabStripItemClosing += new Storm.TabControl.TabStripItemClosingHandler(this.TabComponentsStrip_TabStripItemClosing);
			this.TabComponentsStrip.TabStripItemSelectionChanged += new Storm.TabControl.TabStripItemChangedHandler(this.TabComponentsStrip_TabStripItemSelectionChanged);
			this.TabComponentsStrip.TabStripItemClosed += new System.EventHandler(this.TabComponentsStrip_TabStripItemClosed);
			this.TabComponentsStrip.DragDrop += new System.Windows.Forms.DragEventHandler(this.UEExplorer_Form_DragDrop);
			this.TabComponentsStrip.DragEnter += new System.Windows.Forms.DragEventHandler(this.UEExplorer_Form_DragEnter);
			// 
			// tabStripItem2
			// 
			this.tabStripItem2.IsDrawn = true;
			this.tabStripItem2.Name = "tabStripItem2";
			this.tabStripItem2.Selected = true;
			this.tabStripItem2.Size = new System.Drawing.Size(927, 519);
			this.tabStripItem2.TabIndex = 0;
			this.tabStripItem2.TabStripParent = this.TabComponentsStrip;
			this.tabStripItem2.Title = "TabStrip Page 1";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(0, -37);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(40, 37);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.toolsToolStripMenuItem,
            this.menuItem7,
            this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem8,
            this._ROF,
            this.menuItem9,
            this.menuItem12,
            this.menuItem11,
            this.menuItem10});
			this.menuItem1.Text = "&File";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 0;
			this.menuItem8.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuItem8.Text = "&Open File...";
			this.menuItem8.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
			// 
			// _ROF
			// 
			this._ROF.Enabled = false;
			this._ROF.Index = 1;
			this._ROF.Text = "Recent Opened Files";
			// 
			// menuItem9
			// 
			this.menuItem9.Enabled = false;
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "-";
			this.menuItem9.Visible = false;
			// 
			// menuItem12
			// 
			this.menuItem12.Enabled = false;
			this.menuItem12.Index = 3;
			this.menuItem12.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuItem12.Text = "&Save File";
			this.menuItem12.Visible = false;
			this.menuItem12.Click += new System.EventHandler(this.saveFileToolStripMenuItem_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 4;
			this.menuItem11.Text = "-";
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 5;
			this.menuItem10.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.menuItem10.Text = "E&xit";
			this.menuItem10.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Enabled = false;
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem21});
			this.menuItem2.Text = "Edit";
			this.menuItem2.Visible = false;
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 0;
			this.menuItem21.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem22});
			this.menuItem21.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			this.menuItem21.Text = "Find";
			this.menuItem21.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 0;
			this.menuItem22.Text = "";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.Index = 2;
			this.toolsToolStripMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem13,
            this.menuItem14,
            this._CacheExtractorItem,
            this.menuItem16,
            this.menuItem17,
            this.menuItem18,
            this.menuItem19,
            this.menuItem20});
			this.toolsToolStripMenuItem.Text = "&Tools";
			this.toolsToolStripMenuItem.Popup += new System.EventHandler(this.toolsToolStripMenuItem_DropDownOpening);
			// 
			// menuItem13
			// 
			this.menuItem13.Enabled = false;
			this.menuItem13.Index = 0;
			this.menuItem13.Text = "Extensions";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 1;
			this.menuItem14.Text = "-";
			// 
			// _CacheExtractorItem
			// 
			this._CacheExtractorItem.Enabled = false;
			this._CacheExtractorItem.Index = 2;
			this._CacheExtractorItem.Text = "Cache Extractor";
			this._CacheExtractorItem.Click += new System.EventHandler(this.unrealCacheExtractorToolStripMenuItem_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 3;
			this.menuItem16.Text = "-";
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 4;
			this.menuItem17.Text = "Color Generator";
			this.menuItem17.Click += new System.EventHandler(this.unrealColorGeneratorToolStripMenuItem_Click);
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 5;
			this.menuItem18.Text = "Native Generator";
			this.menuItem18.Click += new System.EventHandler(this.unrealNativeTableGeneratorToolStripMenuItem_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 6;
			this.menuItem19.Text = "-";
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 7;
			this.menuItem20.Text = "Registry Features Enabled";
			this.menuItem20.Click += new System.EventHandler(this.toggleUEExplorerFileIconsToolStripMenuItem_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "&Options";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 4;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem23,
            this.menuItem25,
            menuItem26,
            this.menuItem24,
            this.menuItem6,
            this.menuItem4,
            this.menuItem27,
            this.menuItem5});
			this.menuItem3.Text = "&Help";
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 0;
			this.menuItem23.Text = "&Check for Updates...";
			this.menuItem23.Click += new System.EventHandler(this.checkForUpdates);
			// 
			// menuItem25
			// 
			this.menuItem25.Index = 1;
			this.menuItem25.Text = "-";
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 3;
			this.menuItem24.Text = "> &Forums";
			this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "> Donate";
			this.menuItem6.Click += new System.EventHandler(this.donateToolStripMenuItem1_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 5;
			this.menuItem4.Text = "> Contact";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem27
			// 
			this.menuItem27.Index = 6;
			this.menuItem27.Text = "-";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 7;
			this.menuItem5.Text = "&About...";
			this.menuItem5.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// tabStripItem1
			// 
			this.tabStripItem1.IsDrawn = true;
			this.tabStripItem1.Name = "tabStripItem1";
			this.tabStripItem1.Size = new System.Drawing.Size(927, 519);
			this.tabStripItem1.TabIndex = 0;
			this.tabStripItem1.TabStripParent = this.TabComponentsStrip;
			this.tabStripItem1.Title = "TabStrip Page 1";
			// 
			// tabStripItem3
			// 
			this.tabStripItem3.IsDrawn = true;
			this.tabStripItem3.Name = "tabStripItem3";
			this.tabStripItem3.Size = new System.Drawing.Size(927, 519);
			this.tabStripItem3.TabIndex = 1;
			this.tabStripItem3.TabStripParent = this.TabComponentsStrip;
			this.tabStripItem3.Title = "TabStrip Page 2";
			// 
			// ProgramForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			this.BackgroundImage = global::UEExplorer.Properties.Resources.UE_ProgramLogo;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(929, 562);
			this.Controls.Add(this.TabComponentsStrip);
			this.Controls.Add(this.UEStatusStrip);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "ProgramForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Shown += new System.EventHandler(this.Unreal_Explorer_Form_Shown);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.UEExplorer_Form_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.UEExplorer_Form_DragEnter);
			this.UEStatusStrip.ResumeLayout(false);
			this.UEStatusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TabComponentsStrip)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		internal System.Windows.Forms.StatusStrip UEStatusStrip;
		internal System.Windows.Forms.ToolStripStatusLabel ProgressLabel;
		private System.Windows.Forms.PictureBox pictureBox1;
		internal System.Windows.Forms.ToolStripProgressBar LoadingProgress;
		internal Storm.TabControl.TabStrip TabComponentsStrip;
		internal System.Windows.Forms.ToolStripDropDownButton SelectedNativeTable;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem _CacheExtractorItem;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.ToolStripMenuItem pCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
		internal System.Windows.Forms.ToolStripDropDownButton Platform;
		private Storm.TabControl.TabStripItem tabStripItem1;
		private Storm.TabControl.TabStripItem tabStripItem2;
		private Storm.TabControl.TabStripItem tabStripItem3;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem _ROF;
		private System.Windows.Forms.MenuItem menuItem4;


		// OnClick events!

    }
}

