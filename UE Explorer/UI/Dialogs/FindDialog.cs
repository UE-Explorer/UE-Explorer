using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
	using UEExplorer.UI.Tabs;

	public partial class FindDialog : Form
	{
		private TextEditorPanel _Editor;

		public FindDialog( TextEditorPanel editor )
		{
			_Editor = editor;

			InitializeComponent();
		}

		private void Find_Click( object sender, EventArgs e )
		{
			EditorUtil.FindText( _Editor, FindInput.Text );
		}
	}
}
