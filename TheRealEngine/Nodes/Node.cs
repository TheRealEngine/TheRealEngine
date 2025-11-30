namespace TheRealEngine.Nodes;

public class Node {
    public virtual List<Node> Children { get; init; } = [];
    public string Name { get; set; }
    
    public virtual void Update(double delta) {}

    /// <summary>
    /// Gets all children and children of children etc...
    /// </summary>
    /// <returns></returns>
    public Node[] GetAllChildren() {
        List<Node> childs = [];
        foreach (Node child in Children) {
            childs.Add(child);
            childs.AddRange(child.GetAllChildren());
        }

        return childs.ToArray();
    }
}
