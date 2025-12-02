using GlmSharp;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Renderers;

public class DummyWindowBackend : IWindowBackend {
    public WindowNode Window { get; set; }
    
    public void Render(INode node) {
        
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
    
    public dvec2 GetMousePosition() {
        return dvec2.Zero;
    }
}
