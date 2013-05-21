namespace UEExplorer.UI.Forms
{
	partial class ColorGeneratorForm
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
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorGeneratorForm));
            this.ColoredTextInput = new System.Windows.Forms.TextBox();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.XMLFormatCheckBox = new System.Windows.Forms.CheckBox();
            this.PickColorCodeButton = new System.Windows.Forms.Button();
            this.HTMLColorText = new System.Windows.Forms.TextBox();
            this.PickHTMLColorButton = new System.Windows.Forms.Button();
            this.ColoredTextPreview = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // ColoredTextInput
            // 
            resources.ApplyResources(this.ColoredTextInput, "ColoredTextInput");
            this.ColoredTextInput.Name = "ColoredTextInput";
            this.ColoredTextInput.TextChanged += new System.EventHandler(this.ColoredTextPreview_TextChanged);
            // 
            // ColorDialog
            // 
            this.ColorDialog.AnyColor = true;
            this.ColorDialog.FullOpen = true;
            this.ColorDialog.ShowHelp = true;
            // 
            // XMLFormatCheckBox
            // 
            resources.ApplyResources(this.XMLFormatCheckBox, "XMLFormatCheckBox");
            this.XMLFormatCheckBox.Name = "XMLFormatCheckBox";
            this.XMLFormatCheckBox.UseVisualStyleBackColor = true;
            // 
            // PickColorCodeButton
            // 
            resources.ApplyResources(this.PickColorCodeButton, "PickColorCodeButton");
            this.PickColorCodeButton.Name = "PickColorCodeButton";
            this.PickColorCodeButton.UseVisualStyleBackColor = true;
            this.PickColorCodeButton.Click += new System.EventHandler(this.PickColorCodeButton_Click);
            // 
            // HTMLColorText
            // 
            resources.ApplyResources(this.HTMLColorText, "HTMLColorText");
            this.HTMLColorText.Name = "HTMLColorText";
            this.HTMLColorText.ReadOnly = true;
            // 
            // PickHTMLColorButton
            // 
            resources.ApplyResources(this.PickHTMLColorButton, "PickHTMLColorButton");
            this.PickHTMLColorButton.Name = "PickHTMLColorButton";
            this.PickHTMLColorButton.UseVisualStyleBackColor = true;
            this.PickHTMLColorButton.Click += new System.EventHandler(this.PickHTMLColorButton_Click);
            // 
            // ColoredTextPreview
            // 
            resources.ApplyResources(this.ColoredTextPreview, "ColoredTextPreview");
            this.ColoredTextPreview.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ColoredTextPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ColoredTextPreview.Name = "ColoredTextPreview";
            this.ColoredTextPreview.SizeChanged += new System.EventHandler(this.ColoredTextPreview_SizeChanged);
            this.ColoredTextPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.ColoredTextPreview_Paint);
            // 
            // ColorGeneratorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.ColoredTextPreview);
            this.Controls.Add(label1);
            this.Controls.Add(this.HTMLColorText);
            this.Controls.Add(this.ColoredTextInput);
            this.Controls.Add(this.PickColorCodeButton);
            this.Controls.Add(this.XMLFormatCheckBox);
            this.Controls.Add(this.PickHTMLColorButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorGeneratorForm";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ColoredTextInput;
		private System.Windows.Forms.ColorDialog ColorDialog;
		private System.Windows.Forms.CheckBox XMLFormatCheckBox;
		private System.Windows.Forms.Button PickColorCodeButton;
		private System.Windows.Forms.TextBox HTMLColorText;
		private System.Windows.Forms.Button PickHTMLColorButton;
		private System.Windows.Forms.Panel ColoredTextPreview;
	}
}