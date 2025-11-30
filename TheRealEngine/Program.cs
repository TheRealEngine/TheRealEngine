using System.Reflection;
using Newtonsoft.Json;
using TheRealEngine.Schematics;

namespace TheRealEngine;

internal static class Program {
    
    public static void Main(string[] args) {
        PluginLoadContext context = new(Path.Combine(Directory.GetCurrentDirectory(), "TestGame.dll"));
        Assembly dll = context.LoadFromAssemblyPath(Path.Combine(Directory.GetCurrentDirectory(), "TestGame.dll"));
        
        if (!File.Exists("project.json")) {
            throw new Exception("project.json file not found. Is this a project.");
        }
        
        string projectJson = File.ReadAllText("project.json");
        ProjectRep project = JsonConvert.DeserializeObject<ProjectRep>(projectJson)!;
        
        Game.Init(project, dll);
        Game.Ticker();
    }
}