using GlmSharp;
using TheRealEngine.UniversalRendering;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Nodes.Console;

namespace TestGame;

public class Snek : ConsoleCharacterNode {
    private const double Speed = 1.0;  // Characters per second

    private int _y;
    
    public override void Tick(double delta) {
        Transform.Position = new dvec2(Transform.Position.x + Speed * delta, _y);

        if (this.GetWindow()!.IsButtonJustPressed(KeyboardButton.S)) {
            _y += 1;
        }
        if (this.GetWindow()!.IsButtonJustPressed(KeyboardButton.W)) {
            _y -= 1;
        }
    }
}
