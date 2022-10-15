using System;

namespace UEExplorer.UI
{
    public class ObjectTargetEventArgs : EventArgs
    {
        public ObjectTargetEventArgs(object target, ContentNodeAction action)
        {
            Target = target;
            Action = action;
        }

        public object Target { get; }
        public ContentNodeAction Action { get; }
    }
}