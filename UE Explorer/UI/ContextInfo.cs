using UEExplorer.Framework;

namespace UEExplorer.UI
{
    public class ContextInfo
    {
        public readonly ContextActionKind ActionKind;
        public readonly ProgramLocation Location;
        public readonly object Target;

        public ContextInfo(ContextActionKind actionKind, object target)
        {
            ActionKind = actionKind;
            Target = target;
            Location = new ProgramLocation(SourceLocation.Empty, StreamLocation.Empty);
        }

        public ContextInfo(ContextActionKind actionKind, object target, in SourceLocation location)
        {
            ActionKind = actionKind;
            Target = target;
            Location = new ProgramLocation(location, StreamLocation.Empty);
        }

        public ContextInfo(ContextActionKind actionKind, object target, in StreamLocation location)
        {
            ActionKind = actionKind;
            Target = target;
            Location = new ProgramLocation(SourceLocation.Empty, location);
        }
    }
}
