using System.Windows.Forms;
using UEExplorer.UI.ActionPanels;
using UELib.Core;
using UELib.Engine;

namespace UEExplorer.UI.Pages
{
    internal sealed class ImageEditorPage : TrackingPage
    {
        private readonly ContextProvider _ContextService;
        private readonly ImageEditorPanel _Panel;

        public ImageEditorPage(ContextProvider contextService)
        {
            _ContextService = contextService;
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            Text = "Image";
            TextTitle = "Image";

            _Panel = new ImageEditorPanel();
            _Panel.Name = "Panel";
            _Panel.Dock = DockStyle.Fill;
            Controls.Add(_Panel);
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
            if (!CanAccept(e.Context))
            {
                return;
            }

            Accept(e.Context);
        }

        public override void OnFind(TextSearchHelpers.FindResult findResult)
        {
        }

        public override void OnFind(string text)
        {
        }

        public override bool CanAccept(ContextInfo context)
        {
            return IsTracking && (
                context.Target is UPalette ||
                context.Target is UTexture ||
                context.Target is UPolys
            );
        }

        public override bool Accept(ContextInfo context)
        {
            if (context.Target == null)
            {
                TextTitle = "Image";
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)context.Target);
                TextTitle = string.Format("Image: {0}", path);
                Text = TextTitle;
            }

            _Panel.Object = context.Target;
            return true;
        }
    }
}