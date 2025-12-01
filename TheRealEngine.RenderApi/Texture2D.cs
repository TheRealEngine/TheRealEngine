using System.Numerics;
using TheRealEngine.Nodes;

namespace TheRealEngine.RenderApi;

public class Texture2D : Node2D, IRenderable2D {
    public string Texture { get; set; } = string.Empty;
    
    private bool _textureLoaded;
    private TextureHandle _handle;

    public void Render(RenderContext2D ctx) {
        if (!_textureLoaded) {
            if (string.IsNullOrWhiteSpace(Texture)) {
                return;
            }

            _handle = ctx.LoadTextureFromFile(Texture);
            _textureLoaded = true;
        }

        if (_handle.Handle == 0)
            return;
        
        Matrix4x4 model =
            Matrix4x4.CreateScale((float)Transform.Scale.x, (float)Transform.Scale.y, 1f) *
            Matrix4x4.CreateRotationZ((float)Transform.Rotation) *
            Matrix4x4.CreateTranslation((float)Transform.Position.x, (float)Transform.Position.y, 0f);

        ctx.DrawTexturedQuad(_handle, model);
    }

    public override void Update(double delta) {
        // Anim/movement if you want
        base.Update(delta);
    }
}