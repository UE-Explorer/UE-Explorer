// Copyright © Amer Koleci and Contributors.

using System;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace UEExplorer.Graphics.Framework.Vortice.Vulkan;

public ref struct SwapChainSupportDetails
{
    public VkSurfaceCapabilitiesKHR Capabilities;
    public ReadOnlySpan<VkSurfaceFormatKHR> Formats;
    public ReadOnlySpan<VkPresentModeKHR> PresentModes;
}

public static class Utils
{
    public static SwapChainSupportDetails QuerySwapChainSupport(VkPhysicalDevice physicalDevice,
        VkSurfaceKHR surface)
    {
        SwapChainSupportDetails details = new();
        vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, out details.Capabilities).CheckResult();

        details.Formats = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface);
        details.PresentModes = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface);
        return details;
    }
}