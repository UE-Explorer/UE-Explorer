using System;

namespace UEExplorer.Framework.UI
{
    public class ContextCommandEventArgs : EventArgs
    {
        public readonly object Subject;

        public ContextCommandEventArgs(object subject)
        {
            this.Subject = subject;
        }
    }
}
