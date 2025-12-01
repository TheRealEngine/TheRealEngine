using GlmSharp;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Console;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Renderers;

public class ConsoleRenderer : IRenderer {
    public WindowNode Window { get; set; }

    public void Render(INode node) {
        
        foreach (INode n in node.GetTreeEnumerator()) {
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