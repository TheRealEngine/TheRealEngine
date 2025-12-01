namespace TheRealEngine.Nodes;

public class NodeBase : INode {
    private readonly List<INode> _children = [];
    public virtual INode[] Children => _children.ToArray();
    public INode? Parent { get; set; } = null;
    public string Name { get; set; } = $"Node-{Random.Shared.Next()}";
    
    public virtual void Update(double delta) { }
    public virtual void Tick(double delta) { }
    public virtual void Ready() { }
    public virtual void Leave() { }
    
    public void AddChild(INode child) {
        if (child.Parent != null) {
            throw new Exception("Child already has a parent");
        }
        
        _children.Add(child);
        child.Parent = this;

        if (this.IsInTree()) {
            child.CallOnTree(n => n.Ready());
        }
    }

    public void RemoveChild(INode child) {
        if (!_children.Remove(child)) {
            throw new Exception("Child not found");
        }
        
        child.Parent = null;
        child.CallOnTree(n => n.Leave());
    }
}
