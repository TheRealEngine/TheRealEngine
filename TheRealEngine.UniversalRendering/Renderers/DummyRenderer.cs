using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Renderers;

public class DummyRenderer : IRenderer {
    public WindowNode Window { get; set; }
    
    public void Render(INode node) {
        
    }
}
