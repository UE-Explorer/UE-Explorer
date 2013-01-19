using System;
using System.Collections.Generic;
using System.Linq;
using Storm.TabControl;

namespace UEExplorer.UI
{
    public interface ITabComponent
    {
        TabsCollection Tabs{ set; }
        TabStripItem TabItem{ get; set; }

        void TabInitialize();
        void TabSelected();
        void TabClosing();
        void TabSave();
        void TabFind();
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
        }

        public ITabComponent Add( Type tabType, string tabName )
        {
            if( Components.Any( tc => tc.TabItem.Title == tabName ) )
            {
                return null;
            }

            var tabItem = new TabStripItem
            {
                Selected = true,
                TabStripParent = _TabsControl,
                TabIndex = 0,
                Title = tabName,
                BackColor = System.Drawing.Color.White
            };

            _TabsControl.AddTab( tabItem );
            _TabsControl.SelectedItem = tabItem;
            _TabsControl.Visible = _TabsControl.Items.Count > 0;
            _TabsControl.Refresh();

            var tabComp = Activator.CreateInstance( tabType ) as ITabComponent;
            tabComp.TabItem = tabItem;
            tabComp.Tabs = this;
            tabComp.TabInitialize();
            Components.Add( tabComp );
            return tabComp;
        }

        public void Remove( ITabComponent delComponent )
        {
            delComponent.TabClosing();
            _TabsControl.RemoveTab( delComponent.TabItem );
            Components.Remove( delComponent );	// Handled elsewhere
        }

        public void Dispose()
        {
            Form = null;
            _TabsControl = null;
            Components = null;
        }
    }
}
