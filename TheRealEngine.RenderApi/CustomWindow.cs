using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace TheRealEngine.RenderApi;

public class CustomWindow {
    private IWindow _window = null!;
    private IInputContext _input = null!;

    public void Initialize() {
        WindowOptions options = WindowOptions.Default with {
            Size  = new Vector2D<int>(800, 600),
            Title = "Silk.NET Window"
        };

        _window = Window.Create(options);
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.FramebufferResize += OnFramebufferResize;
        _window.Closing += OnClosing;
        
        _window.Run();
    }

    private void OnLoad() {
        _input = _window.CreateInput();

        foreach (IKeyboard keyboard in _input.Keyboards) {
            keyboard.KeyDown += OnKeyDown;
        }

        // TODO: create GL context here with Silk.NET.OpenGL if you want to render
        // e.g. var gl = GL.GetApi(_window);
    }

    private void OnUpdate(double deltaTime) {
        // Game / app logic goes here
    }

    private void OnRender(double deltaTime) {
        // All rendering goes here
        // If using GL: clear, draw, etc.
    }

    private void OnFramebufferResize(Vector2D<int> newSize) {
        // Update viewport / projection if using OpenGL
        // e.g. gl.Viewport(0, 0, (uint)newSize.X, (uint)newSize.Y);
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int code) {
        if (key == Key.Escape) {
            _window.Close();
        }
    }

    private void OnClosing() {
        // Clean up
        _input?.Dispose();
        _window?.Dispose();
    }
}