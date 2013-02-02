using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
	public partial class UC_Default : UserControl_Tab
	{
		/// <summary>
		/// Called when the Tab is added to the chain.
		/// </summary>
		protected override void TabCreated()
		{
			base.TabCreated();
			DefaultPage.BeginInvoke( (Action)(() => DefaultPage.Navigate( Program.APPS_URL + "?version=" + Application.ProductVersion )) );
		}
	}
}
