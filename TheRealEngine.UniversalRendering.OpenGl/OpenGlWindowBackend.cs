using System.Numerics;
using GlmSharp;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.OpenGl;

public class OpenGlWindowBackend : IWindowBackend {
    public WindowNode Window { get; set; }

    private IWindow _window = null!;
    private IInputContext _input = null!;
    private GL _gl = null!;
    private RenderContext2D _ctx = null!;
    
    private readonly Dictionary<int, TextureHandle> _loadedTextures = [];

    public dvec2 GetMousePosition() {
        throw new NotImplementedException();
    }

    public void Init() {
        WindowOptions options = WindowOptions.Default with {
            Size = new Vector2D<int>(Window.Width, Window.Height),
            Title = Window.Title
        };

        _window = Silk.NET.Windowing.Window.Create(options);
        _window.FramebufferResize += OnFramebufferResize;
        
        _window.Initialize();

        _gl = GL.GetApi(_window);
        _ctx = new RenderContext2D(_gl, _window);

        _input = _window.CreateInput();
        foreach (IKeyboard keyboard in _input.Keyboards) {
            keyboard.KeyDown += OnKeyDown;
        }
        
        _gl.Enable(GLEnum.Blend);
        _gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha);
    }
    
    public void Update(double delta) {
        if (_window.IsClosing && Window.QuitOnClose) {
            Engine.Quit();
        }
    }

    public void Render(INode node) {
        _window.DoEvents();
        
        _gl.ClearColor(0.25f, 0.26f, 0.27f, 1f);
        _gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        RenderSubtree(Window);
        
        _window.SwapBuffers();
    }

    public bool IsButtonPressed(KeyboardButton button) {
        return false;
    }

    public bool IsButtonJustPressedThisUpdate(KeyboardButton button) {
        return false;
    }

    public bool IsButtonJustPressedThisTick(KeyboardButton button) {
        return false;
    }

    private void RenderSubtree(INode root) {
        foreach (INode node in root.GetTreeEnumerator()) {
            switch (node) {
                case SpriteNode sprite: {
                    TextureHandle handle = GetTexture(sprite);
                    
                    if (handle.Handle == 0) {
                        return;
                    }

                    Matrix4x4 model =
                        Matrix4x4.CreateScale((float)sprite.Transform.Scale.x, (float)sprite.Transform.Scale.y, 1f) *
                        Matrix4x4.CreateRotationZ((float)sprite.Transform.Rotation) *
                        Matrix4x4.CreateTranslation((float)sprite.Transform.Position.x, (float)sprite.Transform.Position.y, 0f);

                    _ctx.DrawTexturedQuad(handle, model);
                    break;
                }
            }
        }
    }

    private TextureHandle GetTexture(SpriteNode sprite) {
        int hash = sprite.GetHashCode();
        
        if (_loadedTextures.TryGetValue(hash, out TextureHandle texture)) {
            return texture;
        }
        
        TextureHandle handle = _ctx.LoadTextureFromFile(sprite.TexturePath);
        _loadedTextures[hash] = handle;
        return handle;
    }

    private void OnFramebufferResize(Vector2D<int> newSize) {
        _gl.Viewport(0, 0, (uint)newSize.X, (uint)newSize.Y);
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int code) {
        if (key == Key.Escape) {
            _window.Close();
        }

        // TODO: add / extend engine's input system here
    }
    
    public void Shutdown() {
        foreach (IKeyboard keyboard in _input.Keyboards) {
            keyboard.KeyDown -= OnKeyDown;
        }
        
        _input.Dispose();
        _input = null!;

        _window.Dispose();
        _window = null!;
    }
}
