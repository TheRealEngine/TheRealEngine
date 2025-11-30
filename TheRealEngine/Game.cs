using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using TheRealEngine.Nodes;
using TheRealEngine.Schematics;

namespace TheRealEngine;

public static class Game {
    public static ProjectRep Project { get; internal set; } = null!;
    public static Assembly ProjectAssembly { get; internal set; } = null!;
    public static Node Scene { get; set; } = new();
    public static Node Root { get; } = new() {
        Children = [Scene]
    };

    public static void ChangeScene(string sceneName) {
        Root.Children.Remove(Scene);
        
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Scenes", sceneName + ".json");
        if (!File.Exists(path)) {
            throw new Exception($"Scene not found: {sceneName}");
        }

        NodeRep sceneNode = JsonConvert.DeserializeObject<NodeRep>(File.ReadAllText(path))!;
        Scene = sceneNode.ToNode(ProjectAssembly);
        Root.Children.Add(Scene);
    }

    internal static void Init(ProjectRep project, Assembly assembly) {
        Project = project;
        ProjectAssembly = assembly;
        
        ChangeScene(project.DefaultScene);
    }

    public static void Ticker() {
        Stopwatch sw = Stopwatch.StartNew();
        while (true) {
            Tick(Root, sw.Elapsed.TotalSeconds);
            sw.Restart();
            Thread.Sleep(50);
            Rendering.Render(Root);
        }
    }
    
    private static void Tick(Node node, double delta) {
        Node[] copy = node.Children.ToArray();
        foreach (Node child in copy) {
            Tick(child, delta);
        }
        node.Update(delta);
    }
}
