using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Storm.TabControl;

namespace UEExplorer.UI
{
    public interface ITabComponent : IDisposable
    {
        TabsCollection Tabs{ set; }
        TabStripItem TabItem{ get; set; }

        void TabInitialize();
        void TabClosing();
        void TabSave();
        void TabFind();
    }

    internal class TabItem : TabStripItem
    {
        public TabItem(string title) : base(title, null)
        {
            
        }
    }

    public class TabsCollection : IDisposable
    {
        public ProgramForm                      Form;
        public List<ITabComponent>              Components = new List<ITabComponent>();
        public ITabComponent                    SelectedComponent
        {
            get{ return Components.Find( tabComp => tabComp.TabItem == _TabsControl.SelectedItem ); }
        }
        private TabStrip                        _TabsControl;

        public TabsCollection( ProgramForm owner, TabStrip tabsControl )
        {
            Form = owner;
            _TabsControl = tabsControl;
            
            tabsControl.SelectedItemChanged += TabsControlOnSelectedItemChanged;
        }

        private void TabsControlOnSelectedItemChanged(TabStripItemMouseEventArgs e)
        {
            foreach (TabStripItem item in _TabsControl.Items)
            {
                item.Invalidate();
            }

            e.Item.Select();
        }

        public ITabComponent Add( Type tabType, string tabName )
        {
            if( Components.Any( tc => tc.TabItem.Title == tabName ) )
            {
                return null;
            }

            var tabItem = new TabItem(tabName)
            {
                Parent = _TabsControl,
                TabStripParent = _TabsControl,
                Title = tabName,
            };

            _TabsControl.AddTab( tabItem );
            _TabsControl.Visible = _TabsControl.Items.Count > 0;
            _TabsControl.SelectedItem = tabItem;
            _TabsControl.Refresh();

            var tabComp = (ITabComponent)Activator.CreateInstance( tabType );
            tabComp.TabItem = tabItem;
            tabComp.Tabs = this;
            tabComp.TabInitialize();
            tabItem.Controls.Add((Control)tabComp);
            Components.Add( tabComp );
            return tabComp;
        }

        public void Remove( ITabComponent delComponent, bool fullRemove = false )
        {
            if( fullRemove )
            {
                _TabsControl.RemoveTab( delComponent.TabItem );
                 delComponent.TabItem.Dispose();
            }
            Components.Remove( delComponent );
            delComponent.Dispose();
        }

        public void Dispose()
        {
            Form = null;
            _TabsControl.SelectedItemChanged -= TabsControlOnSelectedItemChanged;
            _TabsControl = null;
            Components = null;
        }
    }
}
