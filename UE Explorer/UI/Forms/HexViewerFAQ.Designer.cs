namespace UEExplorer.UI.Forms
{
    partial class HexViewerFAQ
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
            if( disposing && (components != null) )
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexViewerFAQ));
            this.panel1 = new System.Windows.Forms.Panel();
            this.FAQTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.FAQTextBox);
            this.panel1.Location = new System.Drawing.Point(3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4);
            this.panel1.Size = new System.Drawing.Size(680, 304);
            this.panel1.TabIndex = 2;
            // 
            // FAQTextBox
            // 
            this.FAQTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FAQTextBox.Location = new System.Drawing.Point(4, 4);
            this.FAQTextBox.Multiline = true;
            this.FAQTextBox.Name = "FAQTextBox";
            this.FAQTextBox.ReadOnly = true;
            this.FAQTextBox.Size = new System.Drawing.Size(672, 296);
            this.FAQTextBox.TabIndex = 2;
            this.FAQTextBox.Text = resources.GetString("FAQTextBox.Text");
            // 
            // HexViewerFAQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panel1);
            this.Name = "HexViewerFAQ";
            this.Size = new System.Drawing.Size(686, 311);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox FAQTextBox;

    }
}
