using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;

namespace UEExplorer.UI.Pages
{
    internal sealed class BinaryPage : ObjectBoundPage
    {
        private readonly BinaryDataFieldsPanel _Panel;

        public BinaryPage()
        {
            Text = Resources.BinaryPage_BinaryPage_Binary_Title;
            TextTitle = Resources.BinaryPage_BinaryPage_Binary_Title;

            _Panel = new BinaryDataFieldsPanel();
            _Panel.Name = "Panel";
            _Panel.Dock = DockStyle.Fill;
            Controls.Add(_Panel);
        }

        public override void OnObjectTarget(object target, ContentNodeAction action, bool isPending)
        {
            if (target == null)
            {
                TextTitle = Resources.BinaryPage_BinaryPage_Binary_Title;
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)target);
                TextTitle = string.Format(Resources.BinaryPage_SetNewObjectTarget_BinaryData___0_, path);
                Text = TextTitle;
            }

            _Panel.HasPendingUpdate = isPending;
            _Panel.Object = target;
        }

        public override void OnFind(TextSearchHelpers.FindResult findResult)
        {
        }

        public override void OnFind(string text)
        {
        }
    }
}