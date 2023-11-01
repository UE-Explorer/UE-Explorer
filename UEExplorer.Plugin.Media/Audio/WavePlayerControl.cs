using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.UI;
using UELib.Annotations;
using UELib.Core;

namespace UEExplorer.Plugin.Media.Audio
{
    [UsedImplicitly]
    internal sealed class WavePlayerControl : Control, ITrackingContext, IContextListener
    {
        private readonly ContextService _ContextService;
        private readonly WavePlayerPanel _WavePlayerPanel;

        public WavePlayerControl(ContextService contextService)
        {
            _ContextService = contextService;
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            SuspendLayout();
            _WavePlayerPanel = new WavePlayerPanel();
            _WavePlayerPanel.Name = "Panel";
            _WavePlayerPanel.Dock = DockStyle.Fill;
            Controls.Add(_WavePlayerPanel);
            ResumeLayout();
        }

        public bool CanAccept(ContextInfo context) => context.ResolvedTarget is USound;

        public Task<bool> Accept(ContextInfo context)
        {
            _WavePlayerPanel.WaveSource = (USound)context.ResolvedTarget;

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
