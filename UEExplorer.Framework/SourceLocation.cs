namespace UEExplorer.Framework
{
    public struct SourceLocation
    {
        public static SourceLocation Empty = new SourceLocation(-1, -1, -1, -1);

        public TextLocation Location;
        public TextSegment Segment;

        public SourceLocation(int line, int column, int index, int length)
        {
            Location.Line = line;
            Location.Column = column;
            Segment.Index = index;
            Segment.Length = length;
        }

        public SourceLocation(in TextLocation location, in TextSegment segment)
        {
            Location = location;
            Segment = segment;
        }

        public override string ToString()
        {
            return $"({Location.Line}, {Location.Column})";
        }
    }
}
