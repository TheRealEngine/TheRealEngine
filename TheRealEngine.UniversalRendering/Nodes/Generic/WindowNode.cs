using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Renderers;

namespace TheRealEngine.UniversalRendering.Nodes.Generic;

public class WindowNode : NodeBase {
    public IRenderer Renderer { get; set; } = new DummyRenderer();
    public int Width { get; set; } = 1920/2;
    public int Height { get; set; } = 1080/2;
    public string Title { get; set; }

    public WindowNode() {
        
    }

    public override void Update(double delta) {
        Renderer.Render(this);
    }
}
