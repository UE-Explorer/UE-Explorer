using Krypton.Navigator;

namespace UEExplorer.UI.Pages
{
    public abstract class ObjectBoundPage : KryptonPage
    {
        protected ObjectBoundPage() : base()
        {
            
        }
        
        public abstract void SetNewObjectTarget(object target, ContentNodeAction action);
    }
}
