using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GlmSharp;
using Microsoft.Extensions.Logging;
using Raylib_cs;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Renderers;

public class RaylibRenderer : IRenderer {
    public WindowNode Window { get; set; }
    private readonly Dictionary<object, Texture2D> _loadedTextures = new();

    public void Init() {
        unsafe {
            delegate* unmanaged[Cdecl]<int, sbyte*, sbyte*, void> ptr = &MyLogCallback;
            Raylib.SetTraceLogCallback(ptr);
        }
        Raylib.InitWindow(Window.Width, Window.Height, Window.Title);
    }

    public void Stop() {
        foreach (Texture2D texture in _loadedTextures.Values) {
            Raylib.UnloadTexture(texture);
        }
        Raylib.CloseWindow();
    }

    public void Render(INode root) {
        if (Raylib.WindowShouldClose()) {
            // close window
            Raylib.CloseWindow();
            Environment.Exit(0);
        }
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(ToRaylibColor(Window.BackgroundColour));
        
        foreach (INode node in root.GetTreeEnumerator()) {
            switch (node) {
                case SpriteNode sprite: {
                    Texture2D texture = GetTexture(sprite);
                    
                    dvec2 position = sprite.Transform.Position;
                    Rectangle destRectangle = new(
                        (float)position.x,
                        (float)position.y,
                        (float)(texture.Width * sprite.Scale.x),
                        (float)(texture.Height * sprite.Scale.y)
                    );

                    Rectangle sourceRectangle = new(
                        0, 0,
                        texture.Width, texture.Height
                    );

                    Raylib.DrawTexturePro(
                        texture,
                        sourceRectangle,
                        destRectangle,
                        Vector2.Zero, 
                        (float)glm.Degrees(sprite.Rotation),
                        Color.White
                    );
                    break;
                }

                case TextNode text: {
                    Font font = Raylib.GetFontDefault();
                    
                    Raylib.DrawTextPro(font, text.Text, new Vector2((float)text.Position.x, (float)text.Position.y), 
                        Vector2.Zero, (float)glm.Degrees(text.Transform.Rotation), text.FontSize, text.FontSpacing, 
                        ToRaylibColor(text.FontColour));
                    break;
                }
            }
        }
        
        Raylib.EndDrawing();
    }

    private Texture2D GetTexture(SpriteNode sprite) {
        if (_loadedTextures.ContainsKey(sprite.GetHashCode())) {
            return _loadedTextures[sprite.GetHashCode()];
        }
        
        Texture2D texture = Raylib.LoadTexture(sprite.TexturePath);
        _loadedTextures[sprite.GetHashCode()] = texture;
        return texture;
    }
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe void MyLogCallback(int logLevel, sbyte* msg, sbyte* args) {
        TraceLogLevel level = (TraceLogLevel)logLevel;
        
        string text = Marshal.PtrToStringAnsi((IntPtr)msg)!;

        ILogger logger = Engine.GetLogger<RaylibRenderer>();
        
        switch (level) {
            case TraceLogLevel.All:
                logger.LogError(text);
                break;
            
            case TraceLogLevel.Info:  // Info is a bit spammy
            case TraceLogLevel.Trace:
                logger.LogTrace(text);
                break;
            
            case TraceLogLevel.Debug:
                logger.LogDebug(text);
                break;
            
            case TraceLogLevel.Warning:
                logger.LogWarning(text);
                break;
            
            case TraceLogLevel.Error:
                logger.LogError(text);
                break;
            
            case TraceLogLevel.Fatal:
                logger.LogCritical(text);
                break;
            
            case TraceLogLevel.None:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private static Color ToRaylibColor(System.Drawing.Color color) {
        return new Color(color.R, color.G, color.B, color.A);
    }
}
