using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UELib.Annotations;

namespace UEExplorer.Framework.UI.Commands
{
    public interface IChildCommand
    {
        [CanBeNull]
        IEnumerable<IMenuCommand> GetItems();
    }
    
    // TODO: Implement ICommand or reference an ICommand as a property?
    public interface IMenuCommand : IChildCommand
    {
        Image Icon { get; }
        string Text { get; }
        Keys ShortcutKeys { get; }
    }

    public abstract class MenuCommand : IMenuCommand
    {
        public virtual Image Icon => null;
        public virtual string Text { get; }
        public virtual Keys ShortcutKeys { get; }

        public virtual IEnumerable<IMenuCommand> GetItems()
        {
            return null;
        }
    }
}
