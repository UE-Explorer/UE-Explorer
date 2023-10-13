using System.Drawing;
using System.Windows.Forms;
using UEExplorer.Framework.UI;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;

namespace UEExplorer.Tools.Commands
{
    internal sealed class DecompilerFactoryMenuCommand : SuperContextCommand<IDecompilerFactoryCommand>,
        IContextCommand, IMenuCommand
    {
        public string Text => Resources.NodeItem_Decompile;
        public Image Icon => Resources.Code;
        public Keys ShortcutKeys => Keys.D;
    }
}
