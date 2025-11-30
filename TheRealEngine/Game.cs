using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using TheRealEngine.Nodes;
using TheRealEngine.Schematics;

namespace TheRealEngine;

public static class Game {
    public static ProjectRep Project { get; internal set; } = null!;
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
        Scene = sceneNode.ToNode();
        Root.Children.Add(Scene);
    }

    internal static void Init(ProjectRep project) {
        Project = project;
        
        ChangeScene(project.DefaultScene);
    }

    public static void Ticker() {
        Stopwatch sw = Stopwatch.StartNew();
        while (true) {
            Update(Root, sw.Elapsed.TotalSeconds);
            Tick(Root, sw.Elapsed.TotalSeconds);
            sw.Restart();
            Thread.Sleep(50);
        }
    }
    
    private static void Update(Node node, double delta) {
        Node[] copy = node.Children.ToArray();
        foreach (Node child in copy) {
            Update(child, delta);
        }
        node.Update(delta);
    }

    private static void Tick(Node node, double delta) {
        Node[] copy = node.Children.ToArray();
        foreach (Node child in copy) {
            Tick(child, delta);
        }
        node.Tick(delta);
    }
    
    public static Type GetType(string typeName) {
        Type? builtinType = Type.GetType(typeName);
        if (builtinType != null) {
            return builtinType;
        }

        // foreach (Assembly ass in ProjectAssemblies) {
        //     Type? assType = ass.GetType(typeName);
        //     if (assType != null) {
        //         return assType;
        //     }
        // }
        
        foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies()) {
            Type? assType = ass.GetType(typeName);
            if (assType != null) {
                return assType;
            }
        }
        
        throw new Exception("Type not found: " + typeName);
    }
}
