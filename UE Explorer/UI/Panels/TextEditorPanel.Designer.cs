using System.Windows.Forms;

namespace UEExplorer.UI.Panels
{
    partial class TextEditorPanel
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
            this._TextEditorControlHost = new System.Windows.Forms.Integration.ElementHost();
            this.TextEditorControl = new UEExplorer.UI.Panels.TextEditorControl();
            this.SuspendLayout();
            // 
            // _TextEditorControlHost
            // 
            this._TextEditorControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TextEditorControlHost.Location = new System.Drawing.Point(0, 0);
            this._TextEditorControlHost.Name = "_TextEditorControlHost";
            this._TextEditorControlHost.Size = new System.Drawing.Size(200, 100);
            this._TextEditorControlHost.TabIndex = 0;
            this._TextEditorControlHost.Child = this.TextEditorControl;
            // 
            // TextEditorPanel
            // 
            this.Controls.Add(this._TextEditorControlHost);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Integration.ElementHost _TextEditorControlHost;
        public TextEditorControl TextEditorControl;
    }
}
