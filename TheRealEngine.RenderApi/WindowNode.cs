using TheRealEngine.Nodes;

namespace TheRealEngine.RenderApi;

public class WindowNode : NodeBase {
    public WindowNode() {
        CustomWindow window = new CustomWindow();
        window.Initialize();
    }
}