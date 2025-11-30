using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using TheRealEngine.Nodes;
using TheRealEngine.Schematics;

namespace TheRealEngine;

public static class Game {
    public static ProjectRep Project { get; internal set; } = null!;
    public static NodeBase Scene { get; set; } = new();
    public static NodeBase Root { get; } = new();

    static Game() {
        Root.AddChild(Scene);
    }

    public static void ChangeScene(string sceneName) {
        Root.RemoveChild(Scene);
        
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Scenes", sceneName + ".json");
        if (!File.Exists(path)) {
            throw new Exception($"Scene not found: {sceneName}");
        }

        NodeRep sceneNode = JsonConvert.DeserializeObject<NodeRep>(File.ReadAllText(path))!;
        Scene = sceneNode.ToNode();
        Root.AddChild(Scene);
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
    
    private static void Update(INode node, double delta) {
        INode[] copy = node.Children;
        foreach (INode child in copy) {
            Update(child, delta);
        }
        node.Update(delta);
    }

    private static void Tick(INode node, double delta) {
        INode[] copy = node.Children;
        foreach (INode child in copy) {
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
