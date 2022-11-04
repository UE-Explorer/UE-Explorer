// Copyright © Amer Koleci and Contributors.

using System;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace UEExplorer.Graphics.Framework.Vortice.Vulkan;

public sealed unsafe class SwapChain : IDisposable
{
    private readonly GraphicsDevice _Device;
    private readonly VkSurfaceKHR _Surface;
    private readonly VkImageView[] _SwapChainImageViews;

    public VkSwapchainKHR Handle;
    public VkRenderPass RenderPass;

    public SwapChain(GraphicsDevice device, VkSurfaceKHR surface, VkExtent2D extent)
    {
        _Device = device;
        _Surface = surface;

        var swapChainSupport = Utils.QuerySwapChainSupport(device.PhysicalDevice, surface);

        var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
        var presentMode = ChooseSwapPresentMode(swapChainSupport.PresentModes);
        Extent = ChooseSwapExtent(swapChainSupport.Capabilities, extent);

        CreateRenderPass(surfaceFormat.format);

        var imageCount = swapChainSupport.Capabilities.minImageCount + 1;
        if (swapChainSupport.Capabilities.maxImageCount > 0 &&
            imageCount > swapChainSupport.Capabilities.maxImageCount)
        {
            imageCount = swapChainSupport.Capabilities.maxImageCount;
        }

        VkSwapchainCreateInfoKHR createInfo = new()
        {
            sType = VkStructureType.SwapchainCreateInfoKHR,
            surface = surface,
            minImageCount = imageCount,
            imageFormat = surfaceFormat.format,
            imageColorSpace = surfaceFormat.colorSpace,
            imageExtent = Extent,
            imageArrayLayers = 1,
            imageUsage = VkImageUsageFlags.ColorAttachment,
            imageSharingMode = VkSharingMode.Exclusive,
            preTransform = swapChainSupport.Capabilities.currentTransform,
            compositeAlpha = VkCompositeAlphaFlagsKHR.Opaque,
            presentMode = presentMode,
            clipped = true,
            oldSwapchain = VkSwapchainKHR.Null
        };

        vkCreateSwapchainKHR(device.VkDevice, &createInfo, null, out Handle).CheckResult();
        var swapChainImages = vkGetSwapchainImagesKHR(device.VkDevice, Handle);
        _SwapChainImageViews = new VkImageView[swapChainImages.Length];
        FrameBuffers = new VkFramebuffer[swapChainImages.Length];

        for (var i = 0; i < swapChainImages.Length; i++)
        {
            VkImageViewCreateInfo viewCreateInfo = new(
                swapChainImages[i],
                VkImageViewType.Image2D,
                surfaceFormat.format,
                VkComponentMapping.Rgba,
                new VkImageSubresourceRange(
                    VkImageAspectFlags.Color,
                    0,
                    1,
                    0,
                    1
                )
            );

            vkCreateImageView(_Device.VkDevice, &viewCreateInfo, null, out _SwapChainImageViews[i]).CheckResult();
            vkCreateFramebuffer(_Device.VkDevice, RenderPass, new[] { _SwapChainImageViews[i] }, Extent, 1u,
                out FrameBuffers[i]);
        }
    }

    public int ImageCount => _SwapChainImageViews.Length;
    public VkExtent2D Extent { get; }
    public VkFramebuffer[] FrameBuffers { get; }

    public void Dispose()
    {
        foreach (var view in _SwapChainImageViews)
        {
            vkDestroyImageView(_Device, view);
        }

        foreach (var buffer in FrameBuffers)
        {
            vkDestroyFramebuffer(_Device, buffer);
        }

        vkDestroyRenderPass(_Device, RenderPass);

        if (Handle != VkSwapchainKHR.Null)
        {
            vkDestroySwapchainKHR(_Device, Handle);
        }

        if (_Surface != VkSurfaceKHR.Null)
        {
            vkDestroySurfaceKHR(_Device.VkInstance, _Surface);
        }
    }

    private void CreateRenderPass(VkFormat colorFormat)
    {
        VkAttachmentDescription attachment = new(
            colorFormat,
            VkSampleCountFlags.Count1,
            VkAttachmentLoadOp.Clear, VkAttachmentStoreOp.Store,
            VkAttachmentLoadOp.DontCare, VkAttachmentStoreOp.DontCare,
            VkImageLayout.Undefined, VkImageLayout.PresentSrcKHR
        );

        VkAttachmentReference colorAttachmentRef = new(0, VkImageLayout.ColorAttachmentOptimal);

        VkSubpassDescription subPassDescription = new()
        {
            pipelineBindPoint = VkPipelineBindPoint.Graphics,
            colorAttachmentCount = 1,
            pColorAttachments = &colorAttachmentRef
        };

        var dependencies = new VkSubpassDependency[2];

        dependencies[0] = new VkSubpassDependency
        {
            srcSubpass = VK_SUBPASS_EXTERNAL,
            dstSubpass = 0,
            srcStageMask = VkPipelineStageFlags.BottomOfPipe,
            dstStageMask = VkPipelineStageFlags.ColorAttachmentOutput,
            srcAccessMask = VkAccessFlags.MemoryRead,
            dstAccessMask = VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite,
            dependencyFlags = VkDependencyFlags.ByRegion
        };

        dependencies[1] = new VkSubpassDependency
        {
            srcSubpass = 0,
            dstSubpass = VK_SUBPASS_EXTERNAL,
            srcStageMask = VkPipelineStageFlags.ColorAttachmentOutput,
            dstStageMask = VkPipelineStageFlags.BottomOfPipe,
            srcAccessMask = VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite,
            dstAccessMask = VkAccessFlags.MemoryRead,
            dependencyFlags = VkDependencyFlags.ByRegion
        };

        fixed (VkSubpassDependency* dependenciesPtr = &dependencies[0])
        {
            VkRenderPassCreateInfo createInfo = new()
            {
                sType = VkStructureType.RenderPassCreateInfo,
                attachmentCount = 1,
                pAttachments = &attachment,
                subpassCount = 1,
                pSubpasses = &subPassDescription,
                dependencyCount = 2,
                pDependencies = dependenciesPtr
            };

            vkCreateRenderPass(_Device, &createInfo, null, out RenderPass).CheckResult();
        }
    }


    private static VkExtent2D ChooseSwapExtent(VkSurfaceCapabilitiesKHR capabilities, VkExtent2D extent)
    {
        if (capabilities.currentExtent.width > 0)
        {
            return capabilities.currentExtent;
        }

        var actualExtent = extent;

        actualExtent = new VkExtent2D(
            Math.Max(capabilities.minImageExtent.width,
                Math.Min(capabilities.maxImageExtent.width, actualExtent.width)),
            Math.Max(capabilities.minImageExtent.height,
                Math.Min(capabilities.maxImageExtent.height, actualExtent.height))
        );

        return actualExtent;
    }

    private static VkSurfaceFormatKHR ChooseSwapSurfaceFormat(ReadOnlySpan<VkSurfaceFormatKHR> availableFormats)
    {
        // If the surface format list only includes one entry with VK_FORMAT_UNDEFINED,
        // there is no preferred format, so we assume VK_FORMAT_B8G8R8A8_UNORM
        if (availableFormats.Length == 1 && availableFormats[0].format == VkFormat.Undefined)
        {
            return new VkSurfaceFormatKHR(VkFormat.B8G8R8A8UNorm, availableFormats[0].colorSpace);
        }

        // iterate over the list of available surface format and
        // check for the presence of VK_FORMAT_B8G8R8A8_UNORM
        foreach (var availableFormat in availableFormats)
        {
            if (availableFormat.format == VkFormat.B8G8R8A8UNorm)
            {
                return availableFormat;
            }
        }

        return availableFormats[0];
    }

    private static VkPresentModeKHR ChooseSwapPresentMode(ReadOnlySpan<VkPresentModeKHR> availablePresentModes)
    {
        foreach (var availablePresentMode in availablePresentModes)
        {
            if (availablePresentMode == VkPresentModeKHR.Mailbox)
            {
                return availablePresentMode;
            }
        }

        return VkPresentModeKHR.Fifo;
    }
}