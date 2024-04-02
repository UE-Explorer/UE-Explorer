using System.Windows.Controls;
using ICSharpCode.AvalonEdit.Search;

namespace UEExplorer.UI.Tabs
{
	/// <summary>
	/// Interaction logic for MyTextEditor.xaml
	/// </summary>
	public partial class TextEditorPanel : UserControl
	{
		public TextEditorPanel()
		{
			InitializeComponent();

            var searchPanel = SearchPanel.Install(TextEditor);
            searchPanel.RegisterCommands(TextEditor.CommandBindings);
        }
    }
}
