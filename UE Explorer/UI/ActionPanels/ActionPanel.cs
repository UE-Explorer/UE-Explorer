using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;

namespace UEExplorer.UI.ActionPanels
{
    public class ActionPanel : UserControl, IContextListener
    {
        private object _Object;

        public object Object
        {
            set
            {
                _Object = value;

                UpdateOutput(value);
            }
        }

        protected virtual void UpdateOutput(object target)
        {
            throw new NotImplementedException();
        }

        public bool CanAccept(ContextInfo context) => throw new NotImplementedException();

        public Task<bool> Accept(ContextInfo context) => throw new NotImplementedException();
    }
}
