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
			System.Windows.Forms.Label label2;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			System.Windows.Forms.PictureBox pictureBox1;
			System.Windows.Forms.Label label1;
			this.label4 = new System.Windows.Forms.Label();
			this.CopyrightLabel = new System.Windows.Forms.Label();
			this.VersionLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.LinkLabel = new System.Windows.Forms.LinkLabel();
			label2 = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			label2.Location = new System.Drawing.Point(12, 75);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(366, 96);
			label2.TabIndex = 8;
			label2.Text = resources.GetString("label2.Text");
			// 
			// pictureBox1
			// 
			pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pictureBox1.Image = global::UEExplorer.Properties.Resources.UE_ProgramLogo;
			pictureBox1.Location = new System.Drawing.Point(12, 9);
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
			label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			label1.Location = new System.Drawing.Point(12, 182);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(366, 82);
			label1.TabIndex = 12;
			label1.Text = resources.GetString("label1.Text");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(87, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(73, 24);
			this.label4.TabIndex = 6;
			this.label4.Text = "T Label";
			// 
			// CopyrightLabel
			// 
			this.CopyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CopyrightLabel.Location = new System.Drawing.Point(12, 280);
			this.CopyrightLabel.Name = "CopyrightLabel";
			this.CopyrightLabel.Size = new System.Drawing.Size(244, 13);
			this.CopyrightLabel.TabIndex = 7;
			this.CopyrightLabel.Text = "C Label";
			// 
			// VersionLabel
			// 
			this.VersionLabel.AutoSize = true;
			this.VersionLabel.Location = new System.Drawing.Point(88, 33);
			this.VersionLabel.Name = "VersionLabel";
			this.VersionLabel.Size = new System.Drawing.Size(43, 13);
			this.VersionLabel.TabIndex = 5;
			this.VersionLabel.Text = "V Label";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(255, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(123, 34);
			this.button1.TabIndex = 9;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// LinkLabel
			// 
			this.LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.LinkLabel.AutoSize = true;
			this.LinkLabel.Location = new System.Drawing.Point(262, 280);
			this.LinkLabel.Name = "LinkLabel";
			this.LinkLabel.Size = new System.Drawing.Size(49, 13);
			this.LinkLabel.TabIndex = 11;
			this.LinkLabel.TabStop = true;
			this.LinkLabel.Text = "L LABEL";
			this.LinkLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// AboutForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 302);
			this.ControlBox = false;
			this.Controls.Add(label1);
			this.Controls.Add(this.LinkLabel);
			this.Controls.Add(pictureBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(label2);
			this.Controls.Add(this.CopyrightLabel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.VersionLabel);
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
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label VersionLabel;
		private System.Windows.Forms.Label CopyrightLabel;
		private System.Windows.Forms.Label label4;
		internal System.Windows.Forms.LinkLabel LinkLabel;
	}
}