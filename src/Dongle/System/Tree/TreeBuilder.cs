using System.Collections.Generic;

namespace Dongle.System.Tree
{
    /// <summary>
    /// Stateful helper class to build simple tree structures.
    /// Provides the following methods:
    /// <list>
    /// <item>Add: Adds one or more nodes at the current level</item>
    /// <item>AddWithChild: Adds a new node and goes down one level</item>
    /// <item>Down: goes down one level</item>
    /// <item>Up: goes up one level</item>
    /// <item>sets the current level to the childs of the root node</item>
    /// <item>ToTree: resets the tree builder and returns the tree that was built</item>
    /// </list>
    /// 
    /// The TreeBuilder always generates a root node, and starts with root.Nodes as
    /// current level.
    /// 
    /// </summary>
    public class TreeBuilder<T> where T : class
    {
        private TreeNode<T> _root;
        private TreeNode<T> _current;

        public TreeBuilder()
        {
            Reset();
        }

        public TreeBuilder(T rootValue)
        {
            Reset();
            SetRootValue(rootValue);
        }

        public void Reset()
        {
            _root = new TreeNode<T>(null);
            _current = _root;
        }

        public TreeNode<T> ToTree()
        {
            TreeNode<T> ret = _root;
            Reset();
            return ret;
        }

        public TreeBuilder<T> Add(string key, T value, bool replaceValueIfKeyExists)
        {
            var genericList = new List<TreeNode<T>>(_current.Nodes);
            var nodeToFind = genericList.Find(node => node.Key == key);
            if (nodeToFind == null)
            {
                _current.Nodes.Add(key, value);
            }
            else if (replaceValueIfKeyExists)
            {
                nodeToFind.Value = value;
            }
            return this;
        }

        public TreeBuilder<T> Down()
        {
            _current = _current.Nodes[_current.Nodes.Count - 1];
            return this;
        }

        public TreeBuilder<T> AddWithChild(string key, T value, bool replaceValueIfKeyExists)
        {
            Add(key, value, replaceValueIfKeyExists);
            Down();
            return this;
        }

        public TreeBuilder<T> Up()
        {
            _current = _current.Parent;
            return this;
        }

        public TreeBuilder<T> Root()
        {
            _current = _root;
            return this;
        }

        public TreeBuilder<T> SetRootValue(T value)
        {
            _root.Value = value;
            return this;
        }
    }
}
