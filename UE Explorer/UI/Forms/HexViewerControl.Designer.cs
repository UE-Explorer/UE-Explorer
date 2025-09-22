namespace UEExplorer.UI.Forms
{
	partial class HexViewerControl
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
            System.Windows.Forms.SplitContainer hexViewSplitter;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexViewerControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Context_Structure = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editStructValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.structSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.defineStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HexViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.DataInfoPanel = new System.Windows.Forms.Panel();
            this.SelectionDataGridView = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HexToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.caretTimer = new System.Windows.Forms.Timer(this.components);
            this.HexViewPanel = new UEExplorer.UI.Forms.HexViewerPanel();
            hexViewSplitter = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(hexViewSplitter)).BeginInit();
            hexViewSplitter.Panel1.SuspendLayout();
            hexViewSplitter.Panel2.SuspendLayout();
            hexViewSplitter.SuspendLayout();
            this.Context_Structure.SuspendLayout();
            this.DataInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SelectionDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // hexViewSplitter
            // 
            hexViewSplitter.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::UEExplorer.Properties.Settings.Default, "HexViewer_SplitterDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(hexViewSplitter, "hexViewSplitter");
            hexViewSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            hexViewSplitter.Name = "hexViewSplitter";
            // 
            // hexViewSplitter.Panel1
            // 
            hexViewSplitter.Panel1.Controls.Add(this.HexViewPanel);
            hexViewSplitter.Panel1.Controls.Add(this.HexViewScrollBar);
            // 
            // hexViewSplitter.Panel2
            // 
            hexViewSplitter.Panel2.Controls.Add(this.DataInfoPanel);
            hexViewSplitter.SplitterDistance = global::UEExplorer.Properties.Settings.Default.HexViewer_SplitterDistance;
            hexViewSplitter.TabStop = false;
            // 
            // Context_Structure
            // 
            this.Context_Structure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.editCellToolStripMenuItem,
            this.cellToolStripMenuItem,
            this.toolStripSeparator1,
            this.defineStructToolStripMenuItem,
            this.removeStructToolStripMenuItem});
            this.Context_Structure.Name = "Context_Structure";
            resources.ApplyResources(this.Context_Structure, "Context_Structure");
            this.Context_Structure.Opening += new System.ComponentModel.CancelEventHandler(this.Context_Structure_Opening);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.History;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Paste;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // editCellToolStripMenuItem
            // 
            this.editCellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editStructValueToolStripMenuItem});
            resources.ApplyResources(this.editCellToolStripMenuItem, "editCellToolStripMenuItem");
            this.editCellToolStripMenuItem.Name = "editCellToolStripMenuItem";
            this.editCellToolStripMenuItem.Tag = "Cell";
            this.editCellToolStripMenuItem.Click += new System.EventHandler(this.editCellToolStripMenuItem_Click);
            // 
            // editStructValueToolStripMenuItem
            // 
            this.editStructValueToolStripMenuItem.Name = "editStructValueToolStripMenuItem";
            resources.ApplyResources(this.editStructValueToolStripMenuItem, "editStructValueToolStripMenuItem");
            this.editStructValueToolStripMenuItem.Tag = "Struct";
            this.editStructValueToolStripMenuItem.Click += new System.EventHandler(this.editStructValueToolStripMenuItem_Click);
            // 
            // cellToolStripMenuItem
            // 
            this.cellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexValueToolStripMenuItem,
            this.hexOffsetToolStripMenuItem,
            this.decimalValueToolStripMenuItem,
            this.decimalOffsetToolStripMenuItem,
            this.structNameToolStripMenuItem,
            this.structValueToolStripMenuItem,
            this.toolStripMenuItem1,
            this.structSizeToolStripMenuItem});
            this.cellToolStripMenuItem.Image = global::UEExplorer.Properties.Resources.Copy;
            this.cellToolStripMenuItem.Name = "cellToolStripMenuItem";
            resources.ApplyResources(this.cellToolStripMenuItem, "cellToolStripMenuItem");
            this.cellToolStripMenuItem.Tag = "Cell";
            // 
            // hexValueToolStripMenuItem
            // 
            this.hexValueToolStripMenuItem.Name = "hexValueToolStripMenuItem";
            resources.ApplyResources(this.hexValueToolStripMenuItem, "hexValueToolStripMenuItem");
            this.hexValueToolStripMenuItem.Tag = "Cell";
            this.hexValueToolStripMenuItem.Click += new System.EventHandler(this.hexValueToolStripMenuItem_Click);
            // 
            // hexOffsetToolStripMenuItem
            // 
            this.hexOffsetToolStripMenuItem.Name = "hexOffsetToolStripMenuItem";
            resources.ApplyResources(this.hexOffsetToolStripMenuItem, "hexOffsetToolStripMenuItem");
            this.hexOffsetToolStripMenuItem.Tag = "Cell";
            this.hexOffsetToolStripMenuItem.Click += new System.EventHandler(this.hexOffsetToolStripMenuItem_Click);
            // 
            // decimalValueToolStripMenuItem
            // 
            this.decimalValueToolStripMenuItem.Name = "decimalValueToolStripMenuItem";
            resources.ApplyResources(this.decimalValueToolStripMenuItem, "decimalValueToolStripMenuItem");
            this.decimalValueToolStripMenuItem.Tag = "Cell";
            this.decimalValueToolStripMenuItem.Click += new System.EventHandler(this.decimalValueToolStripMenuItem_Click);
            // 
            // decimalOffsetToolStripMenuItem
            // 
            this.decimalOffsetToolStripMenuItem.Name = "decimalOffsetToolStripMenuItem";
            resources.ApplyResources(this.decimalOffsetToolStripMenuItem, "decimalOffsetToolStripMenuItem");
            this.decimalOffsetToolStripMenuItem.Tag = "Cell";
            this.decimalOffsetToolStripMenuItem.Click += new System.EventHandler(this.decimalOffsetToolStripMenuItem_Click);
            // 
            // structNameToolStripMenuItem
            // 
            this.structNameToolStripMenuItem.Name = "structNameToolStripMenuItem";
            resources.ApplyResources(this.structNameToolStripMenuItem, "structNameToolStripMenuItem");
            this.structNameToolStripMenuItem.Tag = "Struct";
            this.structNameToolStripMenuItem.Click += new System.EventHandler(this.structNameToolStripMenuItem_Click);
            // 
            // structValueToolStripMenuItem
            // 
            this.structValueToolStripMenuItem.Name = "structValueToolStripMenuItem";
            resources.ApplyResources(this.structValueToolStripMenuItem, "structValueToolStripMenuItem");
            this.structValueToolStripMenuItem.Tag = "Struct";
            this.structValueToolStripMenuItem.Click += new System.EventHandler(this.structValueToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Tag = "Struct";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.structHexSizeToolStripMenuItem_Click);
            // 
            // structSizeToolStripMenuItem
            // 
            this.structSizeToolStripMenuItem.Name = "structSizeToolStripMenuItem";
            resources.ApplyResources(this.structSizeToolStripMenuItem, "structSizeToolStripMenuItem");
            this.structSizeToolStripMenuItem.Tag = "Struct";
            this.structSizeToolStripMenuItem.Click += new System.EventHandler(this.structDecimalSizeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // defineStructToolStripMenuItem
            // 
            this.defineStructToolStripMenuItem.Name = "defineStructToolStripMenuItem";
            resources.ApplyResources(this.defineStructToolStripMenuItem, "defineStructToolStripMenuItem");
            this.defineStructToolStripMenuItem.Tag = "Cell";
            this.defineStructToolStripMenuItem.Click += new System.EventHandler(this.defineStructToolStripMenuItem_Click);
            // 
            // removeStructToolStripMenuItem
            // 
            this.removeStructToolStripMenuItem.Name = "removeStructToolStripMenuItem";
            resources.ApplyResources(this.removeStructToolStripMenuItem, "removeStructToolStripMenuItem");
            this.removeStructToolStripMenuItem.Tag = "Struct";
            this.removeStructToolStripMenuItem.Click += new System.EventHandler(this.removeStructToolStripMenuItem_Click);
            // 
            // HexViewScrollBar
            // 
            resources.ApplyResources(this.HexViewScrollBar, "HexViewScrollBar");
            this.HexViewScrollBar.Name = "HexViewScrollBar";
            this.HexViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HexScrollBar_Scroll);
            this.HexViewScrollBar.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnHexViewScrollBarOnPreviewKeyDown);
            // 
            // DataInfoPanel
            // 
            resources.ApplyResources(this.DataInfoPanel, "DataInfoPanel");
            this.DataInfoPanel.Controls.Add(this.SelectionDataGridView);
            this.DataInfoPanel.Name = "DataInfoPanel";
            // 
            // SelectionDataGridView
            // 
            this.SelectionDataGridView.AllowUserToAddRows = false;
            this.SelectionDataGridView.AllowUserToDeleteRows = false;
            this.SelectionDataGridView.AllowUserToOrderColumns = true;
            this.SelectionDataGridView.AllowUserToResizeRows = false;
            this.SelectionDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SelectionDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.SelectionDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SelectionDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.SelectionDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.SelectionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Value});
            resources.ApplyResources(this.SelectionDataGridView, "SelectionDataGridView");
            this.SelectionDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.SelectionDataGridView.EnableHeadersVisualStyles = false;
            this.SelectionDataGridView.GridColor = System.Drawing.SystemColors.Control;
            this.SelectionDataGridView.MultiSelect = false;
            this.SelectionDataGridView.Name = "SelectionDataGridView";
            this.SelectionDataGridView.ReadOnly = true;
            this.SelectionDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SelectionDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SelectionDataGridView.RowHeadersVisible = false;
            this.SelectionDataGridView.RowTemplate.ReadOnly = true;
            this.SelectionDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.SelectionDataGridView.ShowEditingIcon = false;
            this.SelectionDataGridView.ShowRowErrors = false;
            this.SelectionDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.SelectionDataGridView_RowEnter);
            // 
            // Type
            // 
            this.Type.FillWeight = 60.9137F;
            resources.ApplyResources(this.Type, "Type");
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.FillWeight = 139.0863F;
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // caretTimer
            // 
            this.caretTimer.Interval = 1000;
            this.caretTimer.Tick += new System.EventHandler(this.caretTimer_Tick);
            // 
            // HexViewPanel
            // 
            this.HexViewPanel.ContextMenuStrip = this.Context_Structure;
            resources.ApplyResources(this.HexViewPanel, "HexViewPanel");
            this.HexViewPanel.Name = "HexViewPanel";
            this.HexViewPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HexViewPanel_KeyUp);
            this.HexViewPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HexViewPanel_KeyDown);
            this.HexViewPanel.Click += new System.EventHandler(this.HexViewPanel_Click);
            this.HexViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HexLinePanel_Paint);
            this.HexViewPanel.Enter += new System.EventHandler(this.HexViewerControl_Enter);
            this.HexViewPanel.Leave += new System.EventHandler(this.HexViewPanel_Leave);
            this.HexViewPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseClick);
            this.HexViewPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseDoubleClick);
            this.HexViewPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseMove);
            // 
            // HexViewerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(hexViewSplitter);
            resources.ApplyResources(this, "$this");
            this.Name = "HexViewerControl";
            this.Load += new System.EventHandler(this.HexViewerControl_Load);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HexViewerControl_Scroll);
            hexViewSplitter.Panel1.ResumeLayout(false);
            hexViewSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(hexViewSplitter)).EndInit();
            hexViewSplitter.ResumeLayout(false);
            this.Context_Structure.ResumeLayout(false);
            this.DataInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SelectionDataGridView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.VScrollBar HexViewScrollBar;
		private HexViewerPanel HexViewPanel;
		private System.Windows.Forms.ContextMenuStrip Context_Structure;
		private System.Windows.Forms.ToolTip HexToolTip;
        private System.Windows.Forms.Panel DataInfoPanel;
        private System.Windows.Forms.ToolStripMenuItem editCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cellToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem hexValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editStructValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defineStructToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeStructToolStripMenuItem;
        private System.Windows.Forms.DataGridView SelectionDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Timer caretTimer;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    }
}
