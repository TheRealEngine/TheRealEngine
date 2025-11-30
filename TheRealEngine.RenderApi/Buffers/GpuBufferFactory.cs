using Silk.NET.OpenGL;
using Silk.NET.Vulkan;

namespace TheRealEngine.RenderApi.Buffers;

public sealed class GpuBufferFactory(GraphicsBackend backend, GL? gl = null, Vk? vk = null, Device vkDevice = default, PhysicalDevice vkPhysicalDevice = default) {
    
    public GpuBuffer CreateBuffer(ulong size, BufferUsage usage, BufferUsageFlags vkUsage = 0, MemoryPropertyFlags vkMemProps = 0, BufferUsageARB glUsageHint = BufferUsageARB.DynamicDraw) {
        return backend switch {
            GraphicsBackend.OpenGL => CreateGlBuffer(size, usage, glUsageHint),
            GraphicsBackend.Vulkan => CreateVkBuffer(size, usage, vkUsage, vkMemProps),
            _ => throw new NotSupportedException()
        };
    }

    private GpuBuffer CreateGlBuffer(ulong size, BufferUsage usage, BufferUsageARB glUsageHint) {
        if (gl is null) throw new InvalidOperationException("GL instance not provided to factory.");

        return new OpenGlBuffer(gl, size, usage, glUsageHint);
    }

    
    private GpuBuffer CreateVkBuffer(ulong size, BufferUsage usage, BufferUsageFlags vkUsage, MemoryPropertyFlags vkMemProps) {
        if (vk is null) throw new InvalidOperationException("Vulkan instance not provided to factory.");

        if (vkUsage == 0) {
            if (usage.HasFlag(BufferUsage.VertexBuffer)) vkUsage |= BufferUsageFlags.VertexBufferBit;
            if (usage.HasFlag(BufferUsage.IndexBuffer)) vkUsage |= BufferUsageFlags.IndexBufferBit;
            if (usage.HasFlag(BufferUsage.UniformBuffer)) vkUsage |= BufferUsageFlags.UniformBufferBit;
            if (usage.HasFlag(BufferUsage.StorageBuffer)) vkUsage |= BufferUsageFlags.StorageBufferBit;
            if (usage.HasFlag(BufferUsage.TransferSrc)) vkUsage |= BufferUsageFlags.TransferDstBit;
            if (usage.HasFlag(BufferUsage.TransferDst)) vkUsage |= BufferUsageFlags.TransferDstBit;
        }

        if (vkMemProps == 0) {
            vkMemProps = MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit;
        }

        return new VulkanBuffer(vk, vkDevice, vkPhysicalDevice, size, usage, vkUsage, vkMemProps);
    }
}