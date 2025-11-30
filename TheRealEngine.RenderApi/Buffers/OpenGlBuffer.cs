using Silk.NET.OpenGL;

namespace TheRealEngine.RenderApi.Buffers;

public sealed class OpenGlBuffer : GpuBuffer {
    private readonly GL _gl;
    private readonly BufferTargetARB _target;
    private uint Handle { get; set; }

    public OpenGlBuffer(GL gl, ulong size, BufferUsage usage, BufferUsageARB glUsageHint) : base(GraphicsBackend.OpenGL, size, usage) {
        _gl = gl;
        _target = ResolveTarget(usage);

        Handle = _gl.GenBuffer();
        _gl.BindBuffer(_target, Handle);
        
        unsafe { _gl.BufferData(_target, (uint)size, null, glUsageHint); }
        
        _gl.BindBuffer(_target, 0);
    }

    private static BufferTargetARB ResolveTarget(BufferUsage usage) {
        if (usage.HasFlag(BufferUsage.VertexBuffer)) return BufferTargetARB.ArrayBuffer;
        if (usage.HasFlag(BufferUsage.IndexBuffer)) return BufferTargetARB.ElementArrayBuffer;
        if (usage.HasFlag(BufferUsage.UniformBuffer)) return BufferTargetARB.UniformBuffer;
        return usage.HasFlag(BufferUsage.StorageBuffer) ? BufferTargetARB.ShaderStorageBuffer : BufferTargetARB.ArrayBuffer;
    }

    protected override void SetData<T>(ReadOnlySpan<T> data, ulong byteOffset) {
        _gl.BindBuffer(_target, Handle);
        unsafe {
            fixed (T* ptr = data) {
                _gl.BufferSubData(_target, (nint)byteOffset, (uint)(data.Length * sizeof(T)), ptr);
            }
        }
        
        _gl.BindBuffer(_target, 0);
    }

    public override IntPtr Map() {
        _gl.BindBuffer(_target, Handle);
        unsafe {
            void* ptr = _gl.MapBuffer(_target, BufferAccessARB.WriteOnly);
            return (IntPtr)ptr;
        }
    }

    public override void Unmap() {
        _gl.UnmapBuffer(_target);
        _gl.BindBuffer(_target, 0);
    }

    public override void Dispose() {
        if (Handle == 0) return;
        
        _gl.DeleteBuffer(Handle);
        Handle = 0;
    }
}