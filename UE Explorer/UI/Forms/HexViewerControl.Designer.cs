namespace UEExplorer.UI.Forms
{
	partial class HexViewerControl
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexViewerControl));
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel panel1;
            this.DataInfoPanel = new System.Windows.Forms.Panel();
            this.DissambledStruct = new System.Windows.Forms.TextBox();
            this.DissambledIndex = new System.Windows.Forms.TextBox();
            this.DissambledName = new System.Windows.Forms.TextBox();
            this.DissambledObject = new System.Windows.Forms.TextBox();
            this.DissambledFloat = new System.Windows.Forms.TextBox();
            this.DissambledULong = new System.Windows.Forms.TextBox();
            this.DissambledLong = new System.Windows.Forms.TextBox();
            this.DissambledUInt = new System.Windows.Forms.TextBox();
            this.DissambledInt = new System.Windows.Forms.TextBox();
            this.DissambledUShort = new System.Windows.Forms.TextBox();
            this.DissambledShort = new System.Windows.Forms.TextBox();
            this.DissambledByte = new System.Windows.Forms.TextBox();
            this.DissambledChar = new System.Windows.Forms.TextBox();
            this.Context_Structure = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editStructValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.structSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.HexViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.HexToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.defineStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HexViewPanel = new UEExplorer.UI.Forms.HexViewerPanel();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            label14 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.DataInfoPanel.SuspendLayout();
            panel1.SuspendLayout();
            this.Context_Structure.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::UEExplorer.Properties.Settings.Default, "HexPanel_SplitterDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.DataInfoPanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.SplitterDistance = global::UEExplorer.Properties.Settings.Default.HexPanel_SplitterDistance;
            splitContainer1.TabStop = false;
            splitContainer1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HexViewPanel_KeyDown);
            // 
            // DataInfoPanel
            // 
            resources.ApplyResources(this.DataInfoPanel, "DataInfoPanel");
            this.DataInfoPanel.Controls.Add(label14);
            this.DataInfoPanel.Controls.Add(this.DissambledStruct);
            this.DataInfoPanel.Controls.Add(label13);
            this.DataInfoPanel.Controls.Add(this.DissambledIndex);
            this.DataInfoPanel.Controls.Add(this.DissambledName);
            this.DataInfoPanel.Controls.Add(label11);
            this.DataInfoPanel.Controls.Add(this.DissambledObject);
            this.DataInfoPanel.Controls.Add(label10);
            this.DataInfoPanel.Controls.Add(this.DissambledFloat);
            this.DataInfoPanel.Controls.Add(label9);
            this.DataInfoPanel.Controls.Add(this.DissambledULong);
            this.DataInfoPanel.Controls.Add(label7);
            this.DataInfoPanel.Controls.Add(this.DissambledLong);
            this.DataInfoPanel.Controls.Add(label8);
            this.DataInfoPanel.Controls.Add(this.DissambledUInt);
            this.DataInfoPanel.Controls.Add(label6);
            this.DataInfoPanel.Controls.Add(this.DissambledInt);
            this.DataInfoPanel.Controls.Add(label5);
            this.DataInfoPanel.Controls.Add(this.DissambledUShort);
            this.DataInfoPanel.Controls.Add(label4);
            this.DataInfoPanel.Controls.Add(this.DissambledShort);
            this.DataInfoPanel.Controls.Add(label3);
            this.DataInfoPanel.Controls.Add(label2);
            this.DataInfoPanel.Controls.Add(this.DissambledByte);
            this.DataInfoPanel.Controls.Add(label1);
            this.DataInfoPanel.Controls.Add(this.DissambledChar);
            this.DataInfoPanel.Name = "DataInfoPanel";
            // 
            // label14
            // 
            resources.ApplyResources(label14, "label14");
            label14.ForeColor = System.Drawing.Color.SaddleBrown;
            label14.Name = "label14";
            // 
            // DissambledStruct
            // 
            resources.ApplyResources(this.DissambledStruct, "DissambledStruct");
            this.DissambledStruct.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledStruct.Name = "DissambledStruct";
            this.DissambledStruct.ReadOnly = true;
            // 
            // label13
            // 
            resources.ApplyResources(label13, "label13");
            label13.ForeColor = System.Drawing.Color.MediumOrchid;
            label13.Name = "label13";
            // 
            // DissambledIndex
            // 
            resources.ApplyResources(this.DissambledIndex, "DissambledIndex");
            this.DissambledIndex.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledIndex.Name = "DissambledIndex";
            this.DissambledIndex.ReadOnly = true;
            // 
            // DissambledName
            // 
            resources.ApplyResources(this.DissambledName, "DissambledName");
            this.DissambledName.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledName.Name = "DissambledName";
            this.DissambledName.ReadOnly = true;
            // 
            // label11
            // 
            resources.ApplyResources(label11, "label11");
            label11.ForeColor = System.Drawing.Color.Green;
            label11.Name = "label11";
            // 
            // DissambledObject
            // 
            resources.ApplyResources(this.DissambledObject, "DissambledObject");
            this.DissambledObject.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledObject.Name = "DissambledObject";
            this.DissambledObject.ReadOnly = true;
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.ForeColor = System.Drawing.Color.DarkTurquoise;
            label10.Name = "label10";
            // 
            // DissambledFloat
            // 
            resources.ApplyResources(this.DissambledFloat, "DissambledFloat");
            this.DissambledFloat.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledFloat.Name = "DissambledFloat";
            this.DissambledFloat.ReadOnly = true;
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.ForeColor = System.Drawing.Color.SlateBlue;
            label9.Name = "label9";
            // 
            // DissambledULong
            // 
            resources.ApplyResources(this.DissambledULong, "DissambledULong");
            this.DissambledULong.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledULong.Name = "DissambledULong";
            this.DissambledULong.ReadOnly = true;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.ForeColor = System.Drawing.Color.Purple;
            label7.Name = "label7";
            // 
            // DissambledLong
            // 
            resources.ApplyResources(this.DissambledLong, "DissambledLong");
            this.DissambledLong.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledLong.Name = "DissambledLong";
            this.DissambledLong.ReadOnly = true;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.ForeColor = System.Drawing.Color.Purple;
            label8.Name = "label8";
            // 
            // DissambledUInt
            // 
            resources.ApplyResources(this.DissambledUInt, "DissambledUInt");
            this.DissambledUInt.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledUInt.Name = "DissambledUInt";
            this.DissambledUInt.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.ForeColor = System.Drawing.Color.DodgerBlue;
            label6.Name = "label6";
            // 
            // DissambledInt
            // 
            resources.ApplyResources(this.DissambledInt, "DissambledInt");
            this.DissambledInt.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledInt.Name = "DissambledInt";
            this.DissambledInt.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.ForeColor = System.Drawing.Color.DodgerBlue;
            label5.Name = "label5";
            // 
            // DissambledUShort
            // 
            resources.ApplyResources(this.DissambledUShort, "DissambledUShort");
            this.DissambledUShort.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledUShort.Name = "DissambledUShort";
            this.DissambledUShort.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.ForeColor = System.Drawing.Color.MediumBlue;
            label4.Name = "label4";
            // 
            // DissambledShort
            // 
            resources.ApplyResources(this.DissambledShort, "DissambledShort");
            this.DissambledShort.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledShort.Name = "DissambledShort";
            this.DissambledShort.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.ForeColor = System.Drawing.Color.MediumBlue;
            label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.ForeColor = System.Drawing.Color.Peru;
            label2.Name = "label2";
            // 
            // DissambledByte
            // 
            resources.ApplyResources(this.DissambledByte, "DissambledByte");
            this.DissambledByte.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledByte.Name = "DissambledByte";
            this.DissambledByte.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.ForeColor = System.Drawing.Color.DarkBlue;
            label1.Name = "label1";
            // 
            // DissambledChar
            // 
            resources.ApplyResources(this.DissambledChar, "DissambledChar");
            this.DissambledChar.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DissambledChar.Name = "DissambledChar";
            this.DissambledChar.ReadOnly = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(this.HexViewPanel);
            panel1.Controls.Add(this.HexViewScrollBar);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // Context_Structure
            // 
            this.Context_Structure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCellToolStripMenuItem,
            this.cellToolStripMenuItem,
            this.toolStripSeparator1,
            this.defineStructToolStripMenuItem});
            this.Context_Structure.Name = "Context_Structure";
            resources.ApplyResources(this.Context_Structure, "Context_Structure");
            this.Context_Structure.Opening += new System.ComponentModel.CancelEventHandler(this.Context_Structure_Opening);
            // 
            // editCellToolStripMenuItem
            // 
            this.editCellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editStructValueToolStripMenuItem});
            resources.ApplyResources(this.editCellToolStripMenuItem, "editCellToolStripMenuItem");
            this.editCellToolStripMenuItem.Name = "editCellToolStripMenuItem";
            this.editCellToolStripMenuItem.Tag = "Cell";
            this.editCellToolStripMenuItem.Click += new System.EventHandler(this.editCellToolStripMenuItem_Click);
            // 
            // editStructValueToolStripMenuItem
            // 
            this.editStructValueToolStripMenuItem.Name = "editStructValueToolStripMenuItem";
            resources.ApplyResources(this.editStructValueToolStripMenuItem, "editStructValueToolStripMenuItem");
            this.editStructValueToolStripMenuItem.Tag = "Struct";
            this.editStructValueToolStripMenuItem.Click += new System.EventHandler(this.editStructValueToolStripMenuItem_Click);
            // 
            // cellToolStripMenuItem
            // 
            this.cellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexValueToolStripMenuItem,
            this.hexOffsetToolStripMenuItem,
            this.decimalValueToolStripMenuItem,
            this.decimalOffsetToolStripMenuItem,
            this.structNameToolStripMenuItem,
            this.structValueToolStripMenuItem,
            this.toolStripMenuItem1,
            this.structSizeToolStripMenuItem});
            resources.ApplyResources(this.cellToolStripMenuItem, "cellToolStripMenuItem");
            this.cellToolStripMenuItem.Name = "cellToolStripMenuItem";
            this.cellToolStripMenuItem.Tag = "Cell";
            // 
            // hexValueToolStripMenuItem
            // 
            this.hexValueToolStripMenuItem.Name = "hexValueToolStripMenuItem";
            resources.ApplyResources(this.hexValueToolStripMenuItem, "hexValueToolStripMenuItem");
            this.hexValueToolStripMenuItem.Tag = "Cell";
            this.hexValueToolStripMenuItem.Click += new System.EventHandler(this.hexValueToolStripMenuItem_Click);
            // 
            // hexOffsetToolStripMenuItem
            // 
            this.hexOffsetToolStripMenuItem.Name = "hexOffsetToolStripMenuItem";
            resources.ApplyResources(this.hexOffsetToolStripMenuItem, "hexOffsetToolStripMenuItem");
            this.hexOffsetToolStripMenuItem.Tag = "Cell";
            this.hexOffsetToolStripMenuItem.Click += new System.EventHandler(this.hexOffsetToolStripMenuItem_Click);
            // 
            // decimalValueToolStripMenuItem
            // 
            this.decimalValueToolStripMenuItem.Name = "decimalValueToolStripMenuItem";
            resources.ApplyResources(this.decimalValueToolStripMenuItem, "decimalValueToolStripMenuItem");
            this.decimalValueToolStripMenuItem.Tag = "Cell";
            this.decimalValueToolStripMenuItem.Click += new System.EventHandler(this.decimalValueToolStripMenuItem_Click);
            // 
            // decimalOffsetToolStripMenuItem
            // 
            this.decimalOffsetToolStripMenuItem.Name = "decimalOffsetToolStripMenuItem";
            resources.ApplyResources(this.decimalOffsetToolStripMenuItem, "decimalOffsetToolStripMenuItem");
            this.decimalOffsetToolStripMenuItem.Tag = "Cell";
            this.decimalOffsetToolStripMenuItem.Click += new System.EventHandler(this.decimalOffsetToolStripMenuItem_Click);
            // 
            // structNameToolStripMenuItem
            // 
            this.structNameToolStripMenuItem.Name = "structNameToolStripMenuItem";
            resources.ApplyResources(this.structNameToolStripMenuItem, "structNameToolStripMenuItem");
            this.structNameToolStripMenuItem.Tag = "Struct";
            this.structNameToolStripMenuItem.Click += new System.EventHandler(this.structNameToolStripMenuItem_Click);
            // 
            // structValueToolStripMenuItem
            // 
            this.structValueToolStripMenuItem.Name = "structValueToolStripMenuItem";
            resources.ApplyResources(this.structValueToolStripMenuItem, "structValueToolStripMenuItem");
            this.structValueToolStripMenuItem.Tag = "Struct";
            this.structValueToolStripMenuItem.Click += new System.EventHandler(this.structValueToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Tag = "Struct";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.structHexSizeToolStripMenuItem_Click);
            // 
            // structSizeToolStripMenuItem
            // 
            this.structSizeToolStripMenuItem.Name = "structSizeToolStripMenuItem";
            resources.ApplyResources(this.structSizeToolStripMenuItem, "structSizeToolStripMenuItem");
            this.structSizeToolStripMenuItem.Tag = "Struct";
            this.structSizeToolStripMenuItem.Click += new System.EventHandler(this.structDecimalSizeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // HexViewScrollBar
            // 
            resources.ApplyResources(this.HexViewScrollBar, "HexViewScrollBar");
            this.HexViewScrollBar.Name = "HexViewScrollBar";
            this.HexViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HexScrollBar_Scroll);
            // 
            // defineStructToolStripMenuItem
            // 
            this.defineStructToolStripMenuItem.Name = "defineStructToolStripMenuItem";
            resources.ApplyResources(this.defineStructToolStripMenuItem, "defineStructToolStripMenuItem");
            this.defineStructToolStripMenuItem.Tag = "Cell";
            this.defineStructToolStripMenuItem.Click += new System.EventHandler(this.defineStructToolStripMenuItem_Click);
            // 
            // HexViewPanel
            // 
            this.HexViewPanel.ContextMenuStrip = this.Context_Structure;
            resources.ApplyResources(this.HexViewPanel, "HexViewPanel");
            this.HexViewPanel.Name = "HexViewPanel";
            this.HexViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HexLinePanel_Paint);
            this.HexViewPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HexViewPanel_KeyDown);
            this.HexViewPanel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexViewPanel_KeyPress);
            this.HexViewPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HexViewPanel_KeyUp);
            this.HexViewPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseClick);
            this.HexViewPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseDoubleClick);
            this.HexViewPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseMove);
            // 
            // HexViewerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(splitContainer1);
            resources.ApplyResources(this, "$this");
            this.Name = "HexViewerControl";
            this.Load += new System.EventHandler(this.HexViewerControl_Load);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HexViewerControl_Scroll);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            this.DataInfoPanel.ResumeLayout(false);
            this.DataInfoPanel.PerformLayout();
            panel1.ResumeLayout(false);
            this.Context_Structure.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.VScrollBar HexViewScrollBar;
		private HexViewerPanel HexViewPanel;
		private System.Windows.Forms.TextBox DissambledChar;
		private System.Windows.Forms.TextBox DissambledByte;
		private System.Windows.Forms.TextBox DissambledShort;
		private System.Windows.Forms.TextBox DissambledInt;
		private System.Windows.Forms.TextBox DissambledUShort;
		private System.Windows.Forms.TextBox DissambledUInt;
		private System.Windows.Forms.TextBox DissambledFloat;
		private System.Windows.Forms.TextBox DissambledULong;
		private System.Windows.Forms.TextBox DissambledLong;
		private System.Windows.Forms.TextBox DissambledName;
		private System.Windows.Forms.TextBox DissambledObject;
		private System.Windows.Forms.TextBox DissambledIndex;
		private System.Windows.Forms.ContextMenuStrip Context_Structure;
		private System.Windows.Forms.TextBox DissambledStruct;
		private System.Windows.Forms.ToolTip HexToolTip;
        private System.Windows.Forms.Panel DataInfoPanel;
        private System.Windows.Forms.ToolStripMenuItem editCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cellToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem hexValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editStructValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defineStructToolStripMenuItem;
    }
}
