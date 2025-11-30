using TheRealEngine.Nodes;

namespace TestGame;

public class Snek : ConsoleCharacterNode {
    public override void Update(double d) {
        Pos = (Pos.X + 0.01, 5);
    }
}
