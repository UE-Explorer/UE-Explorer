using System.Runtime.InteropServices;
using System.Windows.Forms;
using Storm.TabControl;

namespace UEExplorer.UI.Tabs
{
    [ComVisible( false )]
    public class UserControl_Tab : UserControl, ITabComponent
    {
        [ComVisible( false )]
        public TabsCollection Tabs{ protected get; set; }

        [ComVisible( false )]
        public TabStripItem TabItem{ get; set; }

        protected virtual void InitializeComponent()
        {
        }
        
        /// <summary>
        /// Called after the tab was constructed.
        /// </summary>
        // Not done in the constructor, because of some null references, neither won't use constructor params.
        public virtual void TabInitialize()
        {
            Dock = DockStyle.Fill;
            
            InitializeComponent();
            
            TabCreated();
        }

        /// <summary>
        /// Called when the Tab is added to the chain.
        /// </summary>
        protected virtual void TabCreated()
        {
        }

        /// <summary>
        /// Called when the Tab is closing.
        /// </summary>
        public virtual void TabClosing()
        {
        }

        public virtual void TabSave()
        {
        }

        public virtual void TabFind()
        {
        }
    }
}
