using System.Threading.Tasks;
using UEExplorer.UI.Panels;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.ActionPanels
{
    public partial class DecompileOutputPanel : ActionPanel, IActionPanel<object>
    {
        public TextEditorControl EditorControl { get; }

        public DecompileOutputPanel()
        {
            InitializeComponent();

            EditorControl = TextEditorPanel.TextEditorControl;

            EditorControl.SearchInDocument.Click += (sender, args) =>
            {
                string selectedText = EditorControl.TextEditor.TextArea.Selection.GetText();
                EditorUtil.FindText(EditorControl.TextEditor, selectedText);
            };

            EditorControl.SearchInClasses.Click += (sender, args) =>
            {
                string selectedText = EditorControl.TextEditor.TextArea.Selection.GetText();
                UC_PackageExplorer.Traverse(Parent).EmitSearch<UClass>(selectedText);
            };

            EditorControl.SearchObject.Click += (sender, args) =>
            {
                string selectedText = EditorControl.TextEditor.TextArea.Selection.GetText();
                UC_PackageExplorer.Traverse(Parent).EmitSearchObjectByPath(selectedText.Trim());
            };
        }

        public void RestoreState(ref ActionState state)
        {
            EditorControl.TextEditor.ScrollToVerticalOffset(state.Y);
            EditorControl.TextEditor.ScrollToHorizontalOffset(state.X);
            EditorControl.TextEditor.Select(state.SelectStart, state.SelectLength);
        }

        public void StoreState(ref ActionState state)
        {
            state.X = EditorControl.TextEditor.HorizontalOffset;
            state.Y = EditorControl.TextEditor.VerticalOffset;
            state.SelectStart = EditorControl.TextEditor.SelectionStart;
            state.SelectLength = EditorControl.TextEditor.SelectionLength;
        }

        protected override async void UpdateOutput(object target)
        {
            if (target == null)
            {
                TextEditorPanel.SetText("");
                return;
            }

            switch (target)
            {
                case IUnrealDecompilable decompilable:
                {
                    string content = await Task.Run(() => decompilable.Decompile());
                    TextEditorPanel.SetText(content);
                    break;
                }

                case string s:
                {
                    TextEditorPanel.SetText(s);
                    break;
                }

                default:
                    TextEditorPanel.SetText("");
                    break;
            }
        }
    }
}