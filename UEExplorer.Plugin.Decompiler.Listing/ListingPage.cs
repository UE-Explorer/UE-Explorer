using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit.Document;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Editor;
using UEExplorer.Framework.UI.Pages;
using UELib;
using UELib.Annotations;
using UELib.Core;
using UELib.Decompiler.Listing;

namespace UEExplorer.Plugin.Decompiler.Listing
{
    [UsedImplicitly]
    internal sealed class ListingContext : Page, ITrackingContext, IContextListener
    {
        private readonly ContextService _ContextService;
        private readonly TextEditorPanel _EditorPanel;

        private object _CurrentContextTarget;

        public ListingContext(ContextService contextService)
        {
            _ContextService = contextService;
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            Text = "Listing";
            TextTitle = "Listing";

            SuspendLayout();
            _EditorPanel = new TextEditorPanel();
            _EditorPanel.Dock = DockStyle.Fill;
            Controls.Add(_EditorPanel);
            ResumeLayout();

            _EditorPanel.ActiveSegmentChanged += EditorPanelOnActiveSegmentChanged;
        }

        public bool CanAccept(ContextInfo context) =>
            IsTracking && ListingDecompiler.CanDecompile(context.ResolvedTarget);

        public bool Accept(ContextInfo context)
        {
            _CurrentContextTarget = context.ResolvedTarget;

            if (context.ResolvedTarget == null)
            {
                TextTitle = "Listing";
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)context.ResolvedTarget);
                TextTitle = string.Format("Listing: {0}", path);
                Text = TextTitle;
            }

            BuildListing((UStruct)_CurrentContextTarget);

            return true;
        }

        public bool IsTracking { get; set; }
        public IContainerControl ContainedControl { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ContextService.ContextChanged -= ContextServiceOnContextChanged;
            }

            base.Dispose(disposing);
        }

        private void EditorPanelOnActiveSegmentChanged(object sender, SegmentEventArgs e)
        {
            object target = e.ProgramSegment.Location.StreamLocation.Source;
            var context = new ContextInfo(
                ContextActionKind.Location,
                target,
                StreamLocationFactory.Create(target));
            var contextChangedEvent = new ContextChangedEventArgs(context);
            _ContextService.OnContextChanged(this, contextChangedEvent);
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (sender == this)
            {
                return;
            }

            if (!CanAccept(e.Context))
            {
                return;
            }

            if (_CurrentContextTarget != e.Context.ResolvedTarget)
            {
                Accept(e.Context);
            }

            if (Equals(e.Context.Location.SourceLocation, SourceLocation.Empty))
            {
                return;
            }

            BeginInvoke((MethodInvoker)(() => _EditorPanel.FocusSource(e.Context.Location.SourceLocation)));
        }

        private void BuildListing(IAcceptable target)
        {
            var editor = _EditorPanel.TextEditorControl.TextEditor;
            var uiThread = Thread.CurrentThread;
            TextDocument document = null;
            //await Task.Run(() =>
            //{
            //document = new TextDocument();
            document = editor.Document;

            var text = new StringBuilder();
            var stream = new StringWriter(text);
            var outputStream = new TextEditorOutputStream(document, stream);
            var decompiler = new ListingDecompiler(outputStream);
            decompiler
                .Run(target);

            //document.Text = stream.ToString();
            //document.SetOwnerThread(uiThread);
            //});

            // Experimental stuff here
            editor.Document = document;
            document.Text = stream.ToString();

            _EditorPanel.AddSegments(outputStream.Locations
                .Select(loc => new Segment(loc)));
        }
    }
}
