using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace UEExplorer.Graphics.Framework;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector4 Color;
    public Vector2 TexCoord;

    public static unsafe VkVertexInputBindingDescription* GetBinding()
    {
        VkVertexInputBindingDescription binding = default;
        binding.binding = 0;
        binding.stride = (uint)sizeof(Vertex);
        binding.inputRate = VkVertexInputRate.Vertex;
        return &binding;
    }

    public static unsafe VkVertexInputAttributeDescription* GetAttributes()
    {
        VkVertexInputAttributeDescription* attributes =
            stackalloc VkVertexInputAttributeDescription[4];
        attributes[0].binding = 0;
        attributes[0].location = 0;
        attributes[0].format = VkFormat.R32G32B32SFloat;
        attributes[0].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Position));
        attributes[1].binding = 0;
        attributes[1].location = 1;
        attributes[1].format = VkFormat.R32G32B32SFloat;
        attributes[1].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Normal));
        attributes[2].binding = 0;
        attributes[2].location = 2;
        attributes[2].format = VkFormat.R32G32B32A32SFloat;
        attributes[2].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Color));
        attributes[3].binding = 0;
        attributes[3].location = 3;
        attributes[3].format = VkFormat.R32G32SFloat;
        attributes[3].offset = (uint)Marshal.OffsetOf<Vertex>(nameof(TexCoord));

        return attributes;
    }
}