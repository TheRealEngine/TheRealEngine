using System.Diagnostics;
using TheRealEngine;
using TheRealEngine.Nodes;

namespace TestGame;

public class PlayerScript : Node {
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    
    public override void Update(double d) {
        Console.WriteLine("Player update");
        if (_sw.Elapsed.Seconds > 3) {
            Game.ChangeScene("scene2");
        }
    }
}