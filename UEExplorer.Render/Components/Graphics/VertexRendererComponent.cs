using System;
using System.ComponentModel;

namespace UEExplorer.Render.Components.Graphics
{
    internal class VertexRendererComponent : IComponent
    {
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;
    }
}