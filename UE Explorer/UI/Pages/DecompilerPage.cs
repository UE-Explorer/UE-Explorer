using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;
using UEExplorer.UI.Tabs;

namespace UEExplorer.UI.Pages
{
    internal sealed class DecompilerPage : ObjectBoundPage
    {
        private readonly DecompileOutputPanel _Panel;

        public DecompilerPage()
        {
            Text = Resources.DecompilerPage_DecompilerPage_Decompile_Title;
            TextTitle = Resources.DecompilerPage_DecompilerPage_Decompile_Title;

            _Panel = new DecompileOutputPanel();
            _Panel.Name = "Panel";
            _Panel.Dock = DockStyle.Fill;
            Controls.Add(_Panel);
        }
        
        public override void OnObjectTarget(object target, ContentNodeAction action, bool isPending)
        {
            if (target == null)
            {
                TextTitle = Resources.DecompilerPage_DecompilerPage_Decompile_Title;
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)target);
                TextTitle = string.Format(Resources.DecompilerPage_SetNewObjectTarget_Decompile___0_, path);
                Text = TextTitle;
            }

            _Panel.HasPendingUpdate = isPending;
            _Panel.Object = target;
        }

        public override void OnFind(TextSearchHelpers.FindResult findResult)
        {
            _Panel.TextEditorPanel.TextEditorControl.TextEditor.ScrollTo(findResult.TextLine, findResult.TextColumn);
            _Panel.TextEditorPanel.TextEditorControl.TextEditor.Select(findResult.TextIndex, findResult.TextLength);
        }

        public override void OnFind(string text)
        {
            EditorUtil.FindText(_Panel.TextEditorPanel.TextEditorControl.TextEditor, text);
        }
    }
}