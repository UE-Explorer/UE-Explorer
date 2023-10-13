using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UELib.Annotations;

namespace UEExplorer.Framework.UI.Commands
{
    public class ContextCommandBuilder
    {
        private readonly IServiceProvider _ServiceProvider;

        public ContextCommandBuilder(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }
            
        public IEnumerable<TCommand> Build<TCommand>(object subject)
            where TCommand : IContextCommand
        {
            return _ServiceProvider
                .GetServices<TCommand>()
                .Where(cmd => cmd.CanExecute(subject));
        }

        [CanBeNull]
        public IEnumerable<TCommand> Build<TCommand>(IChildCommand command)
            where TCommand : IMenuCommand
        {
            return (IEnumerable<TCommand>)command.GetItems();
        }
    }
}
