using System.Runtime.CompilerServices;
using Silk.NET.Vulkan;
using Buffer = Silk.NET.Vulkan.Buffer;

namespace TheRealEngine.RenderApi.Buffers;

public sealed class VulkanBuffer : GpuBuffer {
    private readonly Vk _vk;
    private readonly Device _device;
    private readonly PhysicalDevice _physicalDevice;

    private Buffer Buffer { get; set; }
    private DeviceMemory Memory { get; set; }
    
    public VulkanBuffer(Vk vk, Device device, PhysicalDevice physicalDevice, ulong size, BufferUsage usage, BufferUsageFlags vkUsage, MemoryPropertyFlags memoryProps) : base(GraphicsBackend.Vulkan, size, usage) {
        _vk = vk;
        _device = device;
        _physicalDevice = physicalDevice;
    
        CreateBuffer(size, vkUsage, memoryProps);
    }
    
    private unsafe void CreateBuffer(ulong size, BufferUsageFlags usage, MemoryPropertyFlags properties) {
        BufferCreateInfo bufferInfo = new BufferCreateInfo {
            SType = StructureType.BufferCreateInfo,
            Size = size,
            Usage = usage,
            SharingMode = SharingMode.Exclusive
        };
    
        if (_vk.CreateBuffer(_device, bufferInfo, null, out Buffer buffer) != Result.Success) throw new Exception("Failed to create Vulkan buffer.");
        Buffer = buffer;
    
        _vk.GetBufferMemoryRequirements(_device, Buffer, out var memReqs);
    
        uint memoryTypeIndex = FindMemoryType(memReqs.MemoryTypeBits, properties);
    
        MemoryAllocateInfo allocInfo = new MemoryAllocateInfo {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memReqs.Size,
            MemoryTypeIndex = memoryTypeIndex
        };
    
        if (_vk.AllocateMemory(_device, allocInfo, null, out DeviceMemory memory) != Result.Success) throw new Exception("Failed to allocate Vulkan buffer memory.");
        Memory = memory;
        
        _vk.BindBufferMemory(_device, Buffer, Memory, 0);
    }
    
    private uint FindMemoryType(uint typeFilter, MemoryPropertyFlags properties) {
        _vk.GetPhysicalDeviceMemoryProperties(_physicalDevice, out PhysicalDeviceMemoryProperties memProps);
        for (uint i = 0; i < memProps.MemoryTypeCount; i++) {
            bool supported = (typeFilter & (1u << (int)i)) != 0;
            bool hasProps = (memProps.MemoryTypes[(int)i].PropertyFlags & properties) == properties;
    
            if (supported && hasProps)
                return i;
        }
    
        throw new Exception("Failed to find suitable Vulkan memory type.");
    }
    
    protected override unsafe void SetData<T>(ReadOnlySpan<T> data, ulong byteOffset) {
        IntPtr mapped = Map();
        byte* dst = (byte*)mapped + (long)byteOffset;
    
        fixed (T* src = data) {
            uint sizeInBytes = (uint)(data.Length * Unsafe.SizeOf<T>());
            Unsafe.CopyBlock(dst, src, sizeInBytes);
        }
    
        Unmap();
    }
    
    public override unsafe IntPtr Map() {
        void* mapped;
        if (_vk.MapMemory(_device, Memory, 0, Size, 0, &mapped) != Result.Success) throw new Exception("Failed to map Vulkan buffer memory.");
        return (IntPtr)mapped;
    }
    
    public override void Unmap() {
        _vk.UnmapMemory(_device, Memory);
    }
    
    public override void Dispose() {
        if (Buffer.Handle != 0) {
            unsafe {
                _vk.DestroyBuffer(_device, Buffer, null);
            }
            
            Buffer = default;
        }
    
        if (Memory.Handle != 0) {
            unsafe {
                _vk.FreeMemory(_device, Memory, null);
            }
            
            Memory = default;
        }
    }
}