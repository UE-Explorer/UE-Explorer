using System;
using System.Collections.Generic;

namespace UEExplorer.Framework.UI.Commands
{
    public sealed class ContainedCommandsBuilder : ICommandBuilder<IEnumerable<IContextCommand>>
    {
        private readonly Action<object, List<IContextCommand>> _BuildAction;

        public ContainedCommandsBuilder(Action<object, List<IContextCommand>> buildAction) =>
            _BuildAction = buildAction;

        public IEnumerable<IContextCommand> Build(object subject)
        {
            var commands = new List<IContextCommand>();
            _BuildAction.Invoke(subject, commands);
            return commands;
        }
    }
}
