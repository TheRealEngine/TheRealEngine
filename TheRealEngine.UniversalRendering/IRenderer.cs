using TheRealEngine.Nodes;

namespace TheRealEngine.UniversalRendering;

public interface IRenderer {
    void Render(Node node);
    
    void Init() { }
}
