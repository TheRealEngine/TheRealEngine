using Silk.NET.Core.Contexts;
using Silk.NET.Maths;

namespace TheRealEngine.RenderApi;

public class Window {
    private static INativeWindow window;
    
    private static void Main(string[] args) {
        // //Create a window.
        // var options = WindowOptions.Default;
        // options.Size = new Vector2D<int>(800, 600);
        // options.Title = "LearnOpenGL with Silk.NET";
        //
        // window = Window.Create(options);
        //
        // //Assign events.
        // window.Load += OnLoad;
        // window.Update += OnUpdate;
        // window.Render += OnRender;
        // window.FramebufferResize += OnFramebufferResize;
        //
        // //Run the window.
        // window.Run();
        //
        // // window.Run() is a BLOCKING method - this means that it will halt execution of any code in the current
        // // method until the window has finished running. Therefore, this dispose method will not be called until you
        // // close the window.
        // window.Dispose();
    }
}