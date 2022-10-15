using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Storm.TabControl;
using UEExplorer.UI.Main;

namespace UEExplorer.UI.Tabs
{
    public interface ITabComponent
    {
        TabsCollection Tabs { set; }
        TabStripItem TabItem { get; set; }

        void TabInitialize();
        void TabSelected();
        void TabClosing();
        void TabSave();
        void TabFind();
        void TabDeselected();
    }

    public class TabsCollection : IDisposable
    {
        private TabStrip _TabsControl;
        public List<ITabComponent> Components = new List<ITabComponent>();
        public ProgramForm Form;

        public ITabComponent LastSelectedComponent;

        public TabsCollection(ProgramForm owner, TabStrip tabsControl)
        {
            Form = owner;
            _TabsControl = tabsControl;
        }

        public ITabComponent SelectedComponent =>
            Components.Find(tabComp => tabComp.TabItem == _TabsControl.SelectedItem);

        public void Dispose()
        {
            Form = null;
            _TabsControl = null;
            Components = null;
        }

        public ITabComponent Add(Type tabType, string tabName)
        {
            if (Components.Any(tc => tc.TabItem.Title == tabName))
            {
                return null;
            }

            var tabItem = new TabStripItem
            {
                Selected = true,
                TabStripParent = _TabsControl,
                TabIndex = 0,
                Title = tabName,
                BackColor = Color.White
            };

            _TabsControl.AddTab(tabItem);
            _TabsControl.SelectedItem = tabItem;
            _TabsControl.Visible = _TabsControl.Items.Count > 0;
            _TabsControl.Refresh();

            var tabComp = (ITabComponent)Activator.CreateInstance(tabType);
            tabComp.TabItem = tabItem;
            tabComp.Tabs = this;
            tabComp.TabInitialize();
            Components.Add(tabComp);
            return tabComp;
        }

        public void Remove(ITabComponent delComponent, bool fullRemove = false)
        {
            if (fullRemove)
            {
                _TabsControl.RemoveTab(delComponent.TabItem);
                delComponent.TabItem.Dispose();
            }

            Components.Remove(delComponent);
        }
    }
}