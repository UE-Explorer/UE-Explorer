namespace UEExplorer.UI.Dialogs
{
    partial class NameReferenceInputDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NameReferenceInputDialog));
            this.nameLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.inputComboBox = new System.Windows.Forms.ComboBox();
            this.numberNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numberNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            resources.ApplyResources(this.nameLabel, "nameLabel");
            this.nameLabel.Name = "nameLabel";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            // 
            // inputComboBox
            // 
            resources.ApplyResources(this.inputComboBox, "inputComboBox");
            this.inputComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputComboBox.FormattingEnabled = true;
            this.inputComboBox.Name = "inputComboBox";
            // 
            // numberNumericUpDown
            // 
            resources.ApplyResources(this.numberNumericUpDown, "numberNumericUpDown");
            this.numberNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numberNumericUpDown.Name = "numberNumericUpDown";
            // 
            // NameReferenceInputDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numberNumericUpDown);
            this.Controls.Add(this.inputComboBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NameReferenceInputDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.NameReferenceInputDialog_Load);
            this.Shown += new System.EventHandler(this.NameReferenceInputDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numberNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox inputComboBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.NumericUpDown numberNumericUpDown;
    }
}
