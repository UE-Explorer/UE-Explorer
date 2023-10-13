using System.Drawing;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;

namespace UEExplorer.Tools.Commands
{
    internal class UnloadPackageReferenceCommand : IContextCommand
    {
        public bool CanExecute(object subject) =>
            subject is PackageReference packageReference && packageReference.Linker != null;

        public Task Execute(object subject)
        {
            var packageReference = (PackageReference)subject;
            ServiceHost.GetRequired<PackageManager>().UnloadPackage(packageReference);

            return Task.CompletedTask;
        }

        public string Text => "&Unload";
        public Image Icon => Resources.UnloadSolution;
    }
}
