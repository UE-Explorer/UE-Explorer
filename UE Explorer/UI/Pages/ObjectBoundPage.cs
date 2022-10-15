using Krypton.Navigator;

namespace UEExplorer.UI.Pages
{
    public abstract class ObjectBoundPage : KryptonPage
    {
        public bool IsDefault;

        public abstract void OnObjectTarget(object target, ContentNodeAction action, bool isPending);
        public abstract void OnFind(TextSearchHelpers.FindResult findResult);
        public abstract void OnFind(string text);
    }
}