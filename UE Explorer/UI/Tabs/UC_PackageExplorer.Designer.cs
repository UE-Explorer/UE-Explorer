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
		protected override void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem exportingToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_PackageExplorer));
            this.exportDecompiledClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportScriptClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VSIcons = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TabControl_General = new System.Windows.Forms.TabControl();
            this.TabPage_Package = new System.Windows.Forms.TabPage();
            this.FilterText = new System.Windows.Forms.TextBox();
            this.TreeView_Content = new System.Windows.Forms.TreeView();
            this.TabPage_Tables = new System.Windows.Forms.TabPage();
            this.TabControl_Tables = new System.Windows.Forms.TabControl();
            this.TabPage_Generations = new System.Windows.Forms.TabPage();
            this.DataGridView_GenerationsTable = new System.Windows.Forms.DataGridView();
            this.exportCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netObjectCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uGenerationTableItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TabPage_Chunks = new System.Windows.Forms.TabPage();
            this.DataGridView_Chunks = new System.Windows.Forms.DataGridView();
            this.uncompressedOffsetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uncompressedSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.compressedOffsetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.compressedSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.compressedChunkBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TabPage_Names = new System.Windows.Forms.TabPage();
            this.nameDataGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flagsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uNameTableItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TabPage_Imports = new System.Windows.Forms.TabPage();
            this.importsDataGridView = new System.Windows.Forms.DataGridView();
            this.packageNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objectNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outerIndexDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uImportTableItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TreeView_Imports = new System.Windows.Forms.TreeView();
            this.TabPage_Exports = new System.Windows.Forms.TabPage();
            this.exportsDataGridView = new System.Windows.Forms.DataGridView();
            this.classIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.superIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.superDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.templateIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.templateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.archetypeIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.archetypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objectNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outerIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outerDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uExportTableItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TreeView_Exports = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            this._Tools_StripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.viewBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel_Content = new System.Windows.Forms.Panel();
            this.Header = new System.Windows.Forms.Panel();
            this.ToolStrip_Content = new System.Windows.Forms.ToolStrip();
            this.ExportButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SearchBox = new System.Windows.Forms.ToolStripTextBox();
            this.FindButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ViewTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.BinaryDataPanel = new System.Windows.Forms.Panel();
            this.binaryDataFieldsPanel = new UEExplorer.UI.Panels.BinaryDataFieldsPanel();
            this.TextContentPanel = new System.Windows.Forms.Panel();
            this.textEditorPanel = new UEExplorer.UI.Panels.TextEditorPanel();
            this.packageFileSummaryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.Label_ObjectName = new System.Windows.Forms.ToolStripTextBox();
            this.NextButton = new System.Windows.Forms.ToolStripButton();
            this.PrevButton = new System.Windows.Forms.ToolStripButton();
            exportingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.Panel_Main.SuspendLayout();
            this.panel1.SuspendLayout();
            this.TabControl_General.SuspendLayout();
            this.TabPage_Package.SuspendLayout();
            this.TabPage_Tables.SuspendLayout();
            this.TabControl_Tables.SuspendLayout();
            this.TabPage_Generations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_GenerationsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uGenerationTableItemBindingSource)).BeginInit();
            this.TabPage_Chunks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Chunks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressedChunkBindingSource)).BeginInit();
            this.TabPage_Names.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nameDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uNameTableItemBindingSource)).BeginInit();
            this.TabPage_Imports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.importsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uImportTableItemBindingSource)).BeginInit();
            this.TabPage_Exports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uExportTableItemBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            this.ToolStrip_Main.SuspendLayout();
            this.Panel_Content.SuspendLayout();
            this.Header.SuspendLayout();
            this.ToolStrip_Content.SuspendLayout();
            this.BinaryDataPanel.SuspendLayout();
            this.TextContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packageFileSummaryBindingSource)).BeginInit();
            this.SuspendLayout();
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
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Panel_Main);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Panel_Content);
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer1_SplitterMoved);
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
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel4_Paint);
            // 
            // TabControl_General
            // 
            resources.ApplyResources(this.TabControl_General, "TabControl_General");
            this.TabControl_General.Controls.Add(this.TabPage_Package);
            this.TabControl_General.Controls.Add(this.TabPage_Tables);
            this.TabControl_General.HotTrack = true;
            this.TabControl_General.ImageList = this.VSIcons;
            this.TabControl_General.Name = "TabControl_General";
            this.TabControl_General.SelectedIndex = 0;
            // 
            // TabPage_Package
            // 
            this.TabPage_Package.Controls.Add(this.FilterText);
            this.TabPage_Package.Controls.Add(this.TreeView_Content);
            resources.ApplyResources(this.TabPage_Package, "TabPage_Package");
            this.TabPage_Package.Name = "TabPage_Package";
            this.TabPage_Package.UseVisualStyleBackColor = true;
            // 
            // FilterText
            // 
            resources.ApplyResources(this.FilterText, "FilterText");
            this.FilterText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilterText.Name = "FilterText";
            this.FilterText.TextChanged += new System.EventHandler(this.FilterText_TextChanged);
            // 
            // TreeView_Content
            // 
            resources.ApplyResources(this.TreeView_Content, "TreeView_Content");
            this.TreeView_Content.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TreeView_Content.HideSelection = false;
            this.TreeView_Content.ImageList = this.VSIcons;
            this.TreeView_Content.Name = "TreeView_Content";
            this.TreeView_Content.ShowNodeToolTips = true;
            this.TreeView_Content.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView_Content_BeforeExpand);
            this.TreeView_Content.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Content_AfterSelect);
            this.TreeView_Content.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_Content_NodeMouseClick);
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
            this.TabControl_Tables.Controls.Add(this.TabPage_Generations);
            this.TabControl_Tables.Controls.Add(this.TabPage_Chunks);
            this.TabControl_Tables.Controls.Add(this.TabPage_Names);
            this.TabControl_Tables.Controls.Add(this.TabPage_Imports);
            this.TabControl_Tables.Controls.Add(this.TabPage_Exports);
            resources.ApplyResources(this.TabControl_Tables, "TabControl_Tables");
            this.TabControl_Tables.ImageList = this.VSIcons;
            this.TabControl_Tables.Name = "TabControl_Tables";
            this.TabControl_Tables.SelectedIndex = 0;
            // 
            // TabPage_Generations
            // 
            this.TabPage_Generations.Controls.Add(this.DataGridView_GenerationsTable);
            resources.ApplyResources(this.TabPage_Generations, "TabPage_Generations");
            this.TabPage_Generations.Name = "TabPage_Generations";
            this.TabPage_Generations.UseVisualStyleBackColor = true;
            // 
            // DataGridView_GenerationsTable
            // 
            this.DataGridView_GenerationsTable.AutoGenerateColumns = false;
            this.DataGridView_GenerationsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView_GenerationsTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView_GenerationsTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridView_GenerationsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.exportCountDataGridViewTextBoxColumn,
            this.nameCountDataGridViewTextBoxColumn,
            this.netObjectCountDataGridViewTextBoxColumn});
            this.DataGridView_GenerationsTable.DataSource = this.uGenerationTableItemBindingSource;
            resources.ApplyResources(this.DataGridView_GenerationsTable, "DataGridView_GenerationsTable");
            this.DataGridView_GenerationsTable.Name = "DataGridView_GenerationsTable";
            this.DataGridView_GenerationsTable.ReadOnly = true;
            this.DataGridView_GenerationsTable.RowHeadersVisible = false;
            // 
            // exportCountDataGridViewTextBoxColumn
            // 
            this.exportCountDataGridViewTextBoxColumn.DataPropertyName = "ExportCount";
            resources.ApplyResources(this.exportCountDataGridViewTextBoxColumn, "exportCountDataGridViewTextBoxColumn");
            this.exportCountDataGridViewTextBoxColumn.Name = "exportCountDataGridViewTextBoxColumn";
            this.exportCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameCountDataGridViewTextBoxColumn
            // 
            this.nameCountDataGridViewTextBoxColumn.DataPropertyName = "NameCount";
            resources.ApplyResources(this.nameCountDataGridViewTextBoxColumn, "nameCountDataGridViewTextBoxColumn");
            this.nameCountDataGridViewTextBoxColumn.Name = "nameCountDataGridViewTextBoxColumn";
            this.nameCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // netObjectCountDataGridViewTextBoxColumn
            // 
            this.netObjectCountDataGridViewTextBoxColumn.DataPropertyName = "NetObjectCount";
            resources.ApplyResources(this.netObjectCountDataGridViewTextBoxColumn, "netObjectCountDataGridViewTextBoxColumn");
            this.netObjectCountDataGridViewTextBoxColumn.Name = "netObjectCountDataGridViewTextBoxColumn";
            this.netObjectCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uGenerationTableItemBindingSource
            // 
            this.uGenerationTableItemBindingSource.DataSource = typeof(UELib.UGenerationTableItem);
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
            this.DataGridView_Chunks.AutoGenerateColumns = false;
            this.DataGridView_Chunks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView_Chunks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView_Chunks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridView_Chunks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uncompressedOffsetDataGridViewTextBoxColumn,
            this.uncompressedSizeDataGridViewTextBoxColumn,
            this.compressedOffsetDataGridViewTextBoxColumn,
            this.compressedSizeDataGridViewTextBoxColumn});
            this.DataGridView_Chunks.DataSource = this.compressedChunkBindingSource;
            resources.ApplyResources(this.DataGridView_Chunks, "DataGridView_Chunks");
            this.DataGridView_Chunks.Name = "DataGridView_Chunks";
            this.DataGridView_Chunks.ReadOnly = true;
            this.DataGridView_Chunks.RowHeadersVisible = false;
            // 
            // uncompressedOffsetDataGridViewTextBoxColumn
            // 
            this.uncompressedOffsetDataGridViewTextBoxColumn.DataPropertyName = "UncompressedOffset";
            resources.ApplyResources(this.uncompressedOffsetDataGridViewTextBoxColumn, "uncompressedOffsetDataGridViewTextBoxColumn");
            this.uncompressedOffsetDataGridViewTextBoxColumn.Name = "uncompressedOffsetDataGridViewTextBoxColumn";
            this.uncompressedOffsetDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uncompressedSizeDataGridViewTextBoxColumn
            // 
            this.uncompressedSizeDataGridViewTextBoxColumn.DataPropertyName = "UncompressedSize";
            resources.ApplyResources(this.uncompressedSizeDataGridViewTextBoxColumn, "uncompressedSizeDataGridViewTextBoxColumn");
            this.uncompressedSizeDataGridViewTextBoxColumn.Name = "uncompressedSizeDataGridViewTextBoxColumn";
            this.uncompressedSizeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // compressedOffsetDataGridViewTextBoxColumn
            // 
            this.compressedOffsetDataGridViewTextBoxColumn.DataPropertyName = "CompressedOffset";
            resources.ApplyResources(this.compressedOffsetDataGridViewTextBoxColumn, "compressedOffsetDataGridViewTextBoxColumn");
            this.compressedOffsetDataGridViewTextBoxColumn.Name = "compressedOffsetDataGridViewTextBoxColumn";
            this.compressedOffsetDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // compressedSizeDataGridViewTextBoxColumn
            // 
            this.compressedSizeDataGridViewTextBoxColumn.DataPropertyName = "CompressedSize";
            resources.ApplyResources(this.compressedSizeDataGridViewTextBoxColumn, "compressedSizeDataGridViewTextBoxColumn");
            this.compressedSizeDataGridViewTextBoxColumn.Name = "compressedSizeDataGridViewTextBoxColumn";
            this.compressedSizeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // compressedChunkBindingSource
            // 
            this.compressedChunkBindingSource.DataSource = typeof(UELib.CompressedChunk);
            // 
            // TabPage_Names
            // 
            this.TabPage_Names.Controls.Add(this.nameDataGridView);
            resources.ApplyResources(this.TabPage_Names, "TabPage_Names");
            this.TabPage_Names.Name = "TabPage_Names";
            this.TabPage_Names.UseVisualStyleBackColor = true;
            // 
            // nameDataGridView
            // 
            this.nameDataGridView.AllowUserToAddRows = false;
            this.nameDataGridView.AutoGenerateColumns = false;
            this.nameDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.nameDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.flagsDataGridViewTextBoxColumn});
            this.nameDataGridView.DataSource = this.uNameTableItemBindingSource;
            resources.ApplyResources(this.nameDataGridView, "nameDataGridView");
            this.nameDataGridView.EnableHeadersVisualStyles = false;
            this.nameDataGridView.MultiSelect = false;
            this.nameDataGridView.Name = "nameDataGridView";
            this.nameDataGridView.ReadOnly = true;
            this.nameDataGridView.RowHeadersVisible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn, "nameDataGridViewTextBoxColumn");
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // flagsDataGridViewTextBoxColumn
            // 
            this.flagsDataGridViewTextBoxColumn.DataPropertyName = "Flags";
            resources.ApplyResources(this.flagsDataGridViewTextBoxColumn, "flagsDataGridViewTextBoxColumn");
            this.flagsDataGridViewTextBoxColumn.Name = "flagsDataGridViewTextBoxColumn";
            this.flagsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uNameTableItemBindingSource
            // 
            this.uNameTableItemBindingSource.DataSource = typeof(UELib.UNameTableItem);
            // 
            // TabPage_Imports
            // 
            this.TabPage_Imports.BackColor = System.Drawing.Color.White;
            this.TabPage_Imports.Controls.Add(this.importsDataGridView);
            this.TabPage_Imports.Controls.Add(this.TreeView_Imports);
            resources.ApplyResources(this.TabPage_Imports, "TabPage_Imports");
            this.TabPage_Imports.Name = "TabPage_Imports";
            // 
            // importsDataGridView
            // 
            this.importsDataGridView.AllowUserToAddRows = false;
            this.importsDataGridView.AutoGenerateColumns = false;
            this.importsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.importsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.importsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.packageNameDataGridViewTextBoxColumn,
            this.classNameDataGridViewTextBoxColumn,
            this.objectNameDataGridViewTextBoxColumn1,
            this.outerIndexDataGridViewTextBoxColumn1,
            this.outerDataGridViewTextBoxColumn});
            this.importsDataGridView.DataSource = this.uImportTableItemBindingSource;
            resources.ApplyResources(this.importsDataGridView, "importsDataGridView");
            this.importsDataGridView.EnableHeadersVisualStyles = false;
            this.importsDataGridView.MultiSelect = false;
            this.importsDataGridView.Name = "importsDataGridView";
            this.importsDataGridView.ReadOnly = true;
            this.importsDataGridView.RowHeadersVisible = false;
            // 
            // packageNameDataGridViewTextBoxColumn
            // 
            this.packageNameDataGridViewTextBoxColumn.DataPropertyName = "PackageName";
            resources.ApplyResources(this.packageNameDataGridViewTextBoxColumn, "packageNameDataGridViewTextBoxColumn");
            this.packageNameDataGridViewTextBoxColumn.Name = "packageNameDataGridViewTextBoxColumn";
            this.packageNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // classNameDataGridViewTextBoxColumn
            // 
            this.classNameDataGridViewTextBoxColumn.DataPropertyName = "ClassName";
            resources.ApplyResources(this.classNameDataGridViewTextBoxColumn, "classNameDataGridViewTextBoxColumn");
            this.classNameDataGridViewTextBoxColumn.Name = "classNameDataGridViewTextBoxColumn";
            this.classNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // objectNameDataGridViewTextBoxColumn1
            // 
            this.objectNameDataGridViewTextBoxColumn1.DataPropertyName = "ObjectName";
            resources.ApplyResources(this.objectNameDataGridViewTextBoxColumn1, "objectNameDataGridViewTextBoxColumn1");
            this.objectNameDataGridViewTextBoxColumn1.Name = "objectNameDataGridViewTextBoxColumn1";
            this.objectNameDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // outerIndexDataGridViewTextBoxColumn1
            // 
            this.outerIndexDataGridViewTextBoxColumn1.DataPropertyName = "OuterIndex";
            resources.ApplyResources(this.outerIndexDataGridViewTextBoxColumn1, "outerIndexDataGridViewTextBoxColumn1");
            this.outerIndexDataGridViewTextBoxColumn1.Name = "outerIndexDataGridViewTextBoxColumn1";
            this.outerIndexDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // outerDataGridViewTextBoxColumn
            // 
            this.outerDataGridViewTextBoxColumn.DataPropertyName = "Outer";
            resources.ApplyResources(this.outerDataGridViewTextBoxColumn, "outerDataGridViewTextBoxColumn");
            this.outerDataGridViewTextBoxColumn.Name = "outerDataGridViewTextBoxColumn";
            this.outerDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uImportTableItemBindingSource
            // 
            this.uImportTableItemBindingSource.DataSource = typeof(UELib.UImportTableItem);
            // 
            // TreeView_Imports
            // 
            this.TreeView_Imports.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.TreeView_Imports, "TreeView_Imports");
            this.TreeView_Imports.HideSelection = false;
            this.TreeView_Imports.ImageList = this.VSIcons;
            this.TreeView_Imports.Name = "TreeView_Imports";
            this.TreeView_Imports.ShowNodeToolTips = true;
            // 
            // TabPage_Exports
            // 
            this.TabPage_Exports.Controls.Add(this.exportsDataGridView);
            this.TabPage_Exports.Controls.Add(this.TreeView_Exports);
            resources.ApplyResources(this.TabPage_Exports, "TabPage_Exports");
            this.TabPage_Exports.Name = "TabPage_Exports";
            this.TabPage_Exports.UseVisualStyleBackColor = true;
            // 
            // exportsDataGridView
            // 
            this.exportsDataGridView.AllowUserToAddRows = false;
            this.exportsDataGridView.AutoGenerateColumns = false;
            this.exportsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.exportsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exportsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.classIndexDataGridViewTextBoxColumn,
            this.classDataGridViewTextBoxColumn,
            this.superIndexDataGridViewTextBoxColumn,
            this.superDataGridViewTextBoxColumn,
            this.templateIndexDataGridViewTextBoxColumn,
            this.templateDataGridViewTextBoxColumn,
            this.archetypeIndexDataGridViewTextBoxColumn,
            this.archetypeDataGridViewTextBoxColumn,
            this.objectNameDataGridViewTextBoxColumn,
            this.outerIndexDataGridViewTextBoxColumn,
            this.outerDataGridViewTextBoxColumn1});
            this.exportsDataGridView.DataSource = this.uExportTableItemBindingSource;
            resources.ApplyResources(this.exportsDataGridView, "exportsDataGridView");
            this.exportsDataGridView.EnableHeadersVisualStyles = false;
            this.exportsDataGridView.MultiSelect = false;
            this.exportsDataGridView.Name = "exportsDataGridView";
            this.exportsDataGridView.ReadOnly = true;
            this.exportsDataGridView.RowHeadersVisible = false;
            // 
            // classIndexDataGridViewTextBoxColumn
            // 
            this.classIndexDataGridViewTextBoxColumn.DataPropertyName = "ClassIndex";
            resources.ApplyResources(this.classIndexDataGridViewTextBoxColumn, "classIndexDataGridViewTextBoxColumn");
            this.classIndexDataGridViewTextBoxColumn.Name = "classIndexDataGridViewTextBoxColumn";
            this.classIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // classDataGridViewTextBoxColumn
            // 
            this.classDataGridViewTextBoxColumn.DataPropertyName = "Class";
            resources.ApplyResources(this.classDataGridViewTextBoxColumn, "classDataGridViewTextBoxColumn");
            this.classDataGridViewTextBoxColumn.Name = "classDataGridViewTextBoxColumn";
            this.classDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // superIndexDataGridViewTextBoxColumn
            // 
            this.superIndexDataGridViewTextBoxColumn.DataPropertyName = "SuperIndex";
            resources.ApplyResources(this.superIndexDataGridViewTextBoxColumn, "superIndexDataGridViewTextBoxColumn");
            this.superIndexDataGridViewTextBoxColumn.Name = "superIndexDataGridViewTextBoxColumn";
            this.superIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // superDataGridViewTextBoxColumn
            // 
            this.superDataGridViewTextBoxColumn.DataPropertyName = "Super";
            resources.ApplyResources(this.superDataGridViewTextBoxColumn, "superDataGridViewTextBoxColumn");
            this.superDataGridViewTextBoxColumn.Name = "superDataGridViewTextBoxColumn";
            this.superDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // templateIndexDataGridViewTextBoxColumn
            // 
            this.templateIndexDataGridViewTextBoxColumn.DataPropertyName = "TemplateIndex";
            resources.ApplyResources(this.templateIndexDataGridViewTextBoxColumn, "templateIndexDataGridViewTextBoxColumn");
            this.templateIndexDataGridViewTextBoxColumn.Name = "templateIndexDataGridViewTextBoxColumn";
            this.templateIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // templateDataGridViewTextBoxColumn
            // 
            this.templateDataGridViewTextBoxColumn.DataPropertyName = "Template";
            resources.ApplyResources(this.templateDataGridViewTextBoxColumn, "templateDataGridViewTextBoxColumn");
            this.templateDataGridViewTextBoxColumn.Name = "templateDataGridViewTextBoxColumn";
            this.templateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // archetypeIndexDataGridViewTextBoxColumn
            // 
            this.archetypeIndexDataGridViewTextBoxColumn.DataPropertyName = "ArchetypeIndex";
            resources.ApplyResources(this.archetypeIndexDataGridViewTextBoxColumn, "archetypeIndexDataGridViewTextBoxColumn");
            this.archetypeIndexDataGridViewTextBoxColumn.Name = "archetypeIndexDataGridViewTextBoxColumn";
            this.archetypeIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // archetypeDataGridViewTextBoxColumn
            // 
            this.archetypeDataGridViewTextBoxColumn.DataPropertyName = "Archetype";
            resources.ApplyResources(this.archetypeDataGridViewTextBoxColumn, "archetypeDataGridViewTextBoxColumn");
            this.archetypeDataGridViewTextBoxColumn.Name = "archetypeDataGridViewTextBoxColumn";
            this.archetypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // objectNameDataGridViewTextBoxColumn
            // 
            this.objectNameDataGridViewTextBoxColumn.DataPropertyName = "ObjectName";
            resources.ApplyResources(this.objectNameDataGridViewTextBoxColumn, "objectNameDataGridViewTextBoxColumn");
            this.objectNameDataGridViewTextBoxColumn.Name = "objectNameDataGridViewTextBoxColumn";
            this.objectNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // outerIndexDataGridViewTextBoxColumn
            // 
            this.outerIndexDataGridViewTextBoxColumn.DataPropertyName = "OuterIndex";
            resources.ApplyResources(this.outerIndexDataGridViewTextBoxColumn, "outerIndexDataGridViewTextBoxColumn");
            this.outerIndexDataGridViewTextBoxColumn.Name = "outerIndexDataGridViewTextBoxColumn";
            this.outerIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // outerDataGridViewTextBoxColumn1
            // 
            this.outerDataGridViewTextBoxColumn1.DataPropertyName = "Outer";
            resources.ApplyResources(this.outerDataGridViewTextBoxColumn1, "outerDataGridViewTextBoxColumn1");
            this.outerDataGridViewTextBoxColumn1.Name = "outerDataGridViewTextBoxColumn1";
            this.outerDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // uExportTableItemBindingSource
            // 
            this.uExportTableItemBindingSource.DataSource = typeof(UELib.UExportTableItem);
            // 
            // TreeView_Exports
            // 
            this.TreeView_Exports.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.TreeView_Exports, "TreeView_Exports");
            this.TreeView_Exports.HideSelection = false;
            this.TreeView_Exports.ImageList = this.VSIcons;
            this.TreeView_Exports.Name = "TreeView_Exports";
            this.TreeView_Exports.ShowNodeToolTips = true;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.panel2.Controls.Add(this.ToolStrip_Main);
            this.panel2.Name = "panel2";
            // 
            // ToolStrip_Main
            // 
            this.ToolStrip_Main.BackColor = System.Drawing.SystemColors.MenuBar;
            resources.ApplyResources(this.ToolStrip_Main, "ToolStrip_Main");
            this.ToolStrip_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrevButton,
            this.NextButton,
            this._Tools_StripDropDownButton});
            this.ToolStrip_Main.Name = "ToolStrip_Main";
            this.ToolStrip_Main.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolStrip_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolStrip_Content_Paint);
            // 
            // _Tools_StripDropDownButton
            // 
            this._Tools_StripDropDownButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._Tools_StripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._Tools_StripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            exportingToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuItem1,
            this.toolStripSeparator5,
            this.viewBufferToolStripMenuItem,
            this.ReloadButton});
            this._Tools_StripDropDownButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._Tools_StripDropDownButton.Name = "_Tools_StripDropDownButton";
            this._Tools_StripDropDownButton.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this._Tools_StripDropDownButton, "_Tools_StripDropDownButton");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findNextToolStripMenuItem,
            this.findInDocumentToolStripMenuItem,
            this.findInClassesToolStripMenuItem});
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // findNextToolStripMenuItem
            // 
            resources.ApplyResources(this.findNextToolStripMenuItem, "findNextToolStripMenuItem");
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.Click += new System.EventHandler(this.FindNextToolStripMenuItem_Click);
            // 
            // findInDocumentToolStripMenuItem
            // 
            resources.ApplyResources(this.findInDocumentToolStripMenuItem, "findInDocumentToolStripMenuItem");
            this.findInDocumentToolStripMenuItem.Name = "findInDocumentToolStripMenuItem";
            this.findInDocumentToolStripMenuItem.Click += new System.EventHandler(this.FindInDocumentToolStripMenuItem_Click);
            // 
            // findInClassesToolStripMenuItem
            // 
            resources.ApplyResources(this.findInClassesToolStripMenuItem, "findInClassesToolStripMenuItem");
            this.findInClassesToolStripMenuItem.Name = "findInClassesToolStripMenuItem";
            this.findInClassesToolStripMenuItem.Click += new System.EventHandler(this.FindInClassesToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // viewBufferToolStripMenuItem
            // 
            resources.ApplyResources(this.viewBufferToolStripMenuItem, "viewBufferToolStripMenuItem");
            this.viewBufferToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.viewBufferToolStripMenuItem.Name = "viewBufferToolStripMenuItem";
            this.viewBufferToolStripMenuItem.Click += new System.EventHandler(this.ViewBufferToolStripMenuItem_Click);
            // 
            // ReloadButton
            // 
            resources.ApplyResources(this.ReloadButton, "ReloadButton");
            this.ReloadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // Panel_Content
            // 
            this.Panel_Content.Controls.Add(this.Header);
            this.Panel_Content.Controls.Add(this.BinaryDataPanel);
            this.Panel_Content.Controls.Add(this.TextContentPanel);
            resources.ApplyResources(this.Panel_Content, "Panel_Content");
            this.Panel_Content.Name = "Panel_Content";
            // 
            // Header
            // 
            resources.ApplyResources(this.Header, "Header");
            this.Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.Header.Controls.Add(this.ToolStrip_Content);
            this.Header.Name = "Header";
            // 
            // ToolStrip_Content
            // 
            resources.ApplyResources(this.ToolStrip_Content, "ToolStrip_Content");
            this.ToolStrip_Content.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ToolStrip_Content.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip_Content.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportButton,
            this.toolStripSeparator1,
            this.SearchBox,
            this.FindButton,
            this.toolStripSeparator3,
            this.Label_ObjectName,
            this.ViewTools});
            this.ToolStrip_Content.Name = "ToolStrip_Content";
            this.ToolStrip_Content.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolStrip_Content.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolStrip_Content_Paint);
            // 
            // ExportButton
            // 
            this.ExportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ExportButton, "ExportButton");
            this.ExportButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Padding = new System.Windows.Forms.Padding(3);
            this.ExportButton.Click += new System.EventHandler(this.ToolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolStripSeparator1_Paint);
            // 
            // SearchBox
            // 
            resources.ApplyResources(this.SearchBox, "SearchBox");
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchBox_KeyPress_1);
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolStripSeparator1_Paint);
            // 
            // ViewTools
            // 
            this.ViewTools.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ViewTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ViewTools, "ViewTools");
            this.ViewTools.Name = "ViewTools";
            this.ViewTools.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ViewTools_DropDownItemClicked);
            // 
            // BinaryDataPanel
            // 
            resources.ApplyResources(this.BinaryDataPanel, "BinaryDataPanel");
            this.BinaryDataPanel.Controls.Add(this.binaryDataFieldsPanel);
            this.BinaryDataPanel.Name = "BinaryDataPanel";
            this.BinaryDataPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel4_Paint);
            // 
            // binaryDataFieldsPanel
            // 
            resources.ApplyResources(this.binaryDataFieldsPanel, "binaryDataFieldsPanel");
            this.binaryDataFieldsPanel.Name = "binaryDataFieldsPanel";
            // 
            // TextContentPanel
            // 
            resources.ApplyResources(this.TextContentPanel, "TextContentPanel");
            this.TextContentPanel.Controls.Add(this.textEditorPanel);
            this.TextContentPanel.Name = "TextContentPanel";
            this.TextContentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel4_Paint);
            // 
            // textEditorPanel
            // 
            resources.ApplyResources(this.textEditorPanel, "textEditorPanel");
            this.textEditorPanel.Name = "textEditorPanel";
            // 
            // packageFileSummaryBindingSource
            // 
            this.packageFileSummaryBindingSource.DataSource = typeof(UELib.UnrealPackage.PackageFileSummary);
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
            // Label_ObjectName
            // 
            this.Label_ObjectName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Label_ObjectName.BackColor = System.Drawing.SystemColors.MenuBar;
            this.Label_ObjectName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.Label_ObjectName, "Label_ObjectName");
            this.Label_ObjectName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_ObjectName.Name = "Label_ObjectName";
            this.Label_ObjectName.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_ObjectName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchObjectTextBox_KeyPress);
            // 
            // NextButton
            // 
            this.NextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.NextButton, "NextButton");
            this.NextButton.Name = "NextButton";
            this.NextButton.Click += new System.EventHandler(this.ToolStripButton_Forward_Click);
            // 
            // PrevButton
            // 
            this.PrevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.PrevButton, "PrevButton");
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Click += new System.EventHandler(this.ToolStripButton_Backward_Click);
            // 
            // UC_PackageExplorer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UC_PackageExplorer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.Panel_Main.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.TabControl_General.ResumeLayout(false);
            this.TabPage_Package.ResumeLayout(false);
            this.TabPage_Package.PerformLayout();
            this.TabPage_Tables.ResumeLayout(false);
            this.TabControl_Tables.ResumeLayout(false);
            this.TabPage_Generations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_GenerationsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uGenerationTableItemBindingSource)).EndInit();
            this.TabPage_Chunks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Chunks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressedChunkBindingSource)).EndInit();
            this.TabPage_Names.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nameDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uNameTableItemBindingSource)).EndInit();
            this.TabPage_Imports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.importsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uImportTableItemBindingSource)).EndInit();
            this.TabPage_Exports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.exportsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uExportTableItemBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ToolStrip_Main.ResumeLayout(false);
            this.ToolStrip_Main.PerformLayout();
            this.Panel_Content.ResumeLayout(false);
            this.Header.ResumeLayout(false);
            this.ToolStrip_Content.ResumeLayout(false);
            this.ToolStrip_Content.PerformLayout();
            this.BinaryDataPanel.ResumeLayout(false);
            this.TextContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.packageFileSummaryBindingSource)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolStripButton ExportButton;
		internal System.Windows.Forms.Panel Panel_Content;
		private System.Windows.Forms.Panel Panel_Main;
		private System.Windows.Forms.ToolStrip ToolStrip_Main;
		private System.Windows.Forms.ToolStripDropDownButton _Tools_StripDropDownButton;
		public System.Windows.Forms.ToolStripMenuItem exportDecompiledClassesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem exportScriptClassesToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton FindButton;
		private System.Windows.Forms.ToolStripMenuItem viewBufferToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox SearchBox;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel Header;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel TextContentPanel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.ToolStripDropDownButton ViewTools;
		private System.Windows.Forms.ToolStrip ToolStrip_Content;
		private System.Windows.Forms.ImageList VSIcons;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem findInClassesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem findInDocumentToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findNextToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.BindingSource packageFileSummaryBindingSource;
        private System.Windows.Forms.BindingSource uGenerationTableItemBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportsCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn namesCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn netObjectsCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource compressedChunkBindingSource;
        private System.Windows.Forms.TabControl TabControl_General;
        private System.Windows.Forms.TabPage TabPage_Package;
        private System.Windows.Forms.TabPage TabPage_Tables;
        private System.Windows.Forms.TabControl TabControl_Tables;
        internal System.Windows.Forms.TabPage TabPage_Names;
        private System.Windows.Forms.DataGridView nameDataGridView;
        private System.Windows.Forms.TabPage TabPage_Exports;
        private System.Windows.Forms.TreeView TreeView_Exports;
        internal System.Windows.Forms.TabPage TabPage_Imports;
        private System.Windows.Forms.TreeView TreeView_Imports;
        private System.Windows.Forms.TabPage TabPage_Generations;
        private System.Windows.Forms.DataGridView DataGridView_GenerationsTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn netObjectCountDataGridViewTextBoxColumn;
        internal System.Windows.Forms.TabPage TabPage_Chunks;
        private System.Windows.Forms.DataGridView DataGridView_Chunks;
        private System.Windows.Forms.DataGridViewTextBoxColumn uncompressedOffsetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uncompressedSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn compressedOffsetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn compressedSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.TreeView TreeView_Content;
        private System.Windows.Forms.TextBox FilterText;
        private System.Windows.Forms.BindingSource uImportTableItemBindingSource;
        private System.Windows.Forms.BindingSource uNameTableItemBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn offsetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn flagsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView importsDataGridView;
        private System.Windows.Forms.DataGridView exportsDataGridView;
        private System.Windows.Forms.BindingSource uExportTableItemBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn superTableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn superNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn archetypeTableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn archetypeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectTableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classTableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerTableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectTableDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn classTableDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerTableDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn packageNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerIndexDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn superIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn superDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn templateIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn templateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn archetypeIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn archetypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn outerDataGridViewTextBoxColumn1;
        private System.Windows.Forms.Panel BinaryDataPanel;
        private Panels.BinaryDataFieldsPanel binaryDataFieldsPanel;
        private Panels.TextEditorPanel textEditorPanel;
        private System.Windows.Forms.ToolStripTextBox Label_ObjectName;
        private System.Windows.Forms.ToolStripButton NextButton;
        private System.Windows.Forms.ToolStripButton PrevButton;
    }
}
