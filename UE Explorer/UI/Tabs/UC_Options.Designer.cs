namespace UEExplorer.UI.Tabs
{
	partial class UC_Options
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
		protected override void InitializeComponent()
		{
			System.Windows.Forms.Label label2;
			System.Windows.Forms.GroupBox groupBox3;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label6;
			this.IndentionNumeric = new System.Windows.Forms.NumericUpDown();
			this.PreBeginBracket = new System.Windows.Forms.TextBox();
			this.PreEndBracket = new System.Windows.Forms.TextBox();
			this.SuppressComments = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.PathButton = new System.Windows.Forms.Button();
			this.PathText = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.CheckBox_LinkObj = new System.Windows.Forms.CheckBox();
			this.CheckBox_SerObj = new System.Windows.Forms.CheckBox();
			this.CheckBox_ImpObj = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.NumericUpDown_LicenseeMode = new System.Windows.Forms.NumericUpDown();
			this.NumericUpDown_Version = new System.Windows.Forms.NumericUpDown();
			this.CheckBox_LicenseeMode = new System.Windows.Forms.CheckBox();
			this.CheckBox_Version = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ComboBox_NativeTable = new System.Windows.Forms.ComboBox();
			this.Button_Save = new System.Windows.Forms.Button();
			label2 = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			label5 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.IndentionNumeric)).BeginInit();
			this.groupBox4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_LicenseeMode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Version)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(6, 16);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(114, 13);
			label2.TabIndex = 2;
			label2.Text = "UE Model Viewer Path";
			// 
			// groupBox3
			// 
			groupBox3.Controls.Add(this.IndentionNumeric);
			groupBox3.Controls.Add(label5);
			groupBox3.Controls.Add(label4);
			groupBox3.Controls.Add(label3);
			groupBox3.Controls.Add(this.PreBeginBracket);
			groupBox3.Controls.Add(this.PreEndBracket);
			groupBox3.Controls.Add(this.SuppressComments);
			groupBox3.Location = new System.Drawing.Point(337, 8);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(275, 224);
			groupBox3.TabIndex = 15;
			groupBox3.TabStop = false;
			groupBox3.Text = "Decompiler Output";
			// 
			// IndentionNumeric
			// 
			this.IndentionNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.IndentionNumeric.Location = new System.Drawing.Point(179, 112);
			this.IndentionNumeric.Name = "IndentionNumeric";
			this.IndentionNumeric.Size = new System.Drawing.Size(86, 20);
			this.IndentionNumeric.TabIndex = 7;
			// 
			// label5
			// 
			label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(176, 96);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(90, 13);
			label5.TabIndex = 6;
			label5.Text = "Indention Spaces";
			// 
			// label4
			// 
			label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(4, 180);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(120, 13);
			label4.TabIndex = 4;
			label4.Text = "Pre End Bracket Format";
			// 
			// label3
			// 
			label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(4, 141);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(128, 13);
			label3.TabIndex = 3;
			label3.Text = "Pre Begin Bracket Format";
			// 
			// PreBeginBracket
			// 
			this.PreBeginBracket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreBeginBracket.Location = new System.Drawing.Point(7, 157);
			this.PreBeginBracket.Name = "PreBeginBracket";
			this.PreBeginBracket.Size = new System.Drawing.Size(262, 20);
			this.PreBeginBracket.TabIndex = 2;
			// 
			// PreEndBracket
			// 
			this.PreEndBracket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreEndBracket.Location = new System.Drawing.Point(7, 197);
			this.PreEndBracket.Name = "PreEndBracket";
			this.PreEndBracket.Size = new System.Drawing.Size(262, 20);
			this.PreEndBracket.TabIndex = 1;
			// 
			// SuppressComments
			// 
			this.SuppressComments.AutoSize = true;
			this.SuppressComments.Location = new System.Drawing.Point(7, 20);
			this.SuppressComments.Name = "SuppressComments";
			this.SuppressComments.Size = new System.Drawing.Size(122, 17);
			this.SuppressComments.TabIndex = 0;
			this.SuppressComments.Text = "Suppress Comments";
			this.SuppressComments.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(9, 62);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(579, 13);
			label6.TabIndex = 3;
			label6.Text = "Input the path to UE Model Viewer to get integrated support for viewing Meshes an" +
    "d/or Textures through the Content tab!";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(label6);
			this.groupBox4.Controls.Add(label2);
			this.groupBox4.Controls.Add(this.PathButton);
			this.groupBox4.Controls.Add(this.PathText);
			this.groupBox4.Location = new System.Drawing.Point(10, 238);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(602, 82);
			this.groupBox4.TabIndex = 16;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Third-Party";
			// 
			// PathButton
			// 
			this.PathButton.Location = new System.Drawing.Point(7, 32);
			this.PathButton.Name = "PathButton";
			this.PathButton.Size = new System.Drawing.Size(29, 23);
			this.PathButton.TabIndex = 1;
			this.PathButton.Text = "...";
			this.PathButton.UseVisualStyleBackColor = true;
			this.PathButton.Click += new System.EventHandler(this.PathButton_Click);
			// 
			// PathText
			// 
			this.PathText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PathText.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.PathText.Location = new System.Drawing.Point(42, 32);
			this.PathText.Name = "PathText";
			this.PathText.Size = new System.Drawing.Size(554, 20);
			this.PathText.TabIndex = 0;
			this.PathText.TextChanged += new System.EventHandler(this.PathText_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.CheckBox_LinkObj);
			this.groupBox1.Controls.Add(this.CheckBox_SerObj);
			this.groupBox1.Controls.Add(this.CheckBox_ImpObj);
			this.groupBox1.Location = new System.Drawing.Point(10, 142);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(321, 90);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Package Initialization";
			// 
			// CheckBox_LinkObj
			// 
			this.CheckBox_LinkObj.AutoSize = true;
			this.CheckBox_LinkObj.Location = new System.Drawing.Point(6, 42);
			this.CheckBox_LinkObj.Name = "CheckBox_LinkObj";
			this.CheckBox_LinkObj.Size = new System.Drawing.Size(85, 17);
			this.CheckBox_LinkObj.TabIndex = 6;
			this.CheckBox_LinkObj.Text = "Link Objects";
			this.CheckBox_LinkObj.UseVisualStyleBackColor = true;
			// 
			// CheckBox_SerObj
			// 
			this.CheckBox_SerObj.AutoSize = true;
			this.CheckBox_SerObj.Location = new System.Drawing.Point(6, 19);
			this.CheckBox_SerObj.Name = "CheckBox_SerObj";
			this.CheckBox_SerObj.Size = new System.Drawing.Size(104, 17);
			this.CheckBox_SerObj.TabIndex = 5;
			this.CheckBox_SerObj.Text = "Serialize Objects";
			this.CheckBox_SerObj.UseVisualStyleBackColor = true;
			// 
			// CheckBox_ImpObj
			// 
			this.CheckBox_ImpObj.AutoSize = true;
			this.CheckBox_ImpObj.Enabled = false;
			this.CheckBox_ImpObj.Location = new System.Drawing.Point(6, 65);
			this.CheckBox_ImpObj.Name = "CheckBox_ImpObj";
			this.CheckBox_ImpObj.Size = new System.Drawing.Size(94, 17);
			this.CheckBox_ImpObj.TabIndex = 4;
			this.CheckBox_ImpObj.Text = "Import Objects";
			this.CheckBox_ImpObj.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.NumericUpDown_LicenseeMode);
			this.groupBox2.Controls.Add(this.NumericUpDown_Version);
			this.groupBox2.Controls.Add(this.CheckBox_LicenseeMode);
			this.groupBox2.Controls.Add(this.CheckBox_Version);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.ComboBox_NativeTable);
			this.groupBox2.Location = new System.Drawing.Point(10, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(321, 128);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Package Deserialization";
			// 
			// NumericUpDown_LicenseeMode
			// 
			this.NumericUpDown_LicenseeMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.NumericUpDown_LicenseeMode.Location = new System.Drawing.Point(188, 96);
			this.NumericUpDown_LicenseeMode.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.NumericUpDown_LicenseeMode.Name = "NumericUpDown_LicenseeMode";
			this.NumericUpDown_LicenseeMode.Size = new System.Drawing.Size(123, 20);
			this.NumericUpDown_LicenseeMode.TabIndex = 15;
			// 
			// NumericUpDown_Version
			// 
			this.NumericUpDown_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.NumericUpDown_Version.Location = new System.Drawing.Point(188, 70);
			this.NumericUpDown_Version.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.NumericUpDown_Version.Name = "NumericUpDown_Version";
			this.NumericUpDown_Version.Size = new System.Drawing.Size(123, 20);
			this.NumericUpDown_Version.TabIndex = 14;
			// 
			// CheckBox_LicenseeMode
			// 
			this.CheckBox_LicenseeMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CheckBox_LicenseeMode.AutoSize = true;
			this.CheckBox_LicenseeMode.Location = new System.Drawing.Point(10, 96);
			this.CheckBox_LicenseeMode.Name = "CheckBox_LicenseeMode";
			this.CheckBox_LicenseeMode.Size = new System.Drawing.Size(137, 17);
			this.CheckBox_LicenseeMode.TabIndex = 13;
			this.CheckBox_LicenseeMode.Text = "Force Licensee Version";
			this.CheckBox_LicenseeMode.UseVisualStyleBackColor = true;
			// 
			// CheckBox_Version
			// 
			this.CheckBox_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CheckBox_Version.AutoSize = true;
			this.CheckBox_Version.Location = new System.Drawing.Point(10, 73);
			this.CheckBox_Version.Name = "CheckBox_Version";
			this.CheckBox_Version.Size = new System.Drawing.Size(91, 17);
			this.CheckBox_Version.TabIndex = 12;
			this.CheckBox_Version.Text = "Force Version";
			this.CheckBox_Version.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Native Tables";
			// 
			// ComboBox_NativeTable
			// 
			this.ComboBox_NativeTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ComboBox_NativeTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBox_NativeTable.FormattingEnabled = true;
			this.ComboBox_NativeTable.Location = new System.Drawing.Point(10, 39);
			this.ComboBox_NativeTable.Name = "ComboBox_NativeTable";
			this.ComboBox_NativeTable.Size = new System.Drawing.Size(301, 21);
			this.ComboBox_NativeTable.TabIndex = 10;
			// 
			// Button_Save
			// 
			this.Button_Save.Location = new System.Drawing.Point(537, 326);
			this.Button_Save.Name = "Button_Save";
			this.Button_Save.Size = new System.Drawing.Size(75, 23);
			this.Button_Save.TabIndex = 12;
			this.Button_Save.Text = "Save";
			this.Button_Save.UseVisualStyleBackColor = true;
			this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
			// 
			// UC_Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.Button_Save);
			this.Name = "UC_Options";
			this.Size = new System.Drawing.Size(1312, 584);
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.IndentionNumeric)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_LicenseeMode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Version)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button PathButton;
		private System.Windows.Forms.TextBox PathText;
		private System.Windows.Forms.TextBox PreBeginBracket;
		private System.Windows.Forms.TextBox PreEndBracket;
		private System.Windows.Forms.CheckBox SuppressComments;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox CheckBox_LinkObj;
		private System.Windows.Forms.CheckBox CheckBox_SerObj;
		private System.Windows.Forms.CheckBox CheckBox_ImpObj;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown NumericUpDown_LicenseeMode;
		private System.Windows.Forms.NumericUpDown NumericUpDown_Version;
		private System.Windows.Forms.CheckBox CheckBox_LicenseeMode;
		private System.Windows.Forms.CheckBox CheckBox_Version;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox ComboBox_NativeTable;
		private System.Windows.Forms.Button Button_Save;
		private System.Windows.Forms.NumericUpDown IndentionNumeric;

	}
}
