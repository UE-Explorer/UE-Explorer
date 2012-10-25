using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UEExplorer;
using UEExplorer.UI;
using UEExplorer.UI.Dialogs;

namespace UEExplorer.UI.Tabs
{
	public class CacheExtractorTabComponent : TabComponent
	{
		private UC_CacheExtractor _ExTab;

		public CacheExtractorTabComponent() {}

		public override void TabInitialize()
		{
			base.TabInitialize();
		}

		protected override void TabCreated()
		{
			base.TabCreated();

			_ExTab = new UC_CacheExtractor();
			_ExTab.Dock = System.Windows.Forms.DockStyle.Fill;
			Tab.Controls.Add( _ExTab );
		}

		public override void TabSelected()
		{
			base.TabSelected();
		}

		public override void TabClosing()
		{
			base.TabClosing();
		}
	}
}
