using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;

namespace Eliot.Extensions.NativesTableListGenerator
{
    [ComVisible(false)]
    public partial class UC_NativeGenerator : UserControl_Tab
    {
        private readonly NativesTablePackage _NTLPackage = new NativesTablePackage();

        public UC_NativeGenerator()
        {
            InitializeComponent();

            _NTLPackage.NativeTableList = new List<NativeTableItem>();
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
                package.InitializePackage();

                var nativeFunctions = package.Objects
                    .OfType<UFunction>()
                    .Where(fun => fun.NativeToken != 0)
                    .ToList();

                var entries = nativeFunctions
                    .Select(fun => new NativeTableItem(fun))
                    .ToList();
                _NTLPackage.NativeTableList.AddRange(entries);

                TreeView_Packages.BeginUpdate();
                var packageNode = TreeView_Packages.Nodes.Add(package.PackageName);
                foreach (var item in entries)
                {
                    var itemNode = packageNode.Nodes.Add(item.Name);
                    itemNode.Nodes.Add("Format:" + item.Type);
                    itemNode.Nodes.Add("ByteToken:" + item.ByteToken);
                    itemNode.Nodes.Add("OperPrecedence:" + item.OperPrecedence);
                }

                TreeView_Packages.EndUpdate();

                package.Dispose();
            }
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            _NTLPackage.CreatePackage
            (
                Path.Combine
                (
                    Application.StartupPath,
                    "Native Tables",
                    "NativesTableList_" + FileNameTextBox.Text
                )
            );
        }
    }
}