using System;

namespace UEExplorer.Framework.UI.Editor
{
    public class SegmentEventArgs : EventArgs
    {
        public readonly Segment ProgramSegment;

        public SegmentEventArgs(Segment programSegment) => ProgramSegment = programSegment;
    }
}
