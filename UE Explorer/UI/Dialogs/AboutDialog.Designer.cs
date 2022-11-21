namespace UEExplorer.UI.Dialogs
{
	partial class AboutDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.PictureBox pictureBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label10;
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ProductNameLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.DonatorsGrid = new System.Windows.Forms.DataGridView();
            this.DonatorsSet = new System.Data.DataSet();
            this.DonateLink = new System.Windows.Forms.LinkLabel();
            this.LicenseLink = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            panel1 = new System.Windows.Forms.Panel();
            label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DonatorsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DonatorsSet)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Image = global::UEExplorer.Properties.Resources.UE_ProgramLogo;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(this.VersionLabel);
            panel1.Controls.Add(this.ProductNameLabel);
            panel1.Controls.Add(this.okButton);
            panel1.Controls.Add(this.CopyrightLabel);
            panel1.Name = "panel1";
            // 
            // VersionLabel
            // 
            resources.ApplyResources(this.VersionLabel, "VersionLabel");
            this.VersionLabel.Name = "VersionLabel";
            // 
            // ProductNameLabel
            // 
            resources.ApplyResources(this.ProductNameLabel, "ProductNameLabel");
            this.ProductNameLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ProductNameLabel.Name = "ProductNameLabel";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // CopyrightLabel
            // 
            resources.ApplyResources(this.CopyrightLabel, "CopyrightLabel");
            this.CopyrightLabel.Name = "CopyrightLabel";
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.Name = "label10";
            // 
            // DonatorsGrid
            // 
            this.DonatorsGrid.AllowUserToAddRows = false;
            this.DonatorsGrid.AllowUserToDeleteRows = false;
            this.DonatorsGrid.AllowUserToResizeColumns = false;
            this.DonatorsGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.DonatorsGrid, "DonatorsGrid");
            this.DonatorsGrid.AutoGenerateColumns = false;
            this.DonatorsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DonatorsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DonatorsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DonatorsGrid.DataSource = this.DonatorsSet;
            this.DonatorsGrid.EnableHeadersVisualStyles = false;
            this.DonatorsGrid.Name = "DonatorsGrid";
            this.DonatorsGrid.ReadOnly = true;
            this.DonatorsGrid.RowHeadersVisible = false;
            this.DonatorsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            // 
            // DonatorsSet
            // 
            this.DonatorsSet.DataSetName = "NewDataSet";
            // 
            // DonateLink
            // 
            resources.ApplyResources(this.DonateLink, "DonateLink");
            this.DonateLink.Name = "DonateLink";
            this.DonateLink.TabStop = true;
            // 
            // LicenseLink
            // 
            resources.ApplyResources(this.LicenseLink, "LicenseLink");
            this.LicenseLink.LinkColor = System.Drawing.Color.Olive;
            this.LicenseLink.Name = "LicenseLink";
            this.LicenseLink.TabStop = true;
            this.LicenseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseLink_LinkClicked);
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.LicenseLink);
            this.Controls.Add(this.DonateLink);
            this.Controls.Add(label10);
            this.Controls.Add(panel1);
            this.Controls.Add(this.DonatorsGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.Shown += new System.EventHandler(this.AboutForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DonatorsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DonatorsSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label VersionLabel;
		private System.Windows.Forms.Label CopyrightLabel;
		private System.Windows.Forms.Label ProductNameLabel;
		private System.Windows.Forms.DataGridView DonatorsGrid;
		private System.Data.DataSet DonatorsSet;
		private System.Windows.Forms.LinkLabel DonateLink;
		private System.Windows.Forms.LinkLabel LicenseLink;
        private System.Windows.Forms.Label label11;
	}
}