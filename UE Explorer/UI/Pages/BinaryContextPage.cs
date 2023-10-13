using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;
using UELib;
using UELib.Annotations;

namespace UEExplorer.UI.Pages
{
    [UsedImplicitly]
    internal sealed class BinaryContextPage : TrackingContextPage
    {
        private readonly ContextService _ContextService;
        private readonly BinaryDataFieldsPanel _Panel;

        public BinaryContextPage()
        {
            _ContextService = ServiceHost.GetRequired<ContextService>();
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            TextPrefix = Resources.BinaryPage_BinaryPage_Binary_Title;

            SuspendLayout();
            _Panel = new BinaryDataFieldsPanel();
            _Panel.Name = "Panel";
            _Panel.Dock = DockStyle.Fill;
            Controls.Add(_Panel);
            ResumeLayout();
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

        public override bool CanAccept(ContextInfo context)
        {
            return IsTracking && context.ResolvedTarget is IBinaryData;
        }

        public override Task<bool> Accept(ContextInfo context)
        {
            UpdateContextText(context);

            _Panel.Object = context.ResolvedTarget;
            return Task.FromResult(true);
        }
    }
}
