using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;

namespace UEExplorer.Framework.UI
{
    public class SuperContextCommand<TChildCommand> : IChildCommand
        where TChildCommand : class
    {
        public bool CanExecute(object subject) =>
            GetItems()?.Any(cmd => cmd is ICommand<object> ctxCmd && ctxCmd.CanExecute(subject)) ?? false;

        public Task Execute(object subject)
        {
            var defaultCommand = GetItems()
                ?.OfType<IContextCommand>()
                .First(cmd => cmd.CanExecute(subject));

            return defaultCommand?.Execute(subject) ?? Task.CompletedTask;
        }

        public IEnumerable<IMenuCommand> GetItems()
        {
            var modeCommands = ServiceHost
                .GetAll<TChildCommand>()
                .OfType<IMenuCommand>();

            return modeCommands;
        }
    }
}
