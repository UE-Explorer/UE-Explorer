// Copyright © Amer Koleci and Contributors.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace UEExplorer.Graphics.Framework.Vortice.Vulkan;

public sealed unsafe class GraphicsDevice : IDisposable
{
    private static readonly VkString s_engineName = new("Vortice");

    public readonly VkInstance VkInstance;

#if !NET6_0_OR_GREATER
    private readonly PFN_vkDebugUtilsMessengerCallbackEXT _DebugMessagerCallbackDelegate = DebugMessengerCallback;
#endif

    private readonly VkDebugUtilsMessengerEXT _DebugMessenger = VkDebugUtilsMessengerEXT.Null;
    public readonly VkPhysicalDevice PhysicalDevice;
    public readonly VkDevice VkDevice;
    public readonly VkQueue GraphicsQueue;
    public readonly VkQueue PresentQueue;
    public readonly SwapChain SwapChain;
    private readonly PerFrame[] _PerFrame;
    private uint _FrameIndex;

    private readonly List<VkSemaphore> _RecycledSemaphores = new();

    public GraphicsDevice(string applicationName, bool enableValidation, IntPtr windowHandle, int width, int height)
    {
        HashSet<string> availableInstanceLayers = new(EnumerateInstanceLayers());
        HashSet<string> availableInstanceExtensions = new(GetInstanceExtensions());

        List<string> instanceExtensions = new();
        instanceExtensions.AddRange(new[] { VK_KHR_SURFACE_EXTENSION_NAME, KHRWin32SurfaceExtensionName });

        List<string> instanceLayers = new();

        if (enableValidation)
        {
            // Determine the optimal validation layers to enable that are necessary for useful debugging
            GetOptimalValidationLayers(availableInstanceLayers, instanceLayers);
        }

        // Check if VK_EXT_debug_utils is supported, which supersedes VK_EXT_Debug_Report
        foreach (var availableExtension in availableInstanceExtensions)
        {
            switch (availableExtension)
            {
                case VK_EXT_DEBUG_UTILS_EXTENSION_NAME:
                    instanceExtensions.Add(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
                    break;
                case VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME:
                    instanceExtensions.Add(VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME);
                    break;
            }
        }

        VkString name = new(applicationName);
        VkApplicationInfo appInfo = new()
        {
            pApplicationName = name,
            applicationVersion = new VkVersion(1, 0, 0),
            pEngineName = s_engineName,
            engineVersion = new VkVersion(1, 0, 0),
            apiVersion = VK_HEADER_VERSION_COMPLETE
        };

        using VkStringArray vkLayerNames = new(instanceLayers);
        using VkStringArray vkInstanceExtensions = new(instanceExtensions);

        VkInstanceCreateInfo instanceCreateInfo = new()
        {
            pApplicationInfo = &appInfo,
            enabledLayerCount = vkLayerNames.Length,
            ppEnabledLayerNames = vkLayerNames,
            enabledExtensionCount = vkInstanceExtensions.Length,
            ppEnabledExtensionNames = vkInstanceExtensions
        };

        VkDebugUtilsMessengerCreateInfoEXT debugUtilsCreateInfo = new();

        if (instanceLayers.Count > 0)
        {
            debugUtilsCreateInfo.messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.Error |
                                                   VkDebugUtilsMessageSeverityFlagsEXT.Warning;
            debugUtilsCreateInfo.messageType = VkDebugUtilsMessageTypeFlagsEXT.Validation |
                                               VkDebugUtilsMessageTypeFlagsEXT.Performance;
#if NET6_0_OR_GREATER
            debugUtilsCreateInfo.pfnUserCallback = &DebugMessengerCallback;
#else
            debugUtilsCreateInfo.pfnUserCallback =
                Marshal.GetFunctionPointerForDelegate(_DebugMessagerCallbackDelegate);
#endif

            instanceCreateInfo.pNext = &debugUtilsCreateInfo;
        }

        var result = vkCreateInstance(&instanceCreateInfo, null, out VkInstance);
        if (result != VkResult.Success)
        {
            throw new InvalidOperationException($"Failed to create vulkan instance: {result}");
        }

        vkLoadInstanceOnly(VkInstance);

        if (instanceLayers.Count > 0)
        {
            vkCreateDebugUtilsMessengerEXT(VkInstance, &debugUtilsCreateInfo, null, out _DebugMessenger)
                .CheckResult();
        }

        Log.Info(
            $"Created VkInstance with version: {appInfo.apiVersion.Major}.{appInfo.apiVersion.Minor}.{appInfo.apiVersion.Patch}");
        if (instanceLayers.Count > 0)
        {
            foreach (var layer in instanceLayers)
            {
                Log.Info($"Instance layer '{layer}'");
            }
        }

        foreach (var extension in instanceExtensions)
        {
            Log.Info($"Instance extension '{extension}'");
        }

        VkSurfaceKHR surface;

        VkWin32SurfaceCreateInfoKHR info = new()
        {
            sType = VkStructureType.Win32SurfaceCreateInfoKHR, hinstance = IntPtr.Zero, hwnd = windowHandle
        };
        vkCreateWin32SurfaceKHR(VkInstance, &info, null, &surface)
            .CheckResult();

        // Find physical device, setup queue's and create device.
        var physicalDevicesCount = 0;
        vkEnumeratePhysicalDevices(VkInstance, &physicalDevicesCount, null)
            .CheckResult();

        if (physicalDevicesCount == 0)
        {
            throw new Exception("Vulkan: Failed to find GPUs with Vulkan support");
        }

        var physicalDevices = stackalloc VkPhysicalDevice[physicalDevicesCount];
        vkEnumeratePhysicalDevices(VkInstance, &physicalDevicesCount, physicalDevices)
            .CheckResult();

        for (var i = 0; i < physicalDevicesCount; i++)
        {
            var physicalDevice = physicalDevices[i];

            if (IsDeviceSuitable(physicalDevice, surface) == false)
            {
                continue;
            }

            vkGetPhysicalDeviceProperties(physicalDevice, out var checkProperties);
            var discrete = checkProperties.deviceType == VkPhysicalDeviceType.DiscreteGpu;

            if (!discrete && !PhysicalDevice.IsNull)
            {
                continue;
            }

            PhysicalDevice = physicalDevice;
            if (discrete)
            {
                // If this is discrete GPU, look no further (prioritize discrete GPU)
                break;
            }
        }

        vkGetPhysicalDeviceProperties(PhysicalDevice, out var properties);

        var queueFamilies = FindQueueFamilies(PhysicalDevice, surface);
        var availableDeviceExtensions =
            vkEnumerateDeviceExtensionProperties(PhysicalDevice);

        var supportPresent =
            vkGetPhysicalDeviceWin32PresentationSupportKHR(PhysicalDevice, queueFamilies.graphicsFamily);

        HashSet<uint> uniqueQueueFamilies = new() { queueFamilies.graphicsFamily, queueFamilies.presentFamily };

        var priority = 1.0f;
        uint queueCount = 0;
        var queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[2];

        foreach (var queueFamily in uniqueQueueFamilies)
        {
            queueCreateInfos[queueCount++] = new VkDeviceQueueCreateInfo
            {
                queueFamilyIndex = queueFamily, queueCount = 1, pQueuePriorities = &priority
            };
        }

        List<string> enabledExtensions = new() { VK_KHR_SWAPCHAIN_EXTENSION_NAME };

        VkPhysicalDeviceFeatures2 deviceFeatures2 = new();
        VkPhysicalDeviceVulkan11Features features_1_1 = new();
        VkPhysicalDeviceVulkan12Features features_1_2 = new();

        deviceFeatures2.pNext = &features_1_1;
        features_1_1.pNext = &features_1_2;

        var featuresChain = &features_1_2.pNext;

        VkPhysicalDevice8BitStorageFeatures storage8BitFeatures = default;
        if (properties.apiVersion <= VkVersion.Version_1_2)
        {
            if (CheckDeviceExtensionSupport(VK_KHR_8BIT_STORAGE_EXTENSION_NAME, availableDeviceExtensions))
            {
                enabledExtensions.Add(VK_KHR_8BIT_STORAGE_EXTENSION_NAME);
                //storage_8bit_features.sType = VkStructureType.PhysicalDevice8bitStorageFeatures;
                *featuresChain = &storage8BitFeatures;
                featuresChain = &storage8BitFeatures.pNext;
            }
        }

        if (CheckDeviceExtensionSupport(VK_KHR_SPIRV_1_4_EXTENSION_NAME, availableDeviceExtensions))
        {
            // Required by VK_KHR_spirv_1_4
            enabledExtensions.Add(VK_KHR_SHADER_FLOAT_CONTROLS_EXTENSION_NAME);

            // Required for VK_KHR_ray_tracing_pipeline
            enabledExtensions.Add(VK_KHR_SPIRV_1_4_EXTENSION_NAME);
        }

        if (CheckDeviceExtensionSupport(VK_KHR_BUFFER_DEVICE_ADDRESS_EXTENSION_NAME, availableDeviceExtensions))
        {
            // Required by VK_KHR_acceleration_structure
            enabledExtensions.Add(VK_KHR_BUFFER_DEVICE_ADDRESS_EXTENSION_NAME);
        }

        if (CheckDeviceExtensionSupport(VK_EXT_DESCRIPTOR_INDEXING_EXTENSION_NAME, availableDeviceExtensions))
        {
            // Required by VK_KHR_acceleration_structure
            enabledExtensions.Add(VK_EXT_DESCRIPTOR_INDEXING_EXTENSION_NAME);
        }

        VkPhysicalDeviceAccelerationStructureFeaturesKHR accelerationStructureFeatures = default;
        if (CheckDeviceExtensionSupport(VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME, availableDeviceExtensions))
        {
            // Required by VK_KHR_acceleration_structure
            enabledExtensions.Add(VK_KHR_DEFERRED_HOST_OPERATIONS_EXTENSION_NAME);

            enabledExtensions.Add(VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME);
            *featuresChain = &accelerationStructureFeatures;
            featuresChain = &accelerationStructureFeatures.pNext;
        }

        vkGetPhysicalDeviceFeatures2(PhysicalDevice, &deviceFeatures2);

        using VkStringArray deviceExtensionNames = new(enabledExtensions);

        VkDeviceCreateInfo deviceCreateInfo = new()
        {
            pNext = &deviceFeatures2,
            queueCreateInfoCount = queueCount,
            pQueueCreateInfos = queueCreateInfos,
            enabledExtensionCount = deviceExtensionNames.Length,
            ppEnabledExtensionNames = deviceExtensionNames,
            pEnabledFeatures = null
        };

        result = vkCreateDevice(PhysicalDevice, &deviceCreateInfo, null, out VkDevice);
        if (result != VkResult.Success)
        {
            throw new Exception($"Failed to create Vulkan Logical Device, {result}");
        }

        vkLoadDevice(VkDevice);

        vkGetDeviceQueue(VkDevice, queueFamilies.graphicsFamily, 0, out GraphicsQueue);
        vkGetDeviceQueue(VkDevice, queueFamilies.presentFamily, 0, out PresentQueue);

        // Create swap chain
        SwapChain = new SwapChain(this, surface, new VkExtent2D(width, height));
        _PerFrame = new PerFrame[SwapChain.ImageCount];
        for (var i = 0; i < _PerFrame.Length; i++)
        {
            vkCreateFence(VkDevice, VkFenceCreateFlags.Signaled, out _PerFrame[i].QueueSubmitFence).CheckResult();

            VkCommandPoolCreateInfo poolCreateInfo = new()
            {
                flags = VkCommandPoolCreateFlags.Transient, queueFamilyIndex = queueFamilies.graphicsFamily
            };
            vkCreateCommandPool(VkDevice, &poolCreateInfo, null, out _PerFrame[i].PrimaryCommandPool).CheckResult();

            vkAllocateCommandBuffer(VkDevice, _PerFrame[i].PrimaryCommandPool,
                    out _PerFrame[i].PrimaryCommandBuffer)
                .CheckResult();
        }
    }

    public void Dispose()
    {
        SwapChain.Dispose();

        for (var i = 0; i < _PerFrame.Length; i++)
        {
            vkDestroyFence(VkDevice, _PerFrame[i].QueueSubmitFence);

            if (_PerFrame[i].PrimaryCommandBuffer != IntPtr.Zero)
            {
                vkFreeCommandBuffers(VkDevice, _PerFrame[i].PrimaryCommandPool, _PerFrame[i].PrimaryCommandBuffer);

                _PerFrame[i].PrimaryCommandBuffer = IntPtr.Zero;
            }

            vkDestroyCommandPool(VkDevice, _PerFrame[i].PrimaryCommandPool);

            if (_PerFrame[i].SwapChainAcquireSemaphore != VkSemaphore.Null)
            {
                vkDestroySemaphore(VkDevice, _PerFrame[i].SwapChainAcquireSemaphore);
                _PerFrame[i].SwapChainAcquireSemaphore = VkSemaphore.Null;
            }

            if (_PerFrame[i].SwapChainReleaseSemaphore != VkSemaphore.Null)
            {
                vkDestroySemaphore(VkDevice, _PerFrame[i].SwapChainReleaseSemaphore);
                _PerFrame[i].SwapChainReleaseSemaphore = VkSemaphore.Null;
            }
        }

        foreach (var semaphore in _RecycledSemaphores)
        {
            vkDestroySemaphore(VkDevice, semaphore);
        }

        _RecycledSemaphores.Clear();

        if (VkDevice != VkDevice.Null)
        {
            vkDestroyDevice(VkDevice);
        }

        if (_DebugMessenger != VkDebugUtilsMessengerEXT.Null)
        {
            vkDestroyDebugUtilsMessengerEXT(VkInstance, _DebugMessenger);
        }

        if (VkInstance != VkInstance.Null)
        {
            vkDestroyInstance(VkInstance);
        }
    }

    public void WaitIdle()
    {
        vkDeviceWaitIdle(VkDevice).CheckResult();
    }

    public void RenderFrame(Action<VkCommandBuffer, VkFramebuffer, VkExtent2D> renderScene,
        [CallerMemberName] string frameName = null)
    {
        var result = AcquireNextImage(out _FrameIndex);

        // Handle outdated error in acquire.
        if (result is VkResult.SuboptimalKHR or VkResult.ErrorOutOfDateKHR)
        {
            //Resize(context.swapchain_dimensions.width, context.swapchain_dimensions.height);
            result = AcquireNextImage(out _FrameIndex);
        }

        if (result != VkResult.Success)
        {
            vkDeviceWaitIdle(VkDevice);
            return;
        }

        // Begin command recording
        var cmd = _PerFrame[_FrameIndex].PrimaryCommandBuffer;

        VkCommandBufferBeginInfo beginInfo = new() { flags = VkCommandBufferUsageFlags.OneTimeSubmit };
        vkBeginCommandBuffer(cmd, &beginInfo).CheckResult();

        renderScene(cmd, SwapChain.FrameBuffers[_FrameIndex], SwapChain.Extent);

        // Complete the command buffer.
        vkEndCommandBuffer(cmd).CheckResult();

        if (_PerFrame[_FrameIndex].SwapChainReleaseSemaphore == VkSemaphore.Null)
        {
            vkCreateSemaphore(VkDevice, out _PerFrame[_FrameIndex].SwapChainReleaseSemaphore).CheckResult();
        }

        var waitStage = VkPipelineStageFlags.ColorAttachmentOutput;
        var waitSemaphore = _PerFrame[_FrameIndex].SwapChainAcquireSemaphore;
        var signalSemaphore = _PerFrame[_FrameIndex].SwapChainReleaseSemaphore;

        VkSubmitInfo submitInfo = new()
        {
            commandBufferCount = 1u,
            pCommandBuffers = &cmd,
            waitSemaphoreCount = 1u,
            pWaitSemaphores = &waitSemaphore,
            pWaitDstStageMask = &waitStage,
            signalSemaphoreCount = 1u,
            pSignalSemaphores = &signalSemaphore
        };

        // Submit command buffer to graphics queue
        vkQueueSubmit(GraphicsQueue, submitInfo, _PerFrame[_FrameIndex].QueueSubmitFence);

        result = PresentImage(_FrameIndex);

        // Handle Outdated error in present.
        if (result is VkResult.SuboptimalKHR or VkResult.ErrorOutOfDateKHR)
        {
            //Resize(context.swapchain_dimensions.width, context.swapchain_dimensions.height);
        }
        else if (result != VkResult.Success)
        {
            Log.Error("Failed to present swapchain image.");
        }
    }

    public uint GetMemoryTypeIndex(uint typeBits, VkMemoryPropertyFlags properties)
    {
        vkGetPhysicalDeviceMemoryProperties(PhysicalDevice,
            out var deviceMemoryProperties);

        // Iterate over all memory types available for the device used in this example
        for (var i = 0; i < deviceMemoryProperties.memoryTypeCount; i++)
        {
            if ((typeBits & 1) == 1)
            {
                if ((deviceMemoryProperties.memoryTypes[i].propertyFlags & properties) == properties)
                {
                    return (uint)i;
                }
            }

            typeBits >>= 1;
        }

        throw new Exception("Could not find a suitable memory type!");
    }

    public VkCommandBuffer GetCommandBuffer(bool begin = true)
    {
        vkAllocateCommandBuffer(VkDevice,
            _PerFrame[_FrameIndex].PrimaryCommandPool,
            out var commandBuffer).CheckResult();

        // If requested, also start the new command buffer
        if (!begin)
        {
            return commandBuffer;
        }

        VkCommandBufferBeginInfo beginInfo = new() { flags = VkCommandBufferUsageFlags.OneTimeSubmit };
        vkBeginCommandBuffer(commandBuffer, &beginInfo).CheckResult();

        return commandBuffer;
    }

    public void FlushCommandBuffer(VkCommandBuffer commandBuffer)
    {
        vkEndCommandBuffer(commandBuffer).CheckResult();

        VkSubmitInfo submitInfo = new() { commandBufferCount = 1, pCommandBuffers = &commandBuffer };

        // Create fence to ensure that the command buffer has finished executing
        vkCreateFence(VkDevice, out var fence);

        // Submit to the queue
        vkQueueSubmit(GraphicsQueue, 1, &submitInfo, fence).CheckResult();

        // Wait for the fence to signal that command buffer has finished executing
        vkWaitForFences(VkDevice, 1, &fence, true, ulong.MaxValue).CheckResult();

        vkDestroyFence(VkDevice, fence);
    }

    private VkResult AcquireNextImage(out uint imageIndex)
    {
        VkSemaphore acquireSemaphore;
        if (_RecycledSemaphores.Count == 0)
        {
            vkCreateSemaphore(VkDevice, out acquireSemaphore).CheckResult();
        }
        else
        {
            acquireSemaphore = _RecycledSemaphores[_RecycledSemaphores.Count - 1];
            _RecycledSemaphores.RemoveAt(_RecycledSemaphores.Count - 1);
        }

        var result = vkAcquireNextImageKHR(VkDevice, SwapChain.Handle, ulong.MaxValue, acquireSemaphore,
            VkFence.Null, out imageIndex);

        if (result != VkResult.Success)
        {
            _RecycledSemaphores.Add(acquireSemaphore);
            return result;
        }

        if (_PerFrame[imageIndex].QueueSubmitFence != VkFence.Null)
        {
            vkWaitForFences(VkDevice, _PerFrame[imageIndex].QueueSubmitFence, true, ulong.MaxValue);
            vkResetFences(VkDevice, _PerFrame[imageIndex].QueueSubmitFence);
        }

        if (_PerFrame[imageIndex].PrimaryCommandPool != VkCommandPool.Null)
        {
            vkResetCommandPool(VkDevice, _PerFrame[imageIndex].PrimaryCommandPool, VkCommandPoolResetFlags.None);
        }

        // Recycle the old semaphore back into the semaphore manager.
        var oldSemaphore = _PerFrame[imageIndex].SwapChainAcquireSemaphore;

        if (oldSemaphore != VkSemaphore.Null)
        {
            _RecycledSemaphores.Add(oldSemaphore);
        }

        _PerFrame[imageIndex].SwapChainAcquireSemaphore = acquireSemaphore;

        return VkResult.Success;
    }

    private VkResult PresentImage(uint imageIndex)
    {
        return vkQueuePresentKHR(PresentQueue, _PerFrame[imageIndex].SwapChainReleaseSemaphore, SwapChain.Handle,
            imageIndex);
    }

    public static implicit operator VkDevice(GraphicsDevice device)
    {
        return device.VkDevice;
    }

    public VkResult CreateShaderModule(byte[] data, out VkShaderModule module)
    {
        return vkCreateShaderModule(VkDevice, data, null, out module);
    }

    #region Private Methods

    private static bool CheckDeviceExtensionSupport(string extensionName,
        ReadOnlySpan<VkExtensionProperties> availableDeviceExtensions)
    {
        foreach (var property in availableDeviceExtensions)
        {
            if (string.Equals(property.GetExtensionName(), extensionName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static void GetOptimalValidationLayers(HashSet<string> availableLayers, List<string> instanceLayers)
    {
        // The preferred validation layer is "VK_LAYER_KHRONOS_validation"
        List<string> validationLayers = new() { "VK_LAYER_KHRONOS_validation" };

        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }

        // Otherwise we fallback to using the LunarG meta layer
        validationLayers = new List<string> { "VK_LAYER_LUNARG_standard_validation" };

        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }

        // Otherwise we attempt to enable the individual layers that compose the LunarG meta layer since it doesn't exist
        validationLayers = new List<string>
        {
            "VK_LAYER_GOOGLE_threading",
            "VK_LAYER_LUNARG_parameter_validation",
            "VK_LAYER_LUNARG_object_tracker",
            "VK_LAYER_LUNARG_core_validation",
            "VK_LAYER_GOOGLE_unique_objects"
        };

        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }

        // Otherwise as a last resort we fallback to attempting to enable the LunarG core layer
        validationLayers = new List<string> { "VK_LAYER_LUNARG_core_validation" };

        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.AddRange(validationLayers);
        }
    }

    private static bool ValidateLayers(IEnumerable<string> required, HashSet<string> availableLayers)
    {
        return required
            .Select(layer => availableLayers.Any(availableLayer => availableLayer == layer))
            .All(found => found);
    }

    private static bool IsDeviceSuitable(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
    {
        var checkQueueFamilies = FindQueueFamilies(physicalDevice, surface);
        if (checkQueueFamilies.graphicsFamily == VK_QUEUE_FAMILY_IGNORED)
        {
            return false;
        }

        if (checkQueueFamilies.presentFamily == VK_QUEUE_FAMILY_IGNORED)
        {
            return false;
        }

        var swapChainSupport = Utils.QuerySwapChainSupport(physicalDevice, surface);
        return !swapChainSupport.Formats.IsEmpty && !swapChainSupport.PresentModes.IsEmpty;
    }


#if NET6_0_OR_GREATER
    [UnmanagedCallersOnly]
#endif
    private static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity,
        VkDebugUtilsMessageTypeFlagsEXT messageTypes,
        VkDebugUtilsMessengerCallbackDataEXT* pCallbackData,
        void* userData)
    {
        string message = Interop.GetUtf8Span(pCallbackData->pMessage).GetString();
        if (messageTypes == VkDebugUtilsMessageTypeFlagsEXT.Validation)
        {
            switch (messageSeverity)
            {
                case VkDebugUtilsMessageSeverityFlagsEXT.Error:
                    Log.Error($"[Vulkan]: Validation: {messageSeverity} - {message}");
                    break;
                case VkDebugUtilsMessageSeverityFlagsEXT.Warning:
                    Log.Warn($"[Vulkan]: Validation: {messageSeverity} - {message}");
                    break;
            }

            Debug.WriteLine($"[Vulkan]: Validation: {messageSeverity} - {message}");
        }
        else
        {
            switch (messageSeverity)
            {
                case VkDebugUtilsMessageSeverityFlagsEXT.Error:
                    Log.Error($"[Vulkan]: {messageSeverity} - {message}");
                    break;
                case VkDebugUtilsMessageSeverityFlagsEXT.Warning:
                    Log.Warn($"[Vulkan]: {messageSeverity} - {message}");
                    break;
            }

            Debug.WriteLine($"[Vulkan]: {messageSeverity} - {message}");
        }

        return VK_FALSE;
    }

    private static (uint graphicsFamily, uint presentFamily) FindQueueFamilies(
        VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        var queueFamilies = vkGetPhysicalDeviceQueueFamilyProperties(device);

        uint graphicsFamily = VK_QUEUE_FAMILY_IGNORED;
        uint presentFamily = VK_QUEUE_FAMILY_IGNORED;
        uint i = 0;
        foreach (var queueFamily in queueFamilies)
        {
            if ((queueFamily.queueFlags & VkQueueFlags.Graphics) != VkQueueFlags.None)
            {
                graphicsFamily = i;
            }

            vkGetPhysicalDeviceSurfaceSupportKHR(device, i, surface, out var presentSupport);
            if (presentSupport)
            {
                presentFamily = i;
            }

            if (graphicsFamily != VK_QUEUE_FAMILY_IGNORED
                && presentFamily != VK_QUEUE_FAMILY_IGNORED)
            {
                break;
            }

            i++;
        }

        return (graphicsFamily, presentFamily);
    }

    #endregion

    private static readonly Lazy<bool> _IsSupported = new(CheckIsSupported);

    public static bool IsSupported()
    {
        return _IsSupported.Value;
    }

    private static bool CheckIsSupported()
    {
        try
        {
            var result = vkInitialize();
            if (result != VkResult.Success)
            {
                return false;
            }

            // We require Vulkan 1.1 or higher
            var version = vkEnumerateInstanceVersion();
            if (version < VkVersion.Version_1_1)
            {
                return false;
            }

            // TODO: Enumerate physical devices and try to create instance.

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static IEnumerable<string> EnumerateInstanceLayers()
    {
        if (!IsSupported())
        {
            return Array.Empty<string>();
        }

        var count = 0;
        var result = vkEnumerateInstanceLayerProperties(&count, null);
        if (result != VkResult.Success)
        {
            return Array.Empty<string>();
        }

        if (count == 0)
        {
            return Array.Empty<string>();
        }

        var properties = stackalloc VkLayerProperties[count];
        vkEnumerateInstanceLayerProperties(&count, properties).CheckResult();

        var resultExt = new string[count];
        for (var i = 0; i < count; i++)
        {
            resultExt[i] = properties[i].GetLayerName();
        }

        return resultExt;
    }

    private static IEnumerable<string> GetInstanceExtensions()
    {
        var count = 0;
        var result = vkEnumerateInstanceExtensionProperties(null, &count, null);
        if (result != VkResult.Success)
        {
            return Array.Empty<string>();
        }

        if (count == 0)
        {
            return Array.Empty<string>();
        }

        var props = stackalloc VkExtensionProperties[count];
        vkEnumerateInstanceExtensionProperties(null, &count, props);

        var extensions = new string[count];
        for (var i = 0; i < count; i++)
        {
            extensions[i] = props[i].GetExtensionName();
        }

        return extensions;
    }

    private struct PerFrame
    {
        public VkFence QueueSubmitFence;
        public VkCommandPool PrimaryCommandPool;
        public VkCommandBuffer PrimaryCommandBuffer;
        public VkSemaphore SwapChainAcquireSemaphore;
        public VkSemaphore SwapChainReleaseSemaphore;
    }
}
