using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TheRealEngine.Nodes;
using TheRealEngine.Schematics;

namespace TheRealEngine;

public static class Game {
    public static ProjectRep Project { get; internal set; } = null!;
    public static INode Scene { get; set; } = new NodeBase();
    public static INode Root { get; } = new NodeBase();

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
        Engine.GetLogger("Ticker").LogDebug("Ticker entrypoint");
        Stopwatch swu = Stopwatch.StartNew();
        Stopwatch swt = Stopwatch.StartNew();
        while (true) {
            Engine.GetLogger("Ticker").LogDebug("Update");
            Update(Root, swu.Elapsed.TotalSeconds);
            swu.Restart();
            
            if (swt.Elapsed.TotalMilliseconds > 1000f / 180f) {
                Engine.GetLogger("Ticker").LogDebug("Tick");
                Tick(Root, swt.Elapsed.TotalSeconds);
                swt.Restart();
            }
        }
    }

    private static void Update(INode node, double delta) {
        foreach (INode child in node.Children) {
            Update(child, delta);
        }
        
        Engine.GetLogger("Ticker").LogTrace("Updating child node {node}", node.Name);
        try {
            node.Update(delta);
            Engine.GetLogger("Ticker").LogTrace("Updated child node {node}", node.Name);
        }
        catch (Exception e) {
            Engine.GetLogger("Ticker").LogError(e, "Error during Update of node {node}", node.Name);
        }
    }

    private static void Tick(INode node, double delta) {
        foreach (INode child in node.Children) {
            Tick(child, delta);
        }
        
        Engine.GetLogger("Ticker").LogTrace("Ticking child node {node}", node.Name);
        try {
            node.Tick(delta);
            Engine.GetLogger("Ticker").LogTrace("Ticked child node {node}", node.Name);
        }
        catch (Exception e) {
            Engine.GetLogger("Ticker").LogError(e, "Error during Tick of node {node}", node.Name);
        }
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
