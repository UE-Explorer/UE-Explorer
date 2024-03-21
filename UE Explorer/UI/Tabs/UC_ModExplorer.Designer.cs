namespace UEExplorer.UI.Tabs
{
	partial class UC_ModExplorer : UserControl_Tab
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
		protected void InitializeComponent()
		{
			System.Windows.Forms.TableLayoutPanel TabLayout;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( UC_ModExplorer ) );
			this.Panel_Main = new System.Windows.Forms.Panel();
			this.TabControl_General = new System.Windows.Forms.TabControl();
			this.TabPage_Package = new System.Windows.Forms.TabPage();
			this.Label_Version = new System.Windows.Forms.Label();
			this.TabPage_Tables = new System.Windows.Forms.TabPage();
			this.TabControl_Tables = new System.Windows.Forms.TabControl();
			this.TabPage_Files = new System.Windows.Forms.TabPage();
			this.TreeView_Files = new System.Windows.Forms.TreeView();
			this.Panel_Content = new System.Windows.Forms.Panel();
			this.listView1 = new System.Windows.Forms.ListView();
			this.ToolStrip_Content = new System.Windows.Forms.ToolStrip();
			this.Label_ObjectName = new System.Windows.Forms.ToolStripLabel();
			TabLayout = new System.Windows.Forms.TableLayoutPanel();
			TabLayout.SuspendLayout();
			this.Panel_Main.SuspendLayout();
			this.TabControl_General.SuspendLayout();
			this.TabPage_Package.SuspendLayout();
			this.TabPage_Tables.SuspendLayout();
			this.TabControl_Tables.SuspendLayout();
			this.TabPage_Files.SuspendLayout();
			this.Panel_Content.SuspendLayout();
			this.ToolStrip_Content.SuspendLayout();
			this.SuspendLayout();
			// 
			// TabLayout
			// 
			resources.ApplyResources( TabLayout, "TabLayout" );
			TabLayout.Controls.Add( this.Panel_Main, 0, 0 );
			TabLayout.Controls.Add( this.Panel_Content, 1, 0 );
			TabLayout.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
			TabLayout.Name = "TabLayout";
			// 
			// Panel_Main
			// 
			resources.ApplyResources( this.Panel_Main, "Panel_Main" );
			this.Panel_Main.Controls.Add( this.TabControl_General );
			this.Panel_Main.Name = "Panel_Main";
			// 
			// TabControl_General
			// 
			resources.ApplyResources( this.TabControl_General, "TabControl_General" );
			this.TabControl_General.Controls.Add( this.TabPage_Package );
			this.TabControl_General.Controls.Add( this.TabPage_Tables );
			this.TabControl_General.Name = "TabControl_General";
			this.TabControl_General.SelectedIndex = 0;
			// 
			// TabPage_Package
			// 
			this.TabPage_Package.BackColor = System.Drawing.Color.White;
			this.TabPage_Package.CausesValidation = false;
			this.TabPage_Package.Controls.Add( this.Label_Version );
			resources.ApplyResources( this.TabPage_Package, "TabPage_Package" );
			this.TabPage_Package.Name = "TabPage_Package";
			// 
			// Label_Version
			// 
			resources.ApplyResources( this.Label_Version, "Label_Version" );
			this.Label_Version.CausesValidation = false;
			this.Label_Version.Name = "Label_Version";
			// 
			// TabPage_Tables
			// 
			this.TabPage_Tables.Controls.Add( this.TabControl_Tables );
			resources.ApplyResources( this.TabPage_Tables, "TabPage_Tables" );
			this.TabPage_Tables.Name = "TabPage_Tables";
			this.TabPage_Tables.UseVisualStyleBackColor = true;
			// 
			// TabControl_Tables
			// 
			this.TabControl_Tables.Controls.Add( this.TabPage_Files );
			resources.ApplyResources( this.TabControl_Tables, "TabControl_Tables" );
			this.TabControl_Tables.Name = "TabControl_Tables";
			this.TabControl_Tables.SelectedIndex = 0;
			// 
			// TabPage_Files
			// 
			this.TabPage_Files.Controls.Add( this.TreeView_Files );
			resources.ApplyResources( this.TabPage_Files, "TabPage_Files" );
			this.TabPage_Files.Name = "TabPage_Files";
			this.TabPage_Files.UseVisualStyleBackColor = true;
			// 
			// TreeView_Files
			// 
			this.TreeView_Files.CausesValidation = false;
			resources.ApplyResources( this.TreeView_Files, "TreeView_Files" );
			this.TreeView_Files.Name = "TreeView_Files";
			this.TreeView_Files.ShowNodeToolTips = true;
			// 
			// Panel_Content
			// 
			resources.ApplyResources( this.Panel_Content, "Panel_Content" );
			this.Panel_Content.Controls.Add( this.listView1 );
			this.Panel_Content.Controls.Add( this.ToolStrip_Content );
			this.Panel_Content.Name = "Panel_Content";
			// 
			// listView1
			// 
			resources.ApplyResources( this.listView1, "listView1" );
			this.listView1.Name = "listView1";
			this.listView1.UseCompatibleStateImageBehavior = false;
			// 
			// ToolStrip_Content
			// 
			this.ToolStrip_Content.AllowMerge = false;
			resources.ApplyResources( this.ToolStrip_Content, "ToolStrip_Content" );
			this.ToolStrip_Content.BackColor = System.Drawing.Color.White;
			this.ToolStrip_Content.CanOverflow = false;
			this.ToolStrip_Content.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip_Content.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.Label_ObjectName} );
			this.ToolStrip_Content.Name = "ToolStrip_Content";
			this.ToolStrip_Content.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// Label_ObjectName
			// 
			this.Label_ObjectName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.Label_ObjectName.BackColor = System.Drawing.Color.Transparent;
			this.Label_ObjectName.Name = "Label_ObjectName";
			resources.ApplyResources( this.Label_ObjectName, "Label_ObjectName" );
			// 
			// UC_ModExplorer
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add( TabLayout );
			this.DoubleBuffered = true;
			this.Name = "UC_ModExplorer";
			TabLayout.ResumeLayout( false );
			this.Panel_Main.ResumeLayout( false );
			this.TabControl_General.ResumeLayout( false );
			this.TabPage_Package.ResumeLayout( false );
			this.TabPage_Package.PerformLayout();
			this.TabPage_Tables.ResumeLayout( false );
			this.TabControl_Tables.ResumeLayout( false );
			this.TabPage_Files.ResumeLayout( false );
			this.Panel_Content.ResumeLayout( false );
			this.ToolStrip_Content.ResumeLayout( false );
			this.ToolStrip_Content.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		internal System.Windows.Forms.TabControl TabControl_General;
		internal System.Windows.Forms.TabPage TabPage_Package;
		internal System.Windows.Forms.Label Label_Version;
		internal System.Windows.Forms.TabPage TabPage_Tables;
		internal System.Windows.Forms.TabControl TabControl_Tables;
		internal System.Windows.Forms.TabPage TabPage_Files;
		internal System.Windows.Forms.TreeView TreeView_Files;
		internal System.Windows.Forms.ToolStrip ToolStrip_Content;
		internal System.Windows.Forms.ToolStripLabel Label_ObjectName;
		internal System.Windows.Forms.Panel Panel_Content;
		private System.Windows.Forms.Panel Panel_Main;
		private System.Windows.Forms.ListView listView1;


	}
}
