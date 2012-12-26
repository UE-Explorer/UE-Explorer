namespace UEExplorer.UI.Dialogs
{
	partial class ExceptionDialog
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
			if( disposing && (components != null) )
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
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.GroupBox groupBox2;
			System.Windows.Forms.Label label1;
			this.ExceptionMessage = new System.Windows.Forms.TextBox();
			this.ExceptionStack = new System.Windows.Forms.TextBox();
			this.ExceptionError = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.Button_Send = new System.Windows.Forms.Button();
			this.Button_Cancel = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			label1 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupBox1.Controls.Add(this.ExceptionMessage);
			groupBox1.Location = new System.Drawing.Point(7, 27);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(301, 297);
			groupBox1.TabIndex = 3;
			groupBox1.TabStop = false;
			groupBox1.Text = "Message";
			// 
			// ExceptionMessage
			// 
			this.ExceptionMessage.BackColor = System.Drawing.Color.White;
			this.ExceptionMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ExceptionMessage.Location = new System.Drawing.Point(3, 16);
			this.ExceptionMessage.Multiline = true;
			this.ExceptionMessage.Name = "ExceptionMessage";
			this.ExceptionMessage.ReadOnly = true;
			this.ExceptionMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.ExceptionMessage.Size = new System.Drawing.Size(295, 278);
			this.ExceptionMessage.TabIndex = 0;
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(this.ExceptionStack);
			groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox2.Location = new System.Drawing.Point(3, 3);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(316, 372);
			groupBox2.TabIndex = 2;
			groupBox2.TabStop = false;
			groupBox2.Text = "Stack Trace";
			// 
			// ExceptionStack
			// 
			this.ExceptionStack.BackColor = System.Drawing.Color.White;
			this.ExceptionStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ExceptionStack.Location = new System.Drawing.Point(3, 16);
			this.ExceptionStack.Multiline = true;
			this.ExceptionStack.Name = "ExceptionStack";
			this.ExceptionStack.ReadOnly = true;
			this.ExceptionStack.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.ExceptionStack.Size = new System.Drawing.Size(310, 353);
			this.ExceptionStack.TabIndex = 0;
			// 
			// label1
			// 
			label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(4, 327);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(160, 13);
			label1.TabIndex = 4;
			label1.Text = "Do you want to send this report?";
			// 
			// ExceptionError
			// 
			this.ExceptionError.AutoSize = true;
			this.ExceptionError.Location = new System.Drawing.Point(4, 10);
			this.ExceptionError.Name = "ExceptionError";
			this.ExceptionError.Size = new System.Drawing.Size(0, 13);
			this.ExceptionError.TabIndex = 2;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(groupBox2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(645, 378);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(label1);
			this.panel1.Controls.Add(groupBox1);
			this.panel1.Controls.Add(this.ExceptionError);
			this.panel1.Controls.Add(this.Button_Send);
			this.panel1.Controls.Add(this.Button_Cancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(325, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(317, 372);
			this.panel1.TabIndex = 1;
			// 
			// Button_Send
			// 
			this.Button_Send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Button_Send.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Button_Send.Location = new System.Drawing.Point(3, 346);
			this.Button_Send.Name = "Button_Send";
			this.Button_Send.Size = new System.Drawing.Size(94, 23);
			this.Button_Send.TabIndex = 1;
			this.Button_Send.Text = "Send";
			this.Button_Send.UseVisualStyleBackColor = true;
			// 
			// Button_Cancel
			// 
			this.Button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Button_Cancel.Location = new System.Drawing.Point(220, 346);
			this.Button_Cancel.Name = "Button_Cancel";
			this.Button_Cancel.Size = new System.Drawing.Size(94, 23);
			this.Button_Cancel.TabIndex = 0;
			this.Button_Cancel.Text = "Cancel";
			this.Button_Cancel.UseVisualStyleBackColor = true;
			// 
			// ExceptionDialog
			// 
			this.AcceptButton = this.Button_Send;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Button_Cancel;
			this.ClientSize = new System.Drawing.Size(645, 378);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(300, 200);
			this.Name = "ExceptionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Exception Occurred";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button Button_Cancel;
		private System.Windows.Forms.Button Button_Send;
		private System.Windows.Forms.TextBox ExceptionStack;
		private System.Windows.Forms.TextBox ExceptionMessage;
		private System.Windows.Forms.Label ExceptionError;
	}
}