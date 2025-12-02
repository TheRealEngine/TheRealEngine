using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GlmSharp;
using Microsoft.Extensions.Logging;
using Raylib_cs;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Raylib;

public class RaylibWindowBackend : IWindowBackend {
    public WindowNode Window { get; set; }
    
    private readonly Dictionary<object, Texture2D> _loadedTextures = new();
    private readonly UpdateToTickJustPressedHandler _tickInputHandler;

    public RaylibWindowBackend() {
        _tickInputHandler = new UpdateToTickJustPressedHandler(IsButtonJustPressedThisUpdate);
    }
    
    public void Init() {
        unsafe {
            delegate* unmanaged[Cdecl]<int, sbyte*, sbyte*, void> ptr = &MyLogCallback;
            Raylib_cs.Raylib.SetTraceLogCallback(ptr);
        }
        Raylib_cs.Raylib.InitWindow(Window.Width, Window.Height, Window.Title);
    }

    public void Stop() {
        foreach (Texture2D texture in _loadedTextures.Values) {
            Raylib_cs.Raylib.UnloadTexture(texture);
        }
        Raylib_cs.Raylib.CloseWindow();
    }

    public void Update(double dt) {
        // Input handling, check each key if just pressed
        _tickInputHandler.Update();
    }

    public void Tick(double dt) {
        _tickInputHandler.Tick();
    }

    public void Render(INode root) {
        if (Raylib_cs.Raylib.WindowShouldClose()) {
            // close window
            Raylib_cs.Raylib.CloseWindow();
            Environment.Exit(0);
        }
        
        Raylib_cs.Raylib.BeginDrawing();
        Raylib_cs.Raylib.ClearBackground(ToRaylibColor(Window.BackgroundColour));
        
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

                    Raylib_cs.Raylib.DrawTexturePro(
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
                    Font font = Raylib_cs.Raylib.GetFontDefault();
                    
                    Raylib_cs.Raylib.DrawTextPro(font, text.Text, new Vector2((float)text.Position.x, (float)text.Position.y), 
                        Vector2.Zero, (float)glm.Degrees(text.Transform.Rotation), text.FontSize, text.FontSpacing, 
                        ToRaylibColor(text.FontColour));
                    break;
                }
            }
        }
        
        Raylib_cs.Raylib.EndDrawing();
    }

    public bool IsButtonPressed(KeyboardButton button) {
        return Raylib_cs.Raylib.IsKeyDown(KeyboardButtonToRaylibKey(button));
    }

    public bool IsButtonJustPressedThisUpdate(KeyboardButton button) {
        return Raylib_cs.Raylib.IsKeyPressed(KeyboardButtonToRaylibKey(button));
    }

    public bool IsButtonJustPressedThisTick(KeyboardButton button) {
        return _tickInputHandler.IsButtonJustPressedThisTick(button);
    }

    public dvec2 GetMousePosition() {
        Vector2 pos = Raylib_cs.Raylib.GetMousePosition();
        return new dvec2(pos.X, pos.Y);
    }

    private Texture2D GetTexture(SpriteNode sprite) {
        if (_loadedTextures.ContainsKey(sprite.GetHashCode())) {
            return _loadedTextures[sprite.GetHashCode()];
        }
        
        Texture2D texture = Raylib_cs.Raylib.LoadTexture(sprite.TexturePath);
        _loadedTextures[sprite.GetHashCode()] = texture;
        return texture;
    }
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe void MyLogCallback(int logLevel, sbyte* msg, sbyte* args) {
        TraceLogLevel level = (TraceLogLevel)logLevel;
        
        string text = Marshal.PtrToStringAnsi((IntPtr)msg)!;

        ILogger logger = Engine.GetLogger<RaylibWindowBackend>();
        
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
    
    // Converts your KeyboardButton to Raylib's KeyboardKey
    public static KeyboardKey KeyboardButtonToRaylibKey(KeyboardButton button) {
        switch (button) {
            case KeyboardButton.None: return KeyboardKey.Null;
            
            // Letters
            case KeyboardButton.A: return KeyboardKey.A;
            case KeyboardButton.B: return KeyboardKey.B;
            case KeyboardButton.C: return KeyboardKey.C;
            case KeyboardButton.D: return KeyboardKey.D;
            case KeyboardButton.E: return KeyboardKey.E;
            case KeyboardButton.F: return KeyboardKey.F;
            case KeyboardButton.G: return KeyboardKey.G;
            case KeyboardButton.H: return KeyboardKey.H;
            case KeyboardButton.I: return KeyboardKey.I;
            case KeyboardButton.J: return KeyboardKey.J;
            case KeyboardButton.K: return KeyboardKey.K;
            case KeyboardButton.L: return KeyboardKey.L;
            case KeyboardButton.M: return KeyboardKey.M;
            case KeyboardButton.N: return KeyboardKey.N;
            case KeyboardButton.O: return KeyboardKey.O;
            case KeyboardButton.P: return KeyboardKey.P;
            case KeyboardButton.Q: return KeyboardKey.Q;
            case KeyboardButton.R: return KeyboardKey.R;
            case KeyboardButton.S: return KeyboardKey.S;
            case KeyboardButton.T: return KeyboardKey.T;
            case KeyboardButton.U: return KeyboardKey.U;
            case KeyboardButton.V: return KeyboardKey.V;
            case KeyboardButton.W: return KeyboardKey.W;
            case KeyboardButton.X: return KeyboardKey.X;
            case KeyboardButton.Y: return KeyboardKey.Y;
            case KeyboardButton.Z: return KeyboardKey.Z;
            
            // Number row
            case KeyboardButton.D0: return KeyboardKey.Zero;
            case KeyboardButton.D1: return KeyboardKey.One;
            case KeyboardButton.D2: return KeyboardKey.Two;
            case KeyboardButton.D3: return KeyboardKey.Three;
            case KeyboardButton.D4: return KeyboardKey.Four;
            case KeyboardButton.D5: return KeyboardKey.Five;
            case KeyboardButton.D6: return KeyboardKey.Six;
            case KeyboardButton.D7: return KeyboardKey.Seven;
            case KeyboardButton.D8: return KeyboardKey.Eight;
            case KeyboardButton.D9: return KeyboardKey.Nine;
            
            // Function keys
            case KeyboardButton.F1: return KeyboardKey.F1;
            case KeyboardButton.F2: return KeyboardKey.F2;
            case KeyboardButton.F3: return KeyboardKey.F3;
            case KeyboardButton.F4: return KeyboardKey.F4;
            case KeyboardButton.F5: return KeyboardKey.F5;
            case KeyboardButton.F6: return KeyboardKey.F6;
            case KeyboardButton.F7: return KeyboardKey.F7;
            case KeyboardButton.F8: return KeyboardKey.F8;
            case KeyboardButton.F9: return KeyboardKey.F9;
            case KeyboardButton.F10: return KeyboardKey.F10;
            case KeyboardButton.F11: return KeyboardKey.F11;
            case KeyboardButton.F12: return KeyboardKey.F12;
            
            // Controls and special
            case KeyboardButton.Escape: return KeyboardKey.Escape;
            case KeyboardButton.Tab: return KeyboardKey.Tab;
            case KeyboardButton.CapsLock: return KeyboardKey.CapsLock;
            case KeyboardButton.Shift: return KeyboardKey.LeftShift; // Prefer left, or handle both if possible
            case KeyboardButton.Control: return KeyboardKey.LeftControl; // Prefer left
            case KeyboardButton.Alt: return KeyboardKey.LeftAlt; // Prefer left
            case KeyboardButton.Space: return KeyboardKey.Space;
            case KeyboardButton.Enter: return KeyboardKey.Enter;
            case KeyboardButton.Backspace: return KeyboardKey.Backspace;
            
            // Editing/navigation
            case KeyboardButton.Insert: return KeyboardKey.Insert;
            case KeyboardButton.Delete: return KeyboardKey.Delete;
            case KeyboardButton.Home: return KeyboardKey.Home;
            case KeyboardButton.End: return KeyboardKey.End;
            case KeyboardButton.PageUp: return KeyboardKey.PageUp;
            case KeyboardButton.PageDown: return KeyboardKey.PageDown;
            case KeyboardButton.Left: return KeyboardKey.Left;
            case KeyboardButton.Right: return KeyboardKey.Right;
            case KeyboardButton.Up: return KeyboardKey.Up;
            case KeyboardButton.Down: return KeyboardKey.Down;
            
            // Numpad
            case KeyboardButton.NumPad0: return KeyboardKey.Kp0;
            case KeyboardButton.NumPad1: return KeyboardKey.Kp1;
            case KeyboardButton.NumPad2: return KeyboardKey.Kp2;
            case KeyboardButton.NumPad3: return KeyboardKey.Kp3;
            case KeyboardButton.NumPad4: return KeyboardKey.Kp4;
            case KeyboardButton.NumPad5: return KeyboardKey.Kp5;
            case KeyboardButton.NumPad6: return KeyboardKey.Kp6;
            case KeyboardButton.NumPad7: return KeyboardKey.Kp7;
            case KeyboardButton.NumPad8: return KeyboardKey.Kp8;
            case KeyboardButton.NumPad9: return KeyboardKey.Kp9;
            case KeyboardButton.NumLock: return KeyboardKey.NumLock;
            
            // System
            case KeyboardButton.PrintScreen: return KeyboardKey.PrintScreen;
            case KeyboardButton.ScrollLock: return KeyboardKey.ScrollLock;
            case KeyboardButton.Pause: return KeyboardKey.Pause;

            // Sides
            case KeyboardButton.LeftShift: return KeyboardKey.LeftShift;
            case KeyboardButton.RightShift: return KeyboardKey.RightShift;
            case KeyboardButton.LeftControl: return KeyboardKey.LeftControl;
            case KeyboardButton.RightControl: return KeyboardKey.RightControl;
            case KeyboardButton.LeftAlt: return KeyboardKey.LeftAlt;
            case KeyboardButton.RightAlt: return KeyboardKey.RightAlt;
            case KeyboardButton.Menu: return KeyboardKey.KeyboardMenu;
            case KeyboardButton.Super: return KeyboardKey.LeftSuper;

            default: 
                return KeyboardKey.Null;
        }
    }
}
