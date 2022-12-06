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
		private void InitializeComponent()
		{
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox1;
            this.TreeView_Packages = new System.Windows.Forms.TreeView();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.OpenNTLDialog = new System.Windows.Forms.OpenFileDialog();
            this.Button_Save = new System.Windows.Forms.Button();
            this.Button_Add = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.ForeColor = System.Drawing.Color.DimGray;
            label1.Location = new System.Drawing.Point(87, 406);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(96, 13);
            label1.TabIndex = 2;
            label1.Text = "*NativesTableList_";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.BackColor = System.Drawing.SystemColors.Control;
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(this.FileNameTextBox);
            groupBox1.Controls.Add(this.TreeView_Packages);
            groupBox1.Controls.Add(this.Button_Save);
            groupBox1.Controls.Add(this.Button_Add);
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(429, 430);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Scanned Packages";
            // 
            // TreeView_Packages
            // 
            this.TreeView_Packages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeView_Packages.Location = new System.Drawing.Point(6, 14);
            this.TreeView_Packages.Name = "TreeView_Packages";
            this.TreeView_Packages.Size = new System.Drawing.Size(417, 381);
            this.TreeView_Packages.TabIndex = 0;
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Enabled = false;
            this.FileNameTextBox.Location = new System.Drawing.Point(189, 403);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(153, 20);
            this.FileNameTextBox.TabIndex = 1;
            this.FileNameTextBox.Text = "Package Name POSTFIX";
            // 
            // OpenNTLDialog
            // 
            this.OpenNTLDialog.DefaultExt = "u";
            this.OpenNTLDialog.Filter = "UnrealScript(*.u)|*.u";
            this.OpenNTLDialog.Multiselect = true;
            this.OpenNTLDialog.Title = "NTL File Dialog";
            // 
            // Button_Save
            // 
            this.Button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Save.Enabled = false;
            this.Button_Save.Image = global::Eliot.Extensions.NativesTableListGenerator.Properties.Resources.Save;
            this.Button_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button_Save.Location = new System.Drawing.Point(348, 401);
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(75, 23);
            this.Button_Save.TabIndex = 5;
            this.Button_Save.Text = "Save Natives Table List";
            this.Button_Save.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // Button_Add
            // 
            this.Button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Add.Image = global::Eliot.Extensions.NativesTableListGenerator.Properties.Resources.SearchFolderOpened;
            this.Button_Add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button_Add.Location = new System.Drawing.Point(6, 400);
            this.Button_Add.Name = "Button_Add";
            this.Button_Add.Size = new System.Drawing.Size(75, 23);
            this.Button_Add.TabIndex = 4;
            this.Button_Add.Text = "Scan Packages";
            this.Button_Add.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button_Add.UseVisualStyleBackColor = true;
            this.Button_Add.Click += new System.EventHandler(this.Button_Add_Click);
            // 
            // UC_NativeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(groupBox1);
            this.Name = "UC_NativeGenerator";
            this.Size = new System.Drawing.Size(429, 430);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button Button_Save;
		private System.Windows.Forms.TreeView TreeView_Packages;
		private System.Windows.Forms.TextBox FileNameTextBox;
		private System.Windows.Forms.OpenFileDialog OpenNTLDialog;
        private System.Windows.Forms.Button Button_Add;
    }
}
