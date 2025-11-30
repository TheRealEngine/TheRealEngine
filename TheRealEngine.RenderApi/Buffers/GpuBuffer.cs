namespace TheRealEngine.RenderApi.Buffers;

public enum GraphicsBackend {
    OpenGL,
    Vulkan
}

[Flags]
public enum BufferUsage {
    VertexBuffer  = 1 << 0,
    IndexBuffer   = 1 << 1,
    UniformBuffer = 1 << 2,
    StorageBuffer = 1 << 3,
    TransferSrc   = 1 << 4,
    TransferDst   = 1 << 5
}

public abstract class GpuBuffer(GraphicsBackend backend, ulong size, BufferUsage usage) : IDisposable {
    public ulong Size { get; protected set; } = size;
    public BufferUsage Usage { get; protected set; } = usage;
    public GraphicsBackend Backend { get; } = backend;

    public void SetData<T>(ReadOnlySpan<T> data) where T : unmanaged => SetData(data, 0);

    protected abstract void SetData<T>(ReadOnlySpan<T> data, ulong byteOffset) where T : unmanaged;

    public abstract IntPtr Map();
    public abstract void Unmap();

    public abstract void Dispose();
}