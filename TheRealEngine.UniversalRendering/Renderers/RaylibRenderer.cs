using Raylib_cs;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Renderers;

public class RaylibRenderer : IRenderer {
    public WindowNode Window { get; set; }

    public void Init() {
        Raylib.InitWindow(Window.Width, Window.Height, Window.Title);
    }
    
    public void Render(INode root) {
        if (Raylib.WindowShouldClose()) {
            // close window
            Raylib.CloseWindow();
            Environment.Exit(0);
        }
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        
        foreach (INode node in root.GetTreeEnumerator()) {
            switch (node) {
                case SpriteNode sprite: {
                    Texture2D texture = Raylib.LoadTexture(sprite.TexturePath);
                    Raylib.DrawTexture(texture, (int)sprite.Transform.Position.x, (int)sprite.Transform.Position.y, Color.White);
                    break;
                }
            }
        }
        
        Raylib.EndDrawing();
    }
}
