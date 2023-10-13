using System.ComponentModel;
using UEExplorer.Framework.Plugin;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Plugin.Decompiler.Listing.UI;

namespace UEExplorer.Plugin.Decompiler.Listing
{
    [DisplayName("Listing")]
    [PluginProvideService(typeof(IMenuCommand), typeof(ListingViewCommand))]
    public class ListingPluginModule : IPluginModule
    {
        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}
