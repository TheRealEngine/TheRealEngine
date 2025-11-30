using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TheRealEngine.Nodes;

namespace TheRealEngine.Schematics;

public class NodeRep {
    public string Name { get; set; }
    public string? Script { get; set; }
    public NodeRep[] Children { get; set; }
    public Dictionary<string, JToken> Params { get; set; } = [];

    public Node ToNode() {
        Type t = GetNodeType();
        Node node = (Node)Activator.CreateInstance(t)!;
        node.Name = Name;

        foreach (KeyValuePair<string, JToken> paramKp in Params) {
            PropertyInfo prop = t.GetProperty(paramKp.Key)!;

            if (prop.PropertyType != typeof(string) && prop.PropertyType != typeof(char) && paramKp.Value.Type == JTokenType.String) {
                string refStr = paramKp.Value.ToObject<string>()!;
                string[] parts = refStr.Split("::");
                string type = parts[0];
                string value = parts[1];

                switch (type) {
                    case "new": {
                        Type refType = Game.GetType(value);
                        object refObj = Activator.CreateInstance(refType)!;
                        prop.SetValue(node, refObj);
                        break;
                    }
                    
                    default:
                        throw new Exception($"Unknown reference type: {type}");
                }
                
                continue;
            }
            
            prop.SetValue(node, paramKp.Value.ToObject(prop.PropertyType));
        }

        foreach (NodeRep child in Children) {
            node.Children.Add(child.ToNode());
        }

        return node;
    }

    private Type GetNodeType() {
        if (Script == null) {
            return typeof(Node);
        }

        return Game.GetType(Script);
    }
}
