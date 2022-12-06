using Krypton.Navigator;

namespace UEExplorer.UI.Pages
{
    public abstract class TrackingPage : KryptonPage
    {
        public bool IsTracking;

        public abstract void OnFind(TextSearchHelpers.FindResult findResult);
        public abstract void OnFind(string text);
        public abstract bool CanAccept(ContextInfo context);
        public abstract bool Accept(ContextInfo context);
    }
}