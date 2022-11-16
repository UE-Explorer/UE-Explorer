namespace UEExplorer.UI
{
    public class ContextInfo
    {
        public readonly ContextActionKind ActionKind;
        public readonly object Target;
        public readonly ProgramLocation Location;

        public ContextInfo(ContextActionKind actionKind, object target)
        {
            ActionKind = actionKind;
            Target = target;
            Location = new ProgramLocation(new StreamLocation(null, -1));
        }

        public ContextInfo(ContextActionKind actionKind, object target, StreamLocation location)
        {
            ActionKind = actionKind;
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
