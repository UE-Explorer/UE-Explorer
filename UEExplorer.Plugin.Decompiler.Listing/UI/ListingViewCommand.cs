using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Services;

namespace UEExplorer.Plugin.Decompiler.Listing.UI
{
    [CommandCategory(CommandCategories.View)]
    public class ListingViewCommand : MenuCommand, ICommand<object>
    {
        public bool CanExecute(object subject) => true;

        public Task Execute(object subject)
        {
            // TODO: Custom navigator?
            //var viewToolNav = (KryptonDockingNavigator)dockingManager.ResolvePath(pageCommand.Path);
            //if (viewToolNav == null)
            //{
            //    viewToolNav = dockingManager.ManageNavigator(typeof(T).Name, documentsNavigator);
            //}

            //if (viewToolNav.FindPageElement(typeof(T).Name) != null)
            //{
            //    return;
            //}

            //var page = new T();
            //viewToolNav.Append(page);
            //viewToolNav.SelectPage(page.UniqueName);
            ServiceHost.GetRequired<IDockingService>().AddDocumentUnique<ListingContextPage>("Listing", Text);
            return Task.CompletedTask;
        }

        public override Image Icon => Resources.Legend;
        public override string Text => "Listing";
        public override Keys ShortcutKeys => Keys.Control | Keys.L;
    }
}
