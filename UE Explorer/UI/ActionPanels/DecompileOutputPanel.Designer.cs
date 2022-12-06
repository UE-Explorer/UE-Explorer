using UEExplorer.UI.Panels;

namespace UEExplorer.UI.ActionPanels
{
    partial class DecompileOutputPanel
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
            this.TextEditorPanel = new UEExplorer.UI.Panels.TextEditorPanel();
            this.SuspendLayout();
            // 
            // TextEditorPanel
            // 
            this.TextEditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextEditorPanel.Location = new System.Drawing.Point(0, 0);
            this.TextEditorPanel.Name = "TextEditorPanel";
            this.TextEditorPanel.Size = new System.Drawing.Size(451, 410);
            this.TextEditorPanel.TabIndex = 0;
            // 
            // DecompileOutputPanel
            // 
            this.Controls.Add(this.TextEditorPanel);
            this.Size = new System.Drawing.Size(451, 410);
            this.ResumeLayout(false);

        }

        #endregion

        public TextEditorPanel TextEditorPanel;
    }
}
