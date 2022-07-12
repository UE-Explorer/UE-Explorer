namespace UEExplorer.UI.Panels
{
    partial class BinaryDataFieldsPanel
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
            this.binaryDataGridView = new System.Windows.Forms.DataGridView();
            this.offsetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BinaryFieldBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.binaryDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinaryFieldBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // binaryDataGridView
            // 
            this.binaryDataGridView.AllowUserToAddRows = false;
            this.binaryDataGridView.AllowUserToDeleteRows = false;
            this.binaryDataGridView.AllowUserToOrderColumns = true;
            this.binaryDataGridView.AutoGenerateColumns = false;
            this.binaryDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.binaryDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.binaryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.binaryDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.offsetDataGridViewTextBoxColumn,
            this.fieldDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn,
            this.sizeDataGridViewTextBoxColumn});
            this.binaryDataGridView.DataSource = this.BinaryFieldBindingSource;
            this.binaryDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binaryDataGridView.Location = new System.Drawing.Point(0, 0);
            this.binaryDataGridView.MultiSelect = false;
            this.binaryDataGridView.Name = "binaryDataGridView";
            this.binaryDataGridView.ReadOnly = true;
            this.binaryDataGridView.RowHeadersVisible = false;
            this.binaryDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.binaryDataGridView.Size = new System.Drawing.Size(200, 100);
            this.binaryDataGridView.TabIndex = 0;
            // 
            // offsetDataGridViewTextBoxColumn
            // 
            this.offsetDataGridViewTextBoxColumn.DataPropertyName = "Offset";
            this.offsetDataGridViewTextBoxColumn.HeaderText = "Offset";
            this.offsetDataGridViewTextBoxColumn.Name = "offsetDataGridViewTextBoxColumn";
            this.offsetDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fieldDataGridViewTextBoxColumn
            // 
            this.fieldDataGridViewTextBoxColumn.DataPropertyName = "Field";
            this.fieldDataGridViewTextBoxColumn.HeaderText = "Field";
            this.fieldDataGridViewTextBoxColumn.Name = "fieldDataGridViewTextBoxColumn";
            this.fieldDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sizeDataGridViewTextBoxColumn
            // 
            this.sizeDataGridViewTextBoxColumn.DataPropertyName = "Size";
            this.sizeDataGridViewTextBoxColumn.HeaderText = "Size";
            this.sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
            this.sizeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // BinaryFieldBindingSource
            // 
            this.BinaryFieldBindingSource.DataSource = typeof(UELib.BinaryMetaData.BinaryField);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Tag";
            this.dataGridViewTextBoxColumn1.HeaderText = "Tag";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 163;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 164;
            // 
            // BinaryDataFieldsPanel
            // 
            this.Controls.Add(this.binaryDataGridView);
            ((System.ComponentModel.ISupportInitialize)(this.binaryDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinaryFieldBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        public System.Windows.Forms.BindingSource BinaryFieldBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn offsetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView binaryDataGridView;
    }
}
