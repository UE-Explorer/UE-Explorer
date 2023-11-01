using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.UI;
using UELib.Annotations;
using UELib.Engine;

namespace UEExplorer.Plugin.Media.Image
{
    [UsedImplicitly]
    internal sealed class ImageEditorControl : Control, ITrackingContext, IContextListener
    {
        private readonly ContextService _ContextService;
        private readonly ImageViewPanel _ImageViewPanel;

        public ImageEditorControl(ContextService contextService)
        {
            _ContextService = contextService;
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            SuspendLayout();
            _ImageViewPanel = new ImageViewPanel();
            _ImageViewPanel.Dock = DockStyle.Fill;
            Controls.Add(_ImageViewPanel);
            ResumeLayout();
        }

        public bool CanAccept(ContextInfo context) => context.ResolvedTarget is UPalette ||
                                                      context.ResolvedTarget is UTexture;

        public Task<bool> Accept(ContextInfo context)
        {
            dynamic imageSource = AssetToImageHelper.From((dynamic)context.ResolvedTarget);
            _ImageViewPanel.SourceImage = imageSource;

            return Task.FromResult(true);
        }

        public bool IsTracking { get; set; }

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
            if (!IsTracking || !CanAccept(e.Context))
            {
                return;
            }

            Accept(e.Context);
        }
    }
}
