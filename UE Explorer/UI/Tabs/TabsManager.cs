using System;
using System.Collections.Generic;
using Storm.TabControl;
using System.Reflection;

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
		public TabsManager Owner{ get; set; }
		public TabStripItem Tab{ get; set; }

		/// <summary>
		///	Creates a new instance of the TabComponent class.
		/// </summary>
		public TabComponent()
		{
			Owner = null;
			Tab = null;
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
		public readonly ProgramForm Owner;
		private readonly TabStrip _TabsControl;

		public readonly List<ITabComponent> Tabs = new List<ITabComponent>();

		public ITabComponent SelectedComponent
		{
			get
			{
				return Tabs != null && _TabsControl != null 
					? Tabs.Find( (t) => t.Tab == _TabsControl.SelectedItem ) as ITabComponent 
					: null;
			}
		}

		public TabsManager( ProgramForm owner, TabStrip tabsControl )
		{
			this.Owner = owner;
			this._TabsControl = tabsControl;
		}

		public void AddTab( ITabComponent newComponent, TabStripItem newTab, bool bSkipAdd = false )
		{
			_TabsControl.SelectedItem = newTab;

			// Add this page to the real tabs manager
			if( !bSkipAdd )
			{
				_TabsControl.AddTab( newTab );
			}	

			newComponent.Tab = newTab;
			newComponent.Owner = this;
			Tabs.Add( newComponent );

			newComponent.TabInitialize();
		}

		public void RemoveTab( ITabComponent delComponent )
		{
			_TabsControl.RemoveTab( delComponent.Tab );
			Tabs.Remove( delComponent );	// Handled elsewhere
		}

		public TabStripItem CreateTabPage( string title )
		{
			var newTab = new TabStripItem();
			newTab.IsDrawn = true;
			newTab.Selected = true;
			newTab.TabStripParent = _TabsControl;
			newTab.TabIndex = 0;
			newTab.Title = title;
			newTab.BackColor = System.Drawing.Color.White;
			return newTab;
		}

		public ITabComponent AddTabComponent( Type tabType, string tabName )
		{
			// Avoid duping tabs.
			foreach( ITabComponent TC in Tabs )
			{
				if( TC.Tab.Title == tabName )
				{
					return null;
				}
			}

			var newtab = Activator.CreateInstance( tabType ) as ITabComponent;
			var item = CreateTabPage( tabName );
			AddTab( newtab, item );

			_TabsControl.Visible = _TabsControl.Items.Count > 0;
			item.Refresh();
			return newtab;
		}

		public bool IsLoaded( string fileName )
		{
			return Tabs.Exists(
				delegate( ITabComponent tabComponent ){ 
					return (IHasFileName)tabComponent == null 
						|| ((IHasFileName)tabComponent).FileName == fileName; 
				} 
			);
		}
	}

	public interface IHasFileName
	{
		string FileName{ get; }
	}
}
