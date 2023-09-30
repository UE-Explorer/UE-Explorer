namespace UEExplorer.Framework
{
    public struct StreamLocation
    {
        public static StreamLocation Empty = new StreamLocation(null, -1);

        public readonly object Source;
        public readonly long Position;
        public readonly int Size;

        public StreamLocation(object source, long position, int size = -1)
        {
            Source = source;
            Position = position;
            Size = size;
        }
    }
}
