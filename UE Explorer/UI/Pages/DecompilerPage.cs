using Krypton.Navigator;
using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;
using UEExplorer.UI.Tabs;

namespace UEExplorer.UI.Pages
{
    internal sealed class DecompilerPage : ObjectBoundPage
    {
        private readonly DecompileOutputPanel _Panel;

        public DecompilerPage()
        {
            Text = Resources.DecompilerPage_DecompilerPage_Decompile_Title;
            TextTitle = Resources.DecompilerPage_DecompilerPage_Decompile_Title;
            Flags &= (int)~(KryptonPageFlags.DockingAllowClose | KryptonPageFlags.DockingAllowAutoHidden);

            _Panel = new DecompileOutputPanel();
            _Panel.Dock = DockStyle.Fill;
            Controls.Add(_Panel);
        }
        
        public override void SetNewObjectTarget(object target, ContentNodeAction action)
        {
            string path = ObjectPathBuilder.GetPath((dynamic)target);
            TextTitle = string.Format(Resources.DecompilerPage_SetNewObjectTarget_Decompile___0_, path);

            _Panel.Object = target;
        }
    }
}