using Raylib_cs;
using TheRealEngine.Nodes;
namespace KrysRendering;

public static class KrysUtils
{
    public static Color ParseHex(string hex)
    {
        hex = hex.Replace("#", "");
        if (hex.Length == 6) hex += "FF";
        uint v = Convert.ToUInt32(hex, 16);
        return new Color((byte)(v >> 24 & 0xFF), (byte)(v >> 16 & 0xFF), (byte)(v >> 8 & 0xFF), (byte)(v & 0xFF));
    }
}

public abstract record LeafNode () : INode
{
    public INode[] Children { get { return []; } }
    public INode? Parent { get; set; } = null;
    public string Name { get; set; } = "";

    public void AddChild(INode child) {
        throw new Exception("Leaf nodes cannot have children");
    }

    public void RemoveChild(INode child) {
        throw new Exception("Leaf nodes cannot have children");
    }

    public abstract void Update(double delta);
    public abstract void Tick(double delta);
}
