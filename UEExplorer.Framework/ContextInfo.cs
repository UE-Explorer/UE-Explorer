using UEExplorer.Framework.Commands;
using UELib.Annotations;

namespace UEExplorer.Framework
{
    public class ContextInfo
    {
        public readonly ContextActionKind ActionKind;
        [CanBeNull] public readonly ICommand<object> Command;
        public readonly ProgramLocation Location;
        public readonly object Target;
        private object _ResolvedTarget;

        public ContextInfo(ContextActionKind actionKind, object target)
            : this(actionKind, target, new ProgramLocation(SourceLocation.Empty, StreamLocation.Empty))
        {
        }

        public ContextInfo(ICommand<object> command, object target)
            : this(ContextActionKind.Command, target,
                new ProgramLocation(SourceLocation.Empty, StreamLocation.Empty)) =>
            Command = command;

        public ContextInfo(ContextActionKind actionKind, object target, in ProgramLocation location)
        {
            ActionKind = actionKind;
            Target = target;
            Location = location;
        }

        public ContextInfo(ContextActionKind actionKind, object target, in SourceLocation location)
            : this(actionKind, target, new ProgramLocation(location, StreamLocation.Empty))
        {
        }

        public ContextInfo(ContextActionKind actionKind, object target, in StreamLocation location)
            : this(actionKind, target, new ProgramLocation(SourceLocation.Empty, location))
        {
        }

        public object ResolvedTarget
        {
            get => _ResolvedTarget ?? Target;
            set => _ResolvedTarget = value;
        }
    }
}
