using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using TheRealEngine.Nodes;

namespace TheRealEngine.RenderApi;

public class WindowOpenGL : NodeBase {
    private IWindow _window = null!;
    private IInputContext _input = null!;
    private GL _gl = null!;
    private RenderContext2D _ctx = null!;

    public Vector2D<int> Size { get; set; } = new(800, 600);
    public string Title { get; set; } = "Silk.Net Window";

    public WindowOpenGL() {
        Initialize();
    }

    private void Initialize() {
        WindowOptions options = WindowOptions.Default with {
            Size = Size,
            Title = Title
        };

        _window = Window.Create(options);
        _window.FramebufferResize += OnFramebufferResize;
        
        _window.Initialize();

        _gl = GL.GetApi(_window);
        _ctx = new RenderContext2D(_gl);

        _input = _window.CreateInput();
        foreach (IKeyboard keyboard in _input.Keyboards) {
            keyboard.KeyDown += OnKeyDown;
        }
        
        _gl.Enable(GLEnum.Blend);
        _gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha);
    }

    public override void Update(double delta) {
        if (_window.IsClosing) {
            Engine.Quit();
            return;
        }
        
        _window.DoEvents();
        
        _gl.ClearColor(0.25f, 0.26f, 0.27f, 1f);
        _gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        RenderSubtree(this);
        
        _window.SwapBuffers();
    }

    private void RenderSubtree(INode root) {
        foreach (INode node in root.GetTreeEnumerator()) {
            if (node is IRenderable2D renderable) {
                renderable.Render(_ctx);
            }
        }
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