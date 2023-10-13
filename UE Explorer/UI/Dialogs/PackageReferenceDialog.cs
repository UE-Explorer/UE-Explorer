using System;
using System.Windows.Forms;
using Krypton.Toolkit;
using UEExplorer.Framework;
using UEExplorer.Properties;

namespace UEExplorer.UI.Dialogs
{
    public partial class PackageReferenceDialog : KryptonForm
    {
        public PackageReferenceDialog() => InitializeComponent();

        public PackageReference PackageReference { get; set; }

        private void PackageReferenceDialog_Load(object sender, EventArgs e)
        {
            packageReferenceKryptonLabel.Text = PackageReference.ToString();
            settingsPropertyGrid.SelectedObject = PackageReference.Settings;
        }

        private void PackageReferenceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Because we are using a struct we have to copy it back.
            PackageReference.Settings = (PackageSettings)settingsPropertyGrid.SelectedObject;
            UserHistory.Default.Save();
        }
    }
}
