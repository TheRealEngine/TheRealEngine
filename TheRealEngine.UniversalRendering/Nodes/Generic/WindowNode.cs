using System.Drawing;
using GlmSharp;
using Microsoft.Extensions.Logging;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Renderers;

namespace TheRealEngine.UniversalRendering.Nodes.Generic;

public class WindowNode : NodeBase {
    public IWindowBackend WindowBackend { get; set; } = new DummyWindowBackend();
    public int Width { get; set; } = 1920/2;
    public int Height { get; set; } = 1080/2;
    public string Title { get; set; } = "Window";
    public Color BackgroundColour { get; set; } = Color.Transparent;
    public bool QuitOnClose { get; set; } = true;
    
    /// <summary>
    /// True if the window is currently executing a Tick().
    /// Otherwise, it is in Update().
    /// <p/>
    /// Useful for input handling to determine if a button was JUST pressed.
    /// </summary>
    public bool IsTicking { get; set; } = false;

    public override void Ready() {
        WindowBackend.Window = this;
        WindowBackend.Init();
    }

    public override void Leave() {
        WindowBackend.Stop();
    }

    public override void Update(double delta) {
        IsTicking = false;
        Engine.GetLogger<WindowNode>().LogTrace("Updating WindowNode {Title}", Title);
        WindowBackend.Update(delta);
        WindowBackend.Render(this);
    }

    public override void Tick(double delta) {
        IsTicking = true;
        WindowBackend.Tick(delta);
    }
    
    // Passthrough input methods
    
    public bool IsButtonJustPressed(KeyboardButton button) {
        return IsTicking 
            ? WindowBackend.IsButtonJustPressedThisTick(button) 
            : WindowBackend.IsButtonJustPressedThisUpdate(button);
    }

    public bool IsButtonPressed(KeyboardButton button) => WindowBackend.IsButtonPressed(button);
    public dvec2 GetMousePosition() => WindowBackend.GetMousePosition();
}
