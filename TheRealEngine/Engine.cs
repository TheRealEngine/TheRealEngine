using Microsoft.Extensions.Logging;

namespace TheRealEngine;

public static class Engine {
    private const string DefaultCategory = "Generic";
    
    public static ILoggerFactory LoggerFactory { get; set; } = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Debug);
    });
    
    public static ILogger GetLogger(string category = DefaultCategory) {
        return LoggerFactory.CreateLogger(category);
    }

    public static ILogger GetLogger<T>() {
        return LoggerFactory.CreateLogger<T>();
    }
    
    internal static ILogger GetEngineLogger() {
        return GetLogger("Core");
    }
}
