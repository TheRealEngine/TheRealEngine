using GlmSharp;
using TheRealEngine.Nodes;

namespace TheRealEngine.UniversalRendering.Nodes.Console;

public class ConsoleCharacterNode : Node2D {
    public ivec2 SnappedPos => new((int)Math.Round(Transform.Position.x), (int)Math.Round(Transform.Position.y));
    public char Character { get; set; }
}
