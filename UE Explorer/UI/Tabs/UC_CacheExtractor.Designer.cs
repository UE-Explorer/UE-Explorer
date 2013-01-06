namespace UEExplorer
{
	partial class UC_CacheExtractor
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
			this.components = new System.ComponentModel.Container();
			this.Cache_Layout = new System.Windows.Forms.TableLayoutPanel();
			this.Cache_TabControl = new System.Windows.Forms.TabControl();
			this.Cache_Files = new System.Windows.Forms.TabPage();
			this._CacheData = new System.Windows.Forms.DataGridView();
			this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._CacheContext = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.extractToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.Extension = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Guid = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cacheFileStructBindingSource = new System.Windows.Forms.BindingSource( this.components );
			this.panel1 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.Button_SelectDir = new System.Windows.Forms.Button();
			this.CacheFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.Cache_Layout.SuspendLayout();
			this.Cache_TabControl.SuspendLayout();
			this.Cache_Files.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize)( this._CacheData ) ).BeginInit();
			this._CacheContext.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize)( this.cacheFileStructBindingSource ) ).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Cache_Layout
			// 
			this.Cache_Layout.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Cache_Layout.ColumnCount = 2;
			this.Cache_Layout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 82.78221F ) );
			this.Cache_Layout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 17.21779F ) );
			this.Cache_Layout.Controls.Add( this.Cache_TabControl, 0, 0 );
			this.Cache_Layout.Controls.Add( this.panel1, 1, 0 );
			this.Cache_Layout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Cache_Layout.Location = new System.Drawing.Point( 0, 0 );
			this.Cache_Layout.Name = "Cache_Layout";
			this.Cache_Layout.RowCount = 1;
			this.Cache_Layout.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
			this.Cache_Layout.Size = new System.Drawing.Size( 877, 685 );
			this.Cache_Layout.TabIndex = 1;
			// 
			// Cache_TabControl
			// 
			this.Cache_TabControl.Controls.Add( this.Cache_Files );
			this.Cache_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Cache_TabControl.Location = new System.Drawing.Point( 3, 3 );
			this.Cache_TabControl.Name = "Cache_TabControl";
			this.Cache_TabControl.SelectedIndex = 0;
			this.Cache_TabControl.Size = new System.Drawing.Size( 719, 679 );
			this.Cache_TabControl.TabIndex = 0;
			// 
			// Cache_Files
			// 
			this.Cache_Files.Controls.Add( this._CacheData );
			this.Cache_Files.Location = new System.Drawing.Point( 4, 22 );
			this.Cache_Files.Name = "Cache_Files";
			this.Cache_Files.Padding = new System.Windows.Forms.Padding( 3 );
			this.Cache_Files.Size = new System.Drawing.Size( 711, 653 );
			this.Cache_Files.TabIndex = 0;
			this.Cache_Files.Text = "Files";
			this.Cache_Files.UseVisualStyleBackColor = true;
			// 
			// _CacheData
			// 
			this._CacheData.AllowUserToAddRows = false;
			this._CacheData.AllowUserToDeleteRows = false;
			this._CacheData.AllowUserToResizeColumns = false;
			this._CacheData.AllowUserToResizeRows = false;
			this._CacheData.AutoGenerateColumns = false;
			this._CacheData.BackgroundColor = System.Drawing.SystemColors.Menu;
			this._CacheData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._CacheData.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.Extension,
            this.Guid} );
			this._CacheData.DataSource = this.cacheFileStructBindingSource;
			this._CacheData.Dock = System.Windows.Forms.DockStyle.Fill;
			this._CacheData.GridColor = System.Drawing.SystemColors.MenuBar;
			this._CacheData.Location = new System.Drawing.Point( 3, 3 );
			this._CacheData.Name = "_CacheData";
			this._CacheData.ReadOnly = true;
			this._CacheData.RowHeadersVisible = false;
			this._CacheData.RowHeadersWidth = 100;
			this._CacheData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this._CacheData.RowTemplate.ContextMenuStrip = this._CacheContext;
			this._CacheData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._CacheData.Size = new System.Drawing.Size( 705, 647 );
			this._CacheData.TabIndex = 0;
			// 
			// FileName
			// 
			this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.FileName.ContextMenuStrip = this._CacheContext;
			this.FileName.DataPropertyName = "FileName";
			this.FileName.FillWeight = 127.2431F;
			this.FileName.HeaderText = "File Name";
			this.FileName.MinimumWidth = 45;
			this.FileName.Name = "FileName";
			this.FileName.ReadOnly = true;
			// 
			// _CacheContext
			// 
			this._CacheContext.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem1,
            this.deleteToolStripMenuItem1,
            this.removeToolStripMenuItem1} );
			this._CacheContext.Name = "_CacheContext";
			this._CacheContext.Size = new System.Drawing.Size( 143, 70 );
			// 
			// extractToolStripMenuItem1
			// 
			this.extractToolStripMenuItem1.Name = "extractToolStripMenuItem1";
			this.extractToolStripMenuItem1.Size = new System.Drawing.Size( 142, 22 );
			this.extractToolStripMenuItem1.Text = "Extract Entry";
			this.extractToolStripMenuItem1.ToolTipText = "Extracts the cache file to the correct directory";
			this.extractToolStripMenuItem1.Click += new System.EventHandler( this.ExtractToolStripMenuItem1_Click );
			// 
			// deleteToolStripMenuItem1
			// 
			this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
			this.deleteToolStripMenuItem1.Size = new System.Drawing.Size( 142, 22 );
			this.deleteToolStripMenuItem1.Text = "Delete Entry";
			this.deleteToolStripMenuItem1.Click += new System.EventHandler( this.DeleteToolStripMenuItem1_Click );
			// 
			// removeToolStripMenuItem1
			// 
			this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
			this.removeToolStripMenuItem1.Size = new System.Drawing.Size( 142, 22 );
			this.removeToolStripMenuItem1.Text = "Remove Entry";
			this.removeToolStripMenuItem1.Click += new System.EventHandler( this.RemoveToolStripMenuItem1_Click );
			// 
			// Extension
			// 
			this.Extension.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Extension.DataPropertyName = "Extension";
			this.Extension.FillWeight = 38.07107F;
			this.Extension.HeaderText = "File Extension";
			this.Extension.MinimumWidth = 25;
			this.Extension.Name = "Extension";
			this.Extension.ReadOnly = true;
			// 
			// Guid
			// 
			this.Guid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Guid.DataPropertyName = "Guid";
			this.Guid.FillWeight = 134.6859F;
			this.Guid.HeaderText = "File Guid";
			this.Guid.MinimumWidth = 25;
			this.Guid.Name = "Guid";
			this.Guid.ReadOnly = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add( this.button1 );
			this.panel1.Controls.Add( this.Button_SelectDir );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 728, 3 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 146, 679 );
			this.panel1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.button1.Location = new System.Drawing.Point( 4, 34 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 139, 23 );
			this.button1.TabIndex = 1;
			this.button1.Text = "Validate Entries";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.Button1_Click );
			// 
			// Button_SelectDir
			// 
			this.Button_SelectDir.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.Button_SelectDir.Location = new System.Drawing.Point( 4, 4 );
			this.Button_SelectDir.Name = "Button_SelectDir";
			this.Button_SelectDir.Size = new System.Drawing.Size( 139, 23 );
			this.Button_SelectDir.TabIndex = 0;
			this.Button_SelectDir.Text = "Select Cache Directory";
			this.Button_SelectDir.UseVisualStyleBackColor = true;
			this.Button_SelectDir.Click += new System.EventHandler( this.Button_SelectDir_Click );
			// 
			// UserControl_CacheExtractorTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.Cache_Layout );
			this.Name = "UserControl_CacheExtractorTab";
			this.Size = new System.Drawing.Size( 877, 685 );
			this.Cache_Layout.ResumeLayout( false );
			this.Cache_TabControl.ResumeLayout( false );
			this.Cache_Files.ResumeLayout( false );
			( (System.ComponentModel.ISupportInitialize)( this._CacheData ) ).EndInit();
			this._CacheContext.ResumeLayout( false );
			( (System.ComponentModel.ISupportInitialize)( this.cacheFileStructBindingSource ) ).EndInit();
			this.panel1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel Cache_Layout;
		private System.Windows.Forms.TabControl Cache_TabControl;
		private System.Windows.Forms.TabPage Cache_Files;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button Button_SelectDir;
		private System.Windows.Forms.DataGridView _CacheData;
		private System.Windows.Forms.BindingSource cacheFileStructBindingSource;
		private System.Windows.Forms.ContextMenuStrip _CacheContext;
		private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem1;
		private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
		private System.Windows.Forms.DataGridViewTextBoxColumn Extension;
		private System.Windows.Forms.DataGridViewTextBoxColumn Guid;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem1;
		private System.Windows.Forms.FolderBrowserDialog CacheFolderDialog;
		private System.Windows.Forms.Button button1;
	}
}
