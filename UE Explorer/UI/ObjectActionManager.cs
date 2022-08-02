using System;

namespace UEExplorer.UI
{
    public class ObjectTargetEventArgs : EventArgs
    {
        public object Target { get; }
        public ContentNodeAction Action { get; }

        public ObjectTargetEventArgs(object target, ContentNodeAction action)
        {
            Target = target;
            Action = action;
        }
    }

    public delegate void ObjectTargetChangedEventHandler(object sender, ObjectTargetEventArgs e);
}