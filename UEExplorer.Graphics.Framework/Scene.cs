using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using UEExplorer.Graphics.Framework.Vortice.Vulkan;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace UEExplorer.Graphics.Framework
{
    public unsafe class Scene : IDisposable
    {
        private readonly GraphicsDevice _GraphicsDevice;

        private readonly List<RenderObject> _RenderObjects = new();

        private readonly List<BufferHandle> _VertexBuffers = new();
        private VkPipeline _Pipeline;
        private VkPipelineLayout _PipelineLayout, _MeshPipelineLayout;

        public Scene(GraphicsDevice graphicsDevice)
        {
            _GraphicsDevice = graphicsDevice;
            Initialize();
        }

        public Scene(IntPtr windowPtr, int width, int height)
        {
#if DEBUG
            const bool enableValidationLayers = true;
#else
		    const bool enableValidationLayers = false;
#endif
            _GraphicsDevice = new GraphicsDevice("UEExplorer", enableValidationLayers, windowPtr, width, height);
            Initialize();
        }

        public void Dispose()
        {
            _GraphicsDevice.WaitIdle();

            vkDestroyPipelineLayout(_GraphicsDevice, _MeshPipelineLayout);
            vkDestroyPipelineLayout(_GraphicsDevice, _PipelineLayout);
            vkDestroyPipeline(_GraphicsDevice, _Pipeline);

            Reset();

            _GraphicsDevice.Dispose();
        }

        private void CreateShaderModule(string name, out VkShaderModule shaderModule)
        {
            byte[] vertexBytecode = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Shaders", $"{name}.spv"));
            _GraphicsDevice.CreateShaderModule(vertexBytecode, out shaderModule).CheckResult();
        }

        private void Initialize()
        {
            var pushConstants = stackalloc VkPushConstantRange[1];
            pushConstants[0].stageFlags = VkShaderStageFlags.Vertex;
            pushConstants[0].offset = 0;
            pushConstants[0].size = (uint)sizeof(MeshPushConstants);

            VkPipelineLayoutCreateInfo meshPipelineLayoutCreateInfo = new()
            {
                pushConstantRangeCount = 1, pPushConstantRanges = pushConstants
            };

            vkCreatePipelineLayout(_GraphicsDevice, &meshPipelineLayoutCreateInfo, null, out _MeshPipelineLayout)
                .CheckResult();

            VkPipelineLayoutCreateInfo pipelineLayoutCreateInfo = new() { pNext = &meshPipelineLayoutCreateInfo };

            vkCreatePipelineLayout(_GraphicsDevice, &pipelineLayoutCreateInfo, null, out _PipelineLayout)
                .CheckResult();

            VkString entryPoint = new("main");

            CreateShaderModule("triangle.vert", out var vertexShader);
            CreateShaderModule("triangle.frag", out var fragmentShader);

            var shaderStages = stackalloc VkPipelineShaderStageCreateInfo[2];
            shaderStages[0] = new VkPipelineShaderStageCreateInfo
            {
                stage = VkShaderStageFlags.Vertex, module = vertexShader, pName = entryPoint
            };

            shaderStages[1] = new VkPipelineShaderStageCreateInfo
            {
                stage = VkShaderStageFlags.Fragment, module = fragmentShader, pName = entryPoint
            };

            var vertexInputBinding = Vertex.GetBinding();
            var vertexInputAttrs = Vertex.GetAttributes();
            VkPipelineVertexInputStateCreateInfo vertexInputState = new()
            {
                vertexBindingDescriptionCount = 1,
                pVertexBindingDescriptions = vertexInputBinding,
                vertexAttributeDescriptionCount = 4,
                pVertexAttributeDescriptions = vertexInputAttrs
            };

            VkPipelineInputAssemblyStateCreateInfo
                inputAssemblyState = new() { topology = VkPrimitiveTopology.TriangleList };

            VkPipelineViewportStateCreateInfo viewportState = new() { viewportCount = 1, scissorCount = 1 };

            VkPipelineRasterizationStateCreateInfo rasterizationState = new()
            {
                polygonMode = VkPolygonMode.Fill,
                cullMode = VkCullModeFlags.None,
                frontFace = VkFrontFace.CounterClockwise,
                depthClampEnable = false,
                rasterizerDiscardEnable = false,
                depthBiasEnable = false,
                lineWidth = 1.0f
            };

            VkPipelineMultisampleStateCreateInfo multisampleState = new()
            {
                rasterizationSamples = VkSampleCountFlags.Count1, pSampleMask = null
            };

            VkPipelineDepthStencilStateCreateInfo depthStencilState = new()
            {
                depthTestEnable = true,
                depthWriteEnable = true,
                depthCompareOp = VkCompareOp.LessOrEqual,
                depthBoundsTestEnable = false
            };
            depthStencilState.back.failOp = VkStencilOp.Keep;
            depthStencilState.back.passOp = VkStencilOp.Keep;
            depthStencilState.back.compareOp = VkCompareOp.Always;
            depthStencilState.stencilTestEnable = false;
            depthStencilState.front = depthStencilState.back;

            VkPipelineColorBlendAttachmentState blendAttachmentState = default;
            blendAttachmentState.colorWriteMask = VkColorComponentFlags.All;
            blendAttachmentState.blendEnable = false;

            VkPipelineColorBlendStateCreateInfo colorBlendState = new()
            {
                attachmentCount = 1, pAttachments = &blendAttachmentState
            };

            var dynamicStateEnables = stackalloc VkDynamicState[2];
            dynamicStateEnables[0] = VkDynamicState.Viewport;
            dynamicStateEnables[1] = VkDynamicState.Scissor;

            VkPipelineDynamicStateCreateInfo dynamicState = new()
            {
                dynamicStateCount = 2, pDynamicStates = dynamicStateEnables
            };

            VkGraphicsPipelineCreateInfo pipelineCreateInfo = new()
            {
                stageCount = 2,
                pStages = shaderStages,
                pVertexInputState = &vertexInputState,
                pInputAssemblyState = &inputAssemblyState,
                pTessellationState = null,
                pViewportState = &viewportState,
                pRasterizationState = &rasterizationState,
                pMultisampleState = &multisampleState,
                pDepthStencilState = &depthStencilState,
                pColorBlendState = &colorBlendState,
                pDynamicState = &dynamicState,
                layout = _PipelineLayout,
                renderPass = _GraphicsDevice.SwapChain.RenderPass
            };

            vkCreateGraphicsPipeline(_GraphicsDevice, pipelineCreateInfo, out _Pipeline).CheckResult();

            vkDestroyShaderModule(_GraphicsDevice, vertexShader);
            vkDestroyShaderModule(_GraphicsDevice, fragmentShader);
        }

        public VkBuffer* CreateVerticesBuffer(ReadOnlySpan<Vertex> vertices)
        {
            BufferHandle bufferHandle = new();

            VkBufferCreateInfo bufferInfo = new()
            {
                size = (ulong)(vertices.Length * sizeof(Vertex)), usage = VkBufferUsageFlags.VertexBuffer
            };

            // Create a host-visible buffer to copy the vertex data to (staging buffer)
            vkCreateBuffer(_GraphicsDevice, &bufferInfo, null, out var stagingBuffer).CheckResult();

            vkGetBufferMemoryRequirements(_GraphicsDevice, stagingBuffer, out var memReqs);

            VkMemoryAllocateInfo memAlloc = new()
            {
                allocationSize = memReqs.size,
                // Request a host visible memory type that can be used to copy our data do
                // Also request it to be coherent, so that writes are visible to the GPU right after unmapping the buffer
                memoryTypeIndex = _GraphicsDevice.GetMemoryTypeIndex(memReqs.memoryTypeBits,
                    VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent)
            };
            vkAllocateMemory(_GraphicsDevice, &memAlloc, null, out var stagingBufferMemory);

            // Map and copy
            void* pMappedData;
            vkMapMemory(_GraphicsDevice, stagingBufferMemory, 0, memAlloc.allocationSize, 0, &pMappedData)
                .CheckResult();
            Span<Vertex> destinationData = new(pMappedData, vertices.Length);
            vertices.CopyTo(destinationData);
            vkUnmapMemory(_GraphicsDevice, stagingBufferMemory);
            vkBindBufferMemory(_GraphicsDevice, stagingBuffer, stagingBufferMemory, 0).CheckResult();

            bufferInfo.usage = VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst;
            vkCreateBuffer(_GraphicsDevice, &bufferInfo, null, out bufferHandle.VertexBuffer).CheckResult();

            vkGetBufferMemoryRequirements(_GraphicsDevice, bufferHandle.VertexBuffer, out memReqs);
            memAlloc.allocationSize = memReqs.size;
            memAlloc.memoryTypeIndex =
                _GraphicsDevice.GetMemoryTypeIndex(memReqs.memoryTypeBits, VkMemoryPropertyFlags.DeviceLocal);
            vkAllocateMemory(_GraphicsDevice, &memAlloc, null, out bufferHandle.DeviceMemory).CheckResult();
            vkBindBufferMemory(_GraphicsDevice, bufferHandle.VertexBuffer, bufferHandle.DeviceMemory, 0).CheckResult();

            var copyCmd = _GraphicsDevice.GetCommandBuffer();

            // Put buffer region copies into command buffer
            VkBufferCopy copyRegion = default;

            // Vertex buffer
            copyRegion.size = memReqs.size;
            vkCmdCopyBuffer(copyCmd, stagingBuffer, bufferHandle.VertexBuffer, 1, &copyRegion);

            // Flushing the command buffer will also submit it to the queue and uses a fence to ensure that all commands have been executed before returning
            _GraphicsDevice.FlushCommandBuffer(copyCmd);

            vkDestroyBuffer(_GraphicsDevice, stagingBuffer);
            vkFreeMemory(_GraphicsDevice, stagingBufferMemory);

            _VertexBuffers.Add(bufferHandle);
            return &bufferHandle.VertexBuffer;
        }

        public void RenderFrame() => _GraphicsDevice.RenderFrame(RenderScene);

        private void RenderScene(VkCommandBuffer commandBuffer, VkFramebuffer frameBuffer, VkExtent2D size)
        {
            BeginSceneRender(commandBuffer, frameBuffer, size);
            DrawScene(commandBuffer);
            EndSceneRender(commandBuffer, frameBuffer, size);
        }

        private void BeginSceneRender(VkCommandBuffer cmd, VkFramebuffer frameBuffer, VkExtent2D size)
        {
            var clearValue = new VkClearValue(0.0f, 0.0f, 0.2f);
            var depthClearValue = new VkClearDepthStencilValue(1.0f, 0u);

            // Begin the render pass.
            VkRenderPassBeginInfo renderPassBeginInfo = new()
            {
                renderPass = _GraphicsDevice.SwapChain.RenderPass,
                framebuffer = frameBuffer,
                renderArea = new VkRect2D(size),
                clearValueCount = 2,
                pClearValues = &clearValue
            };

            vkCmdBeginRenderPass(cmd, &renderPassBeginInfo, VkSubpassContents.Inline);

            // Update dynamic viewport state
            // Flip coordinate to map DirectX coordinate system.
            VkViewport viewport = new()
            {
                x = 0.0f,
                y = _GraphicsDevice.SwapChain.Extent.height,
                width = _GraphicsDevice.SwapChain.Extent.width,
                height = -_GraphicsDevice.SwapChain.Extent.height,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };
            vkCmdSetViewport(cmd, viewport);

            // Update dynamic scissor state
            VkRect2D scissor = new(_GraphicsDevice.SwapChain.Extent);
            vkCmdSetScissor(cmd, scissor);

            // Bind the rendering pipeline
            vkCmdBindPipeline(cmd, VkPipelineBindPoint.Graphics, _Pipeline);
        }

        private void DrawScene(VkCommandBuffer cmd)
        {
            Vector3 camPos = new(0.0f, 0.0f, -2.0f);
            var view = Matrix4x4.CreateTranslation(camPos);
            var projection = Matrix4x4.CreatePerspective(70.0f, 1700.0f / 900.0f, 0.1f, 200.0f);
            projection.M22 *= -1;

            foreach (var renderObject in _RenderObjects)
            {
                ulong offsets = 0;
                vkCmdBindVertexBuffers(cmd, 0, 1, renderObject.Mesh.VertexBuffer, &offsets);

                var model = Matrix4x4.CreateRotationY(1.0f, new Vector3(0, 1, 0));
                var meshMatrix = projection * view * model;

                var constants = new MeshPushConstants
                {
                    //Data = renderObject.
                    RenderMatrix = meshMatrix
                };

                //upload the matrix to the GPU via push constants
                vkCmdPushConstants(cmd, _MeshPipelineLayout, VkShaderStageFlags.Vertex, 0,
                    (uint)sizeof(MeshPushConstants),
                    &constants);

                vkCmdDraw(cmd, renderObject.Mesh.Vertices.Length, 1, 0, 0);
            }
        }

        private void EndSceneRender(VkCommandBuffer cmd, VkFramebuffer frameBuffer, VkExtent2D size) =>
            vkCmdEndRenderPass(cmd);

        public void Add(RenderObject renderObject)
        {
            renderObject.Mesh.VertexBuffer = CreateVerticesBuffer(renderObject.Mesh.Vertices);
            _RenderObjects.Add(renderObject);
        }

        public void Remove(RenderObject renderObject)
        {
            var buffer = *renderObject.Mesh.VertexBuffer;
            _RenderObjects.Remove(renderObject);
            //vkDestroyBuffer(_GraphicsDevice, buffer, null);
        }

        public void Reset()
        {
            foreach (var buffer in _VertexBuffers)
            {
                vkDestroyBuffer(_GraphicsDevice, buffer.VertexBuffer, null);
                vkFreeMemory(_GraphicsDevice, buffer.DeviceMemory);
            }

            _VertexBuffers.Clear();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MeshPushConstants
        {
            public Vector4 Data;
            public Matrix4x4 RenderMatrix;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BufferHandle
        {
            public VkBuffer VertexBuffer;
            public VkDeviceMemory DeviceMemory;

            public BufferHandle()
            {
            }
        }
    }
}
