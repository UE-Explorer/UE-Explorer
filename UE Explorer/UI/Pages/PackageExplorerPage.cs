using System.Windows.Forms;
using Krypton.Navigator;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;

namespace UEExplorer.UI.Pages
{
    public sealed class PackageExplorerPage : KryptonPage
    {
        public PackageExplorerPage(ContextProvider contextService)
        {
            Text = Resources.PackageExplorerPage_PackageExplorerPage_Package_Explorer_Title;
            TextTitle = Resources.PackageExplorerPage_PackageExplorerPage_Package_Explorer_Title;
            PackageExplorerPanel = new PackageExplorerPanel(contextService);
            PackageExplorerPanel.Dock = DockStyle.Fill;
            Controls.Add(PackageExplorerPanel);
        }

        public PackageExplorerPanel PackageExplorerPanel { get; }
    }
}