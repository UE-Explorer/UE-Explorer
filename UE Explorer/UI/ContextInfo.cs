namespace UEExplorer.UI
{
    public class ContextInfo
    {
        public readonly ContextActionKind ActionKindKind;
        public readonly object Target;
        public readonly ProgramLocation Location;

        public ContextInfo(ContextActionKind actionKindKind, object target)
        {
            ActionKindKind = actionKindKind;
            Target = target;
            Location = new ProgramLocation(new StreamLocation(null, -1));
        }

        public ContextInfo(ContextActionKind actionKindKind, object target, StreamLocation location)
        {
            ActionKindKind = actionKindKind;
            Target = target;
            Location = new ProgramLocation(location);
        }
    }

    public struct ProgramLocation
    {
        public readonly StreamLocation StreamLocation;

        public ProgramLocation(StreamLocation streamLocation)
        {
            StreamLocation = streamLocation;
        }
    }

    public struct StreamLocation
    {
        public object Source;
        public readonly long Position;

        public StreamLocation(object source, long position)
        {
            Source = source;
            Position = position;
        }
    }
}