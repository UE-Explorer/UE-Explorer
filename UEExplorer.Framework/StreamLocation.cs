namespace UEExplorer.Framework
{
    public struct StreamLocation
    {
        public static StreamLocation Empty = new StreamLocation(null, -1);

        public readonly object Source;
        public readonly long Position;

        public StreamLocation(object source, long position)
        {
            Source = source;
            Position = position;
        }
    }
}
