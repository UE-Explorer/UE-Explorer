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
            this.treeViewFindResults = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewFindResults
            // 
            this.treeViewFindResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFindResults.Location = new System.Drawing.Point(0, 0);
            this.treeViewFindResults.Name = "treeViewFindResults";
            this.treeViewFindResults.Size = new System.Drawing.Size(488, 476);
            this.treeViewFindResults.TabIndex = 0;
            this.treeViewFindResults.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFindResults_AfterSelect);
            // 
            // FindResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewFindResults);
            this.Name = "FindResultsPanel";
            this.Size = new System.Drawing.Size(488, 476);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFindResults;
    }
}
