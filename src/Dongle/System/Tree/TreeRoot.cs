namespace Dongle.System.Tree
{
    /// <summary>
    /// A TreeRoot object acts as source of tree node events. A single instance is associated
    /// with each tree (the Root property of all nodes of a tree return the same instance, nodes
    /// from different trees return different instances)
    /// </summary>
    /// <typeparam name="T">type of the data value at each tree node</typeparam>
    public class TreeRoot<T>
    {
        public TreeRoot(TreeNode<T> root)
        {
            RootNode = root;
        }

        public TreeNode<T> RootNode { get; private set; }
    }
}