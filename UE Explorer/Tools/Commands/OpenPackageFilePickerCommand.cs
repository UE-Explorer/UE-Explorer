using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;
using UEExplorer.UI.Main;
using UELib;

namespace UEExplorer.Tools.Commands
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class OpenPackageFilePickerCommand : MenuCommand, ICommand<object>
    {
        public override Image Icon => Resources.OpenfileDialog;
        public override Keys ShortcutKeys => Keys.Control | Keys.O;
        public bool CanExecute(object subject) => true;

        public Task Execute(object subject)
        {
            using (var openFileDialog = new OpenFileDialog
                   {
                       Filter = UnrealExtensions.FormatUnrealExtensionsAsFilter(),
                       FilterIndex = 1,
                       Title = Resources.Open_File,
                       Multiselect = true
                   })
            {
                if (openFileDialog.ShowDialog(ServiceHost.Get<ProgramForm>()) != DialogResult.OK)
                {
                    return Task.CompletedTask;
                }

                var packageManager = ServiceHost.GetRequired<PackageManager>();
                foreach (string filePath in openFileDialog.FileNames)
                {
                    Program.PushRecentOpenedFile(filePath);
                    packageManager.RegisterPackage(filePath);
                }
            }

            return Task.CompletedTask;
        }
    }
}
