using GlmSharp;
using TheRealEngine.Structs;

namespace TheRealEngine.Nodes;

public class Node3D : NodeBase {
    public Transform3D Transform;
    
    public dvec3 Position {
        get => Transform.Position;
        set => Transform.Position = value;
    }
    
    public dvec3 Scale {
        get => Transform.Scale;
        set => Transform.Scale = value;
    }
    
    public dvec2 Rotation {
        get => Transform.Rotation;
        set => Transform.Rotation = value;
    }
}
