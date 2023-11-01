using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace UEExplorer.Graphics.Framework;

public readonly struct Vertex(Vector3 position, Vector3 normal, Vector4 color, Vector2 texCoord)
{
    public readonly Vector3 Position = position;
    public readonly Vector3 Normal = normal;
    public readonly Vector4 Color = color;
    public readonly Vector2 TexCoord = texCoord;

    public static unsafe VkVertexInputBindingDescription* GetBinding()
    {
        VkVertexInputBindingDescription binding = new()
        {
            binding = 0, stride = (uint)sizeof(Vertex), inputRate = VkVertexInputRate.Vertex
        };
        return &binding;
    }

    public static unsafe VkVertexInputAttributeDescription* GetAttributes()
    {
        var attributes =
            stackalloc VkVertexInputAttributeDescription[4];
        attributes[0].binding = 0;
        attributes[0].location = 0;
        attributes[0].format = VkFormat.R32G32B32Sfloat;
        attributes[0].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Position));
        attributes[1].binding = 0;
        attributes[1].location = 1;
        attributes[1].format = VkFormat.R32G32B32Sfloat;
        attributes[1].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Normal));
        attributes[2].binding = 0;
        attributes[2].location = 2;
        attributes[2].format = VkFormat.R32G32B32A32Sfloat;
        attributes[2].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Color));
        attributes[3].binding = 0;
        attributes[3].location = 3;
        attributes[3].format = VkFormat.R32G32Sfloat;
        attributes[3].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(TexCoord));

        return (VkVertexInputAttributeDescription*)(&attributes);
    }
}
