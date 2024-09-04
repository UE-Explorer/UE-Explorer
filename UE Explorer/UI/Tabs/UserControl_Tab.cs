using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    // TODO: Deprecate
    [ComVisible(false)]
    public class UserControl_Tab : UserControl, ITabComponent
    {
        public virtual void TabSave()
        {
        }

        public virtual void TabFind()
        {
        }
    }
}
