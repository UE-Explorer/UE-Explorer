using System.Drawing;

namespace UEExplorer.Framework.UI.Editor
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
            this.components = new System.ComponentModel.Container();
            this.hoverToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.textEditorControlHost = new System.Windows.Forms.Integration.ElementHost();
            this.textEditorControl = new TextEditorControl();
            this.SuspendLayout();
            // 
            // hoverToolTip
            // 
            this.hoverToolTip.AutoPopDelay = 0;
            this.hoverToolTip.InitialDelay = 50;
            this.hoverToolTip.ReshowDelay = 50;
            // 
            // _TextEditorControlHost
            // 
            this.textEditorControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorControlHost.Location = new Point(0, 0);
            this.textEditorControlHost.Name = "textEditorControlHost";
            this.textEditorControlHost.Size = new System.Drawing.Size(200, 100);
            this.textEditorControlHost.TabIndex = 0;
            this.textEditorControlHost.Child = this.textEditorControl;
            // 
            // TextEditorPanel
            // 
            this.Controls.Add(this.textEditorControlHost);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip hoverToolTip;
        private System.Windows.Forms.Integration.ElementHost textEditorControlHost;
        private TextEditorControl textEditorControl;
    }
}
