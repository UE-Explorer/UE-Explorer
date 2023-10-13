using System.Drawing;
using UEExplorer.Framework.Commands;

namespace UEExplorer.Framework.UI.Commands
{
    public interface IContextCommand : ICommand<object>
    {
        string Text { get; }
        Image Icon { get; }
    }
}
