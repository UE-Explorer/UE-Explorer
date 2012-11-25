namespace UEExplorer.UI.Dialogs
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Panel panel1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label7;
			System.Windows.Forms.Label label8;
			System.Windows.Forms.Label label9;
			System.Windows.Forms.Panel panel2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label10;
			this.VersionLabel = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.CopyrightLabel = new System.Windows.Forms.Label();
			this.LinkLabel = new System.Windows.Forms.LinkLabel();
			this.DonatorsGrid = new System.Windows.Forms.DataGridView();
			this.DonatorsSet = new System.Data.DataSet();
			this.DonateLink = new System.Windows.Forms.LinkLabel();
			this.LicenseLink = new System.Windows.Forms.LinkLabel();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			label1 = new System.Windows.Forms.Label();
			panel1 = new System.Windows.Forms.Panel();
			label2 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			panel2 = new System.Windows.Forms.Panel();
			label3 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			panel1.SuspendLayout();
			panel2.SuspendLayout();
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
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
			label1.Name = "label1";
			// 
			// panel1
			// 
			resources.ApplyResources(panel1, "panel1");
			panel1.BackColor = System.Drawing.Color.White;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Controls.Add(pictureBox1);
			panel1.Controls.Add(this.VersionLabel);
			panel1.Controls.Add(this.label4);
			panel1.Controls.Add(this.button1);
			panel1.Controls.Add(this.CopyrightLabel);
			panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
			panel1.Name = "panel1";
			// 
			// VersionLabel
			// 
			resources.ApplyResources(this.VersionLabel, "VersionLabel");
			this.VersionLabel.Name = "VersionLabel";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Name = "label4";
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// CopyrightLabel
			// 
			resources.ApplyResources(this.CopyrightLabel, "CopyrightLabel");
			this.CopyrightLabel.Name = "CopyrightLabel";
			// 
			// label2
			// 
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			// 
			// label5
			// 
			resources.ApplyResources(label5, "label5");
			label5.Name = "label5";
			// 
			// label6
			// 
			resources.ApplyResources(label6, "label6");
			label6.Name = "label6";
			// 
			// label7
			// 
			resources.ApplyResources(label7, "label7");
			label7.Name = "label7";
			// 
			// label8
			// 
			resources.ApplyResources(label8, "label8");
			label8.Name = "label8";
			// 
			// label9
			// 
			resources.ApplyResources(label9, "label9");
			label9.Name = "label9";
			// 
			// panel2
			// 
			resources.ApplyResources(panel2, "panel2");
			panel2.Controls.Add(label2);
			panel2.Controls.Add(label9);
			panel2.Controls.Add(label5);
			panel2.Controls.Add(label8);
			panel2.Controls.Add(label6);
			panel2.Controls.Add(label7);
			panel2.Name = "panel2";
			// 
			// label3
			// 
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			// 
			// label10
			// 
			resources.ApplyResources(label10, "label10");
			label10.Name = "label10";
			// 
			// LinkLabel
			// 
			resources.ApplyResources(this.LinkLabel, "LinkLabel");
			this.LinkLabel.Name = "LinkLabel";
			this.LinkLabel.TabStop = true;
			// 
			// DonatorsGrid
			// 
			resources.ApplyResources(this.DonatorsGrid, "DonatorsGrid");
			this.DonatorsGrid.AllowUserToAddRows = false;
			this.DonatorsGrid.AllowUserToDeleteRows = false;
			this.DonatorsGrid.AllowUserToResizeColumns = false;
			this.DonatorsGrid.AllowUserToResizeRows = false;
			this.DonatorsGrid.AutoGenerateColumns = false;
			this.DonatorsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.DonatorsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
			this.DonatorsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DonatorsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DonatorsGrid.DataSource = this.DonatorsSet;
			this.DonatorsGrid.EnableHeadersVisualStyles = false;
			this.DonatorsGrid.GridColor = System.Drawing.Color.WhiteSmoke;
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
			// AboutForm
			// 
			this.AcceptButton = this.button1;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add(this.LicenseLink);
			this.Controls.Add(this.DonateLink);
			this.Controls.Add(label10);
			this.Controls.Add(label3);
			this.Controls.Add(panel2);
			this.Controls.Add(panel1);
			this.Controls.Add(this.DonatorsGrid);
			this.Controls.Add(label1);
			this.Controls.Add(this.LinkLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Load += new System.EventHandler(this.AboutForm_Load);
			this.Shown += new System.EventHandler(this.AboutForm_Shown);
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.DonatorsGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DonatorsSet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label VersionLabel;
		private System.Windows.Forms.Label CopyrightLabel;
		private System.Windows.Forms.Label label4;
		internal System.Windows.Forms.LinkLabel LinkLabel;
		private System.Windows.Forms.DataGridView DonatorsGrid;
		private System.Data.DataSet DonatorsSet;
		private System.Windows.Forms.LinkLabel DonateLink;
		private System.Windows.Forms.LinkLabel LicenseLink;
	}
}