using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.Development;
using UEExplorer.UI;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;
using UELib.Flags;

namespace Eliot.Extensions.NativesTableListGenerator
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class UC_NativeGenerator : UserControl_Tab
    {
        private readonly NativesTablePackage _NTLPackage = new();

        public UC_NativeGenerator()
        {
            _NTLPackage.NativeTableList = new List<NativeTableItem>();

            InitializeComponent();
        }

        private void Button_Add_Click(object sender, EventArgs e)
        {
            var dialogResult = OpenNTLDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            var packages = new Stack<UnrealPackage>();
            foreach (string fileName in OpenNTLDialog.FileNames)
            {
                packages.Push(UnrealLoader.LoadPackage(fileName));
            }

            if (packages.Count > 0)
            {
                FileNameTextBox.Enabled = true;
                Button_Save.Enabled = true;
            }

            foreach (var package in packages)
            {
                if (TreeView_Packages.Nodes.ContainsKey(package.PackageName))
                {
                    package.Stream.Close();

                    continue;
                }

                package.InitializePackage();
                //package.AddClassType("Function", typeof(UFunction));
                //package.InitializeExportObjects(UnrealPackage.InitFlags.Deserialize);
                package.Stream.Close();

                TreeView_Packages.BeginUpdate();
                var packageNode = TreeView_Packages.Nodes.Add(package.PackageName, package.PackageName);

                foreach (var function in package.Objects.OfType<UFunction>())
                {
                    if (!function.FunctionFlags.HasFlag(FunctionFlag.Native) || function.NativeToken == 0)
                        continue;

                    var item = new NativeTableItem(function);
                    var itemNode = packageNode.Nodes.Add(item.Name);
                    itemNode.Nodes.Add("Type:" + item.Type);
                    itemNode.Nodes.Add("ByteToken:" + item.ByteToken);
                    itemNode.Nodes.Add("OperPrecedence:" + item.OperPrecedence);

                    _NTLPackage.NativeTableList.Add(item);
                }

                TreeView_Packages.EndUpdate();
                TreeView_Packages.Invalidate();
            }
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            string fileName = $"NativesTableList_{FileNameTextBox.Text}{NativesTablePackage.Extension}";

            var dialog = new SaveFileDialog
            {
                FileName = fileName,
                InitialDirectory = Path.Combine
                (
                    Application.StartupPath,
                    "Native Tables"
                )
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using var stream = dialog.OpenFile();
            _NTLPackage.Serialize(stream);
        }
    }

    [ExtensionTitle("NTL Generator")]
    public class ExtNativeGen : IExtension
    {
        private ProgramForm _Form;

        /// <summary>
        /// Called after UEExplorer_Form is initialized.
        /// </summary>
        /// <param name="form"></param>
        public void Initialize(ProgramForm form)
        {
            _Form = form;
        }

        /// <summary>
        /// Called when activated by end-user.
        /// </summary>
        public void OnActivate(object sender, EventArgs e)
        {
            _Form.Tabs.InsertTab(typeof(UC_NativeGenerator), "Natives Table File Generator");
        }
    }
}
