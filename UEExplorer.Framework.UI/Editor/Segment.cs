namespace UEExplorer.Framework.UI.Editor
{
    public sealed class Segment : ICSharpCode.AvalonEdit.Document.TextSegment
    {
        public Segment(in ProgramLocation location)
        {
            Location = location;

            StartOffset = location.SourceLocation.Segment.Index;
            Length = location.SourceLocation.Segment.Length;
        }

        public ProgramLocation Location { get; }
    }
}
