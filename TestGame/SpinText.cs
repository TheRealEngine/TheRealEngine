using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TestGame;

public class SpinText : TextNode {
    private const double RotationSpeed = 1.0; // Rotations per second
    
    public override void Tick(double delta) {
        Rotation += RotationSpeed * delta * 2.0 * Math.PI;
    }
}
