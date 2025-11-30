using TheRealEngine.Nodes;

namespace TheRealEngine;

public class ConsoleRenderer : IRenderer {
    
    public void Render(Node node) {
        // Get self and all children
        List<Node> allNodes = [ node ];
        allNodes.AddRange(node.GetAllChildren());

        foreach (Node n in allNodes) {
            if (n is not ConsoleCharacterNode charNode) {
                continue;
            }
            
            var (x, y) = charNode.SnappedPos;

            // Set cursor position (beware: validate console bounds in production)
            try {
                Console.SetCursorPosition(x, y);
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