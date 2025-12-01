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
    /// <remarks>Implementations MUST call .Ready() on the child node's tree.</remarks>
    void AddChild(INode child);
    
    /// <summary>
    /// Remove the given child node from this node.
    /// </summary>
    /// <param name="child">The child to remove from the current node.</param>
    /// <remarks>Implementations MUST call .Leave() on the child node's tree.</remarks>
    void RemoveChild(INode child);
}

public static class NodeExtensions {
    
    /// <summary>
    /// Gets an enumerator that traverses the node tree in a depth-first manner.
    /// </summary>
    /// <param name="self">The node to start with.</param>
    /// <param name="includeSelf">Whether to include the current node.</param>
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

    /// <summary>
    /// Reparents this node to a new parent.
    /// </summary>
    /// <param name="self">The node to reparent.</param>
    /// <param name="newParent">The new parent node.</param>
    /// <remarks>Leave() and Ready() will both be called.</remarks>
    public static void Reparent(this INode self, INode newParent) {
        self.Parent?.RemoveChild(self);
        newParent.AddChild(self);
    }
    
    /// <summary>
    /// Checks if this node is an ancestor of the given node.
    /// </summary>
    /// <param name="self">The node the check.</param>
    /// <param name="node">The potential descendant node.</param>
    /// <returns>True if the node is an ancestor of the given node, otherwise false.</returns>
    public static bool IsAncestorOf(this INode self, INode node) {
        while (node.Parent != null) {
            if (node.Parent == self) {
                return true;
            }
            node = node.Parent;
        }
        return false;
    }
    
    /// <summary>
    /// Checks if this node is a descendant of the given node.
    /// </summary>
    /// <param name="self">The node the check.</param>
    /// <param name="node">The potential ancestor node.</param>
    /// <returns>True if the node is a descendant of the given node, otherwise false.</returns>
    public static bool IsDescendantOf(this INode self, INode node) {
        return node.IsAncestorOf(self);
    }

    /// <summary>
    /// Checks if this node is in the main tree (i.e., is a descendant of Game.Root or is Game.Root).
    /// </summary>
    /// <param name="self">The node the check.</param>
    /// <returns>True if the node is a descendant of Game.Root or is Game.Root, otherwise false.</returns>
    public static bool IsInTree(this INode self) {
        if (self == Game.Root) {
            return true;
        }
        
        return self.IsDescendantOf(Game.Root);
    }
    
    /// <summary>
    /// Run the action on this node and all its children in the tree.
    /// </summary>
    /// <param name="self">The base node to execute the action on.</param>
    /// <param name="action">The action to run.</param>
    public static void CallOnTree(this INode self, Action<INode> action) {
        foreach (INode node in self.GetTreeEnumerator()) {
            action(node);
        }
    }
}
