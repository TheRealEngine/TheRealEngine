using System.Reflection;
using System.Runtime.Loader;

namespace TheRealEngine;

class PluginLoadContext : AssemblyLoadContext {
    private string pluginPath;
    public PluginLoadContext(string pluginPath) => this.pluginPath = pluginPath;
    
    protected override Assembly Load(AssemblyName name) {
        Assembly? mainAssembly = Assembly.GetEntryAssembly();
        if (name.Name == mainAssembly.GetName().Name) return mainAssembly;
        
        string depPath = Path.Combine(Path.GetDirectoryName(pluginPath), $"{name.Name}.dll");
        if (File.Exists(depPath)) return LoadFromAssemblyPath(depPath);
        
        return null;
    }
}