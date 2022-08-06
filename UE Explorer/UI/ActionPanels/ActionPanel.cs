using System;
using System.Windows.Forms;
using UEExplorer.UI.Tabs;

namespace UEExplorer.UI.ActionPanels
{
    public class ActionPanel : Panel
    {
        public bool HasPendingUpdate;
        private object _Object;

        public object Object
        {
            get => _Object;
            set
            {
                _Object = value;
                if (HasPendingUpdate)
                {
                    return;
                }
                UpdateOutput(value);
            }
        }
        
        protected UC_PackageExplorer GetMain()
        {
            for (var c = Parent; c != null; c = c.Parent)
            {
                if ((c is UC_PackageExplorer packageExplorer)) return packageExplorer;
            }
            throw new NotSupportedException();
        }

        protected virtual void UpdateOutput(object target)
        {
            throw new NotImplementedException();
        }
    }
}
