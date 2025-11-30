namespace TheRealEngine.Nodes;

public class ConsoleCharacterNode : Node2D {
    public (int X, int Y) SnappedPos => ((int)Math.Round(Pos.X), (int)Math.Round(Pos.Y));
    public char Character { get; set; }
}
