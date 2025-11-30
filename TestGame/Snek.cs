using GlmSharp;
using TheRealEngine.UniversalRendering.Nodes.Console;

namespace TestGame;

public class Snek : ConsoleCharacterNode {
    private const double Speed = 1.0;  // Characters per second
    
    public override void Tick(double delta) {
        Transform.Position = new dvec2(Transform.Position.x + Speed * delta, 5);
    }
}
