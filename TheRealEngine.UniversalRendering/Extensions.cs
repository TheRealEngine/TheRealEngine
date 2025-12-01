using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering;

public static class Extensions {

    public static WindowNode? GetWindow(this INode node) {
        INode current = node;
        while (true) {
            if (current is WindowNode windowNode) {
                return windowNode;
            }
            
            current = current.Parent;
            if (current == null) {
                return null;
            }
        }
    }
}
