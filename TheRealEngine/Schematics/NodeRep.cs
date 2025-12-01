using System.Reflection;
using Newtonsoft.Json.Linq;
using TheRealEngine.Nodes;

namespace TheRealEngine.Schematics;

public class NodeRep {
    public string Name { get; set; }
    public string? Script { get; set; }
    public NodeRep[] Children { get; set; }
    public Dictionary<string, JToken> Params { get; set; } = [];

    public INode ToNode() {
        Type t = GetNodeType();

        // 1. Get the constructor parameters
        ConstructorInfo ctor = t.GetConstructors().First();
        ParameterInfo[] ctorParams = ctor.GetParameters();
        object[] args = new object[ctorParams.Length];

        // 2. Match JSON Params to Constructor Arguments
        HashSet<string> usedKeys = [];
        for (int i = 0; i < ctorParams.Length; i++) {
            ParameterInfo p = ctorParams[i];

            // Try find parameter in JSON (Case-insensitive to allow "Color" in JSON matching "color" in ctor)
            string? key = Params.Keys.FirstOrDefault(k => string.Equals(k, p.Name, StringComparison.OrdinalIgnoreCase));

            if (key != null) {
                JToken token = Params[key];

                // Handle "new::" syntax or standard conversion
                args[i] = ConvertToken(token, p.ParameterType);
                
                usedKeys.Add(key);
            }
            else if (p.HasDefaultValue) {
                args[i] = p.DefaultValue!;
            }
            else {
                throw new Exception($"Missing required parameter '{p.Name}' for node '{Name}' ({t.Name})");
            }
        }

        // 3. Create Instance with args
        INode node = (INode)Activator.CreateInstance(t, args)!;
        node.Name = Name;

        foreach (KeyValuePair<string, JToken> paramKp in Params) {
            if (usedKeys.Contains(paramKp.Key)) {
                continue;  // Ignore values used in constructor
            }
            
            PropertyInfo prop = t.GetProperty(paramKp.Key)!;
            object value = ConvertToken(paramKp.Value, prop.PropertyType);
            prop.SetValue(node, value);
        }

        // 4. Add Children
        foreach (NodeRep child in Children) {
            node.AddChild(child.ToNode());
        }

        return node;
    }

    private object ConvertToken(JToken token, Type targetType) {
        // Check for special string syntax if target isn't a string/char
        if (targetType != typeof(string) && targetType != typeof(char) && token.Type == JTokenType.String) {
            string refStr = token.ToObject<string>()!;
            if (refStr.Contains("::")) {
                string[] parts = refStr.Split("::");
                string type = parts[0];
                string value = parts[1];

                switch (type) {
                    case "new": {
                        Type refType = Game.GetType(value);
                        return Activator.CreateInstance(refType)!;
                    }
                    default:
                        throw new Exception($"Unknown reference type: {type}");
                }
            }
        }

        // Standard conversion
        return token.ToObject(targetType)!;
    }

    private Type GetNodeType() {
        if (Script == null) {
            return typeof(INode);
        }

        return Game.GetType(Script);
    }
}
