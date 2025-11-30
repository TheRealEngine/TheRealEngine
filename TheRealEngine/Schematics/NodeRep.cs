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

    public Node ToNode(Assembly assembly) {
        Type t = GetNodeType(assembly);
        Node node = (Node)Activator.CreateInstance(t)!;
        node.Name = Name;

        foreach (KeyValuePair<string, JToken> paramKp in Params) {
            PropertyInfo prop = t.GetProperty(paramKp.Key)!;
            prop.SetValue(node, paramKp.Value.ToObject(prop.PropertyType));
        }

        foreach (NodeRep child in Children) {
            node.Children.Add(child.ToNode(assembly));
        }

        return node;
    }

    private Type GetNodeType(Assembly assembly) {
        if (Script == null) {
            return typeof(Node);
        }
        
        return assembly.GetType(Script) ?? throw new Exception();
    }
}
