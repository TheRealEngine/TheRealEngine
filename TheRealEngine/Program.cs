using System.Runtime.Loader;
using Newtonsoft.Json;
using TheRealEngine.Schematics;

namespace TheRealEngine;

internal static class Program {
    
    public static void Main(string[] args) {
        string assembliesPath = Path.Combine(Directory.GetCurrentDirectory(), "Assemblies");
        
        if (Directory.Exists(assembliesPath)) {
            foreach (string dllPath in Directory.GetFiles(assembliesPath, "*.dll")) {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
            }
        }
        
        if (!File.Exists("project.json")) {
            throw new Exception("project.json file not found. Is this a project.");
        }
        
        string projectJson = File.ReadAllText("project.json");
        ProjectRep project = JsonConvert.DeserializeObject<ProjectRep>(projectJson)!;
        
        
        Game.Init(project);
        Game.Ticker();
    }
}