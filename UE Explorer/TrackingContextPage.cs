using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.UI;
using UEExplorer.Framework.UI.Pages;

namespace UEExplorer
{
    /// <summary>
    /// A KryptonPage capable of tracking the current active context.
    /// </summary>
    public class TrackingContextPage : Page, ITrackingContext, IContextListener
    {
        protected string TextPrefix;
        
        public bool IsTracking { get; set; }

        public virtual bool CanAccept(ContextInfo context)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<bool> Accept(ContextInfo context)
        {
            throw new System.NotImplementedException();
        }

        protected void UpdateContextText(ContextInfo context)
        {
            if (context.ResolvedTarget == null)
            {
                TextTitle = TextPrefix;
            }
            else
            {
                string path = ObjectPathBuilder.GetPath((dynamic)context.ResolvedTarget);
                TextTitle = $@"{TextPrefix}: {path}";
                Text = TextTitle;
            }
        }
    }
}
