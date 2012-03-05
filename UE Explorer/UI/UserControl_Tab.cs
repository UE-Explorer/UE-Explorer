using System.Windows.Forms;
using Storm.TabControl;

namespace UEExplorer.UI
{
	public partial class UserControl_Tab : UserControl, ITabComponent
	{
		public TabsManager Owner{ get; set; }
		public TabStripItem Tab{ get; set; }

		public UserControl_Tab()
		{
			Owner = null;
			Tab = null;
			Dock = DockStyle.Fill;
			InitializeComponent();
		}

		/// <summary>
		/// Called after the tab was constructed.
		/// </summary>
		// Not done in the constructor, because of some null references, neither won't use constructor params.
		public virtual void TabInitialize()
		{
			TabCreated();
			TabSelected();
		}

		/// <summary>
		/// Called when the Tab is added to the chain.
		/// </summary>
		protected virtual void TabCreated()
		{
			Tab.Controls.Add( this );
		}

		/// <summary>
		/// Called when the Tab is selected.
		/// </summary>
		public virtual void TabSelected()
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
