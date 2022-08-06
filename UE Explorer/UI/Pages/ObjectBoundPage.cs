using Krypton.Navigator;

namespace UEExplorer.UI.Pages
{
    public abstract class ObjectBoundPage : KryptonPage
    {
        public bool IsDefault;
        
        protected ObjectBoundPage() : base()
        {
            
        }
        
        public abstract void SetNewObjectTarget(object target, ContentNodeAction action, bool isPending);
    }
}
