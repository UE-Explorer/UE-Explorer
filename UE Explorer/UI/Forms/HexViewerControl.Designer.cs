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
            this.HexLinePanel = new UEExplorer.UI.Forms.HexViewerPanel();
            this.Context_Structure = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.defineCharToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineByteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineShortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineIntToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineLongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineFloatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HexScrollBar = new System.Windows.Forms.VScrollBar();
            this.HexToolTip = new System.Windows.Forms.ToolTip(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.DataInfoPanel.SuspendLayout();
            this.Context_Structure.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::UEExplorer.Properties.Settings.Default, "HexPanel_SplitterDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.DataInfoPanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.HexLinePanel);
            splitContainer1.SplitterDistance = global::UEExplorer.Properties.Settings.Default.HexPanel_SplitterDistance;
            splitContainer1.TabStop = false;
            splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer1_SplitterMoved);
            // 
            // DataInfoPanel
            // 
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
            resources.ApplyResources(this.DataInfoPanel, "DataInfoPanel");
            this.DataInfoPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DataInfoPanel.Name = "DataInfoPanel";
            this.DataInfoPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DataInfoPanel_Paint);
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
            this.DissambledStruct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledStruct.Name = "DissambledStruct";
            this.DissambledStruct.ReadOnly = true;
            this.DissambledStruct.TabStop = false;
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
            this.DissambledIndex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledIndex.Name = "DissambledIndex";
            this.DissambledIndex.ReadOnly = true;
            this.DissambledIndex.TabStop = false;
            // 
            // DissambledName
            // 
            resources.ApplyResources(this.DissambledName, "DissambledName");
            this.DissambledName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledName.Name = "DissambledName";
            this.DissambledName.ReadOnly = true;
            this.DissambledName.TabStop = false;
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
            this.DissambledObject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledObject.Name = "DissambledObject";
            this.DissambledObject.ReadOnly = true;
            this.DissambledObject.TabStop = false;
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
            this.DissambledFloat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledFloat.Name = "DissambledFloat";
            this.DissambledFloat.ReadOnly = true;
            this.DissambledFloat.TabStop = false;
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
            this.DissambledULong.ForeColor = System.Drawing.Color.Silver;
            this.DissambledULong.Name = "DissambledULong";
            this.DissambledULong.ReadOnly = true;
            this.DissambledULong.TabStop = false;
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
            this.DissambledLong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledLong.Name = "DissambledLong";
            this.DissambledLong.ReadOnly = true;
            this.DissambledLong.TabStop = false;
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
            this.DissambledUInt.ForeColor = System.Drawing.Color.Silver;
            this.DissambledUInt.Name = "DissambledUInt";
            this.DissambledUInt.ReadOnly = true;
            this.DissambledUInt.TabStop = false;
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
            this.DissambledInt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledInt.Name = "DissambledInt";
            this.DissambledInt.ReadOnly = true;
            this.DissambledInt.TabStop = false;
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
            this.DissambledUShort.ForeColor = System.Drawing.Color.Silver;
            this.DissambledUShort.Name = "DissambledUShort";
            this.DissambledUShort.ReadOnly = true;
            this.DissambledUShort.TabStop = false;
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
            this.DissambledShort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledShort.Name = "DissambledShort";
            this.DissambledShort.ReadOnly = true;
            this.DissambledShort.TabStop = false;
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
            this.DissambledByte.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledByte.Name = "DissambledByte";
            this.DissambledByte.ReadOnly = true;
            this.DissambledByte.TabStop = false;
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
            this.DissambledChar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DissambledChar.Name = "DissambledChar";
            this.DissambledChar.ReadOnly = true;
            this.DissambledChar.TabStop = false;
            // 
            // HexLinePanel
            // 
            this.HexLinePanel.ContextMenuStrip = this.Context_Structure;
            resources.ApplyResources(this.HexLinePanel, "HexLinePanel");
            this.HexLinePanel.Name = "HexLinePanel";
            this.HexLinePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HexLinePanel_Paint);
            this.HexLinePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseClick);
            this.HexLinePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseMove);
            // 
            // Context_Structure
            // 
            this.Context_Structure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defineCharToolStripMenuItem,
            this.defineByteToolStripMenuItem,
            this.defineShortToolStripMenuItem,
            this.defineIntToolStripMenuItem,
            this.defineLongToolStripMenuItem,
            this.defineFloatToolStripMenuItem,
            this.defineObjectToolStripMenuItem,
            this.defineNameToolStripMenuItem,
            this.defineCodeToolStripMenuItem,
            this.defineIndexToolStripMenuItem});
            this.Context_Structure.Name = "Context_Structure";
            resources.ApplyResources(this.Context_Structure, "Context_Structure");
            this.Context_Structure.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Context_Structure_ItemClicked);
            // 
            // defineCharToolStripMenuItem
            // 
            this.defineCharToolStripMenuItem.Name = "defineCharToolStripMenuItem";
            resources.ApplyResources(this.defineCharToolStripMenuItem, "defineCharToolStripMenuItem");
            // 
            // defineByteToolStripMenuItem
            // 
            this.defineByteToolStripMenuItem.Name = "defineByteToolStripMenuItem";
            resources.ApplyResources(this.defineByteToolStripMenuItem, "defineByteToolStripMenuItem");
            // 
            // defineShortToolStripMenuItem
            // 
            this.defineShortToolStripMenuItem.Name = "defineShortToolStripMenuItem";
            resources.ApplyResources(this.defineShortToolStripMenuItem, "defineShortToolStripMenuItem");
            // 
            // defineIntToolStripMenuItem
            // 
            this.defineIntToolStripMenuItem.Name = "defineIntToolStripMenuItem";
            resources.ApplyResources(this.defineIntToolStripMenuItem, "defineIntToolStripMenuItem");
            // 
            // defineLongToolStripMenuItem
            // 
            this.defineLongToolStripMenuItem.Name = "defineLongToolStripMenuItem";
            resources.ApplyResources(this.defineLongToolStripMenuItem, "defineLongToolStripMenuItem");
            // 
            // defineFloatToolStripMenuItem
            // 
            this.defineFloatToolStripMenuItem.Name = "defineFloatToolStripMenuItem";
            resources.ApplyResources(this.defineFloatToolStripMenuItem, "defineFloatToolStripMenuItem");
            // 
            // defineObjectToolStripMenuItem
            // 
            this.defineObjectToolStripMenuItem.Name = "defineObjectToolStripMenuItem";
            resources.ApplyResources(this.defineObjectToolStripMenuItem, "defineObjectToolStripMenuItem");
            // 
            // defineNameToolStripMenuItem
            // 
            this.defineNameToolStripMenuItem.Name = "defineNameToolStripMenuItem";
            resources.ApplyResources(this.defineNameToolStripMenuItem, "defineNameToolStripMenuItem");
            // 
            // defineCodeToolStripMenuItem
            // 
            this.defineCodeToolStripMenuItem.Name = "defineCodeToolStripMenuItem";
            resources.ApplyResources(this.defineCodeToolStripMenuItem, "defineCodeToolStripMenuItem");
            // 
            // defineIndexToolStripMenuItem
            // 
            this.defineIndexToolStripMenuItem.Name = "defineIndexToolStripMenuItem";
            resources.ApplyResources(this.defineIndexToolStripMenuItem, "defineIndexToolStripMenuItem");
            // 
            // HexScrollBar
            // 
            resources.ApplyResources(this.HexScrollBar, "HexScrollBar");
            this.HexScrollBar.Name = "HexScrollBar";
            this.HexScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HexScrollBar_Scroll);
            // 
            // UserControl_HexView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.HexScrollBar);
            this.Controls.Add(splitContainer1);
            resources.ApplyResources(this, "$this");
            this.Name = "UserControl_HexView";
            this.Resize += new System.EventHandler(this.UserControl_HexView_Resize);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            this.DataInfoPanel.ResumeLayout(false);
            this.DataInfoPanel.PerformLayout();
            this.Context_Structure.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar HexScrollBar;
		private HexViewerPanel HexLinePanel;
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
		private System.Windows.Forms.ToolStripMenuItem defineCharToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineByteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineShortToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineIntToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineLongToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineFloatToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineObjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineNameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defineIndexToolStripMenuItem;
		private System.Windows.Forms.TextBox DissambledStruct;
		private System.Windows.Forms.ToolTip HexToolTip;
        private System.Windows.Forms.Panel DataInfoPanel;
	}
}
