using System.Numerics;
using KrysRendering.Ops;
using Raylib_cs;
using TheRealEngine.Nodes;
namespace KrysRendering.Shapes;

public class MouseCircle : NodeBase
{
    private readonly DrawCircleOp _drawCircleOp;

    public MouseCircle(float radius, string color)
    {
        _drawCircleOp = new DrawCircleOp(0, 0, radius, color);
        AddChild(_drawCircleOp);
    }

    public override void Update(double delta)
    {
        _drawCircleOp.CenterX = Raylib.GetMouseX();
        _drawCircleOp.CenterY = Raylib.GetMouseY();
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            this.AddChild(new FadingCircle(_drawCircleOp.CenterX, _drawCircleOp.CenterY, _drawCircleOp.Radius, "#FFFFFF"));
        }
    }

    public override void Tick(double delta)
    {
        var mouseDelta = Raylib.GetMouseDelta();
        _drawCircleOp.Radius = MathF.Sqrt(mouseDelta.Length() * (float) delta * 10000f);
    }
}
