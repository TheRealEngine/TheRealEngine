using Newtonsoft.Json;

namespace TheRealEngine.Schematics;

public class ProjectRep {
    
    public string Name { get; set; }
    
    public string Version { get; set; }
    
    public string Description { get; set; }
    
    [JsonProperty("default_scene")]
    public string DefaultScene { get; set; }

    public int Tps { get; set; } = 60;
}
