using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TheRealEngine.Nodes;
using TheRealEngine.Schematics;

namespace TheRealEngine;

public static class Game {
    private const string SceneFileExtension = "rscene";
    
    public static ProjectRep Project { get; internal set; } = null!;
    public static INode Scene { get; set; } = new NodeBase();
    public static INode Root { get; } = new NodeBase();
    public static double TimeBetweenTicks => 1d / Project.Tps;
    internal static CancellationTokenSource TickerTokenSource { get; } = new();

    static Game() {
        Root.AddChild(Scene);  // Make there always a scene loaded (though empty)
    }
    
    private static string GetScenePath(string sceneName) {
        string explicitPath = Path.Combine(Directory.GetCurrentDirectory(), sceneName);
        if (File.Exists(explicitPath)) {
            return explicitPath;
        }
        
        string implicitPath = Path.Combine(Directory.GetCurrentDirectory(), Project.MainScenesFolder, $"{sceneName}.{SceneFileExtension}");
        if (File.Exists(implicitPath)) {
            return implicitPath;
        }
        
        throw new Exception($"Scene not found: {sceneName}.{SceneFileExtension}");
    }
    
    public static INode LoadScene(string sceneName) {
        NodeRep sceneNode = JsonConvert.DeserializeObject<NodeRep>(File.ReadAllText(GetScenePath(sceneName)))!;
        return sceneNode.ToNode();
    }

    public static void ChangeScene(string sceneName) {
        Root.RemoveChild(Scene);
        Scene = LoadScene(sceneName);
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
        while (!TickerTokenSource.IsCancellationRequested) {
            // Tick if enough time has passed
            if (swt.Elapsed.TotalSeconds >= TimeBetweenTicks) {
                Engine.GetLogger("Ticker").LogDebug("Tick");
                swt.Restart();
                Tick(Root, TimeBetweenTicks);
            }
            
            Engine.GetLogger("Ticker").LogDebug("Update");
            double delta = swu.Elapsed.TotalSeconds;
            swu.Restart();
            Update(Root, delta);
        }
    }

    private static void Update(INode node, double delta) {
        node.CallOnTree(n => {
            Engine.GetLogger("Ticker").LogTrace("Updating child node {node}", n.Name);
            try {
                n.Update(delta);
                Engine.GetLogger("Ticker").LogTrace("Updated child node {node}", n.Name);
            }
            catch (Exception e) {
                Engine.GetLogger("Ticker").LogError(e, "Error during Update of node {node}", n.Name);
            }
        });
    }

    private static void Tick(INode node, double delta) {
        node.CallOnTree(n => {
            Engine.GetLogger("Ticker").LogTrace("Ticking child node {node}", n.Name);
            try {
                n.Tick(delta);
                Engine.GetLogger("Ticker").LogTrace("Ticked child node {node}", n.Name);
            }
            catch (Exception e) {
                Engine.GetLogger("Ticker").LogError(e, "Error during Tick of node {node}", n.Name);
            }
        });
    }

    public static Type GetType(string typeName) {
        Type? builtinType = Type.GetType(typeName);
        if (builtinType != null) {
            return builtinType;
        }

        foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies()) {
            Type? assType = ass.GetType(typeName);
            if (assType != null) {
                return assType;
            }
        }

        throw new Exception("Type not found: " + typeName);
    }
}
