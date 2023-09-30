using System;
using System.Runtime.InteropServices;
using UELib.Annotations;

namespace UEExplorer.Framework
{
    public class ContextChangedEventArgs : EventArgs
    {
        public readonly ContextInfo Context;

        public ContextChangedEventArgs(ContextInfo context)
        {
            Context = context;
        }
    }

    public delegate void ContextChangedEventHandler(object sender, ContextChangedEventArgs e);

    [ComVisible(true)]
    public sealed class ContextService
    {
        [CanBeNull] public ContextInfo ActiveContext { get; private set; }

        public event ContextChangedEventHandler ContextChanged;

        public void OnContextChanged(object sender, ContextChangedEventArgs e)
        {
            ActiveContext = e.Context;
            ContextChanged?.Invoke(sender, e);
        }

        public bool IsActiveTarget(object target) => ActiveContext?.Target == target;
    }
}
