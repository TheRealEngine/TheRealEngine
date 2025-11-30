using Raylib_cs;
using TheRealEngine.Nodes;
namespace KrysRendering;

public class KrysWindow : NodeBase
{
    public KrysWindow(int width, int height, string title)
    {
        Raylib.InitWindow(width, height, title);
    }

    public override void Update(double delta)
    {
        if (Raylib.WindowShouldClose())
        {
            // close window
            Raylib.CloseWindow();
            Environment.Exit(0);
        }

        Raylib.BeginDrawing();
        base.Update(delta);
        Raylib.EndDrawing();
    }
}
