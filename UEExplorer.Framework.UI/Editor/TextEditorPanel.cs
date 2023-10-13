using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using Krypton.Toolkit;
using Application = System.Windows.Forms.Application;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace UEExplorer.Framework.UI.Editor
{
    public partial class TextEditorPanel : KryptonPanel
    {
        private readonly SegmentService _StreamSegmentService;

        public readonly TextEditorControl TextEditorControl;

        public TextEditorPanel()
        {
            // Fix for blurriness after scrolling
            SetStyle(ControlStyles.ContainerControl, true);

            InitializeComponent();
            TextEditorControl = textEditorControl;

            var searchPanel = SearchPanel.Install(TextArea);
            searchPanel.RegisterCommands(TextEditorControl.CommandBindings);

            SuspendLayout();

            _StreamSegmentService = new SegmentService(textEditorControl.TextEditor);
            InitializeTextEditorControl();

            ResumeLayout();
        }

        public TextArea TextArea => textEditorControl.TextEditor.TextArea;
        public TextView TextView => textEditorControl.TextEditor.TextArea.TextView;

        private void InitializeTextEditorControl()
        {
            // Fold all { } blocks
            var foldingManager = FoldingManager.Install(textEditorControl.TextEditor.TextArea);
            var foldingStrategy = new XmlFoldingStrategy();
            foldingStrategy.UpdateFoldings(foldingManager, textEditorControl.TextEditor.Document);

            string syntaxFilePath = Path.Combine(Application.StartupPath, "Config", "UnrealScript.xshd");
            if (File.Exists(syntaxFilePath))
            {
                using (var stream = new XmlTextReader(syntaxFilePath))
                {
                    textEditorControl.TextEditor.SyntaxHighlighting = HighlightingLoader.Load(
                        stream, HighlightingManager.Instance);
                }
            }

            textEditorControl.TextEditor.TextArea.TextView.BackgroundRenderers.Add(_StreamSegmentService);
            textEditorControl.TextEditor.TextArea.TextView.LineTransformers.Add(_StreamSegmentService);
            textEditorControl.TextEditor.TextArea.Caret.PositionChanged += CaretOnPositionChanged;
            textEditorControl.TextEditor.TextArea.MouseDoubleClick += TextAreaOnMouseDoubleClick;
            textEditorControl.TextEditor.PreviewMouseHover += TextEditorOnMouseHover;
            textEditorControl.TextEditor.ContextMenuOpening += TextEditorOnContextMenuOpening;
        }

        public event EventHandler<SegmentEventArgs> ActiveSegmentChanged;
        public event EventHandler<SegmentEventArgs> SegmentDoubleClicked;
        public event EventHandler<SegmentEventArgs> ContextMenuOpening;

        private void TextAreaOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var segment = GetSegment(e.GetPosition(TextView));
            if (segment == null)
            {
                return;
            }

            SegmentDoubleClicked?.Invoke(this, new SegmentEventArgs(segment));
        }

        private void CaretOnPositionChanged(object sender, EventArgs e)
        {
            int offset = TextArea.Caret.Offset;
            var segment = GetSegment(offset);
            if (segment == null)
            {
                return;
            }

            ActiveSegmentChanged?.Invoke(this, new SegmentEventArgs(segment));
        }

        private void TextEditorOnMouseHover(object sender, MouseEventArgs e)
        {
            hoverToolTip.Hide(this);

            var segment = GetSegment(e.GetPosition(TextView));
            _StreamSegmentService.ActiveSegment = segment;
            if (segment == null)
            {
                return;
            }

            string text = ""; //ObjectToolTipTextBuilder.GetToolTipText(segment.Location.StreamLocation.Source);

            var p = e.GetPosition(textEditorControl);
            int x = (int)p.X;
            int y = (int)p.Y;

            var vp = TextView.GetPosition(p);
            if (vp.HasValue)
            {
                p = TextView.GetVisualPosition(vp.Value, VisualYPosition.LineBottom);
                y = (int)p.Y;
            }

            hoverToolTip.Show(text,
                this,
                x,
                y);
            e.Handled = true;
        }

        private void TextEditorOnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Segment segment = null;

            var position = TextView.GetPosition(new Point(e.CursorLeft, e.CursorTop));
            if (position.HasValue)
            {
                segment = GetSegment(TextView.GetVisualPosition(position.Value, VisualYPosition.Baseline));
            }

            ContextMenuOpening?.Invoke(this, new SegmentEventArgs(segment));
        }

        public void SetText(string text, bool resetView = true)
        {
            textEditorControl.TextEditor.Text = text;
            if (resetView)
            {
                textEditorControl.TextEditor.ScrollToHome();
            }
        }

        public void FocusSource(in SourceLocation source)
        {
            textEditorControl.TextEditor.ScrollTo(source.Location.Line, source.Location.Column);
            textEditorControl.TextEditor.Select(source.Segment.Index, source.Segment.Length);
        }

        public Segment GetSegment(Point position)
        {
            var editor = textEditorControl.TextEditor;
            var pos = TextView.GetPositionFloor(position + TextView.ScrollOffset);
            bool inDocument = pos.HasValue;
            if (!inDocument)
            {
                return null;
            }

            var logicalPosition = pos.Value.Location;
            int offset = editor.Document.GetOffset(logicalPosition);

            var segments = _StreamSegmentService.Segments.FindSegmentsContaining(offset);
            var segment = segments.FirstOrDefault();
            return segment;
        }

        public Segment GetSegment(int offset)
        {
            var segments = _StreamSegmentService.Segments.FindSegmentsContaining(offset);
            var segment = segments.FirstOrDefault();
            return segment;
        }

        public void AddSegments(IEnumerable<Segment> segments)
        {
            foreach (var segment in segments)
            {
                _StreamSegmentService.Segments.Add(segment);
            }
        }
    }
}
