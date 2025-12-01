using System.Runtime.Loader;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TheRealEngine.Schematics;

namespace TheRealEngine;

internal static class Program {
    
    public static int Main(string[] args) {
        // Load project first to get settings (like logging)
        string projectJson = File.ReadAllText("project.json");
        ProjectRep project = JsonConvert.DeserializeObject<ProjectRep>(projectJson)!;

        Engine.LoggerFactory = LoggerFactory.Create(builder => {
            if (project.ConsoleLogging) {
                builder.AddConsole();
            }

            if (project.LogFile != null) {
                // not supported yet
            }

            builder.SetMinimumLevel(project.GetLogLevel());
        });
        
        string assembliesPath = Path.Combine(Directory.GetCurrentDirectory(), "Assemblies");
        
        if (Directory.Exists(assembliesPath)) {
            Engine.GetEngineLogger().LogDebug("Loading assemblies from {path}", assembliesPath);
            foreach (string dllPath in Directory.GetFiles(assembliesPath, "*.dll")) {
                try {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                    Engine.GetEngineLogger().LogDebug("Loaded assembly: {dll}", dllPath);
                }
                catch (Exception) {
                    // Ignore (mostly native libs)
                }
            }
        }
        
        if (!File.Exists("project.json")) {
            throw new Exception("project.json file not found. Is this a project.");
        }
        
        Engine.GetEngineLogger().LogInformation("Starting project: {name} v{version}", project.Name, project.Version);
        Game.Init(project);
        
        Engine.GetEngineLogger().LogInformation("Entering main loop");
        Game.Ticker();

        return 0;
    }
}