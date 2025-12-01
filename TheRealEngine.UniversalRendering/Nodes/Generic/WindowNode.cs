using System.Drawing;
using Microsoft.Extensions.Logging;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Renderers;

namespace TheRealEngine.UniversalRendering.Nodes.Generic;

public class WindowNode : NodeBase {
    public IRenderer Renderer { get; set; } = new DummyRenderer();
    public int Width { get; set; } = 1920/2;
    public int Height { get; set; } = 1080/2;
    public string Title { get; set; }
    public Color BackgroundColour { get; set; } = Color.Transparent;

    public override void Ready() {
        Renderer.Window = this;
        Renderer.Init();
    }

    public override void Update(double delta) {
        Engine.GetLogger<WindowNode>().LogTrace("Rendering WindowNode {Title}", Title);
        Renderer.Render(this);
    }
}
