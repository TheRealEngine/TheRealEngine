using GlmSharp;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering;

public interface IWindowBackend {
    // setter is internal (but can't be because external lib)
    WindowNode Window { get; set; }
    
    // rendering
    void Render(INode node);
    
    // input
    bool IsButtonPressed(KeyboardButton button);
    bool IsButtonJustPressedThisUpdate(KeyboardButton button);
    bool IsButtonJustPressedThisTick(KeyboardButton button);
    dvec2 GetMousePosition();
    
    void Init() { }
    void Stop() { }
    
    void Tick(double dt) { }
    void Update(double dt) { }
}
