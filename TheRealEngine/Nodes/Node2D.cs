using GlmSharp;
using TheRealEngine.Structs;

namespace TheRealEngine.Nodes;

public class Node2D : NodeBase {
    public Transform2D Transform;
    
    public dvec2 Position {
        get => Transform.Position;
        set => Transform.Position = value;
    }
    
    public dvec2 Scale {
        get => Transform.Scale;
        set => Transform.Scale = value;
    }
    
    public double Rotation {
        get => Transform.Rotation;
        set => Transform.Rotation = value;
    }
}
