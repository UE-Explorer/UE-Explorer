namespace UEExplorer.UI.Panels
{
	partial class SettingsPanel
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPanel));
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label6;
            this.BracketPreview = new System.Windows.Forms.RichTextBox();
            this.IndentionNumeric = new System.Windows.Forms.NumericUpDown();
            this.PreBeginBracket = new System.Windows.Forms.TextBox();
            this.PreEndBracket = new System.Windows.Forms.TextBox();
            this.SuppressComments = new System.Windows.Forms.CheckBox();
            this.VariableTypesTree = new System.Windows.Forms.TreeView();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.VariableType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.VariableTypeGroup = new System.Windows.Forms.TextBox();
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.DeleteArrayType = new System.Windows.Forms.Button();
            this.NewArrayType = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            label2 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            groupBox3.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IndentionNumeric)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_LicenseeMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Version)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // groupBox3
            // 
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Controls.Add(panel1);
            groupBox3.Controls.Add(this.IndentionNumeric);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(this.PreBeginBracket);
            groupBox3.Controls.Add(this.PreEndBracket);
            groupBox3.Controls.Add(this.SuppressComments);
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(this.BracketPreview);
            panel1.Name = "panel1";
            // 
            // BracketPreview
            // 
            resources.ApplyResources(this.BracketPreview, "BracketPreview");
            this.BracketPreview.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BracketPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BracketPreview.DetectUrls = false;
            this.BracketPreview.Name = "BracketPreview";
            this.BracketPreview.ReadOnly = true;
            // 
            // IndentionNumeric
            // 
            resources.ApplyResources(this.IndentionNumeric, "IndentionNumeric");
            this.IndentionNumeric.Name = "IndentionNumeric";
            this.toolTip1.SetToolTip(this.IndentionNumeric, resources.GetString("IndentionNumeric.ToolTip"));
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // PreBeginBracket
            // 
            resources.ApplyResources(this.PreBeginBracket, "PreBeginBracket");
            this.PreBeginBracket.Name = "PreBeginBracket";
            this.toolTip1.SetToolTip(this.PreBeginBracket, resources.GetString("PreBeginBracket.ToolTip"));
            // 
            // PreEndBracket
            // 
            resources.ApplyResources(this.PreEndBracket, "PreEndBracket");
            this.PreEndBracket.Name = "PreEndBracket";
            this.toolTip1.SetToolTip(this.PreEndBracket, resources.GetString("PreEndBracket.ToolTip"));
            // 
            // SuppressComments
            // 
            resources.ApplyResources(this.SuppressComments, "SuppressComments");
            this.SuppressComments.Name = "SuppressComments";
            this.toolTip1.SetToolTip(this.SuppressComments, resources.GetString("SuppressComments.ToolTip"));
            this.SuppressComments.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // VariableTypesTree
            // 
            resources.ApplyResources(this.VariableTypesTree, "VariableTypesTree");
            this.VariableTypesTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VariableTypesTree.ForeColor = System.Drawing.SystemColors.Highlight;
            this.VariableTypesTree.HideSelection = false;
            this.VariableTypesTree.Name = "VariableTypesTree";
            this.VariableTypesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.VariableTypesTree_AfterSelect);
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.VariableType);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.VariableTypeGroup);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // VariableType
            // 
            resources.ApplyResources(this.VariableType, "VariableType");
            this.VariableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VariableType.FormattingEnabled = true;
            this.VariableType.Name = "VariableType";
            this.toolTip1.SetToolTip(this.VariableType, resources.GetString("VariableType.ToolTip"));
            this.VariableType.SelectedIndexChanged += new System.EventHandler(this.VariableType_SelectedIndexChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // VariableTypeGroup
            // 
            resources.ApplyResources(this.VariableTypeGroup, "VariableTypeGroup");
            this.VariableTypeGroup.Name = "VariableTypeGroup";
            this.toolTip1.SetToolTip(this.VariableTypeGroup, resources.GetString("VariableTypeGroup.ToolTip"));
            this.VariableTypeGroup.TextChanged += new System.EventHandler(this.VariableTypeGroup_TextChanged);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(label6);
            this.groupBox4.Controls.Add(label2);
            this.groupBox4.Controls.Add(this.PathButton);
            this.groupBox4.Controls.Add(this.PathText);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // PathButton
            // 
            resources.ApplyResources(this.PathButton, "PathButton");
            this.PathButton.Name = "PathButton";
            this.toolTip1.SetToolTip(this.PathButton, resources.GetString("PathButton.ToolTip"));
            this.PathButton.UseVisualStyleBackColor = true;
            this.PathButton.Click += new System.EventHandler(this.PathButton_Click);
            // 
            // PathText
            // 
            resources.ApplyResources(this.PathText, "PathText");
            this.PathText.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.PathText.Name = "PathText";
            this.PathText.TextChanged += new System.EventHandler(this.PathText_TextChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.CheckBox_LinkObj);
            this.groupBox1.Controls.Add(this.CheckBox_SerObj);
            this.groupBox1.Controls.Add(this.CheckBox_ImpObj);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // CheckBox_LinkObj
            // 
            resources.ApplyResources(this.CheckBox_LinkObj, "CheckBox_LinkObj");
            this.CheckBox_LinkObj.Name = "CheckBox_LinkObj";
            this.toolTip1.SetToolTip(this.CheckBox_LinkObj, resources.GetString("CheckBox_LinkObj.ToolTip"));
            this.CheckBox_LinkObj.UseVisualStyleBackColor = true;
            // 
            // CheckBox_SerObj
            // 
            resources.ApplyResources(this.CheckBox_SerObj, "CheckBox_SerObj");
            this.CheckBox_SerObj.Name = "CheckBox_SerObj";
            this.toolTip1.SetToolTip(this.CheckBox_SerObj, resources.GetString("CheckBox_SerObj.ToolTip"));
            this.CheckBox_SerObj.UseVisualStyleBackColor = true;
            // 
            // CheckBox_ImpObj
            // 
            resources.ApplyResources(this.CheckBox_ImpObj, "CheckBox_ImpObj");
            this.CheckBox_ImpObj.Name = "CheckBox_ImpObj";
            this.CheckBox_ImpObj.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.NumericUpDown_LicenseeMode);
            this.groupBox2.Controls.Add(this.NumericUpDown_Version);
            this.groupBox2.Controls.Add(this.CheckBox_LicenseeMode);
            this.groupBox2.Controls.Add(this.CheckBox_Version);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ComboBox_NativeTable);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // NumericUpDown_LicenseeMode
            // 
            resources.ApplyResources(this.NumericUpDown_LicenseeMode, "NumericUpDown_LicenseeMode");
            this.NumericUpDown_LicenseeMode.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumericUpDown_LicenseeMode.Name = "NumericUpDown_LicenseeMode";
            // 
            // NumericUpDown_Version
            // 
            resources.ApplyResources(this.NumericUpDown_Version, "NumericUpDown_Version");
            this.NumericUpDown_Version.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumericUpDown_Version.Name = "NumericUpDown_Version";
            // 
            // CheckBox_LicenseeMode
            // 
            resources.ApplyResources(this.CheckBox_LicenseeMode, "CheckBox_LicenseeMode");
            this.CheckBox_LicenseeMode.Name = "CheckBox_LicenseeMode";
            this.toolTip1.SetToolTip(this.CheckBox_LicenseeMode, resources.GetString("CheckBox_LicenseeMode.ToolTip"));
            this.CheckBox_LicenseeMode.UseVisualStyleBackColor = true;
            this.CheckBox_LicenseeMode.CheckedChanged += new System.EventHandler(this.CheckBox_LicenseeMode_CheckedChanged);
            // 
            // CheckBox_Version
            // 
            resources.ApplyResources(this.CheckBox_Version, "CheckBox_Version");
            this.CheckBox_Version.Name = "CheckBox_Version";
            this.toolTip1.SetToolTip(this.CheckBox_Version, resources.GetString("CheckBox_Version.ToolTip"));
            this.CheckBox_Version.UseVisualStyleBackColor = true;
            this.CheckBox_Version.CheckedChanged += new System.EventHandler(this.CheckBox_Version_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ComboBox_NativeTable
            // 
            resources.ApplyResources(this.ComboBox_NativeTable, "ComboBox_NativeTable");
            this.ComboBox_NativeTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_NativeTable.FormattingEnabled = true;
            this.ComboBox_NativeTable.Name = "ComboBox_NativeTable";
            this.toolTip1.SetToolTip(this.ComboBox_NativeTable, resources.GetString("ComboBox_NativeTable.ToolTip"));
            // 
            // Button_Save
            // 
            resources.ApplyResources(this.Button_Save, "Button_Save");
            this.Button_Save.Name = "Button_Save";
            this.toolTip1.SetToolTip(this.Button_Save, resources.GetString("Button_Save.ToolTip"));
            this.Button_Save.UseVisualStyleBackColor = true;
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Controls.Add(this.VariableTypesTree);
            this.groupBox5.Controls.Add(this.DeleteArrayType);
            this.groupBox5.Controls.Add(this.NewArrayType);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // DeleteArrayType
            // 
            resources.ApplyResources(this.DeleteArrayType, "DeleteArrayType");
            this.DeleteArrayType.Name = "DeleteArrayType";
            this.toolTip1.SetToolTip(this.DeleteArrayType, resources.GetString("DeleteArrayType.ToolTip"));
            this.DeleteArrayType.UseVisualStyleBackColor = true;
            this.DeleteArrayType.Click += new System.EventHandler(this.DeleteArrayType_Click);
            // 
            // NewArrayType
            // 
            resources.ApplyResources(this.NewArrayType, "NewArrayType");
            this.NewArrayType.Name = "NewArrayType";
            this.toolTip1.SetToolTip(this.NewArrayType, resources.GetString("NewArrayType.ToolTip"));
            this.NewArrayType.UseVisualStyleBackColor = true;
            this.NewArrayType.Click += new System.EventHandler(this.NewArrayType_Click);
            // 
            // UC_Options
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Button_Save);
            this.Name = "SettingsPanel";
            this.Load += new System.EventHandler(this.UC_Options_Load);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IndentionNumeric)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_LicenseeMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Version)).EndInit();
            this.groupBox5.ResumeLayout(false);
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
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ComboBox VariableType;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox VariableTypeGroup;
		private System.Windows.Forms.TreeView VariableTypesTree;
		private System.Windows.Forms.Button DeleteArrayType;
		private System.Windows.Forms.Button NewArrayType;
		private System.Windows.Forms.RichTextBox BracketPreview;
		private System.Windows.Forms.ToolTip toolTip1;

	}
}
