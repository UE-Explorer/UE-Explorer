namespace UEExplorer.Framework
{
    public struct ProgramLocation
    {
        public SourceLocation SourceLocation;
        public StreamLocation StreamLocation;

        public ProgramLocation(in SourceLocation sourceLocation, in StreamLocation streamLocation)
        {
            SourceLocation = sourceLocation;
            StreamLocation = streamLocation;
        }
    }
}
