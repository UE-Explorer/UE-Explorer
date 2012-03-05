using System.Collections.Generic;
using Storm.TabControl;

namespace UEExplorer.UI
{
	public interface ITabComponent
	{
		TabsManager Owner{get;set;}
		TabStripItem Tab{get;set;}

		void TabInitialize();
		void TabSelected();
		void TabClosing();
		void TabSave();
		void TabFind();
	}

	/// <summary>
	/// Represents a tabcomponent of a tabcontrol tab with functionality. 
	/// </summary>
	public class TabComponent : ITabComponent
	{
		/// <summary>
		/// The one who created me!
		/// </summary>
		public TabsManager _Owner = null;
		public TabsManager Owner
		{
			get{ return _Owner; }
			set{ _Owner = value; }
		}

		/// <summary>
		/// The tab this tabcomponent works for.
		/// </summary>
		public TabStripItem _Tab = null;
		public TabStripItem Tab
		{
			get { return _Tab; }
			set { _Tab = value; }
		}

		/// <summary>
		///	Creates a new instance of the TabComponent class.
		/// </summary>
		public TabComponent()
		{
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

	public class FileTabComponent : TabComponent
	{
		public readonly string FileName;

		public FileTabComponent()
		{
		}

		public FileTabComponent( string fileName )
		{
			FileName = fileName;
		}

		public override void TabInitialize()
		{
			//FileName = Tab.Title;
			base.TabInitialize();
		}
	}

	public class TabsManager
	{
		public readonly UEExplorer_Form Owner = null;
		public readonly TabStrip TabsControl = null;

		public readonly List<ITabComponent> Tabs = new List<ITabComponent>();

		public ITabComponent SelectedComponent
		{
			get
			{
				return Tabs != null && TabsControl != null 
					? Tabs.Find( (t) => t.Tab == TabsControl.SelectedItem ) as ITabComponent 
					: null;
			}
		}

		public TabsManager( UEExplorer_Form owner )
		{
			this.Owner = owner;
			this.TabsControl = owner.TabComponentsStrip;
		}

		public void AddTab( ITabComponent newComponent, TabStripItem newTab, bool bSkipAdd = false )
		{
			TabsControl.SelectedItem = newTab;

			// Add this page to the real tabs manager
			if( !bSkipAdd )
			{
				TabsControl.AddTab( newTab );
			}	

			newComponent.Tab = newTab;
			newComponent.Owner = this;
			Tabs.Add( newComponent );

			newComponent.TabInitialize();
		}

		public void RemoveTab( ITabComponent delComponent )
		{
			TabsControl.RemoveTab( delComponent.Tab );
			//Tabs.Remove( delComponent );	// Handled elsewhere
		}

		public TabStripItem CreateTabPage( string title )
		{
			TabStripItem newTab = new TabStripItem();
			newTab.IsDrawn = true;
			newTab.Selected = true;
			newTab.TabStripParent = TabsControl;
			newTab.TabIndex = 0;
			newTab.Title = title;
			newTab.BackColor = System.Drawing.Color.White;
			return newTab;
		}
	}
}
