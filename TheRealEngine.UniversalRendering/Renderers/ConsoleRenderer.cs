using GlmSharp;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Console;

namespace TheRealEngine.UniversalRendering.Renderers;

public class ConsoleRenderer : IRenderer {
    
    public void Render(Node node) {
        // Get self and all children
        List<Node> allNodes = [ node ];
        allNodes.AddRange(node.GetAllChildren());

        foreach (Node n in allNodes) {
            if (n is not ConsoleCharacterNode charNode) {
                continue;
            }
            
            ivec2 sp = charNode.SnappedPos;

            // Set cursor position (beware: validate console bounds in production)
            try {
                Console.SetCursorPosition(sp.x, sp.y);
                Console.Write(charNode.Character);
            }
            catch (ArgumentOutOfRangeException) {
                // Ignore or handle if the position is outside the buffer
                Console.WriteLine("ERROR");
            }
        }

        // Move cursor to next line to avoid overwrite
        Console.SetCursorPosition(0, Console.CursorTop + 1);
    }
}