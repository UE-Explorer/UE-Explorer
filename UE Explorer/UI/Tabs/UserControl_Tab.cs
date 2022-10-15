using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Storm.TabControl;

namespace UEExplorer.UI.Tabs
{
    [ComVisible(false)]
    public abstract class UserControl_Tab : UserControl, ITabComponent
    {
        protected UserControl_Tab()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                Dock = DockStyle.Fill;
            }
        }

        public TabsCollection Tabs { protected get; set; }

        public TabStripItem TabItem { get; set; }

        // Not done in the constructor, because of some null references, neither won't use constructor params.
        public virtual void TabInitialize()
        {
            TabCreated();
            TabSelected();
        }

        public virtual void TabSelected()
        {
        }

        public virtual void TabDeselected()
        {
        }

        public virtual void TabClosing()
        {
        }

        public virtual void TabSave()
        {
        }

        public virtual void TabFind()
        {
        }

        protected virtual void TabCreated()
        {
            TabItem.Controls.Add(this);
        }
    }
}