namespace UE_Extensions
{
	using UEExplorer.UI;

	partial class UC_ExecGenerator : UserControl_Tab
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.TabPage_Packages = new System.Windows.Forms.TabPage();
			this.TreeView_Packages = new System.Windows.Forms.TreeView();
			this.Button_Add = new System.Windows.Forms.Button();
			this.Button_Save = new System.Windows.Forms.Button();
			this.TextBox_GamePrefix = new System.Windows.Forms.TextBox();
			this.TextBox_EnginePrefix = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.TabPage_Packages.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add( this.TabPage_Packages );
			this.tabControl1.Location = new System.Drawing.Point( 0, 84 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 1017, 476 );
			this.tabControl1.TabIndex = 0;
			// 
			// TabPage_Packages
			// 
			this.TabPage_Packages.Controls.Add( this.TreeView_Packages );
			this.TabPage_Packages.Location = new System.Drawing.Point( 4, 22 );
			this.TabPage_Packages.Name = "TabPage_Packages";
			this.TabPage_Packages.Padding = new System.Windows.Forms.Padding( 3 );
			this.TabPage_Packages.Size = new System.Drawing.Size( 1009, 450 );
			this.TabPage_Packages.TabIndex = 0;
			this.TabPage_Packages.Text = "Loaded Packages";
			this.TabPage_Packages.UseVisualStyleBackColor = true;
			// 
			// TreeView_Packages
			// 
			this.TreeView_Packages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TreeView_Packages.Location = new System.Drawing.Point( 3, 3 );
			this.TreeView_Packages.Name = "TreeView_Packages";
			this.TreeView_Packages.Size = new System.Drawing.Size( 1003, 444 );
			this.TreeView_Packages.TabIndex = 0;
			// 
			// Button_Add
			// 
			this.Button_Add.Location = new System.Drawing.Point( 3, 3 );
			this.Button_Add.Name = "Button_Add";
			this.Button_Add.Size = new System.Drawing.Size( 99, 23 );
			this.Button_Add.TabIndex = 4;
			this.Button_Add.Text = "Load Packages";
			this.Button_Add.UseVisualStyleBackColor = true;
			this.Button_Add.Click += new System.EventHandler( this.Button_Add_Click );
			// 
			// Button_Save
			// 
			this.Button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Save.Location = new System.Drawing.Point( 887, 58 );
			this.Button_Save.Name = "Button_Save";
			this.Button_Save.Size = new System.Drawing.Size( 123, 23 );
			this.Button_Save.TabIndex = 5;
			this.Button_Save.Text = "Generate WikiArticle";
			this.Button_Save.UseVisualStyleBackColor = true;
			this.Button_Save.Click += new System.EventHandler( this.Button_Save_Click );
			// 
			// TextBox_GamePrefix
			// 
			this.TextBox_GamePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TextBox_GamePrefix.Location = new System.Drawing.Point( 910, 6 );
			this.TextBox_GamePrefix.Name = "TextBox_GamePrefix";
			this.TextBox_GamePrefix.Size = new System.Drawing.Size( 100, 20 );
			this.TextBox_GamePrefix.TabIndex = 6;
			this.TextBox_GamePrefix.Text = "UT2004";
			// 
			// TextBox_EnginePrefix
			// 
			this.TextBox_EnginePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TextBox_EnginePrefix.Location = new System.Drawing.Point( 910, 32 );
			this.TextBox_EnginePrefix.Name = "TextBox_EnginePrefix";
			this.TextBox_EnginePrefix.Size = new System.Drawing.Size( 100, 20 );
			this.TextBox_EnginePrefix.TabIndex = 7;
			this.TextBox_EnginePrefix.Text = "UE2";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 840, 13 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 64, 13 );
			this.label1.TabIndex = 8;
			this.label1.Text = "Game Prefix";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 835, 35 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 69, 13 );
			this.label2.TabIndex = 9;
			this.label2.Text = "Engine Prefix";
			// 
			// UserControl_UExecGen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.TextBox_EnginePrefix );
			this.Controls.Add( this.TextBox_GamePrefix );
			this.Controls.Add( this.Button_Save );
			this.Controls.Add( this.Button_Add );
			this.Controls.Add( this.tabControl1 );
			this.Name = "UserControl_UExecGen";
			this.Size = new System.Drawing.Size( 1017, 560 );
			this.tabControl1.ResumeLayout( false );
			this.TabPage_Packages.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabPage TabPage_Packages;
		internal System.Windows.Forms.TabControl tabControl1;
		internal System.Windows.Forms.TreeView TreeView_Packages;
		internal System.Windows.Forms.Button Button_Add;
		internal System.Windows.Forms.Button Button_Save;
		private System.Windows.Forms.TextBox TextBox_GamePrefix;
		private System.Windows.Forms.TextBox TextBox_EnginePrefix;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}
