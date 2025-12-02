using System.Numerics;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using StbImageSharp;

namespace TheRealEngine.RenderApi;

public readonly struct TextureHandle {
    public uint Handle { get; }
    public TextureHandle(uint handle) => Handle = handle;
}

public sealed class RenderContext2D {
    public GL Gl { get; }
    
    public Matrix4x4 Projection;

    private readonly uint _vao;
    private readonly uint _vbo;
    private readonly uint _ebo;
    private readonly uint _shader;
    
    public RenderContext2D(GL gl, IWindow window) {
        Gl = gl;
        
        Projection = Matrix4x4.CreateOrthographicOffCenter(0, window.Size.X, 0, window.Size.Y, -1f, 1f);
        
        float[] vertices = {
            0.5f,  0.5f, 0.0f,  1.0f, 1.0f,
            0.5f, -0.5f, 0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f, 0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, 0.0f,  0.0f, 1.0f
        };

        uint[] indices = {
            3, 2, 1,
            3, 1, 0
        };
        
        _vao = gl.GenVertexArray();
        _vbo = gl.GenBuffer();
        _ebo = gl.GenBuffer();

        gl.BindVertexArray(_vao);

        gl.BindBuffer(GLEnum.ArrayBuffer, _vbo);
        unsafe {
            fixed (float* v = vertices) {
                gl.BufferData(GLEnum.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), v, GLEnum.StaticDraw);
            }
        }

        gl.BindBuffer(GLEnum.ElementArrayBuffer, _ebo);
        unsafe {
            fixed (uint* i = indices) {
                gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), i, GLEnum.StaticDraw);
            }
        }

        gl.EnableVertexAttribArray(0);
        unsafe {
            gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)0);
        }
        
        gl.EnableVertexAttribArray(1);
        unsafe {
            gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)(3 * sizeof(float)));
        }

        gl.BindVertexArray(0);

        _shader = CreateShader(gl);
    }
    
    public TextureHandle LoadTextureFromFile(string path) {
        StbImage.stbi_set_flip_vertically_on_load(1);
        
        uint texture = Gl.GenTexture();
        Gl.ActiveTexture(TextureUnit.Texture0);
        Gl.BindTexture(TextureTarget.Texture2D, texture);
        
        ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);
        
        unsafe {
            fixed (byte* ptr = result.Data) {
                Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, 
                    (uint)result.Width, 
                    (uint)result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
            }
        }

        Gl.GenerateMipmap(GLEnum.Texture2D);
        
        Gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)GLEnum.Repeat);
        Gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)GLEnum.Repeat);
        Gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
        Gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);

        Gl.BindTexture(TextureTarget.Texture2D, 0);

        return new TextureHandle(texture);
    }

    public void DrawTexturedQuad(TextureHandle texture, in Matrix4x4 model) {
        GL gl = Gl;

        gl.UseProgram(_shader);
        
        int modelLoc = gl.GetUniformLocation(_shader, "uModel");
        unsafe {
            fixed (float* m = &model.M11) { gl.UniformMatrix4(modelLoc, 1, false, m); }
        }
        
        int projLoc = gl.GetUniformLocation(_shader, "uProjection");
        unsafe {
            fixed (float* p = &Projection.M11) { gl.UniformMatrix4(projLoc, 1, false, p); }
        }

        int texLoc = gl.GetUniformLocation(_shader, "uTexture");
        gl.Uniform1(texLoc, 0);
        
        gl.ActiveTexture(TextureUnit.Texture0);
        gl.BindTexture(TextureTarget.Texture2D, texture.Handle);

        gl.BindVertexArray(_vao);
        unsafe {
            gl.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, null);
        }
        
        gl.BindVertexArray(0);
    }

    private static uint CreateShader(GL gl) {
        const string vs = @"
        #version 330 core

        layout (location = 0) in vec3 aPosition;
        layout (location = 1) in vec2 aTextureCoord;

        uniform mat4 uModel;
        uniform mat4 uProjection;

        out vec2 frag_texCoords;

        void main()
        {
            gl_Position = uProjection * uModel * vec4(aPosition, 1.0);
            frag_texCoords = aTextureCoord;
        }";

        const string fs = @"
        #version 330 core

        uniform sampler2D uTexture;

        in vec2 frag_texCoords;

        out vec4 out_color;

        void main()
        {
            out_color = texture(uTexture, frag_texCoords);
        }";

        uint vsHandle = gl.CreateShader(GLEnum.VertexShader);
        gl.ShaderSource(vsHandle, vs);
        gl.CompileShader(vsHandle);

        uint fsHandle = gl.CreateShader(GLEnum.FragmentShader);
        gl.ShaderSource(fsHandle, fs);
        gl.CompileShader(fsHandle);

        uint program = gl.CreateProgram();
        gl.AttachShader(program, vsHandle);
        gl.AttachShader(program, fsHandle);
        gl.LinkProgram(program);

        gl.DeleteShader(vsHandle);
        gl.DeleteShader(fsHandle);

        return program;
    }
}