namespace UEExplorer.UI.Dialogs
{
	partial class UserControl_HexView
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
			System.Windows.Forms.Label label1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_HexView));
			System.Windows.Forms.SplitContainer splitContainer1;
			System.Windows.Forms.Panel panel1;
			System.Windows.Forms.Label label14;
			System.Windows.Forms.Label label13;
			System.Windows.Forms.Label label12;
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
			this.DissambledStruct = new System.Windows.Forms.TextBox();
			this.DissambledIndex = new System.Windows.Forms.TextBox();
			this.DissambledByteCode = new System.Windows.Forms.TextBox();
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
			this.HexLinePanel = new UEExplorer.UI.Dialogs.HexPanel();
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
			label1 = new System.Windows.Forms.Label();
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			panel1 = new System.Windows.Forms.Panel();
			label14 = new System.Windows.Forms.Label();
			label13 = new System.Windows.Forms.Label();
			label12 = new System.Windows.Forms.Label();
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
			((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			panel1.SuspendLayout();
			this.Context_Structure.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.ForeColor = System.Drawing.Color.DarkBlue;
			label1.Name = "label1";
			this.HexToolTip.SetToolTip(label1, resources.GetString("label1.ToolTip"));
			// 
			// splitContainer1
			// 
			resources.ApplyResources(splitContainer1, "splitContainer1");
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			resources.ApplyResources(splitContainer1.Panel1, "splitContainer1.Panel1");
			splitContainer1.Panel1.Controls.Add(panel1);
			this.HexToolTip.SetToolTip(splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
			// 
			// splitContainer1.Panel2
			// 
			resources.ApplyResources(splitContainer1.Panel2, "splitContainer1.Panel2");
			splitContainer1.Panel2.Controls.Add(this.HexLinePanel);
			this.HexToolTip.SetToolTip(splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
			splitContainer1.TabStop = false;
			this.HexToolTip.SetToolTip(splitContainer1, resources.GetString("splitContainer1.ToolTip"));
			// 
			// panel1
			// 
			resources.ApplyResources(panel1, "panel1");
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Controls.Add(label14);
			panel1.Controls.Add(this.DissambledStruct);
			panel1.Controls.Add(label13);
			panel1.Controls.Add(this.DissambledIndex);
			panel1.Controls.Add(label12);
			panel1.Controls.Add(this.DissambledByteCode);
			panel1.Controls.Add(this.DissambledName);
			panel1.Controls.Add(label11);
			panel1.Controls.Add(this.DissambledObject);
			panel1.Controls.Add(label10);
			panel1.Controls.Add(this.DissambledFloat);
			panel1.Controls.Add(label9);
			panel1.Controls.Add(this.DissambledULong);
			panel1.Controls.Add(label7);
			panel1.Controls.Add(this.DissambledLong);
			panel1.Controls.Add(label8);
			panel1.Controls.Add(this.DissambledUInt);
			panel1.Controls.Add(label6);
			panel1.Controls.Add(this.DissambledInt);
			panel1.Controls.Add(label5);
			panel1.Controls.Add(this.DissambledUShort);
			panel1.Controls.Add(label4);
			panel1.Controls.Add(this.DissambledShort);
			panel1.Controls.Add(label3);
			panel1.Controls.Add(label2);
			panel1.Controls.Add(this.DissambledByte);
			panel1.Controls.Add(label1);
			panel1.Controls.Add(this.DissambledChar);
			panel1.Name = "panel1";
			this.HexToolTip.SetToolTip(panel1, resources.GetString("panel1.ToolTip"));
			// 
			// label14
			// 
			resources.ApplyResources(label14, "label14");
			label14.ForeColor = System.Drawing.Color.SaddleBrown;
			label14.Name = "label14";
			this.HexToolTip.SetToolTip(label14, resources.GetString("label14.ToolTip"));
			// 
			// DissambledStruct
			// 
			resources.ApplyResources(this.DissambledStruct, "DissambledStruct");
			this.DissambledStruct.Name = "DissambledStruct";
			this.DissambledStruct.ReadOnly = true;
			this.DissambledStruct.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledStruct, resources.GetString("DissambledStruct.ToolTip"));
			// 
			// label13
			// 
			resources.ApplyResources(label13, "label13");
			label13.ForeColor = System.Drawing.Color.MediumOrchid;
			label13.Name = "label13";
			this.HexToolTip.SetToolTip(label13, resources.GetString("label13.ToolTip"));
			// 
			// DissambledIndex
			// 
			resources.ApplyResources(this.DissambledIndex, "DissambledIndex");
			this.DissambledIndex.Name = "DissambledIndex";
			this.DissambledIndex.ReadOnly = true;
			this.DissambledIndex.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledIndex, resources.GetString("DissambledIndex.ToolTip"));
			// 
			// label12
			// 
			resources.ApplyResources(label12, "label12");
			label12.ForeColor = System.Drawing.Color.Firebrick;
			label12.Name = "label12";
			this.HexToolTip.SetToolTip(label12, resources.GetString("label12.ToolTip"));
			// 
			// DissambledByteCode
			// 
			resources.ApplyResources(this.DissambledByteCode, "DissambledByteCode");
			this.DissambledByteCode.Name = "DissambledByteCode";
			this.DissambledByteCode.ReadOnly = true;
			this.DissambledByteCode.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledByteCode, resources.GetString("DissambledByteCode.ToolTip"));
			// 
			// DissambledName
			// 
			resources.ApplyResources(this.DissambledName, "DissambledName");
			this.DissambledName.Name = "DissambledName";
			this.DissambledName.ReadOnly = true;
			this.DissambledName.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledName, resources.GetString("DissambledName.ToolTip"));
			// 
			// label11
			// 
			resources.ApplyResources(label11, "label11");
			label11.ForeColor = System.Drawing.Color.Green;
			label11.Name = "label11";
			this.HexToolTip.SetToolTip(label11, resources.GetString("label11.ToolTip"));
			// 
			// DissambledObject
			// 
			resources.ApplyResources(this.DissambledObject, "DissambledObject");
			this.DissambledObject.Name = "DissambledObject";
			this.DissambledObject.ReadOnly = true;
			this.DissambledObject.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledObject, resources.GetString("DissambledObject.ToolTip"));
			// 
			// label10
			// 
			resources.ApplyResources(label10, "label10");
			label10.ForeColor = System.Drawing.Color.DarkTurquoise;
			label10.Name = "label10";
			this.HexToolTip.SetToolTip(label10, resources.GetString("label10.ToolTip"));
			// 
			// DissambledFloat
			// 
			resources.ApplyResources(this.DissambledFloat, "DissambledFloat");
			this.DissambledFloat.Name = "DissambledFloat";
			this.DissambledFloat.ReadOnly = true;
			this.DissambledFloat.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledFloat, resources.GetString("DissambledFloat.ToolTip"));
			// 
			// label9
			// 
			resources.ApplyResources(label9, "label9");
			label9.ForeColor = System.Drawing.Color.SlateBlue;
			label9.Name = "label9";
			this.HexToolTip.SetToolTip(label9, resources.GetString("label9.ToolTip"));
			// 
			// DissambledULong
			// 
			resources.ApplyResources(this.DissambledULong, "DissambledULong");
			this.DissambledULong.Name = "DissambledULong";
			this.DissambledULong.ReadOnly = true;
			this.DissambledULong.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledULong, resources.GetString("DissambledULong.ToolTip"));
			// 
			// label7
			// 
			resources.ApplyResources(label7, "label7");
			label7.ForeColor = System.Drawing.Color.Purple;
			label7.Name = "label7";
			this.HexToolTip.SetToolTip(label7, resources.GetString("label7.ToolTip"));
			// 
			// DissambledLong
			// 
			resources.ApplyResources(this.DissambledLong, "DissambledLong");
			this.DissambledLong.Name = "DissambledLong";
			this.DissambledLong.ReadOnly = true;
			this.DissambledLong.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledLong, resources.GetString("DissambledLong.ToolTip"));
			// 
			// label8
			// 
			resources.ApplyResources(label8, "label8");
			label8.ForeColor = System.Drawing.Color.Purple;
			label8.Name = "label8";
			this.HexToolTip.SetToolTip(label8, resources.GetString("label8.ToolTip"));
			// 
			// DissambledUInt
			// 
			resources.ApplyResources(this.DissambledUInt, "DissambledUInt");
			this.DissambledUInt.Name = "DissambledUInt";
			this.DissambledUInt.ReadOnly = true;
			this.DissambledUInt.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledUInt, resources.GetString("DissambledUInt.ToolTip"));
			// 
			// label6
			// 
			resources.ApplyResources(label6, "label6");
			label6.ForeColor = System.Drawing.Color.DodgerBlue;
			label6.Name = "label6";
			this.HexToolTip.SetToolTip(label6, resources.GetString("label6.ToolTip"));
			// 
			// DissambledInt
			// 
			resources.ApplyResources(this.DissambledInt, "DissambledInt");
			this.DissambledInt.Name = "DissambledInt";
			this.DissambledInt.ReadOnly = true;
			this.DissambledInt.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledInt, resources.GetString("DissambledInt.ToolTip"));
			// 
			// label5
			// 
			resources.ApplyResources(label5, "label5");
			label5.ForeColor = System.Drawing.Color.DodgerBlue;
			label5.Name = "label5";
			this.HexToolTip.SetToolTip(label5, resources.GetString("label5.ToolTip"));
			// 
			// DissambledUShort
			// 
			resources.ApplyResources(this.DissambledUShort, "DissambledUShort");
			this.DissambledUShort.Name = "DissambledUShort";
			this.DissambledUShort.ReadOnly = true;
			this.DissambledUShort.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledUShort, resources.GetString("DissambledUShort.ToolTip"));
			// 
			// label4
			// 
			resources.ApplyResources(label4, "label4");
			label4.ForeColor = System.Drawing.Color.MediumBlue;
			label4.Name = "label4";
			this.HexToolTip.SetToolTip(label4, resources.GetString("label4.ToolTip"));
			// 
			// DissambledShort
			// 
			resources.ApplyResources(this.DissambledShort, "DissambledShort");
			this.DissambledShort.Name = "DissambledShort";
			this.DissambledShort.ReadOnly = true;
			this.DissambledShort.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledShort, resources.GetString("DissambledShort.ToolTip"));
			// 
			// label3
			// 
			resources.ApplyResources(label3, "label3");
			label3.ForeColor = System.Drawing.Color.MediumBlue;
			label3.Name = "label3";
			this.HexToolTip.SetToolTip(label3, resources.GetString("label3.ToolTip"));
			// 
			// label2
			// 
			resources.ApplyResources(label2, "label2");
			label2.ForeColor = System.Drawing.Color.Peru;
			label2.Name = "label2";
			this.HexToolTip.SetToolTip(label2, resources.GetString("label2.ToolTip"));
			// 
			// DissambledByte
			// 
			resources.ApplyResources(this.DissambledByte, "DissambledByte");
			this.DissambledByte.Name = "DissambledByte";
			this.DissambledByte.ReadOnly = true;
			this.DissambledByte.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledByte, resources.GetString("DissambledByte.ToolTip"));
			// 
			// DissambledChar
			// 
			resources.ApplyResources(this.DissambledChar, "DissambledChar");
			this.DissambledChar.Name = "DissambledChar";
			this.DissambledChar.ReadOnly = true;
			this.DissambledChar.TabStop = false;
			this.HexToolTip.SetToolTip(this.DissambledChar, resources.GetString("DissambledChar.ToolTip"));
			// 
			// HexLinePanel
			// 
			resources.ApplyResources(this.HexLinePanel, "HexLinePanel");
			this.HexLinePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.HexLinePanel.ContextMenuStrip = this.Context_Structure;
			this.HexLinePanel.Name = "HexLinePanel";
			this.HexToolTip.SetToolTip(this.HexLinePanel, resources.GetString("HexLinePanel.ToolTip"));
			this.HexLinePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HexLinePanel_Paint);
			this.HexLinePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseClick);
			this.HexLinePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HexLinePanel_MouseMove);
			// 
			// Context_Structure
			// 
			resources.ApplyResources(this.Context_Structure, "Context_Structure");
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
			this.HexToolTip.SetToolTip(this.Context_Structure, resources.GetString("Context_Structure.ToolTip"));
			this.Context_Structure.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Context_Structure_ItemClicked);
			// 
			// defineCharToolStripMenuItem
			// 
			resources.ApplyResources(this.defineCharToolStripMenuItem, "defineCharToolStripMenuItem");
			this.defineCharToolStripMenuItem.Name = "defineCharToolStripMenuItem";
			// 
			// defineByteToolStripMenuItem
			// 
			resources.ApplyResources(this.defineByteToolStripMenuItem, "defineByteToolStripMenuItem");
			this.defineByteToolStripMenuItem.Name = "defineByteToolStripMenuItem";
			// 
			// defineShortToolStripMenuItem
			// 
			resources.ApplyResources(this.defineShortToolStripMenuItem, "defineShortToolStripMenuItem");
			this.defineShortToolStripMenuItem.Name = "defineShortToolStripMenuItem";
			// 
			// defineIntToolStripMenuItem
			// 
			resources.ApplyResources(this.defineIntToolStripMenuItem, "defineIntToolStripMenuItem");
			this.defineIntToolStripMenuItem.Name = "defineIntToolStripMenuItem";
			// 
			// defineLongToolStripMenuItem
			// 
			resources.ApplyResources(this.defineLongToolStripMenuItem, "defineLongToolStripMenuItem");
			this.defineLongToolStripMenuItem.Name = "defineLongToolStripMenuItem";
			// 
			// defineFloatToolStripMenuItem
			// 
			resources.ApplyResources(this.defineFloatToolStripMenuItem, "defineFloatToolStripMenuItem");
			this.defineFloatToolStripMenuItem.Name = "defineFloatToolStripMenuItem";
			// 
			// defineObjectToolStripMenuItem
			// 
			resources.ApplyResources(this.defineObjectToolStripMenuItem, "defineObjectToolStripMenuItem");
			this.defineObjectToolStripMenuItem.Name = "defineObjectToolStripMenuItem";
			// 
			// defineNameToolStripMenuItem
			// 
			resources.ApplyResources(this.defineNameToolStripMenuItem, "defineNameToolStripMenuItem");
			this.defineNameToolStripMenuItem.Name = "defineNameToolStripMenuItem";
			// 
			// defineCodeToolStripMenuItem
			// 
			resources.ApplyResources(this.defineCodeToolStripMenuItem, "defineCodeToolStripMenuItem");
			this.defineCodeToolStripMenuItem.Name = "defineCodeToolStripMenuItem";
			// 
			// defineIndexToolStripMenuItem
			// 
			resources.ApplyResources(this.defineIndexToolStripMenuItem, "defineIndexToolStripMenuItem");
			this.defineIndexToolStripMenuItem.Name = "defineIndexToolStripMenuItem";
			// 
			// HexScrollBar
			// 
			resources.ApplyResources(this.HexScrollBar, "HexScrollBar");
			this.HexScrollBar.Name = "HexScrollBar";
			this.HexToolTip.SetToolTip(this.HexScrollBar, resources.GetString("HexScrollBar.ToolTip"));
			this.HexScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HexScrollBar_Scroll);
			// 
			// UserControl_HexView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.HexScrollBar);
			this.Controls.Add(splitContainer1);
			this.Name = "UserControl_HexView";
			this.HexToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
			splitContainer1.ResumeLayout(false);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			this.Context_Structure.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar HexScrollBar;
		private HexPanel HexLinePanel;
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
		private System.Windows.Forms.TextBox DissambledByteCode;
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
	}
}
