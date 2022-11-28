namespace UEExplorer.Framework
{
    public struct ProgramLocation
    {
        public readonly SourceLocation SourceLocation;
        public readonly StreamLocation StreamLocation;

        public ProgramLocation(in SourceLocation sourceLocation, in StreamLocation streamLocation)
        {
            SourceLocation = sourceLocation;
            StreamLocation = streamLocation;
        }
    }
}
