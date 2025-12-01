using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TheRealEngine.Schematics;

public class ProjectRep {
    
    public string Name { get; set; }
    
    public string Version { get; set; }
    
    public string Description { get; set; }
    
    [JsonProperty("default_scene")]
    public string DefaultScene { get; set; }

    public int Tps { get; set; } = 60;
    
    [JsonProperty("log_level")]
    public string LogLevel { get; set; } = "Debug";
    
    [JsonProperty("console_logging")]
    public bool ConsoleLogging { get; set; } = true;
    
    [JsonProperty("log_file")]
    public string? LogFile { get; set; } = null;
    
    [JsonProperty("main_scenes_folder")]
    public string MainScenesFolder { get; set; } = "Scenes";
    
    public LogLevel GetLogLevel() {
        return LogLevel.ToLower() switch {
            "trace" => Microsoft.Extensions.Logging.LogLevel.Trace,
            "debug" => Microsoft.Extensions.Logging.LogLevel.Debug,
            "information" or "info" => Microsoft.Extensions.Logging.LogLevel.Information,
            "warning" or "warn" => Microsoft.Extensions.Logging.LogLevel.Warning,
            "error" or "err" => Microsoft.Extensions.Logging.LogLevel.Error,
            "critical" or "crit" => Microsoft.Extensions.Logging.LogLevel.Critical,
            _ => throw new Exception($"Unknown log level: {LogLevel}"),
        };
    }
}
