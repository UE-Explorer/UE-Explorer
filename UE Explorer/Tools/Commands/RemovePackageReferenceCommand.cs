using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;

namespace UEExplorer.Tools.Commands
{
    [Category(CommandCategories.Edit)]
    internal class RemovePackageReferenceCommand : IContextCommand
    {
        public bool CanExecute(object subject) => subject is PackageReference;

        public Task Execute(object subject)
        {
            var packageReference = (PackageReference)subject;
            ServiceHost.GetRequired<PackageManager>().RemovePackage(packageReference);

            return Task.CompletedTask;
        }

        public string Text => "&Remove";
        public Image Icon => Resources.Remove;
    }
}
