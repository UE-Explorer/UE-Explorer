using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;
using UEExplorer.UI.Dialogs;

namespace UEExplorer.Tools.Commands
{
    [Category(CommandCategories.Options)]
    internal class OpenPackageReferenceSettingsCommand : MenuCommand, IContextCommand
    {
        public bool CanExecute(object subject) =>
            subject is PackageReference;

        public Task Execute(object subject)
        {
            using (var dialog = new PackageReferenceDialog())
            {
                dialog.PackageReference = (PackageReference)subject;
                dialog.ShowDialog();
            }

            return Task.CompletedTask;
        }

        public override string Text => "&Settings";
        public override Image Icon => Resources.Settings;
    }
}
