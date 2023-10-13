using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;
using UELib;
using UELib.Core;

namespace UEExplorer.Tools.Commands
{
    [Category(CommandCategories.Export)]
    internal class LegacyExportAsFactoryCommand : MenuCommand, IContextCommand, IExportFactoryCommand
    {
        public bool CanExecute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);
            return resolvedTarget is IUnrealExportable exportable && exportable.CanExport();
        }

        public Task Execute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);

            var exportableObject = (IUnrealExportable)resolvedTarget;

            var obj = resolvedTarget as UObject;
            obj?.BeginDeserializing();

            string fileExtensions = string.Empty;
            foreach (string ext in exportableObject.ExportableExtensions)
            {
                fileExtensions += $"{ext}(*.{ext})|*.{ext}";
                if (ext != exportableObject.ExportableExtensions.Last())
                {
                    fileExtensions += "|";
                }
            }

            string fileName = resolvedTarget.ToString();
            var dialog = new SaveFileDialog { Filter = fileExtensions, FileName = fileName };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return Task.CompletedTask;
            }

            using (var stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
            {
                exportableObject.SerializeExport(
                    exportableObject.ExportableExtensions.ElementAt(dialog.FilterIndex - 1), stream);
                stream.Flush();
            }

            return Task.CompletedTask;
        }

        [Localizable(true)] public override string Text => Resources.ExportAsCommand;

        public override Image Icon => Resources.ExportData;
    }
}
