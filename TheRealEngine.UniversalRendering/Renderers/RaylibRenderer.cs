using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Raylib_cs;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Renderers;

public class RaylibRenderer : IRenderer {
    public WindowNode Window { get; set; }

    public void Init() {
        unsafe {
            delegate* unmanaged[Cdecl]<int, sbyte*, sbyte*, void> ptr = &MyLogCallback;
            Raylib.SetTraceLogCallback(ptr);
        }
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
}
