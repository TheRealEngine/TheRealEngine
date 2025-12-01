namespace TheRealEngine.Nodes;

public interface INode {
    INode[] Children { get; }
    INode? Parent { get; set; }
    string Name { get; set; }

    void Update(double delta) { }
    void Tick(double delta) { }

    void AddChild(INode child);
    void RemoveChild(INode child);
}

public static class NodeExtensions {

    /// <summary>
    /// Gets all children and children of children etc...
    /// </summary>
    /// <returns></returns>
    public static INode[] GetAllChildren(this INode self) {
        List<INode> childs = [];
        foreach (INode child in self.Children) {
            childs.Add(child);
            childs.AddRange(child.GetAllChildren());
        }

        return childs.ToArray();
    }
    
    public static IEnumerable<INode> GetTreeEnumerator(this INode self) {
        yield return self;
        foreach (INode child in self.Children) {
            foreach (INode descendant in child.GetTreeEnumerator()) {
                yield return descendant;
            }
        }
    }

    public static void Reparent(this INode self, INode newParent) {
        self.Parent?.RemoveChild(self);
        newParent.AddChild(self);
    }
}
