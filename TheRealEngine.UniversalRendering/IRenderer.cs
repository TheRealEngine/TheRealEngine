using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering;

public interface IRenderer {
    WindowNode Window { get; internal set; }
    
    void Render(INode node);
    
    void Init() { }
}
