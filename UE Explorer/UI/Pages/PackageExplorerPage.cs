using Krypton.Navigator;
using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;

namespace UEExplorer.UI.Pages
{
    public sealed class PackageExplorerPage : KryptonPage
    {
        public PackageExplorerPanel PackageExplorerPanel { get; }

        public PackageExplorerPage()
        {
            Text = Resources.PackageExplorerPage_PackageExplorerPage_Package_Explorer_Title;
            TextTitle = Resources.PackageExplorerPage_PackageExplorerPage_Package_Explorer_Title;
            PackageExplorerPanel = new PackageExplorerPanel();
            PackageExplorerPanel.Dock = DockStyle.Fill;
            Controls.Add(PackageExplorerPanel);
        }
    }
}
