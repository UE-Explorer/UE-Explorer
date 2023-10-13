namespace UEExplorer.UI.Panels
{
    partial class FindResultsPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindResultsPanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.findResultsTreeGridView = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView();
            this.Document = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridColumn();
            this.Line = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.Col = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.findResultsImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.findResultsTreeGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // findResultsTreeGridView
            // 
            this.findResultsTreeGridView.AllowUserToAddRows = false;
            this.findResultsTreeGridView.AllowUserToDeleteRows = false;
            this.findResultsTreeGridView.AllowUserToOrderColumns = true;
            this.findResultsTreeGridView.AllowUserToResizeRows = false;
            this.findResultsTreeGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.findResultsTreeGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.findResultsTreeGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Document,
            this.Line,
            this.Col});
            this.findResultsTreeGridView.DataSource = null;
            resources.ApplyResources(this.findResultsTreeGridView, "findResultsTreeGridView");
            this.findResultsTreeGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.findResultsTreeGridView.GridStyles.Style = Krypton.Toolkit.DataGridViewStyle.Sheet;
            this.findResultsTreeGridView.GridStyles.StyleBackground = Krypton.Toolkit.PaletteBackStyle.GridBackgroundSheet;
            this.findResultsTreeGridView.GridStyles.StyleColumn = Krypton.Toolkit.GridStyle.Sheet;
            this.findResultsTreeGridView.GridStyles.StyleDataCells = Krypton.Toolkit.GridStyle.Sheet;
            this.findResultsTreeGridView.GridStyles.StyleRow = Krypton.Toolkit.GridStyle.Sheet;
            this.findResultsTreeGridView.HideOuterBorders = true;
            this.findResultsTreeGridView.ImageList = this.findResultsImageList;
            this.findResultsTreeGridView.MultiSelect = false;
            this.findResultsTreeGridView.Name = "findResultsTreeGridView";
            this.findResultsTreeGridView.ReadOnly = true;
            this.findResultsTreeGridView.RowHeadersVisible = false;
            this.findResultsTreeGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.findResultsTreeGridView.ShowLines = false;
            this.findResultsTreeGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.treeGridFindResults_RowEnter);
            // 
            // Document
            // 
            this.Document.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Document.DefaultCellStyle = dataGridViewCellStyle1;
            this.Document.DefaultNodeImage = global::UEExplorer.Properties.Resources.Code;
            resources.ApplyResources(this.Document, "Document");
            this.Document.Name = "Document";
            this.Document.ReadOnly = true;
            this.Document.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Document.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Line
            // 
            this.Line.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.Line, "Line");
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            this.Line.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Line.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col
            // 
            this.Col.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Col, "Col");
            this.Col.Name = "Col";
            this.Col.ReadOnly = true;
            this.Col.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // findResultsImageList
            // 
            this.findResultsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("findResultsImageList.ImageStream")));
            this.findResultsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.findResultsImageList.Images.SetKeyName(0, "FirstIndent.png");
            this.findResultsImageList.Images.SetKeyName(1, "Document.png");
            this.findResultsImageList.Images.SetKeyName(2, "ResultsToTextFile.png");
            // 
            // FindResultsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.findResultsTreeGridView);
            this.Name = "FindResultsPanel";
            ((System.ComponentModel.ISupportInitialize)(this.findResultsTreeGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView findResultsTreeGridView;
        private System.Windows.Forms.ImageList findResultsImageList;
        private Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridColumn Document;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn Line;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn Col;
    }
}
