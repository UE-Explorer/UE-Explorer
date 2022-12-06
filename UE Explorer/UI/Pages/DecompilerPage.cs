using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;
using UELib;

namespace UEExplorer.UI.Pages
{
    internal sealed class DecompilerPage : TrackingPage
    {
        private readonly ContextProvider _ContextService;
        private readonly DecompileOutputPanel _Panel;

        public DecompilerPage(ContextProvider contextService)
        {
            _ContextService = contextService;
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            Text = Resources.DecompilerPage_DecompilerPage_Decompile_Title;
            TextTitle = Resources.DecompilerPage_DecompilerPage_Decompile_Title;

            _Panel = new DecompileOutputPanel();
            _Panel.Name = "Panel";
            _Panel.Dock = DockStyle.Fill;
            Controls.Add(_Panel);

            _Panel.EditorControl.TextEditor.TextArea.Caret.PositionChanged += (sender, args) =>
            {
                // Placeholder for when we have tree nodes, we'll traverse that to retrieve the stream's location
                var context = new ContextInfo(
                    ContextActionKind.Location,
                    _Panel.Object,
                    StreamLocationFactory.Create(_Panel.Object));
                var contextChangedEvent = new ContextChangedEventArgs(context);
                contextService.OnContextChanged(this, contextChangedEvent);
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ContextService.ContextChanged -= ContextServiceOnContextChanged;
            }

            base.Dispose(disposing);
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (!CanAccept(e.Context) || e.Context.ActionKind == ContextActionKind.Location)
            {
                return;
            }

            Accept(e.Context);
        }

        public override void OnFind(TextSearchHelpers.FindResult findResult)
        {
            _Panel.TextEditorPanel.TextEditorControl.TextEditor.ScrollTo(findResult.TextLine, findResult.TextColumn);
            _Panel.TextEditorPanel.TextEditorControl.TextEditor.Select(findResult.TextIndex, findResult.TextLength);
        }

        public override void OnFind(string text)
        {
            EditorUtil.FindText(_Panel.TextEditorPanel.TextEditorControl.TextEditor, text);
        }

        public override bool CanAccept(ContextInfo context)
        {
            return IsTracking && context.Target is IUnrealDecompilable;
        }

        public override bool Accept(ContextInfo context)
        {
            if (context.Target == null)
            {
                TextTitle = Resources.DecompilerPage_DecompilerPage_Decompile_Title;
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)context.Target);
                TextTitle = string.Format(Resources.DecompilerPage_OnObjectTarget_Decompile___0_, path);
                Text = TextTitle;
            }

            _Panel.Object = context.Target;
            return true;
        }
    }
}
