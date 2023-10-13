using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UEExplorer.Framework.UI;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;

namespace UEExplorer.Tools.Commands
{
    [Category(CommandCategories.Export)]
    internal class ExportFactoryMenuCommand : SuperContextCommand<IExportFactoryCommand>, IContextCommand, IMenuCommand
    {
        [Localizable(true)] public string Text => Resources.ExportFactoryTool_Text__Export;

        public Image Icon => Resources.Export;
        public Keys ShortcutKeys => Keys.E;
    }
}
