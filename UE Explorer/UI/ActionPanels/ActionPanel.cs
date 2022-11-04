using System;
using System.Windows.Forms;

namespace UEExplorer.UI.ActionPanels
{
    public class ActionPanel : UserControl
    {
        private object _Object;
        public bool HasPendingUpdate;

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

        protected virtual async void UpdateOutput(object target)
        {
            throw new NotImplementedException();
        }
    }
}