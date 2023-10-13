using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;
using UELib.Core;

namespace UEExplorer.Tools.Commands
{
    // TODO: Re-implement as a dialog with the option to choose which data types are to be exported.
    
    [Category(CommandCategories.Export)]
    internal class ExportPackageReferenceClassesMenuCommand : MenuCommand, IContextCommand, IExportFactoryCommand
    {
        public bool CanExecute(object subject) =>
            subject is PackageReference packageReference
            && packageReference.Linker != null
            && packageReference.Linker.Exports.Any(exp => exp.Class == null);

        public Task Execute(object subject)
        {
            var packageReference = (PackageReference)subject;
            Debug.Assert(packageReference.Linker != null, "packageReference.Linker != null");
            
            using (var dialog = new CommonOpenFileDialog
                   {
                       InitialDirectory = Application.StartupPath,
                       IsFolderPicker = true
                   })
            {
                var result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    ExportPackageObjects<UClass>(packageReference, dialog.FileName);
                }
            }

            return Task.CompletedTask;
        }

        public override string Text => "&Classes...";
        public override Image Icon => Resources.Code;

        // TODO: Implement this as a cancellable task.
        internal static void ExportPackageObjects<T>(PackageReference packageReference, string path)
            where T : UObject
        {
            var linker = packageReference.Linker;
            linker.ExportPackageObjects<T>(path);

            var dialogResult = MessageBox.Show(
                string.Format(Resources.EXPORTED_ALL_PACKAGE_CLASSES, path),
                Application.ProductName,
                MessageBoxButtons.YesNo
            );
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start(path);
            }
        }
    }

    [Category(CommandCategories.Export)]
    internal class ExportPackageReferenceScriptsMenuCommand : MenuCommand, IContextCommand, IExportFactoryCommand
    {
        public bool CanExecute(object subject) =>
            subject is PackageReference packageReference
            && packageReference.Linker != null
            && packageReference.Linker.Exports.Any(exp => exp.Class != null && exp.Class.ObjectName == "TextBuffer");

        public Task Execute(object subject)
        {
            var packageReference = (PackageReference)subject;
            Debug.Assert(packageReference.Linker != null, "packageReference.Linker != null");

            using (var dialog = new CommonOpenFileDialog
                   {
                       InitialDirectory = Application.StartupPath,
                       IsFolderPicker = true
                   })
            {
                var result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    ExportPackageReferenceClassesMenuCommand.ExportPackageObjects<UClass>(packageReference, dialog.FileName);
                }
            }

            return Task.CompletedTask;
        }

        public override string Text => "&Scripts...";
        public override Image Icon => Resources.ExportScript;
    }
}
