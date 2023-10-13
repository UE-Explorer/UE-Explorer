namespace UEExplorer.UI.Panels
{
    partial class FindInPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindInPanel));
            this.findAllButton = new Krypton.Toolkit.KryptonButton();
            this.findAllCommand = new Krypton.Toolkit.KryptonCommand();
            this.matchCaseCheckBox = new Krypton.Toolkit.KryptonCheckBox();
            this.findTextBox = new Krypton.Toolkit.KryptonTextBox();
            this.lookInComboBox = new Krypton.Toolkit.KryptonComboBox();
            this.packageReferenceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lookInCheckBox = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.lookInComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packageReferenceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // findAllButton
            // 
            resources.ApplyResources(this.findAllButton, "findAllButton");
            this.findAllButton.CornerRoundingRadius = -1F;
            this.findAllButton.KryptonCommand = this.findAllCommand;
            this.findAllButton.Name = "findAllButton";
            this.findAllButton.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.findAllButton.Values.Text = resources.GetString("findAllButton.Values.Text");
            // 
            // findAllCommand
            // 
            resources.ApplyResources(this.findAllCommand, "findAllCommand");
            this.findAllCommand.Execute += new System.EventHandler(this.findAllCommand_Execute);
            // 
            // matchCaseCheckBox
            // 
            this.matchCaseCheckBox.Checked = global::UEExplorer.Properties.Settings.Default.FindIn_MatchCase;
            this.matchCaseCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::UEExplorer.Properties.Settings.Default, "FindIn_MatchCase", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.matchCaseCheckBox, "matchCaseCheckBox");
            this.matchCaseCheckBox.Name = "matchCaseCheckBox";
            this.matchCaseCheckBox.Values.Text = resources.GetString("matchCaseCheckBox.Values.Text");
            // 
            // findTextBox
            // 
            this.findTextBox.AllowDrop = true;
            resources.ApplyResources(this.findTextBox, "findTextBox");
            this.findTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.findTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.findTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::UEExplorer.Properties.Settings.Default, "FindIn_FindText", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Text = global::UEExplorer.Properties.Settings.Default.FindIn_FindText;
            this.findTextBox.TextChanged += new System.EventHandler(this.findTextBox_TextChanged);
            this.findTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.findTextBox_KeyUp);
            // 
            // lookInComboBox
            // 
            resources.ApplyResources(this.lookInComboBox, "lookInComboBox");
            this.lookInComboBox.CornerRoundingRadius = -1F;
            this.lookInComboBox.DataSource = this.packageReferenceBindingSource;
            this.lookInComboBox.DisplayMember = "ShortFilePath";
            this.lookInComboBox.DropDownWidth = 305;
            this.lookInComboBox.IntegralHeight = false;
            this.lookInComboBox.Name = "lookInComboBox";
            this.lookInComboBox.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            // 
            // packageReferenceBindingSource
            // 
            this.packageReferenceBindingSource.DataSource = typeof(UEExplorer.Framework.PackageReference);
            // 
            // lookInCheckBox
            // 
            this.lookInCheckBox.Checked = global::UEExplorer.Properties.Settings.Default.FindIn_LookIn;
            this.lookInCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::UEExplorer.Properties.Settings.Default, "FindIn_LookIn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.lookInCheckBox, "lookInCheckBox");
            this.lookInCheckBox.Name = "lookInCheckBox";
            this.lookInCheckBox.Values.Text = resources.GetString("lookInCheckBox.Values.Text");
            this.lookInCheckBox.CheckedChanged += new System.EventHandler(this.lookInCheckBox_CheckedChanged);
            // 
            // FindInPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lookInCheckBox);
            this.Controls.Add(this.lookInComboBox);
            this.Controls.Add(this.matchCaseCheckBox);
            this.Controls.Add(this.findAllButton);
            this.Controls.Add(this.findTextBox);
            this.Name = "FindInPanel";
            this.Load += new System.EventHandler(this.FindInPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookInComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packageReferenceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonTextBox findTextBox;
        private Krypton.Toolkit.KryptonCheckBox matchCaseCheckBox;
        private Krypton.Toolkit.KryptonButton findAllButton;
        private Krypton.Toolkit.KryptonCommand findAllCommand;
        private Krypton.Toolkit.KryptonComboBox lookInComboBox;
        private System.Windows.Forms.BindingSource packageReferenceBindingSource;
        private Krypton.Toolkit.KryptonCheckBox lookInCheckBox;
    }
}
