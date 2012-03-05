namespace UEExplorer.UI.Dialogs
{
	partial class ColorGeneratorForm
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point( 12, 51 );
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size( 390, 20 );
			this.textBox1.TabIndex = 0;
			this.textBox1.TextChanged += new System.EventHandler( this.textBox1_TextChanged );
			// 
			// colorDialog1
			// 
			this.colorDialog1.AnyColor = true;
			this.colorDialog1.FullOpen = true;
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point( 408, 25 );
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size( 88, 17 );
			this.checkBox1.TabIndex = 2;
			this.checkBox1.Text = "UE3 Format?";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point( 408, 51 );
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size( 75, 20 );
			this.button2.TabIndex = 3;
			this.button2.Text = "Pick Color";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler( this.button2_Click );
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point( 12, 25 );
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size( 75, 20 );
			this.textBox3.TabIndex = 5;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 93, 25 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 75, 20 );
			this.button1.TabIndex = 6;
			this.button1.Text = "Pick Color";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 37, 13 );
			this.label1.TabIndex = 8;
			this.label1.Text = "HTML";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Location = new System.Drawing.Point( 12, 77 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 471, 52 );
			this.panel1.TabIndex = 9;
			this.panel1.SizeChanged += new System.EventHandler( this.panel1_SizeChanged );
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
			// 
			// ColorGeneratorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 495, 135 );
			this.Controls.Add( this.panel1 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.textBox3 );
			this.Controls.Add( this.textBox1 );
			this.Controls.Add( this.button2 );
			this.Controls.Add( this.checkBox1 );
			this.Controls.Add( this.button1 );
			this.MinimumSize = new System.Drawing.Size( 503, 169 );
			this.Name = "ColorGeneratorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color Generator";
			this.FontChanged += new System.EventHandler( this.ColorGeneratorForm_FontChanged );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
	}
}