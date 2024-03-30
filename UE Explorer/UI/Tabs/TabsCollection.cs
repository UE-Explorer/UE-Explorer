using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Storm.TabControl;

namespace UEExplorer.UI
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

        public TabStripItem GetTab(string uniqueName)
        {
            return _TabStrip.Controls[uniqueName] as TabStripItem;
        }
        
        public bool HasTab(string uniqueName)
        {
            return _TabStrip.Controls[uniqueName] != null;
        }

        public void InsertTab(Type componentType, string caption)
        {
            Debug.Assert(componentType.IsSubclassOf(typeof(Control)));

            string uniqueName = componentType.Name;
            if (HasTab(uniqueName))
            {
                return;
            }

            var tabComp = (Control)Activator.CreateInstance(componentType);
            AddTab(tabComp, caption, uniqueName);
        }

        public void AddTab(Control component, string caption, string uniqueName = "")
        {
            _TabStrip.Visible = true;

            var tabItem = new TabStripItem(caption, null)
            {
                Name = uniqueName == "" 
                    ? component.GetType().Name 
                    : uniqueName,
            };
            _TabStrip.AddTab(tabItem, true);

            // We have to add this last for proper layout rendering.
            tabItem.Controls.Add(component);
        }

        public void CloseTab(TabStripItem itemToRemove)
        {
            _TabStrip.RemoveTab(itemToRemove);
        }
    }
}
