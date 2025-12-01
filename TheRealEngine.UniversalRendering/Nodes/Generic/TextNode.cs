using System.Drawing;
using TheRealEngine.Nodes;

namespace TheRealEngine.UniversalRendering.Nodes.Generic;

public class TextNode : Node2D {
    public string Text { get; set; } = "Hello World!";
    public Color FontColour { get; set; } = Color.Black;
    public int FontSize { get; set; } = 16;
}
