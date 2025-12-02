using GlmSharp;
using Microsoft.Extensions.Logging;
using TheRealEngine.Nodes;
using TheRealEngine.UniversalRendering.Input;
using TheRealEngine.UniversalRendering.Nodes.Console;
using TheRealEngine.UniversalRendering.Nodes.Generic;

namespace TheRealEngine.UniversalRendering.Console;

public class ConsoleWindowBackend : IWindowBackend {
    public WindowNode Window { get; set; }

    private HashSet<KeyboardButton> _newPresses = [];
    private HashSet<KeyboardButton> _pressedKeys = [];
    private readonly object _inputLock = new();
    private readonly UpdateToTickJustPressedHandler _tickInputHandler;

    public ConsoleWindowBackend() {
        // Start input polling in a background thread
        Thread inputThread = new(InputPollingLoop) {
            IsBackground = true
        };
        inputThread.Start();
        
        _tickInputHandler = new UpdateToTickJustPressedHandler(IsButtonJustPressedThisUpdate);
    }

    public void Init() {
        // Apply width and height to console window
        try {
            System.Console.SetWindowSize(Window.Width, Window.Height);
            System.Console.SetBufferSize(Window.Width, Window.Height);
        }
        catch (ArgumentOutOfRangeException) {
            // Ignore if the size is outside the console's capabilities
            Engine.GetLogger<ConsoleWindowBackend>().LogInformation("Selected console window size is out of range.");
        }
        catch (PlatformNotSupportedException) {
            // Ignore if not supported (e.g., on some OSes)
            Engine.GetLogger<ConsoleWindowBackend>().LogInformation("Console window resizing not supported on this platform.");
            
            Window.Height = System.Console.WindowHeight;
            Window.Width = System.Console.WindowWidth;
        }
        
        try {
            System.Console.SetBufferSize(Window.Width, Window.Height);
        }
        catch (PlatformNotSupportedException) {
            // Ignore if not supported (e.g., on some OSes)
            Engine.GetLogger<ConsoleWindowBackend>().LogInformation("Console window buffer resizing not supported on this platform.");
        }
    }

    private void InputPollingLoop() {
        while (true) {
            if (System.Console.KeyAvailable) {
                ConsoleKeyInfo keyInfo = System.Console.ReadKey(intercept: true);
                KeyboardButton btn = ConsoleKeyToKeyboardButton(keyInfo.Key);
                if (btn == KeyboardButton.None) {
                    continue;
                }

                lock (_inputLock) {
                    _newPresses.Add(btn);
                }
            }
            else {
                // Sleep a bit to avoid busy looping
                Thread.Sleep(10);
            }
        }
    }

    public void Update(double dt) {
        lock (_inputLock) {
            _pressedKeys = _newPresses;
            _newPresses = [];
        }
        
        _tickInputHandler.Update();
    }

    public void Tick(double dt) {
        _tickInputHandler.Tick();
    }

    public void Render(INode node) {
        foreach (INode n in node.GetTreeEnumerator()) {
            if (n is ConsoleCharacterNode charNode) {
                ivec2 sp = SnapPos(charNode);

                // Set cursor position
                try {
                    System.Console.SetCursorPosition(sp.x, sp.y);
                    System.Console.Write(charNode.Character);
                }
                catch (ArgumentOutOfRangeException) {
                    // Ignore if outside the buffer
                }
            }
            
            else if (n is TextNode textNode) {
                ivec2 sp = SnapPos(textNode);

                // Set cursor position
                try {
                    System.Console.SetCursorPosition(sp.x, sp.y);
                    System.Console.Write(textNode.Text);
                }
                catch (ArgumentOutOfRangeException) {
                    // Ignore if outside the buffer
                }
            }
        }

        // Move cursor to next line to avoid overwrite
        System.Console.SetCursorPosition(0, System.Console.CursorTop + 1);
    }

    /// <summary>
    /// Console does not support holding keys, so we treat any key press as a "just pressed" event.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool IsButtonPressed(KeyboardButton button) {
        return Window.IsButtonJustPressed(button);
    }

    public bool IsButtonJustPressedThisUpdate(KeyboardButton button) {
        return _pressedKeys.Contains(button);
    }

    public bool IsButtonJustPressedThisTick(KeyboardButton button) {
        return _tickInputHandler.IsButtonJustPressedThisTick(button);
    }

    public dvec2 GetMousePosition() {
        return dvec2.Zero;
    }

    private static ivec2 SnapPos(Node2D node) {
        return new ivec2(
            (int)Math.Round(node.Transform.Position.x), 
            (int)Math.Round(node.Transform.Position.y));
    }

    private static KeyboardButton ConsoleKeyToKeyboardButton(ConsoleKey key) {
        return key switch {
            ConsoleKey.None => KeyboardButton.None,
            // Letters
            ConsoleKey.A => KeyboardButton.A,
            ConsoleKey.B => KeyboardButton.B,
            ConsoleKey.C => KeyboardButton.C,
            ConsoleKey.D => KeyboardButton.D,
            ConsoleKey.E => KeyboardButton.E,
            ConsoleKey.F => KeyboardButton.F,
            ConsoleKey.G => KeyboardButton.G,
            ConsoleKey.H => KeyboardButton.H,
            ConsoleKey.I => KeyboardButton.I,
            ConsoleKey.J => KeyboardButton.J,
            ConsoleKey.K => KeyboardButton.K,
            ConsoleKey.L => KeyboardButton.L,
            ConsoleKey.M => KeyboardButton.M,
            ConsoleKey.N => KeyboardButton.N,
            ConsoleKey.O => KeyboardButton.O,
            ConsoleKey.P => KeyboardButton.P,
            ConsoleKey.Q => KeyboardButton.Q,
            ConsoleKey.R => KeyboardButton.R,
            ConsoleKey.S => KeyboardButton.S,
            ConsoleKey.T => KeyboardButton.T,
            ConsoleKey.U => KeyboardButton.U,
            ConsoleKey.V => KeyboardButton.V,
            ConsoleKey.W => KeyboardButton.W,
            ConsoleKey.X => KeyboardButton.X,
            ConsoleKey.Y => KeyboardButton.Y,
            ConsoleKey.Z => KeyboardButton.Z,
            // Top-row numbers
            ConsoleKey.D0 => KeyboardButton.D0,
            ConsoleKey.D1 => KeyboardButton.D1,
            ConsoleKey.D2 => KeyboardButton.D2,
            ConsoleKey.D3 => KeyboardButton.D3,
            ConsoleKey.D4 => KeyboardButton.D4,
            ConsoleKey.D5 => KeyboardButton.D5,
            ConsoleKey.D6 => KeyboardButton.D6,
            ConsoleKey.D7 => KeyboardButton.D7,
            ConsoleKey.D8 => KeyboardButton.D8,
            ConsoleKey.D9 => KeyboardButton.D9,
            // F keys
            ConsoleKey.F1 => KeyboardButton.F1,
            ConsoleKey.F2 => KeyboardButton.F2,
            ConsoleKey.F3 => KeyboardButton.F3,
            ConsoleKey.F4 => KeyboardButton.F4,
            ConsoleKey.F5 => KeyboardButton.F5,
            ConsoleKey.F6 => KeyboardButton.F6,
            ConsoleKey.F7 => KeyboardButton.F7,
            ConsoleKey.F8 => KeyboardButton.F8,
            ConsoleKey.F9 => KeyboardButton.F9,
            ConsoleKey.F10 => KeyboardButton.F10,
            ConsoleKey.F11 => KeyboardButton.F11,
            ConsoleKey.F12 => KeyboardButton.F12,
            // Control keys
            ConsoleKey.Escape => KeyboardButton.Escape,
            ConsoleKey.Tab => KeyboardButton.Tab,
            ConsoleKey.Spacebar => KeyboardButton.Space,
            ConsoleKey.Enter => KeyboardButton.Enter,
            ConsoleKey.Backspace => KeyboardButton.Backspace,
            // Editing/navigation
            ConsoleKey.Insert => KeyboardButton.Insert,
            ConsoleKey.Delete => KeyboardButton.Delete,
            ConsoleKey.Home => KeyboardButton.Home,
            ConsoleKey.End => KeyboardButton.End,
            ConsoleKey.PageUp => KeyboardButton.PageUp,
            ConsoleKey.PageDown => KeyboardButton.PageDown,
            ConsoleKey.LeftArrow => KeyboardButton.Left,
            ConsoleKey.RightArrow => KeyboardButton.Right,
            ConsoleKey.UpArrow => KeyboardButton.Up,
            ConsoleKey.DownArrow => KeyboardButton.Down,
            // NumPad
            ConsoleKey.NumPad0 => KeyboardButton.NumPad0,
            ConsoleKey.NumPad1 => KeyboardButton.NumPad1,
            ConsoleKey.NumPad2 => KeyboardButton.NumPad2,
            ConsoleKey.NumPad3 => KeyboardButton.NumPad3,
            ConsoleKey.NumPad4 => KeyboardButton.NumPad4,
            ConsoleKey.NumPad5 => KeyboardButton.NumPad5,
            ConsoleKey.NumPad6 => KeyboardButton.NumPad6,
            ConsoleKey.NumPad7 => KeyboardButton.NumPad7,
            ConsoleKey.NumPad8 => KeyboardButton.NumPad8,
            ConsoleKey.NumPad9 => KeyboardButton.NumPad9,
            // Print/Scroll/Pause
            ConsoleKey.PrintScreen => KeyboardButton.PrintScreen,
            ConsoleKey.Pause => KeyboardButton.Pause,
            // Windows/Application/Menu keys
            ConsoleKey.LeftWindows => KeyboardButton.Super,
            ConsoleKey.RightWindows => KeyboardButton.Super,
            ConsoleKey.Applications => KeyboardButton.Menu,
            _ => KeyboardButton.None
        };
    }
}
