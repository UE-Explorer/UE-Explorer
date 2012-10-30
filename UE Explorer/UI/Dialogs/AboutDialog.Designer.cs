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
			System.Windows.Forms.Label label1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
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
			pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pictureBox1.Image = global::UEExplorer.Properties.Resources.UE_ProgramLogo;
			pictureBox1.Location = new System.Drawing.Point(3, 5);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(69, 58);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 10;
			pictureBox1.TabStop = false;
			// 
			// label1
			// 
			label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
			label1.Location = new System.Drawing.Point(12, 318);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(366, 79);
			label1.TabIndex = 12;
			label1.Text = resources.GetString("label1.Text");
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			panel1.BackColor = System.Drawing.Color.White;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Controls.Add(pictureBox1);
			panel1.Controls.Add(this.VersionLabel);
			panel1.Controls.Add(this.label4);
			panel1.Controls.Add(this.button1);
			panel1.Controls.Add(this.CopyrightLabel);
			panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
			panel1.Location = new System.Drawing.Point(0, 1);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(390, 71);
			panel1.TabIndex = 15;
			// 
			// VersionLabel
			// 
			this.VersionLabel.AutoSize = true;
			this.VersionLabel.Location = new System.Drawing.Point(79, 27);
			this.VersionLabel.Name = "VersionLabel";
			this.VersionLabel.Size = new System.Drawing.Size(43, 13);
			this.VersionLabel.TabIndex = 5;
			this.VersionLabel.Text = "V Label";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(78, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(73, 24);
			this.label4.TabIndex = 6;
			this.label4.Text = "T Label";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(262, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(123, 34);
			this.button1.TabIndex = 9;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// CopyrightLabel
			// 
			this.CopyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CopyrightLabel.Location = new System.Drawing.Point(79, 48);
			this.CopyrightLabel.Name = "CopyrightLabel";
			this.CopyrightLabel.Size = new System.Drawing.Size(306, 13);
			this.CopyrightLabel.TabIndex = 7;
			this.CopyrightLabel.Text = "C Label";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(3, 7);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(105, 13);
			label2.TabIndex = 16;
			label2.Text = "Storm.TabControl";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label5.Location = new System.Drawing.Point(3, 24);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(146, 13);
			label5.TabIndex = 17;
			label5.Text = "ICSharpCode.AvalonEdit";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label6.Location = new System.Drawing.Point(3, 41);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(70, 13);
			label6.TabIndex = 18;
			label6.Text = "Eliot.UELib";
			// 
			// label7
			// 
			label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(250, 7);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(129, 13);
			label7.TabIndex = 19;
			label7.Text = "Theodor Storm Kristensen";
			// 
			// label8
			// 
			label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(250, 24);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(85, 13);
			label8.TabIndex = 20;
			label8.Text = "Daniel Grunwald";
			// 
			// label9
			// 
			label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(250, 41);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(100, 13);
			label9.TabIndex = 21;
			label9.Text = "Eliot van Uytfanghe";
			// 
			// panel2
			// 
			panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			panel2.Controls.Add(label2);
			panel2.Controls.Add(label9);
			panel2.Controls.Add(label5);
			panel2.Controls.Add(label8);
			panel2.Controls.Add(label6);
			panel2.Controls.Add(label7);
			panel2.Location = new System.Drawing.Point(4, 101);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(382, 63);
			panel2.TabIndex = 22;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label3.Location = new System.Drawing.Point(7, 81);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(46, 13);
			label3.TabIndex = 23;
			label3.Text = "Credits";
			// 
			// label10
			// 
			label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label10.AutoSize = true;
			label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label10.Location = new System.Drawing.Point(13, 404);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(52, 13);
			label10.TabIndex = 24;
			label10.Text = "Donate:";
			// 
			// LinkLabel
			// 
			this.LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LinkLabel.AutoSize = true;
			this.LinkLabel.Location = new System.Drawing.Point(254, 81);
			this.LinkLabel.Name = "LinkLabel";
			this.LinkLabel.Size = new System.Drawing.Size(49, 13);
			this.LinkLabel.TabIndex = 11;
			this.LinkLabel.TabStop = true;
			this.LinkLabel.Text = "L LABEL";
			this.LinkLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// DonatorsGrid
			// 
			this.DonatorsGrid.AllowUserToAddRows = false;
			this.DonatorsGrid.AllowUserToDeleteRows = false;
			this.DonatorsGrid.AllowUserToResizeColumns = false;
			this.DonatorsGrid.AllowUserToResizeRows = false;
			this.DonatorsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DonatorsGrid.AutoGenerateColumns = false;
			this.DonatorsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.DonatorsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
			this.DonatorsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DonatorsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DonatorsGrid.DataSource = this.DonatorsSet;
			this.DonatorsGrid.EnableHeadersVisualStyles = false;
			this.DonatorsGrid.GridColor = System.Drawing.Color.WhiteSmoke;
			this.DonatorsGrid.Location = new System.Drawing.Point(12, 170);
			this.DonatorsGrid.Name = "DonatorsGrid";
			this.DonatorsGrid.ReadOnly = true;
			this.DonatorsGrid.RowHeadersVisible = false;
			this.DonatorsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.DonatorsGrid.Size = new System.Drawing.Size(366, 145);
			this.DonatorsGrid.TabIndex = 14;
			// 
			// DonatorsSet
			// 
			this.DonatorsSet.DataSetName = "NewDataSet";
			// 
			// DonateLink
			// 
			this.DonateLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.DonateLink.AutoSize = true;
			this.DonateLink.Location = new System.Drawing.Point(66, 404);
			this.DonateLink.Name = "DonateLink";
			this.DonateLink.Size = new System.Drawing.Size(65, 13);
			this.DonateLink.TabIndex = 25;
			this.DonateLink.TabStop = true;
			this.DonateLink.Text = "Donate Link";
			// 
			// LicenseLink
			// 
			this.LicenseLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.LicenseLink.AutoSize = true;
			this.LicenseLink.LinkColor = System.Drawing.Color.Olive;
			this.LicenseLink.Location = new System.Drawing.Point(303, 404);
			this.LicenseLink.Name = "LicenseLink";
			this.LicenseLink.Size = new System.Drawing.Size(75, 13);
			this.LicenseLink.TabIndex = 26;
			this.LicenseLink.TabStop = true;
			this.LicenseLink.Text = "View Licenses";
			this.LicenseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseLink_LinkClicked);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 427);
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
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.Load += new System.EventHandler(this.AboutForm_Load);
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