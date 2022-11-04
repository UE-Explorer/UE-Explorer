using Vortice.Vulkan;

namespace UEExplorer.Graphics.Framework;

public struct Mesh
{
    public Vertex[] Vertices;
    internal unsafe VkBuffer* VertexBuffer;
}