using KrysRendering.Ops;
using Raylib_cs;
using TheRealEngine.Nodes;
namespace KrysRendering.Shapes;

public class FadingCircle : NodeBase
{
    private readonly DrawCircleOp _drawCircleOp;

    public FadingCircle(int posX, int posY, float radius, string color)
    {
        _drawCircleOp = new DrawCircleOp(posX, posY, radius, color);
        AddChild(_drawCircleOp);
    }

    public override void Update(double delta)
    {
        // Fade out the circle over time
        _drawCircleOp.Radius *= 1f - 0.4f * (float) Math.Sqrt(delta);

        if (_drawCircleOp.Radius < 1f)
        {
            Parent?.RemoveChild(this);
        }
    }
}
