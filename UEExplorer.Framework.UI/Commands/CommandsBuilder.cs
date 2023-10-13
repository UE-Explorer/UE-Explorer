using System.Collections.Generic;

namespace UEExplorer.Framework.UI.Commands
{
    public sealed class CommandsBuilder
    {
        private readonly ICommandBuilder<IEnumerable<IContextCommand>>[] _Builders;

        public CommandsBuilder(ICommandBuilder<IEnumerable<IContextCommand>>[] builders) => _Builders = builders;

        public IEnumerable<IContextCommand> Build(object subject)
        {
            var commands = new List<IContextCommand>();
            foreach (var builder in _Builders)
            {
                var newCommands = builder.Build(subject);
                commands.AddRange(newCommands);
            }

            return commands;
        }
    }
}
