using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;
using UELib.Core;

namespace UEExplorer.UI.Pages
{
    internal sealed class WavePlayerPage : TrackingPage
    {
        private readonly ContextProvider _ContextService;
        private readonly WavePlayerPanel _Panel;

        public WavePlayerPage(ContextProvider contextService)
        {
            _ContextService = contextService;
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            Text = Resources.WavePlayerPage_WavePlayerPage_WavePlayer_Title;
            TextTitle = Resources.WavePlayerPage_WavePlayerPage_WavePlayer_Title;

            _Panel = new WavePlayerPanel();
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
            return IsTracking && context.Target is USound;
        }

        public override bool Accept(ContextInfo context)
        {
            if (context.Target == null)
            {
                TextTitle = Resources.WavePlayerPage_WavePlayerPage_WavePlayer_Title;
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)context.Target);
                TextTitle = string.Format(Resources.WavePlayerPage_OnObjectTarget_WavePlayer___0_, path);
                Text = TextTitle;
            }

            _Panel.Object = context.Target;
            return true;
        }
    }
}