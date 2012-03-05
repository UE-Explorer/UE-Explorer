namespace UEExplorer.UI.Tabs
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
		private void InitializeComponent()
		{
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.TabPage_Packages = new System.Windows.Forms.TabPage();
			this.TreeView_Packages = new System.Windows.Forms.TreeView();
			this.textBox_Name = new System.Windows.Forms.TextBox();
			this.Button_Add = new System.Windows.Forms.Button();
			this.Button_Save = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.TabPage_Packages.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.tabControl1.Controls.Add( this.TabPage_Packages );
			this.tabControl1.Location = new System.Drawing.Point( 0, 79 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 510, 471 );
			this.tabControl1.TabIndex = 0;
			// 
			// TabPage_Packages
			// 
			this.TabPage_Packages.Controls.Add( this.TreeView_Packages );
			this.TabPage_Packages.Location = new System.Drawing.Point( 4, 22 );
			this.TabPage_Packages.Name = "TabPage_Packages";
			this.TabPage_Packages.Padding = new System.Windows.Forms.Padding( 3 );
			this.TabPage_Packages.Size = new System.Drawing.Size( 502, 445 );
			this.TabPage_Packages.TabIndex = 0;
			this.TabPage_Packages.Text = "Packages";
			this.TabPage_Packages.UseVisualStyleBackColor = true;
			// 
			// TreeView_Packages
			// 
			this.TreeView_Packages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TreeView_Packages.Location = new System.Drawing.Point( 3, 3 );
			this.TreeView_Packages.Name = "TreeView_Packages";
			this.TreeView_Packages.Size = new System.Drawing.Size( 496, 439 );
			this.TreeView_Packages.TabIndex = 0;
			// 
			// textBox_Name
			// 
			this.textBox_Name.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.textBox_Name.Location = new System.Drawing.Point( 112, 57 );
			this.textBox_Name.Name = "textBox_Name";
			this.textBox_Name.Size = new System.Drawing.Size( 391, 20 );
			this.textBox_Name.TabIndex = 1;
			this.textBox_Name.Text = "UT2004";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point( 4, 60 );
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size( 102, 13 );
			label1.TabIndex = 2;
			label1.Text = "\\\\NativesTableList_";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point( 8, 40 );
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size( 54, 13 );
			label2.TabIndex = 3;
			label2.Text = "File Name";
			// 
			// Button_Add
			// 
			this.Button_Add.Location = new System.Drawing.Point( 3, 3 );
			this.Button_Add.Name = "Button_Add";
			this.Button_Add.Size = new System.Drawing.Size( 99, 23 );
			this.Button_Add.TabIndex = 4;
			this.Button_Add.Text = "Load Package";
			this.Button_Add.UseVisualStyleBackColor = true;
			// 
			// Button_Save
			// 
			this.Button_Save.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.Button_Save.Location = new System.Drawing.Point( 384, 3 );
			this.Button_Save.Name = "Button_Save";
			this.Button_Save.Size = new System.Drawing.Size( 123, 23 );
			this.Button_Save.TabIndex = 5;
			this.Button_Save.Text = "Save Natives Table";
			this.Button_Save.UseVisualStyleBackColor = true;
			// 
			// UserControl_UNativeGen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.Button_Save );
			this.Controls.Add( this.Button_Add );
			this.Controls.Add( label2 );
			this.Controls.Add( label1 );
			this.Controls.Add( this.textBox_Name );
			this.Controls.Add( this.tabControl1 );
			this.Name = "UserControl_UNativeGen";
			this.Size = new System.Drawing.Size( 510, 550 );
			this.tabControl1.ResumeLayout( false );
			this.TabPage_Packages.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabPage TabPage_Packages;
		public System.Windows.Forms.TabControl tabControl1;
		public System.Windows.Forms.TreeView TreeView_Packages;
		public System.Windows.Forms.Button Button_Add;
		public System.Windows.Forms.Button Button_Save;
		public System.Windows.Forms.TextBox textBox_Name;
	}
}
