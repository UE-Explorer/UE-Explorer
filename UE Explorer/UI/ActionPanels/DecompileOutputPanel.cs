using System;
using UEExplorer.UI.Tabs;
using UELib;

namespace UEExplorer.UI.ActionPanels
{
    public partial class DecompileOutputPanel : ActionPanel, IActionPanel<object>
    {
        public ContentNodeAction Action { get; } = ContentNodeAction.Decompile;

        public DecompileOutputPanel()
        {
            InitializeComponent();

            TextEditorPanel.TextEditorControl.SearchDocument.Click += (sender, args) =>
            {
                string selectedText = TextEditorPanel.TextEditorControl.TextEditor.TextArea.Selection.GetText();
                EditorUtil.FindText(TextEditorPanel.TextEditorControl.TextEditor, selectedText);
            };
            
            TextEditorPanel.TextEditorControl.SearchPackage.Click += (sender, args) =>
            {
                string selectedText = TextEditorPanel.TextEditorControl.TextEditor.TextArea.Selection.GetText();
                GetMain().OnSearchInClasses(selectedText);
            };

            TextEditorPanel.TextEditorControl.SearchObject.Click += (sender, args) =>
            {
                string selectedText = TextEditorPanel.TextEditorControl.TextEditor.TextArea.Selection.GetText();
                GetMain().OnSearchObjectByPath(selectedText.Trim());
            };
        }
        
        protected override void UpdateOutput(object target)
        {
            if (target == null)
            {
                TextEditorPanel.SetText("");
                return;
            }

            switch (target)
            {
                case IUnrealDecompilable decompilable:
                    string content = decompilable.Decompile();
                    TextEditorPanel.SetText(content);
                    break;

                case string s:
                {
                    TextEditorPanel.SetText(s);
                    break;
                }
                
                default:
                    throw new NotSupportedException($"{target} is not a supported type");
            }
        }

        public void RestoreState(ref ActionState state)
        {
            TextEditorPanel.TextEditorControl.TextEditor.ScrollToVerticalOffset(state.Y);
            TextEditorPanel.TextEditorControl.TextEditor.ScrollToHorizontalOffset(state.X);
            TextEditorPanel.TextEditorControl.TextEditor.Select(state.SelectStart, state.SelectLength);
        }   

        public void StoreState(ref ActionState state)
        {
            state.X = TextEditorPanel.TextEditorControl.TextEditor.HorizontalOffset;
            state.Y = TextEditorPanel.TextEditorControl.TextEditor.VerticalOffset;
            state.SelectStart = TextEditorPanel.TextEditorControl.TextEditor.SelectionStart;
            state.SelectLength = TextEditorPanel.TextEditorControl.TextEditor.SelectionLength;
        }
    }
}
