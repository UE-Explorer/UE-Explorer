namespace Eliot.Extensions.NativesTableListGenerator
{
	partial class UC_NativeGenerator
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.GroupBox groupBox2;
			System.Windows.Forms.SplitContainer splitContainer1;
			this.TreeView_Packages = new System.Windows.Forms.TreeView();
			this.Button_Add = new System.Windows.Forms.Button();
			this.Button_Save = new System.Windows.Forms.Button();
			this.FileNameTextBox = new System.Windows.Forms.TextBox();
			this.OpenNTLDialog = new System.Windows.Forms.OpenFileDialog();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.ForeColor = System.Drawing.Color.DimGray;
			label1.Location = new System.Drawing.Point(66, 16);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(96, 13);
			label1.TabIndex = 2;
			label1.Text = "*NativesTableList_";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(6, 16);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(54, 13);
			label2.TabIndex = 3;
			label2.Text = "File Name";
			// 
			// groupBox1
			// 
			groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupBox1.Controls.Add(this.TreeView_Packages);
			groupBox1.Controls.Add(this.Button_Add);
			groupBox1.Location = new System.Drawing.Point(3, 4);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(431, 494);
			groupBox1.TabIndex = 5;
			groupBox1.TabStop = false;
			groupBox1.Text = "Scanned Packages";
			// 
			// TreeView_Packages
			// 
			this.TreeView_Packages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TreeView_Packages.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Packages.Location = new System.Drawing.Point(6, 14);
			this.TreeView_Packages.Name = "TreeView_Packages";
			this.TreeView_Packages.Size = new System.Drawing.Size(419, 445);
			this.TreeView_Packages.TabIndex = 0;
			// 
			// Button_Add
			// 
			this.Button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Add.Location = new System.Drawing.Point(6, 465);
			this.Button_Add.Name = "Button_Add";
			this.Button_Add.Size = new System.Drawing.Size(419, 23);
			this.Button_Add.TabIndex = 4;
			this.Button_Add.Text = "Scan Package";
			this.Button_Add.UseVisualStyleBackColor = true;
			this.Button_Add.Click += new System.EventHandler(this.Button_Add_Click);
			// 
			// groupBox2
			// 
			groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupBox2.Controls.Add(label2);
			groupBox2.Controls.Add(this.Button_Save);
			groupBox2.Controls.Add(this.FileNameTextBox);
			groupBox2.Controls.Add(label1);
			groupBox2.Location = new System.Drawing.Point(3, 4);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(865, 93);
			groupBox2.TabIndex = 6;
			groupBox2.TabStop = false;
			groupBox2.Text = "Natives Table List Package";
			// 
			// Button_Save
			// 
			this.Button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Button_Save.Enabled = false;
			this.Button_Save.Location = new System.Drawing.Point(6, 64);
			this.Button_Save.Name = "Button_Save";
			this.Button_Save.Size = new System.Drawing.Size(151, 23);
			this.Button_Save.TabIndex = 5;
			this.Button_Save.Text = "Save Natives Table List";
			this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
			// 
			// FileNameTextBox
			// 
			this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FileNameTextBox.Enabled = false;
			this.FileNameTextBox.Location = new System.Drawing.Point(168, 14);
			this.FileNameTextBox.Name = "FileNameTextBox";
			this.FileNameTextBox.Size = new System.Drawing.Size(691, 20);
			this.FileNameTextBox.TabIndex = 1;
			this.FileNameTextBox.Text = "Package Name POSTFIX";
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer1.Location = new System.Drawing.Point(0, 0);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(groupBox1);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(groupBox2);
			splitContainer1.Size = new System.Drawing.Size(1312, 501);
			splitContainer1.SplitterDistance = 437;
			splitContainer1.TabIndex = 6;
			// 
			// OpenNTLDialog
			// 
			this.OpenNTLDialog.DefaultExt = "u";
			this.OpenNTLDialog.Filter = "UnrealScript(*.u)|*.u";
			this.OpenNTLDialog.Multiselect = true;
			this.OpenNTLDialog.Title = "NTL File Dialog";
			// 
			// UC_NativeGenerator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(splitContainer1);
			this.Name = "UC_NativeGenerator";
			this.Size = new System.Drawing.Size(1312, 501);
			groupBox1.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
			splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Button Button_Add;
		private System.Windows.Forms.Button Button_Save;
		private System.Windows.Forms.TreeView TreeView_Packages;
		private System.Windows.Forms.TextBox FileNameTextBox;
		private System.Windows.Forms.OpenFileDialog OpenNTLDialog;
	}
}
