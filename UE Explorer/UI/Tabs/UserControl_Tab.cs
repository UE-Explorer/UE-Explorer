using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    // TODO: Deprecate
    [ComVisible(false)]
    public class UserControl_Tab : UserControl, ITabComponent
    {
        protected UserControl_Tab()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                Dock = DockStyle.Fill;
            }
        }

        public virtual void TabSave()
        {
        }

        public virtual void TabFind()
        {
        }
    }
}