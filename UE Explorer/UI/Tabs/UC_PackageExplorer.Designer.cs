namespace UEExplorer.UI.Tabs
{
	partial class UC_PackageExplorer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.TableLayoutPanel UScriptLayout;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_PackageExplorer));
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label Label_DetectedBuild;
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.Label Label_LicenseeMode;
			System.Windows.Forms.Label Label_Flags;
			System.Windows.Forms.Label Label_Version;
			System.Windows.Forms.ToolStripMenuItem exportingToolStripMenuItem;
			this.Panel_Content = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.WPFHost = new System.Windows.Forms.Integration.ElementHost();
			this.TextEditorPanel = new UEExplorer.UI.Tabs.TextEditorPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.ToolStrip_Content = new System.Windows.Forms.ToolStrip();
			this.PrevButton = new System.Windows.Forms.ToolStripButton();
			this.NextButton = new System.Windows.Forms.ToolStripButton();
			this.ExportButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.SearchBox = new System.Windows.Forms.ToolStripTextBox();
			this.FindButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.Label_ObjectName = new System.Windows.Forms.ToolStripLabel();
			this.ViewTools = new System.Windows.Forms.ToolStripDropDownButton();
			this.Panel_Main = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.TabControl_General = new System.Windows.Forms.TabControl();
			this.TabPage_Package = new System.Windows.Forms.TabPage();
			this.Num_NameIndex = new System.Windows.Forms.NumericUpDown();
			this.Num_ObjectIndex = new System.Windows.Forms.NumericUpDown();
			this.FolderValue = new System.Windows.Forms.Label();
			this.Button_FindObject = new System.Windows.Forms.Button();
			this.Label_Folder = new System.Windows.Forms.Label();
			this.Button_FindName = new System.Windows.Forms.Button();
			this.BuildValue = new System.Windows.Forms.Label();
			this.CookerValue = new System.Windows.Forms.Label();
			this.EngineValue = new System.Windows.Forms.Label();
			this.VersionValue = new System.Windows.Forms.Label();
			this.FlagsValue = new System.Windows.Forms.Label();
			this.LicenseeValue = new System.Windows.Forms.Label();
			this.Label_GUID = new System.Windows.Forms.TextBox();
			this.LABEL_Copyright = new System.Windows.Forms.Label();
			this.LABEL_Author = new System.Windows.Forms.Label();
			this.DataGridView_Flags = new System.Windows.Forms.DataGridView();
			this.Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Label_CookerVersion = new System.Windows.Forms.Label();
			this.Label_EngineVersion = new System.Windows.Forms.Label();
			this.TabPage_Tables = new System.Windows.Forms.TabPage();
			this.TabControl_Tables = new System.Windows.Forms.TabControl();
			this.TabPage_Names = new System.Windows.Forms.TabPage();
			this.DataGridView_NameTable = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TabPage_Exports = new System.Windows.Forms.TabPage();
			this.filterPanel = new System.Windows.Forms.Panel();
			this.checkBox9 = new System.Windows.Forms.CheckBox();
			this.VSIcons = new System.Windows.Forms.ImageList(this.components);
			this.checkBox8 = new System.Windows.Forms.CheckBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.TreeView_Exports = new System.Windows.Forms.TreeView();
			this.TabPage_Imports = new System.Windows.Forms.TabPage();
			this.TreeView_Imports = new System.Windows.Forms.TreeView();
			this.TabPage_Generations = new System.Windows.Forms.TabPage();
			this.DataGridView_GenerationsTable = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TabPage_Objects = new System.Windows.Forms.TabPage();
			this.TabControl_Objects = new System.Windows.Forms.TabControl();
			this.TabPage_Classes = new System.Windows.Forms.TabPage();
			this._SearchIcon = new System.Windows.Forms.PictureBox();
			this.TreeView_Classes = new System.Windows.Forms.TreeView();
			this.FilterText = new System.Windows.Forms.TextBox();
			this.TabPage_Content = new System.Windows.Forms.TabPage();
			this.Button_Export = new System.Windows.Forms.Button();
			this.TreeView_Content = new System.Windows.Forms.TreeView();
			this.TabPage_Deps = new System.Windows.Forms.TabPage();
			this.TreeView_Deps = new System.Windows.Forms.TreeView();
			this.TabPage_Chunks = new System.Windows.Forms.TabPage();
			this.DataGridView_Chunks = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CompressedOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CompressedSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel2 = new System.Windows.Forms.Panel();
			this.ToolStrip_Main = new System.Windows.Forms.ToolStrip();
			this._Tools_StripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.exportDecompiledClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportScriptClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ReloadButton = new System.Windows.Forms.ToolStripButton();
			this.label3 = new System.Windows.Forms.Label();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			UScriptLayout = new System.Windows.Forms.TableLayoutPanel();
			label4 = new System.Windows.Forms.Label();
			Label_DetectedBuild = new System.Windows.Forms.Label();
			Label_LicenseeMode = new System.Windows.Forms.Label();
			Label_Flags = new System.Windows.Forms.Label();
			Label_Version = new System.Windows.Forms.Label();
			exportingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			UScriptLayout.SuspendLayout();
			this.Panel_Content.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.ToolStrip_Content.SuspendLayout();
			this.Panel_Main.SuspendLayout();
			this.panel1.SuspendLayout();
			this.TabControl_General.SuspendLayout();
			this.TabPage_Package.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Num_NameIndex)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Num_ObjectIndex)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_Flags)).BeginInit();
			this.TabPage_Tables.SuspendLayout();
			this.TabControl_Tables.SuspendLayout();
			this.TabPage_Names.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_NameTable)).BeginInit();
			this.TabPage_Exports.SuspendLayout();
			this.filterPanel.SuspendLayout();
			this.TabPage_Imports.SuspendLayout();
			this.TabPage_Generations.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_GenerationsTable)).BeginInit();
			this.TabPage_Objects.SuspendLayout();
			this.TabControl_Objects.SuspendLayout();
			this.TabPage_Classes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._SearchIcon)).BeginInit();
			this.TabPage_Content.SuspendLayout();
			this.TabPage_Deps.SuspendLayout();
			this.TabPage_Chunks.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_Chunks)).BeginInit();
			this.panel2.SuspendLayout();
			this.ToolStrip_Main.SuspendLayout();
			this.SuspendLayout();
			// 
			// UScriptLayout
			// 
			resources.ApplyResources(UScriptLayout, "UScriptLayout");
			UScriptLayout.Controls.Add(this.Panel_Content, 1, 0);
			UScriptLayout.Controls.Add(this.Panel_Main);
			UScriptLayout.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
			UScriptLayout.Name = "UScriptLayout";
			// 
			// Panel_Content
			// 
			this.Panel_Content.Controls.Add(this.panel4);
			this.Panel_Content.Controls.Add(this.panel3);
			resources.ApplyResources(this.Panel_Content, "Panel_Content");
			this.Panel_Content.Name = "Panel_Content";
			// 
			// panel4
			// 
			resources.ApplyResources(this.panel4, "panel4");
			this.panel4.Controls.Add(this.WPFHost);
			this.panel4.Name = "panel4";
			this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
			// 
			// WPFHost
			// 
			resources.ApplyResources(this.WPFHost, "WPFHost");
			this.WPFHost.Name = "WPFHost";
			this.WPFHost.Child = this.TextEditorPanel;
			// 
			// panel3
			// 
			resources.ApplyResources(this.panel3, "panel3");
			this.panel3.Controls.Add(this.ToolStrip_Content);
			this.panel3.Name = "panel3";
			// 
			// ToolStrip_Content
			// 
			resources.ApplyResources(this.ToolStrip_Content, "ToolStrip_Content");
			this.ToolStrip_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.ToolStrip_Content.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip_Content.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrevButton,
            this.NextButton,
            this.ExportButton,
            this.toolStripSeparator1,
            this.SearchBox,
            this.FindButton,
            this.toolStripSeparator4,
            this.toolStripSeparator3,
            this.Label_ObjectName,
            this.ViewTools});
			this.ToolStrip_Content.Name = "ToolStrip_Content";
			this.ToolStrip_Content.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.ToolStrip_Content.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolStrip_Content_Paint);
			// 
			// PrevButton
			// 
			this.PrevButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.PrevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.PrevButton, "PrevButton");
			this.PrevButton.Name = "PrevButton";
			this.PrevButton.Click += new System.EventHandler(this.ToolStripButton_Backward_Click);
			// 
			// NextButton
			// 
			this.NextButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.NextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.NextButton, "NextButton");
			this.NextButton.Name = "NextButton";
			this.NextButton.Click += new System.EventHandler(this.ToolStripButton_Forward_Click);
			// 
			// ExportButton
			// 
			this.ExportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			resources.ApplyResources(this.ExportButton, "ExportButton");
			this.ExportButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
			this.ExportButton.Name = "ExportButton";
			this.ExportButton.Padding = new System.Windows.Forms.Padding(3);
			this.ExportButton.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripSeparator1
			// 
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripSeparator1_Paint);
			// 
			// SearchBox
			// 
			resources.ApplyResources(this.SearchBox, "SearchBox");
			this.SearchBox.Name = "SearchBox";
			this.SearchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchBox_KeyPress_1);
			// 
			// FindButton
			// 
			this.FindButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			resources.ApplyResources(this.FindButton, "FindButton");
			this.FindButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
			this.FindButton.Name = "FindButton";
			this.FindButton.Padding = new System.Windows.Forms.Padding(3);
			this.FindButton.Click += new System.EventHandler(this.ToolStripButton_Find_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.toolStripSeparator4.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripSeparator1_Paint);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripSeparator1_Paint);
			// 
			// Label_ObjectName
			// 
			this.Label_ObjectName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.Label_ObjectName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.Label_ObjectName.Name = "Label_ObjectName";
			resources.ApplyResources(this.Label_ObjectName, "Label_ObjectName");
			// 
			// ViewTools
			// 
			this.ViewTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			resources.ApplyResources(this.ViewTools, "ViewTools");
			this.ViewTools.Name = "ViewTools";
			this.ViewTools.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ViewTools_DropDownItemClicked);
			// 
			// Panel_Main
			// 
			this.Panel_Main.Controls.Add(this.panel1);
			this.Panel_Main.Controls.Add(this.panel2);
			resources.ApplyResources(this.Panel_Main, "Panel_Main");
			this.Panel_Main.Name = "Panel_Main";
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.TabControl_General);
			this.panel1.Name = "panel1";
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// TabControl_General
			// 
			resources.ApplyResources(this.TabControl_General, "TabControl_General");
			this.TabControl_General.Controls.Add(this.TabPage_Package);
			this.TabControl_General.Controls.Add(this.TabPage_Tables);
			this.TabControl_General.Controls.Add(this.TabPage_Objects);
			this.TabControl_General.Controls.Add(this.TabPage_Chunks);
			this.TabControl_General.HotTrack = true;
			this.TabControl_General.ImageList = this.VSIcons;
			this.TabControl_General.Name = "TabControl_General";
			this.TabControl_General.SelectedIndex = 0;
			this.TabControl_General.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControl_General_Selected);
			// 
			// TabPage_Package
			// 
			this.TabPage_Package.BackColor = System.Drawing.Color.White;
			this.TabPage_Package.CausesValidation = false;
			this.TabPage_Package.Controls.Add(this.Num_NameIndex);
			this.TabPage_Package.Controls.Add(this.Num_ObjectIndex);
			this.TabPage_Package.Controls.Add(this.FolderValue);
			this.TabPage_Package.Controls.Add(this.Button_FindObject);
			this.TabPage_Package.Controls.Add(this.Label_Folder);
			this.TabPage_Package.Controls.Add(this.Button_FindName);
			this.TabPage_Package.Controls.Add(this.BuildValue);
			this.TabPage_Package.Controls.Add(this.CookerValue);
			this.TabPage_Package.Controls.Add(this.EngineValue);
			this.TabPage_Package.Controls.Add(this.VersionValue);
			this.TabPage_Package.Controls.Add(this.FlagsValue);
			this.TabPage_Package.Controls.Add(this.LicenseeValue);
			this.TabPage_Package.Controls.Add(label4);
			this.TabPage_Package.Controls.Add(this.Label_GUID);
			this.TabPage_Package.Controls.Add(Label_DetectedBuild);
			this.TabPage_Package.Controls.Add(this.LABEL_Copyright);
			this.TabPage_Package.Controls.Add(this.LABEL_Author);
			this.TabPage_Package.Controls.Add(this.DataGridView_Flags);
			this.TabPage_Package.Controls.Add(this.Label_CookerVersion);
			this.TabPage_Package.Controls.Add(this.Label_EngineVersion);
			this.TabPage_Package.Controls.Add(Label_LicenseeMode);
			this.TabPage_Package.Controls.Add(Label_Flags);
			this.TabPage_Package.Controls.Add(Label_Version);
			this.TabPage_Package.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.TabPage_Package, "TabPage_Package");
			this.TabPage_Package.Name = "TabPage_Package";
			// 
			// Num_NameIndex
			// 
			resources.ApplyResources(this.Num_NameIndex, "Num_NameIndex");
			this.Num_NameIndex.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.Num_NameIndex.Name = "Num_NameIndex";
			// 
			// Num_ObjectIndex
			// 
			resources.ApplyResources(this.Num_ObjectIndex, "Num_ObjectIndex");
			this.Num_ObjectIndex.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.Num_ObjectIndex.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
			this.Num_ObjectIndex.Name = "Num_ObjectIndex";
			// 
			// FolderValue
			// 
			resources.ApplyResources(this.FolderValue, "FolderValue");
			this.FolderValue.CausesValidation = false;
			this.FolderValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.FolderValue.Name = "FolderValue";
			// 
			// Button_FindObject
			// 
			this.Button_FindObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			this.Button_FindObject.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
			this.Button_FindObject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.Button_FindObject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			resources.ApplyResources(this.Button_FindObject, "Button_FindObject");
			this.Button_FindObject.Name = "Button_FindObject";
			this.Button_FindObject.UseVisualStyleBackColor = false;
			// 
			// Label_Folder
			// 
			resources.ApplyResources(this.Label_Folder, "Label_Folder");
			this.Label_Folder.CausesValidation = false;
			this.Label_Folder.Name = "Label_Folder";
			// 
			// Button_FindName
			// 
			this.Button_FindName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			this.Button_FindName.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
			this.Button_FindName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.Button_FindName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			resources.ApplyResources(this.Button_FindName, "Button_FindName");
			this.Button_FindName.Name = "Button_FindName";
			this.Button_FindName.UseVisualStyleBackColor = false;
			// 
			// BuildValue
			// 
			resources.ApplyResources(this.BuildValue, "BuildValue");
			this.BuildValue.CausesValidation = false;
			this.BuildValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.BuildValue.Name = "BuildValue";
			// 
			// CookerValue
			// 
			this.CookerValue.CausesValidation = false;
			this.CookerValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			resources.ApplyResources(this.CookerValue, "CookerValue");
			this.CookerValue.Name = "CookerValue";
			// 
			// EngineValue
			// 
			this.EngineValue.CausesValidation = false;
			this.EngineValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			resources.ApplyResources(this.EngineValue, "EngineValue");
			this.EngineValue.Name = "EngineValue";
			// 
			// VersionValue
			// 
			this.VersionValue.CausesValidation = false;
			this.VersionValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			resources.ApplyResources(this.VersionValue, "VersionValue");
			this.VersionValue.Name = "VersionValue";
			// 
			// FlagsValue
			// 
			resources.ApplyResources(this.FlagsValue, "FlagsValue");
			this.FlagsValue.CausesValidation = false;
			this.FlagsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			this.FlagsValue.Name = "FlagsValue";
			// 
			// LicenseeValue
			// 
			this.LicenseeValue.CausesValidation = false;
			this.LicenseeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
			resources.ApplyResources(this.LicenseeValue, "LicenseeValue");
			this.LicenseeValue.Name = "LicenseeValue";
			// 
			// label4
			// 
			resources.ApplyResources(label4, "label4");
			label4.CausesValidation = false;
			label4.Name = "label4";
			// 
			// Label_GUID
			// 
			resources.ApplyResources(this.Label_GUID, "Label_GUID");
			this.Label_GUID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			this.Label_GUID.Name = "Label_GUID";
			this.Label_GUID.ReadOnly = true;
			// 
			// Label_DetectedBuild
			// 
			resources.ApplyResources(Label_DetectedBuild, "Label_DetectedBuild");
			Label_DetectedBuild.CausesValidation = false;
			Label_DetectedBuild.Name = "Label_DetectedBuild";
			// 
			// LABEL_Copyright
			// 
			resources.ApplyResources(this.LABEL_Copyright, "LABEL_Copyright");
			this.LABEL_Copyright.CausesValidation = false;
			this.LABEL_Copyright.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
			this.LABEL_Copyright.Name = "LABEL_Copyright";
			// 
			// LABEL_Author
			// 
			resources.ApplyResources(this.LABEL_Author, "LABEL_Author");
			this.LABEL_Author.CausesValidation = false;
			this.LABEL_Author.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
			this.LABEL_Author.Name = "LABEL_Author";
			// 
			// DataGridView_Flags
			// 
			this.DataGridView_Flags.AllowUserToAddRows = false;
			this.DataGridView_Flags.AllowUserToDeleteRows = false;
			this.DataGridView_Flags.AllowUserToResizeColumns = false;
			this.DataGridView_Flags.AllowUserToResizeRows = false;
			resources.ApplyResources(this.DataGridView_Flags, "DataGridView_Flags");
			this.DataGridView_Flags.BackgroundColor = System.Drawing.Color.White;
			this.DataGridView_Flags.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataGridView_Flags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataGridView_Flags.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Flag,
            this.Value});
			this.DataGridView_Flags.EnableHeadersVisualStyles = false;
			this.DataGridView_Flags.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			this.DataGridView_Flags.MultiSelect = false;
			this.DataGridView_Flags.Name = "DataGridView_Flags";
			this.DataGridView_Flags.ReadOnly = true;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.DataGridView_Flags.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.DataGridView_Flags.RowHeadersVisible = false;
			this.DataGridView_Flags.ShowCellErrors = false;
			this.DataGridView_Flags.ShowEditingIcon = false;
			this.DataGridView_Flags.ShowRowErrors = false;
			// 
			// Flag
			// 
			this.Flag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Flag.FillWeight = 50F;
			resources.ApplyResources(this.Flag, "Flag");
			this.Flag.Name = "Flag";
			this.Flag.ReadOnly = true;
			this.Flag.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Flag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			// 
			// Value
			// 
			this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Value.FillWeight = 40F;
			resources.ApplyResources(this.Value, "Value");
			this.Value.Name = "Value";
			this.Value.ReadOnly = true;
			this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			// 
			// Label_CookerVersion
			// 
			resources.ApplyResources(this.Label_CookerVersion, "Label_CookerVersion");
			this.Label_CookerVersion.CausesValidation = false;
			this.Label_CookerVersion.Name = "Label_CookerVersion";
			// 
			// Label_EngineVersion
			// 
			resources.ApplyResources(this.Label_EngineVersion, "Label_EngineVersion");
			this.Label_EngineVersion.CausesValidation = false;
			this.Label_EngineVersion.Name = "Label_EngineVersion";
			// 
			// Label_LicenseeMode
			// 
			Label_LicenseeMode.CausesValidation = false;
			resources.ApplyResources(Label_LicenseeMode, "Label_LicenseeMode");
			Label_LicenseeMode.Name = "Label_LicenseeMode";
			// 
			// Label_Flags
			// 
			resources.ApplyResources(Label_Flags, "Label_Flags");
			Label_Flags.CausesValidation = false;
			Label_Flags.Name = "Label_Flags";
			// 
			// Label_Version
			// 
			Label_Version.CausesValidation = false;
			resources.ApplyResources(Label_Version, "Label_Version");
			Label_Version.Name = "Label_Version";
			// 
			// TabPage_Tables
			// 
			this.TabPage_Tables.Controls.Add(this.TabControl_Tables);
			resources.ApplyResources(this.TabPage_Tables, "TabPage_Tables");
			this.TabPage_Tables.Name = "TabPage_Tables";
			this.TabPage_Tables.UseVisualStyleBackColor = true;
			// 
			// TabControl_Tables
			// 
			this.TabControl_Tables.Controls.Add(this.TabPage_Names);
			this.TabControl_Tables.Controls.Add(this.TabPage_Exports);
			this.TabControl_Tables.Controls.Add(this.TabPage_Imports);
			this.TabControl_Tables.Controls.Add(this.TabPage_Generations);
			resources.ApplyResources(this.TabControl_Tables, "TabControl_Tables");
			this.TabControl_Tables.ImageList = this.VSIcons;
			this.TabControl_Tables.Name = "TabControl_Tables";
			this.TabControl_Tables.SelectedIndex = 0;
			this.TabControl_Tables.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControl_Tables_Selected);
			// 
			// TabPage_Names
			// 
			this.TabPage_Names.BackColor = System.Drawing.Color.White;
			this.TabPage_Names.Controls.Add(this.DataGridView_NameTable);
			resources.ApplyResources(this.TabPage_Names, "TabPage_Names");
			this.TabPage_Names.Name = "TabPage_Names";
			// 
			// DataGridView_NameTable
			// 
			this.DataGridView_NameTable.AllowUserToAddRows = false;
			this.DataGridView_NameTable.AllowUserToDeleteRows = false;
			this.DataGridView_NameTable.AllowUserToOrderColumns = true;
			this.DataGridView_NameTable.AllowUserToResizeRows = false;
			resources.ApplyResources(this.DataGridView_NameTable, "DataGridView_NameTable");
			this.DataGridView_NameTable.BackgroundColor = System.Drawing.Color.White;
			this.DataGridView_NameTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataGridView_NameTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataGridView_NameTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.DataGridView_NameTable.EnableHeadersVisualStyles = false;
			this.DataGridView_NameTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			this.DataGridView_NameTable.MultiSelect = false;
			this.DataGridView_NameTable.Name = "DataGridView_NameTable";
			this.DataGridView_NameTable.ReadOnly = true;
			this.DataGridView_NameTable.RowHeadersVisible = false;
			this.DataGridView_NameTable.ShowCellErrors = false;
			this.DataGridView_NameTable.ShowEditingIcon = false;
			this.DataGridView_NameTable.ShowRowErrors = false;
			// 
			// Column1
			// 
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			// 
			// Column2
			// 
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			// 
			// TabPage_Exports
			// 
			this.TabPage_Exports.BackColor = System.Drawing.Color.White;
			this.TabPage_Exports.Controls.Add(this.filterPanel);
			this.TabPage_Exports.Controls.Add(this.TreeView_Exports);
			resources.ApplyResources(this.TabPage_Exports, "TabPage_Exports");
			this.TabPage_Exports.Name = "TabPage_Exports";
			// 
			// filterPanel
			// 
			this.filterPanel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.filterPanel.Controls.Add(this.checkBox9);
			this.filterPanel.Controls.Add(this.checkBox8);
			this.filterPanel.Controls.Add(this.checkBox7);
			this.filterPanel.Controls.Add(this.checkBox6);
			this.filterPanel.Controls.Add(this.checkBox5);
			this.filterPanel.Controls.Add(this.checkBox4);
			this.filterPanel.Controls.Add(this.checkBox3);
			this.filterPanel.Controls.Add(this.checkBox2);
			this.filterPanel.Controls.Add(this.checkBox1);
			resources.ApplyResources(this.filterPanel, "filterPanel");
			this.filterPanel.Name = "filterPanel";
			// 
			// checkBox9
			// 
			resources.ApplyResources(this.checkBox9, "checkBox9");
			this.checkBox9.Checked = true;
			this.checkBox9.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox9.ImageList = this.VSIcons;
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.UseVisualStyleBackColor = false;
			this.checkBox9.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// VSIcons
			// 
			this.VSIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("VSIcons.ImageStream")));
			this.VSIcons.TransparentColor = System.Drawing.Color.Fuchsia;
			this.VSIcons.Images.SetKeyName(0, "Unknown");
			this.VSIcons.Images.SetKeyName(1, "UClass");
			this.VSIcons.Images.SetKeyName(2, "UObject");
			this.VSIcons.Images.SetKeyName(3, "UConst");
			this.VSIcons.Images.SetKeyName(4, "UEnum");
			this.VSIcons.Images.SetKeyName(5, "UStruct");
			this.VSIcons.Images.SetKeyName(6, "UProperty");
			this.VSIcons.Images.SetKeyName(7, "UFunction");
			this.VSIcons.Images.SetKeyName(8, "UState");
			this.VSIcons.Images.SetKeyName(9, "List");
			this.VSIcons.Images.SetKeyName(10, "text");
			this.VSIcons.Images.SetKeyName(11, "tree");
			this.VSIcons.Images.SetKeyName(12, "object");
			this.VSIcons.Images.SetKeyName(13, "multiple");
			this.VSIcons.Images.SetKeyName(14, "import");
			this.VSIcons.Images.SetKeyName(15, "export");
			this.VSIcons.Images.SetKeyName(16, "text-left");
			// 
			// checkBox8
			// 
			resources.ApplyResources(this.checkBox8, "checkBox8");
			this.checkBox8.Checked = true;
			this.checkBox8.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox8.ImageList = this.VSIcons;
			this.checkBox8.Name = "checkBox8";
			this.checkBox8.UseVisualStyleBackColor = false;
			this.checkBox8.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox7
			// 
			resources.ApplyResources(this.checkBox7, "checkBox7");
			this.checkBox7.Checked = true;
			this.checkBox7.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox7.ImageList = this.VSIcons;
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.UseVisualStyleBackColor = false;
			this.checkBox7.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox6
			// 
			resources.ApplyResources(this.checkBox6, "checkBox6");
			this.checkBox6.Checked = true;
			this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox6.ImageList = this.VSIcons;
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.UseVisualStyleBackColor = false;
			this.checkBox6.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox5
			// 
			resources.ApplyResources(this.checkBox5, "checkBox5");
			this.checkBox5.Checked = true;
			this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox5.ImageList = this.VSIcons;
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.UseVisualStyleBackColor = false;
			this.checkBox5.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox4
			// 
			resources.ApplyResources(this.checkBox4, "checkBox4");
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox4.ImageList = this.VSIcons;
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.UseVisualStyleBackColor = false;
			this.checkBox4.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox3
			// 
			resources.ApplyResources(this.checkBox3, "checkBox3");
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.ImageList = this.VSIcons;
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.UseVisualStyleBackColor = false;
			this.checkBox3.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox2
			// 
			resources.ApplyResources(this.checkBox2, "checkBox2");
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.ImageList = this.VSIcons;
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.UseVisualStyleBackColor = false;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// checkBox1
			// 
			resources.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.ImageList = this.VSIcons;
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = false;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.FilterByClassCheckBox);
			// 
			// TreeView_Exports
			// 
			resources.ApplyResources(this.TreeView_Exports, "TreeView_Exports");
			this.TreeView_Exports.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Exports.CausesValidation = false;
			this.TreeView_Exports.HideSelection = false;
			this.TreeView_Exports.ImageList = this.VSIcons;
			this.TreeView_Exports.Name = "TreeView_Exports";
			this.TreeView_Exports.ShowNodeToolTips = true;
			this.TreeView_Exports.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._OnExportsNodeSelected);
			this.TreeView_Exports.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_Exports_NodeMouseClick);
			// 
			// TabPage_Imports
			// 
			this.TabPage_Imports.BackColor = System.Drawing.Color.White;
			this.TabPage_Imports.Controls.Add(this.TreeView_Imports);
			resources.ApplyResources(this.TabPage_Imports, "TabPage_Imports");
			this.TabPage_Imports.Name = "TabPage_Imports";
			// 
			// TreeView_Imports
			// 
			resources.ApplyResources(this.TreeView_Imports, "TreeView_Imports");
			this.TreeView_Imports.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Imports.CausesValidation = false;
			this.TreeView_Imports.HideSelection = false;
			this.TreeView_Imports.ImageList = this.VSIcons;
			this.TreeView_Imports.Name = "TreeView_Imports";
			this.TreeView_Imports.ShowNodeToolTips = true;
			// 
			// TabPage_Generations
			// 
			this.TabPage_Generations.BackColor = System.Drawing.Color.White;
			this.TabPage_Generations.Controls.Add(this.DataGridView_GenerationsTable);
			resources.ApplyResources(this.TabPage_Generations, "TabPage_Generations");
			this.TabPage_Generations.Name = "TabPage_Generations";
			// 
			// DataGridView_GenerationsTable
			// 
			this.DataGridView_GenerationsTable.AllowUserToAddRows = false;
			this.DataGridView_GenerationsTable.AllowUserToDeleteRows = false;
			this.DataGridView_GenerationsTable.AllowUserToOrderColumns = true;
			this.DataGridView_GenerationsTable.AllowUserToResizeRows = false;
			resources.ApplyResources(this.DataGridView_GenerationsTable, "DataGridView_GenerationsTable");
			this.DataGridView_GenerationsTable.BackgroundColor = System.Drawing.Color.White;
			this.DataGridView_GenerationsTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataGridView_GenerationsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataGridView_GenerationsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column4});
			this.DataGridView_GenerationsTable.EnableHeadersVisualStyles = false;
			this.DataGridView_GenerationsTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			this.DataGridView_GenerationsTable.MultiSelect = false;
			this.DataGridView_GenerationsTable.Name = "DataGridView_GenerationsTable";
			this.DataGridView_GenerationsTable.ReadOnly = true;
			this.DataGridView_GenerationsTable.RowHeadersVisible = false;
			this.DataGridView_GenerationsTable.ShowCellErrors = false;
			this.DataGridView_GenerationsTable.ShowEditingIcon = false;
			this.DataGridView_GenerationsTable.ShowRowErrors = false;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// Column4
			// 
			resources.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			// 
			// TabPage_Objects
			// 
			this.TabPage_Objects.Controls.Add(this.TabControl_Objects);
			resources.ApplyResources(this.TabPage_Objects, "TabPage_Objects");
			this.TabPage_Objects.Name = "TabPage_Objects";
			this.TabPage_Objects.UseVisualStyleBackColor = true;
			// 
			// TabControl_Objects
			// 
			this.TabControl_Objects.Controls.Add(this.TabPage_Classes);
			this.TabControl_Objects.Controls.Add(this.TabPage_Content);
			this.TabControl_Objects.Controls.Add(this.TabPage_Deps);
			resources.ApplyResources(this.TabControl_Objects, "TabControl_Objects");
			this.TabControl_Objects.ImageList = this.VSIcons;
			this.TabControl_Objects.Name = "TabControl_Objects";
			this.TabControl_Objects.SelectedIndex = 0;
			// 
			// TabPage_Classes
			// 
			this.TabPage_Classes.BackColor = System.Drawing.Color.White;
			this.TabPage_Classes.Controls.Add(this._SearchIcon);
			this.TabPage_Classes.Controls.Add(this.TreeView_Classes);
			this.TabPage_Classes.Controls.Add(this.FilterText);
			resources.ApplyResources(this.TabPage_Classes, "TabPage_Classes");
			this.TabPage_Classes.Name = "TabPage_Classes";
			// 
			// _SearchIcon
			// 
			resources.ApplyResources(this._SearchIcon, "_SearchIcon");
			this._SearchIcon.BackgroundImage = global::UEExplorer.Properties.Resources.search;
			this._SearchIcon.Name = "_SearchIcon";
			this._SearchIcon.TabStop = false;
			// 
			// TreeView_Classes
			// 
			resources.ApplyResources(this.TreeView_Classes, "TreeView_Classes");
			this.TreeView_Classes.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Classes.HideSelection = false;
			this.TreeView_Classes.ImageList = this.VSIcons;
			this.TreeView_Classes.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			this.TreeView_Classes.Name = "TreeView_Classes";
			this.TreeView_Classes.ShowNodeToolTips = true;
			this.TreeView_Classes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_Classes_NodeMouseClick);
			// 
			// FilterText
			// 
			resources.ApplyResources(this.FilterText, "FilterText");
			this.FilterText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
			this.FilterText.Name = "FilterText";
			this.FilterText.TextChanged += new System.EventHandler(this.FilterText_TextChanged);
			// 
			// TabPage_Content
			// 
			this.TabPage_Content.BackColor = System.Drawing.Color.White;
			this.TabPage_Content.Controls.Add(this.Button_Export);
			this.TabPage_Content.Controls.Add(this.TreeView_Content);
			resources.ApplyResources(this.TabPage_Content, "TabPage_Content");
			this.TabPage_Content.Name = "TabPage_Content";
			// 
			// Button_Export
			// 
			resources.ApplyResources(this.Button_Export, "Button_Export");
			this.Button_Export.Name = "Button_Export";
			this.Button_Export.UseVisualStyleBackColor = true;
			this.Button_Export.Click += new System.EventHandler(this.Button_Export_Click);
			// 
			// TreeView_Content
			// 
			resources.ApplyResources(this.TreeView_Content, "TreeView_Content");
			this.TreeView_Content.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Content.HideSelection = false;
			this.TreeView_Content.Name = "TreeView_Content";
			this.TreeView_Content.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Content_AfterSelect);
			this.TreeView_Content.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_Content_NodeMouseClick);
			// 
			// TabPage_Deps
			// 
			this.TabPage_Deps.BackColor = System.Drawing.Color.White;
			this.TabPage_Deps.Controls.Add(this.TreeView_Deps);
			resources.ApplyResources(this.TabPage_Deps, "TabPage_Deps");
			this.TabPage_Deps.Name = "TabPage_Deps";
			// 
			// TreeView_Deps
			// 
			resources.ApplyResources(this.TreeView_Deps, "TreeView_Deps");
			this.TreeView_Deps.BackColor = System.Drawing.Color.White;
			this.TreeView_Deps.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Deps.HideSelection = false;
			this.TreeView_Deps.ImageList = this.VSIcons;
			this.TreeView_Deps.Name = "TreeView_Deps";
			this.TreeView_Deps.ShowNodeToolTips = true;
			this.TreeView_Deps.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.TreeView_Deps_DrawNode);
			// 
			// TabPage_Chunks
			// 
			this.TabPage_Chunks.BackColor = System.Drawing.Color.White;
			this.TabPage_Chunks.Controls.Add(this.DataGridView_Chunks);
			resources.ApplyResources(this.TabPage_Chunks, "TabPage_Chunks");
			this.TabPage_Chunks.Name = "TabPage_Chunks";
			// 
			// DataGridView_Chunks
			// 
			this.DataGridView_Chunks.AllowUserToAddRows = false;
			this.DataGridView_Chunks.AllowUserToDeleteRows = false;
			this.DataGridView_Chunks.AllowUserToOrderColumns = true;
			this.DataGridView_Chunks.AllowUserToResizeRows = false;
			this.DataGridView_Chunks.BackgroundColor = System.Drawing.Color.White;
			this.DataGridView_Chunks.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DataGridView_Chunks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.DataGridView_Chunks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.CompressedOffset,
            this.CompressedSize});
			resources.ApplyResources(this.DataGridView_Chunks, "DataGridView_Chunks");
			this.DataGridView_Chunks.EnableHeadersVisualStyles = false;
			this.DataGridView_Chunks.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			this.DataGridView_Chunks.MultiSelect = false;
			this.DataGridView_Chunks.Name = "DataGridView_Chunks";
			this.DataGridView_Chunks.ReadOnly = true;
			this.DataGridView_Chunks.RowHeadersVisible = false;
			this.DataGridView_Chunks.ShowCellErrors = false;
			this.DataGridView_Chunks.ShowEditingIcon = false;
			this.DataGridView_Chunks.ShowRowErrors = false;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			// 
			// CompressedOffset
			// 
			this.CompressedOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.CompressedOffset, "CompressedOffset");
			this.CompressedOffset.Name = "CompressedOffset";
			this.CompressedOffset.ReadOnly = true;
			// 
			// CompressedSize
			// 
			this.CompressedSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.CompressedSize, "CompressedSize");
			this.CompressedSize.Name = "CompressedSize";
			this.CompressedSize.ReadOnly = true;
			// 
			// panel2
			// 
			resources.ApplyResources(this.panel2, "panel2");
			this.panel2.Controls.Add(this.ToolStrip_Main);
			this.panel2.Name = "panel2";
			// 
			// ToolStrip_Main
			// 
			this.ToolStrip_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			resources.ApplyResources(this.ToolStrip_Main, "ToolStrip_Main");
			this.ToolStrip_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._Tools_StripDropDownButton,
            this.ReloadButton});
			this.ToolStrip_Main.Name = "ToolStrip_Main";
			this.ToolStrip_Main.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.ToolStrip_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolStrip_Content_Paint);
			// 
			// _Tools_StripDropDownButton
			// 
			this._Tools_StripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._Tools_StripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            exportingToolStripMenuItem,
            this.viewBufferToolStripMenuItem});
			this._Tools_StripDropDownButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this._Tools_StripDropDownButton.Name = "_Tools_StripDropDownButton";
			this._Tools_StripDropDownButton.Padding = new System.Windows.Forms.Padding(3);
			resources.ApplyResources(this._Tools_StripDropDownButton, "_Tools_StripDropDownButton");
			// 
			// exportingToolStripMenuItem
			// 
			exportingToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			exportingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportDecompiledClassesToolStripMenuItem,
            this.exportScriptClassesToolStripMenuItem});
			resources.ApplyResources(exportingToolStripMenuItem, "exportingToolStripMenuItem");
			exportingToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			exportingToolStripMenuItem.Name = "exportingToolStripMenuItem";
			// 
			// exportDecompiledClassesToolStripMenuItem
			// 
			resources.ApplyResources(this.exportDecompiledClassesToolStripMenuItem, "exportDecompiledClassesToolStripMenuItem");
			this.exportDecompiledClassesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.exportDecompiledClassesToolStripMenuItem.Name = "exportDecompiledClassesToolStripMenuItem";
			// 
			// exportScriptClassesToolStripMenuItem
			// 
			resources.ApplyResources(this.exportScriptClassesToolStripMenuItem, "exportScriptClassesToolStripMenuItem");
			this.exportScriptClassesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.exportScriptClassesToolStripMenuItem.Name = "exportScriptClassesToolStripMenuItem";
			// 
			// viewBufferToolStripMenuItem
			// 
			resources.ApplyResources(this.viewBufferToolStripMenuItem, "viewBufferToolStripMenuItem");
			this.viewBufferToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.viewBufferToolStripMenuItem.Name = "viewBufferToolStripMenuItem";
			this.viewBufferToolStripMenuItem.Click += new System.EventHandler(this.viewBufferToolStripMenuItem_Click);
			// 
			// ReloadButton
			// 
			this.ReloadButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.ReloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.ReloadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
			this.ReloadButton.Name = "ReloadButton";
			this.ReloadButton.Padding = new System.Windows.Forms.Padding(3);
			resources.ApplyResources(this.ReloadButton, "ReloadButton");
			this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
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
			// UC_PackageExplorer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(UScriptLayout);
			this.DoubleBuffered = true;
			this.Name = "UC_PackageExplorer";
			UScriptLayout.ResumeLayout(false);
			this.Panel_Content.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ToolStrip_Content.ResumeLayout(false);
			this.ToolStrip_Content.PerformLayout();
			this.Panel_Main.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.TabControl_General.ResumeLayout(false);
			this.TabPage_Package.ResumeLayout(false);
			this.TabPage_Package.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Num_NameIndex)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Num_ObjectIndex)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_Flags)).EndInit();
			this.TabPage_Tables.ResumeLayout(false);
			this.TabControl_Tables.ResumeLayout(false);
			this.TabPage_Names.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_NameTable)).EndInit();
			this.TabPage_Exports.ResumeLayout(false);
			this.filterPanel.ResumeLayout(false);
			this.TabPage_Imports.ResumeLayout(false);
			this.TabPage_Generations.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_GenerationsTable)).EndInit();
			this.TabPage_Objects.ResumeLayout(false);
			this.TabControl_Objects.ResumeLayout(false);
			this.TabPage_Classes.ResumeLayout(false);
			this.TabPage_Classes.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._SearchIcon)).EndInit();
			this.TabPage_Content.ResumeLayout(false);
			this.TabPage_Deps.ResumeLayout(false);
			this.TabPage_Chunks.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DataGridView_Chunks)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ToolStrip_Main.ResumeLayout(false);
			this.ToolStrip_Main.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.TabControl TabControl_General;
		internal System.Windows.Forms.TabPage TabPage_Package;
		internal System.Windows.Forms.TabPage TabPage_Tables;
		internal System.Windows.Forms.TabControl TabControl_Tables;
		internal System.Windows.Forms.TabPage TabPage_Names;
		internal System.Windows.Forms.TabPage TabPage_Exports;
		internal System.Windows.Forms.TabPage TabPage_Imports;
		internal System.Windows.Forms.TreeView TreeView_Exports;
		internal System.Windows.Forms.TreeView TreeView_Imports;
		internal System.Windows.Forms.Button Button_FindObject;
		internal System.Windows.Forms.NumericUpDown Num_ObjectIndex;
		internal System.Windows.Forms.NumericUpDown Num_NameIndex;
		internal System.Windows.Forms.Button Button_FindName;
		internal System.Windows.Forms.ToolStrip ToolStrip_Content;
		private System.Windows.Forms.ToolStripButton ExportButton;
		internal System.Windows.Forms.ToolStripLabel Label_ObjectName;
		internal System.Windows.Forms.Panel Panel_Content;
		internal System.Windows.Forms.DataGridView DataGridView_Flags;
		private System.Windows.Forms.Panel Panel_Main;
		private System.Windows.Forms.ToolStrip ToolStrip_Main;
		private System.Windows.Forms.ToolStripDropDownButton _Tools_StripDropDownButton;
		public System.Windows.Forms.ToolStripMenuItem exportDecompiledClassesToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem exportScriptClassesToolStripMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn Flag;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		public System.Windows.Forms.ImageList VSIcons;
		internal System.Windows.Forms.TreeView TreeView_Classes;
		internal System.Windows.Forms.TreeView TreeView_Content;
		internal System.Windows.Forms.TabPage TabPage_Objects;
		internal System.Windows.Forms.TabControl TabControl_Objects;
		internal System.Windows.Forms.TabPage TabPage_Classes;
		internal System.Windows.Forms.TabPage TabPage_Content;
		internal System.Windows.Forms.TabPage TabPage_Deps;
		internal System.Windows.Forms.TreeView TreeView_Deps;
		private System.Windows.Forms.DataGridView DataGridView_NameTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.ToolStripButton FindButton;
		private System.Windows.Forms.ToolStripButton NextButton;
		private System.Windows.Forms.ToolStripButton PrevButton;
		private System.Windows.Forms.Integration.ElementHost WPFHost;
		private TextEditorPanel TextEditorPanel;
		internal System.Windows.Forms.TabPage TabPage_Generations;
		private System.Windows.Forms.DataGridView DataGridView_GenerationsTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.ToolStripMenuItem viewBufferToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox SearchBox;
		internal System.Windows.Forms.TabPage TabPage_Chunks;
		private System.Windows.Forms.DataGridView DataGridView_Chunks;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn CompressedOffset;
		private System.Windows.Forms.DataGridViewTextBoxColumn CompressedSize;
		internal System.Windows.Forms.Label LABEL_Copyright;
		internal System.Windows.Forms.Label LABEL_Author;
		private System.Windows.Forms.TextBox FilterText;
		private System.Windows.Forms.ToolStripButton ReloadButton;
		private System.Windows.Forms.TextBox Label_GUID;
		internal System.Windows.Forms.Label VersionValue;
		internal System.Windows.Forms.Label FlagsValue;
		internal System.Windows.Forms.Label LicenseeValue;
		internal System.Windows.Forms.Label BuildValue;
		internal System.Windows.Forms.Label CookerValue;
		internal System.Windows.Forms.Label EngineValue;
		internal System.Windows.Forms.Label FolderValue;
		internal System.Windows.Forms.Label Label_Folder;
		internal System.Windows.Forms.Label Label_CookerVersion;
		internal System.Windows.Forms.Label Label_EngineVersion;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.PictureBox _SearchIcon;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel filterPanel;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.CheckBox checkBox8;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox3;
		internal System.Windows.Forms.Button Button_Export;
		public System.Windows.Forms.ToolStripDropDownButton ViewTools;
	}
}
