using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;

namespace UEExplorer.Tools.Commands
{
    [Category(CommandCategories.External)]
    internal class OpenPackageReferenceInExplorerCommand : MenuCommand, IContextCommand
    {
        public bool CanExecute(object subject) =>
            subject is PackageReference packageReference && !string.IsNullOrEmpty(packageReference.FilePath);

        public Task Execute(object subject)
        {
            string packageFilePath = ((PackageReference)subject).FilePath;
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer", Arguments = $"/select, \"{packageFilePath}\""
            });

            return Task.CompletedTask;
        }

        public override string Text => "&Open Folder in File Explorer";
        public override Image Icon => Resources.Open;
    }
}
