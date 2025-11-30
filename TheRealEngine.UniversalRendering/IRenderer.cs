using TheRealEngine.Nodes;

namespace TheRealEngine.UniversalRendering;

public interface IRenderer {
    void Render(INode node);
    
    void Init() { }
}
