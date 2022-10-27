using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Storm.TabControl;

namespace UEExplorer.UI.Tabs
{
    // TODO: Deprecate
    public interface ITabComponent
    {
        void TabSave();
        void TabFind();
    }

    // TODO: Deprecate
    public class TabsCollection : IDisposable
    {
        private TabStrip _TabStrip;

        public TabsCollection(TabStrip tabStrip)
        {
            _TabStrip = tabStrip;
        }

        public ITabComponent SelectedComponent => (ITabComponent)_TabStrip.SelectedItem?.Controls[0];

        public void Dispose()
        {
            _TabStrip = null;
        }

        public void InsertTab(Type componentType, string caption)
        {
            Debug.Assert(componentType.IsSubclassOf(typeof(Control)));

            var tabComp = (Control)Activator.CreateInstance(componentType);
            AddTab(tabComp, caption);
        }

        public void AddTab(Control component, string caption)
        {
            _TabStrip.Visible = true;

            var tabItem = new TabStripItem(caption, null)
            {
                BackColor = Color.White
            };

            _TabStrip.AddTab(tabItem, true);

            // We have to add this last for properly layout rendering.
            tabItem.Controls.Add(component);
        }

        // FIXME: Closes more than tab :S
        public void CloseTab(TabStripItem itemToRemove)
        {
            _TabStrip.RemoveTab(itemToRemove);
        }
    }
}