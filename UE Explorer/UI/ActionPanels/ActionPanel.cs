using System;
using System.Windows.Forms;

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

        protected virtual void UpdateOutput(object target)
        {
            throw new NotImplementedException();
        }
    }
}
