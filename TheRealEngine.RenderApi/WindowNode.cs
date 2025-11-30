using TheRealEngine.Nodes;

namespace TheRealEngine.RenderApi;

public class WindowNode : Node {
    public WindowNode() {
        CustomWindow window = new CustomWindow();
        window.Initialize();
    }
}