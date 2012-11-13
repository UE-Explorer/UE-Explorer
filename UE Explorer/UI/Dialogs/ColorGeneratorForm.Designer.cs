namespace UEExplorer.UI.Dialogs
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
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 16);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(37, 13);
			label1.TabIndex = 8;
			label1.Text = "HTML";
			// 
			// ColoredTextInput
			// 
			this.ColoredTextInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ColoredTextInput.Location = new System.Drawing.Point(12, 35);
			this.ColoredTextInput.Name = "ColoredTextInput";
			this.ColoredTextInput.Size = new System.Drawing.Size(471, 20);
			this.ColoredTextInput.TabIndex = 0;
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
			this.XMLFormatCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.XMLFormatCheckBox.AutoSize = true;
			this.XMLFormatCheckBox.Location = new System.Drawing.Point(348, 15);
			this.XMLFormatCheckBox.Name = "XMLFormatCheckBox";
			this.XMLFormatCheckBox.Size = new System.Drawing.Size(54, 17);
			this.XMLFormatCheckBox.TabIndex = 2;
			this.XMLFormatCheckBox.Text = "XML?";
			this.XMLFormatCheckBox.UseVisualStyleBackColor = true;
			// 
			// PickColorCodeButton
			// 
			this.PickColorCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PickColorCodeButton.Location = new System.Drawing.Point(408, 12);
			this.PickColorCodeButton.Name = "PickColorCodeButton";
			this.PickColorCodeButton.Size = new System.Drawing.Size(75, 20);
			this.PickColorCodeButton.TabIndex = 3;
			this.PickColorCodeButton.Text = "Pick Color";
			this.PickColorCodeButton.UseVisualStyleBackColor = true;
			this.PickColorCodeButton.Click += new System.EventHandler(this.PickColorCodeButton_Click);
			// 
			// HTMLColorText
			// 
			this.HTMLColorText.Location = new System.Drawing.Point(55, 12);
			this.HTMLColorText.Name = "HTMLColorText";
			this.HTMLColorText.ReadOnly = true;
			this.HTMLColorText.Size = new System.Drawing.Size(75, 20);
			this.HTMLColorText.TabIndex = 5;
			// 
			// PickHTMLColorButton
			// 
			this.PickHTMLColorButton.Location = new System.Drawing.Point(136, 11);
			this.PickHTMLColorButton.Name = "PickHTMLColorButton";
			this.PickHTMLColorButton.Size = new System.Drawing.Size(75, 20);
			this.PickHTMLColorButton.TabIndex = 6;
			this.PickHTMLColorButton.Text = "Pick Color";
			this.PickHTMLColorButton.UseVisualStyleBackColor = true;
			this.PickHTMLColorButton.Click += new System.EventHandler(this.PickHTMLColorButton_Click);
			// 
			// ColoredTextPreview
			// 
			this.ColoredTextPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ColoredTextPreview.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ColoredTextPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ColoredTextPreview.Location = new System.Drawing.Point(12, 61);
			this.ColoredTextPreview.Name = "ColoredTextPreview";
			this.ColoredTextPreview.Size = new System.Drawing.Size(471, 84);
			this.ColoredTextPreview.TabIndex = 9;
			this.ColoredTextPreview.SizeChanged += new System.EventHandler(this.ColoredTextPreview_SizeChanged);
			this.ColoredTextPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.ColoredTextPreview_Paint);
			// 
			// ColorGeneratorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
			this.ClientSize = new System.Drawing.Size(495, 157);
			this.Controls.Add(this.ColoredTextPreview);
			this.Controls.Add(label1);
			this.Controls.Add(this.HTMLColorText);
			this.Controls.Add(this.ColoredTextInput);
			this.Controls.Add(this.PickColorCodeButton);
			this.Controls.Add(this.XMLFormatCheckBox);
			this.Controls.Add(this.PickHTMLColorButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(503, 169);
			this.Name = "ColorGeneratorForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "UE Color Generator";
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