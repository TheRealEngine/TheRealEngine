using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Renderers;

namespace TheRealEngine.UniversalRendering.Nodes.Generic;

public class WindowNode : Node {
    public IRenderer Renderer { get; set; } = new DummyRenderer();

    public override void Update(double delta) {
        Renderer.Render(this);
    }
}
