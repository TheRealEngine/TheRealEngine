namespace TheRealEngine.Nodes;

public interface INode {
    /// <summary>
    /// Readonly array of child nodes.
    /// </summary>
    INode[] Children { get; }
    
    /// <summary>
    /// The parent node. Null if this node has no parent.
    /// </summary>
    INode? Parent { get; set; }
    
    /// <summary>
    /// The name of the node.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="delta">The time since the last frame.</param>
    void Update(double delta) { }
    
    /// <summary>
    /// Called every tick. What a tick is depends on the project's TPS setting.
    /// </summary>
    /// <param name="delta">The time since the last tick.</param>
    void Tick(double delta) { }
    
    /// <summary>
    /// Called when the node is added as a child to another node.
    /// </summary>
    void Ready() { }
    
    /// <summary>
    /// Called when the node is removed from its parent.
    /// </summary>
    void Leave() { }

    /// <summary>
    /// Make this node a parent of the given child node.
    /// </summary>
    /// <param name="child">The node to make a child of the current node.</param>
    /// <remarks>Implementations MUST call .Ready() on the child node.</remarks>
    void AddChild(INode child);
    
    /// <summary>
    /// Remove the given child node from this node.
    /// </summary>
    /// <param name="child">The child to remove from the current node.</param>
    /// <remarks>Implementations MUST call .Leave() on the child node.</remarks>
    void RemoveChild(INode child);
}

public static class NodeExtensions {
    
    /// <summary>
    /// Gets an enumerator that traverses the node tree in a depth-first manner.
    /// </summary>
    /// <param name="self">The node to start with.</param>
    /// <param name="includeSelf">Whether or not to include the current node.</param>
    /// <returns>An enumerator for the tree.</returns>
    public static IEnumerable<INode> GetTreeEnumerator(this INode self, bool includeSelf = true) {
        if (includeSelf) {
            yield return self;
        }
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
