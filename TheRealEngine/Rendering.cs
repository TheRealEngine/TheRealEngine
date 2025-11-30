using TheRealEngine.Nodes;

namespace TheRealEngine;

public static class Rendering {
    public static IRenderer Renderer { get; set; } = new ConsoleRenderer();

    public static void Render(Node node) {
        Renderer.Render(node);
    }
}
