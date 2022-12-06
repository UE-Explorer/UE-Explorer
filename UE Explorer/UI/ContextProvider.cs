using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UELib.Annotations;

namespace UEExplorer.UI
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
    public sealed class ContextProvider : IComponent
    {
        [CanBeNull] public ContextInfo ActiveContext { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ISite Site { get; set; }

        /// <inheritdoc />
        public event EventHandler Disposed;

        public event ContextChangedEventHandler ContextChanged;

        public void OnContextChanged(object sender, ContextChangedEventArgs e)
        {
            ActiveContext = e.Context;
            ContextChanged?.Invoke(sender, e);
        }

        public bool IsActiveTarget(object target) => ActiveContext?.Target == target;
    }
}
