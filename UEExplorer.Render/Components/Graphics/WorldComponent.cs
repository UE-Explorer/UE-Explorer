using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using UEExplorer.Graphics.Framework;
using UELib.Engine;

namespace UEExplorer.Render.Components.Graphics
{
    public class WorldComponent : IComponent
    {
        private Scene _Scene;

        public bool IsInitialized => _Scene != null;

        public void Initialize(IntPtr windowPtr, int width, int height)
        {
            Contract.Assert(_Scene == null);
            _Scene = new Scene(windowPtr, width, height);
        }

        // FIXME: SLOW
        public void Resize(IntPtr windowPtr, int width, int height)
        {
            if (_Scene != null)
            {
                _Scene.Dispose();
                _Scene = null;
            }

            // Re-setup everything
            Initialize(windowPtr, width, height);
        }

        public void Update()
        {
            _Scene.RenderFrame();
        }

        // TODO: Implement UObject to Scene adapters
        public void AddToScene(UPolys mesh)
        {
            Debug.Assert(_Scene != null);

            var renderObject = new RenderObject();
            Debug.Assert(mesh.Element.Any(), "No render data!");
            mesh.Element
                .ForEach(e =>
                    {
                        var vertices = e.Vertex
                            .Select(v => new Vertex(
                                position: (Vector3)v,
                                normal: (Vector3)e.Normal,
                                color: new Vector4(e.Normal.X, e.Normal.Y, e.Normal.Z, 1.0f),
                                texCoord: new Vector2(e.PanU, e.PanV)))
                            .ToArray();

                        renderObject.Mesh = new Mesh { Vertices = vertices };

                        _Scene.Add(renderObject);
                    }
                );
        }

        public void ResetScene()
        {
            _Scene.Reset();
        }

        public void Dispose()
        {
            _Scene?.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;
    }
}
