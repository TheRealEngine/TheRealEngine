using TheRealEngine.Nodes;

namespace TheRealEngine;

public interface IRenderer {
    void Render(Node node);
    
    void Init() {
        
    }
}
