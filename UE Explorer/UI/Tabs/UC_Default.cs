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
			// ...

			DefaultPage.Navigate( Program.APPS_URL );
			base.TabCreated();

			Dock = DockStyle.Fill;
		}
	}
}
