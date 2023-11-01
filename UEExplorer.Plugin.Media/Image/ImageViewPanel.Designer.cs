namespace UEExplorer.Plugin.Media.Image
{
    partial class ImageViewPanel
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
            this.canvasControl = new UEExplorer.Plugin.Media.Controls.CanvasControl();
            this.SuspendLayout();
            // 
            // canvasControl
            // 
            this.canvasControl.CausesValidation = false;
            this.canvasControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasControl.Location = new System.Drawing.Point(4, 4);
            this.canvasControl.Name = "canvasControl";
            this.canvasControl.Size = new System.Drawing.Size(142, 142);
            this.canvasControl.TabIndex = 0;
            this.canvasControl.Text = "canvasControl1";
            this.canvasControl.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasControl_Paint);
            // 
            // ImageViewPanel
            // 
            this.Controls.Add(this.canvasControl);
            this.Name = "ImageViewPanel";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CanvasControl canvasControl;
    }
}
