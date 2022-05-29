using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UEExplorer.Properties;
using UELib;
using System.Drawing;
using UELib.Types;

namespace UEExplorer.UI.Tabs
{
    public partial class UC_Options : UserControl_Tab
    {
        /// <summary>
        /// Called when the Tab is added to the chain.
        /// </summary>
        protected override void TabCreated()
        {
            Program.LoadConfig();
            CheckBox_SerObj.Checked = Program.Options.InitFlags.HasFlag(UnrealPackage.InitFlags.Deserialize);
            CheckBox_LinkObj.Checked = Program.Options.InitFlags.HasFlag(UnrealPackage.InitFlags.Link);
            CheckBox_ImpObj.Checked = Program.Options.InitFlags.HasFlag(UnrealPackage.InitFlags.Import);

            foreach (string filePath in GetNativeTables())
                ComboBox_NativeTable.Items.Add(Path.GetFileNameWithoutExtension(filePath));

            ComboBox_NativeTable.SelectedIndex = ComboBox_NativeTable.Items.IndexOf(Program.Options.NTLPath);

            CheckBox_Version.Checked = Program.Options.bForceVersion;
            NumericUpDown_Version.Value = Program.Options.Version;
            CheckBox_LicenseeMode.Checked = Program.Options.bForceLicenseeMode;
            NumericUpDown_LicenseeMode.Value = Program.Options.LicenseeMode;
            SuppressComments.Checked = Program.Options.bSuppressComments;
            PreBeginBracket.Text = Program.Options.PreBeginBracket;
            PreEndBracket.Text = Program.Options.PreEndBracket;

            PathText.Text = Program.Options.UEModelAppPath;
            PathText_TextChanged(PathText, new EventArgs());
            IndentionNumeric.Value = Program.Options.Indention;

            foreach (string enumElement in Enum.GetNames(typeof(PropertyType)))
            {
                if (enumElement == "StructOffset")
                    continue;

                VariableType.Items.Add(enumElement);
            }

            foreach (string pair in Program.Options.VariableTypes)
                VariableTypesTree.Nodes.Add(new TreeNode(pair) { Tag = pair });

            base.TabCreated();

            PreBeginBracket.TextChanged += PreBeginBracket_TextChanged;
            PreEndBracket.TextChanged += PreEndBracket_TextChanged;
            IndentionNumeric.ValueChanged += IndentionNumeric_ValueChanged;
            UpdateBracketPreview();
        }

        public static IEnumerable<string> GetNativeTables()
        {
            return Directory.GetFiles(Path.Combine(Application.StartupPath, "Native Tables"), "*"
                + NativesTablePackage.Extension);
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            Program.Options.NTLPath = ComboBox_NativeTable.SelectedItem.ToString();
            Program.Options.InitFlags = UnrealPackage.InitFlags.All;

            if (!CheckBox_LinkObj.Checked)
                Program.Options.InitFlags &= ~UnrealPackage.InitFlags.Link;

            if (!CheckBox_ImpObj.Checked)
                Program.Options.InitFlags &= ~UnrealPackage.InitFlags.Import;

            Program.Options.InitFlags |= UnrealPackage.InitFlags.Deserialize;

            if (!CheckBox_SerObj.Checked)
                Program.Options.InitFlags &= ~UnrealPackage.InitFlags.Deserialize;

            Program.Options.InitFlags |= UnrealPackage.InitFlags.RegisterClasses;

            Program.Options.bForceVersion = CheckBox_Version.Checked;
            Program.Options.Version = (ushort)NumericUpDown_Version.Value;

            Program.Options.bForceLicenseeMode = CheckBox_LicenseeMode.Checked;
            Program.Options.LicenseeMode = (ushort)NumericUpDown_LicenseeMode.Value;

            Program.Options.bSuppressComments = SuppressComments.Checked;
            UnrealConfig.SuppressComments = SuppressComments.Checked;
            Program.Options.UEModelAppPath = PathText.Text;

            Program.Options.PreBeginBracket = PreBeginBracket.Text;
            Program.Options.PreEndBracket = PreEndBracket.Text;
            UnrealConfig.PreBeginBracket = Program.ParseFormatOption(Program.Options.PreBeginBracket);
            UnrealConfig.PreEndBracket = Program.ParseFormatOption(Program.Options.PreEndBracket);

            Program.Options.Indention = (int)IndentionNumeric.Value;
            UnrealConfig.Indention = Program.ParseIndention(Program.Options.Indention);

            Program.Options.VariableTypes.Clear();
            foreach (TreeNode node in VariableTypesTree.Nodes) Program.Options.VariableTypes.Add((string)node.Tag);
            Program.CopyVariableTypes();

            Program.SaveConfig();
            MessageBox.Show(Resources.SAVE_SUCCESS, Resources.SAVED,
                MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private static string FormatVariable(KeyValuePair<string, Tuple<string, PropertyType>> keyPair)
        {
            return keyPair.Value.Item1 + ":" + keyPair.Value.Item2;
        }

        private void PathButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "UE Model View (umodel.exe)|umodel.exe"
            };

            if (dialog.ShowDialog() == DialogResult.OK) PathText.Text = dialog.FileName;
        }

        private void PathText_TextChanged(object sender, EventArgs e)
        {
            PathText.BackColor = File.Exists(PathText.Text)
                                 && Path.GetFileName(PathText.Text) == "umodel.exe"
                ? Color.Green
                : Color.Red;
        }

        private void VariableTypesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var varTuple = Program.ParseVariable((string)e.Node.Tag);

            VariableTypeGroup.Text = (string)e.Node.Tag;
            VariableType.SelectedIndex = VariableType.Items.IndexOf(varTuple.Item3.ToString());

            VariableTypeGroup.Enabled = true;
            VariableType.Enabled = true;

            DeleteArrayType.Enabled = true;
        }

        private void VariableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var varData = Program.ParseVariable((string)VariableTypesTree.SelectedNode.Tag);
            VariableTypeGroup.Text = varData.Item2 + ":" + VariableType.SelectedItem;
        }

        private void VariableTypeGroup_TextChanged(object sender, EventArgs e)
        {
            VariableTypesTree.SelectedNode.Tag = VariableTypeGroup.Text;
            VariableTypesTree.SelectedNode.Text = (string)VariableTypesTree.SelectedNode.Tag;
        }

        private void NewArrayType_Click(object sender, EventArgs e)
        {
            var node = new TreeNode("Package.Class.Property:ObjectProperty")
                { Tag = "Package.Class.Property:ObjectProperty" };
            VariableTypesTree.Nodes.Add(node);
            VariableTypesTree.SelectedNode = node;
        }

        private void DeleteArrayType_Click(object sender, EventArgs e)
        {
            if (VariableTypesTree.SelectedNode == null)
                return;

            VariableTypesTree.Nodes.Remove(VariableTypesTree.SelectedNode);

            if (VariableTypesTree.Nodes.Count == 0)
            {
                //VariableTypeGroup.Text = String.Empty;
                VariableTypeGroup.Enabled = false;
                VariableType.Enabled = false;
                DeleteArrayType.Enabled = false;
            }
        }

        private void PreBeginBracket_TextChanged(object sender, EventArgs e)
        {
            UpdateBracketPreview();
        }

        private void PreEndBracket_TextChanged(object sender, EventArgs e)
        {
            UpdateBracketPreview();
        }

        private void IndentionNumeric_ValueChanged(object sender, EventArgs e)
        {
            UpdateBracketPreview();
        }

        private void UpdateBracketPreview()
        {
            string preBB = UnrealConfig.PreBeginBracket;
            string preEB = UnrealConfig.PreEndBracket;
            string preTABS = UnrealConfig.Indention;
            UnrealConfig.PreBeginBracket = Program.ParseFormatOption(PreBeginBracket.Text);
            UnrealConfig.PreEndBracket = Program.ParseFormatOption(PreEndBracket.Text);
            UnrealConfig.Indention = Program.ParseIndention((int)IndentionNumeric.Value);

            UDecompilingState.ResetTabs();

            string output = "function Preview()" + UnrealConfig.PrintBeginBracket();
            UDecompilingState.AddTab();
            output += "\r\n" + UDecompilingState.Tabs + "if( true )" + UnrealConfig.PrintBeginBracket();
            UDecompilingState.AddTab();
            output += "\r\n" + UDecompilingState.Tabs + "[CODE]";
            UDecompilingState.RemoveTab();
            output += UnrealConfig.PrintEndBracket();
            UDecompilingState.RemoveTab();
            output += UnrealConfig.PrintEndBracket();
            BracketPreview.Text = output;

            UnrealConfig.PreBeginBracket = preBB;
            UnrealConfig.PreEndBracket = preEB;
            UnrealConfig.Indention = preTABS;
        }

        private void CheckBox_Version_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDown_Version.Enabled = CheckBox_Version.Checked;
        }

        private void CheckBox_LicenseeMode_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDown_LicenseeMode.Enabled = CheckBox_LicenseeMode.Checked;
        }
    }
}