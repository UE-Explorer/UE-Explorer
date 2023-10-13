using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using UELib.Core;

namespace UEExplorer.Framework.UI.Editor
{
    public sealed class SegmentService : DocumentColorizingTransformer, IBackgroundRenderer
    {
        private readonly TextEditor _Editor;
        private readonly TextDocument _Document;
        private Segment _ActiveSegment;

        public Segment ActiveSegment
        {
            get => _ActiveSegment;
            set
            {
                if (_ActiveSegment != null)
                {
                    var lastSegment = _ActiveSegment;
                    _ActiveSegment = null;
                    _Editor.TextArea.TextView.Redraw(lastSegment.StartOffset, lastSegment.Length);
                }

                _ActiveSegment = value;
                if (_ActiveSegment != null)
                {
                    _Editor.TextArea.TextView.Redraw(_ActiveSegment.StartOffset, _ActiveSegment.Length);
                }
            }
        }

        public SegmentService(TextEditor editor)
        {
            _Editor = editor;
            _Document = editor.Document;
            Segments = new TextSegmentCollection<Segment>(_Document);
        }

        public TextSegmentCollection<Segment> Segments { get; }

        public Brush KeywordForegroundBrush { get; } = new SolidColorBrush(Brushes.Blue.Color);

        public Brush RectBrush { get; } = new SolidColorBrush(Color.FromRgb(98, 98, 98));
        public Brush OpCodeBrush { get; } = new SolidColorBrush(Brushes.DarkKhaki.Color);
        public Brush TypeBrush { get; } = new SolidColorBrush(Brushes.DarkCyan.Color);
        public Brush ActiveBrush { get; } = new SolidColorBrush(Brushes.CadetBlue.Color);
        public Brush ActiveForegroundBrush { get; } = new SolidColorBrush(Brushes.Black.Color);

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (textView == null)
            {
                throw new ArgumentNullException(nameof(textView));
            }

            if (drawingContext == null)
            {
                throw new ArgumentNullException(nameof(drawingContext));
            }

            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
            {
                return;
            }

            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
            var segments = Segments.FindOverlappingSegments(viewStart, viewEnd - viewStart);
            foreach (var segment in segments)
            {
                var geoBuilder = new BackgroundGeometryBuilder { AlignToWholePixels = true, };
                geoBuilder.AddSegment(textView, segment);
                var geometry = geoBuilder.CreateGeometry();
                if (geometry == null)
                {
                    continue;
                }

                if (segment == ActiveSegment)
                {
                    drawingContext.DrawGeometry(ActiveBrush, null, geometry);
                }
            }
        }

        public KnownLayer Layer => KnownLayer.Selection;

        protected override void ColorizeLine(DocumentLine line)
        {
            var segments = Segments.FindOverlappingSegments(line.Offset, line.TotalLength);
            foreach (var segment in segments)
            {
                if (segment == ActiveSegment)
                {
                    ChangeLinePart(segment.StartOffset, segment.EndOffset, ApplyActiveLine);
                    continue;
                    ;
                }

                switch (segment.Location.StreamLocation.Source)
                {
                    case UStruct.UByteCodeDecompiler.Token _:
                        //ChangeLinePart(line.Offset, line.EndOffset, ApplyOpCodeLine);
                        ChangeLinePart(segment.StartOffset, segment.EndOffset, ApplyOpCodeSegment);
                        break;

                    case UObject _:
                        ChangeLinePart(segment.StartOffset, segment.EndOffset, ApplyTypeSegment);
                        break;

                    case null:
                        ChangeLinePart(segment.StartOffset, segment.EndOffset, ApplyKeywordSegment);
                        break;
                }
            }
        }

        private void ApplyKeywordSegment(VisualLineElement element)
        {
            element.TextRunProperties.SetForegroundBrush(KeywordForegroundBrush);
        }

        void ApplyActiveLine(VisualLineElement element)
        {
            element.TextRunProperties.SetBackgroundBrush(ActiveBrush);
            element.TextRunProperties.SetForegroundBrush(ActiveForegroundBrush);
        }

        void ApplyOpCodeLine(VisualLineElement element)
        {
            element.TextRunProperties.SetBackgroundBrush(RectBrush);
        }

        void ApplyOpCodeSegment(VisualLineElement element)
        {
            var tf = element.TextRunProperties.Typeface;
            element.TextRunProperties.SetTypeface(new Typeface(tf.FontFamily, tf.Style, FontWeights.Bold, tf.Stretch));
            element.TextRunProperties.SetForegroundBrush(OpCodeBrush);
        }

        void ApplyTypeSegment(VisualLineElement element)
        {
            element.TextRunProperties.SetForegroundBrush(TypeBrush);
        }
    }
}
