namespace UEExplorer.UI.Main
{
	partial class ProgramConsole
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramConsole));
			this.ConsoleOutput = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// ConsoleOutput
			// 
			this.ConsoleOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ConsoleOutput.BackColor = System.Drawing.Color.White;
			this.ConsoleOutput.Location = new System.Drawing.Point(13, 13);
			this.ConsoleOutput.Name = "ConsoleOutput";
			this.ConsoleOutput.ReadOnly = true;
			this.ConsoleOutput.Size = new System.Drawing.Size(583, 315);
			this.ConsoleOutput.TabIndex = 0;
			this.ConsoleOutput.Text = "";
			// 
			// ProgramConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(608, 340);
			this.Controls.Add(this.ConsoleOutput);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(300, 150);
			this.Name = "ProgramConsole";
			this.Text = "UE Explorer - Console";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Console_FormClosed);
			this.Shown += new System.EventHandler(this.ProgramConsole_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.RichTextBox ConsoleOutput;

	}
}