using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace WebUtils.System.Tree
{
    public class TreeNode<T>
    {
        private TreeNode<T> _parent;
        private TreeNodeCollection<T> _nodes;

        public string Key { get; private set; }

        public T Value { get; set; }

        #region CTORs

        public TreeNode(string key)
        {
            Key = key;
            Value = default(T);
            Root = new TreeRoot<T>(this);
        }

        public TreeNode(string key, T value)
        {
            Key = key;
            Value = value;
            Root = new TreeRoot<T>(this);
        }

        public TreeNode(string key, T value, TreeNode<T> parent)
        {
            Key = key;
            Value = value;
            InternalSetParent(parent);
        }
        #endregion

        #region Navigation

        [ScriptIgnore, XmlIgnore]
        public TreeNode<T> Parent { get { return _parent; } }

        /// <summary>
        /// returns all siblings as a NodeList<T>. If this is a root node, the function returns null.
        /// </summary>
        [ScriptIgnore, XmlIgnore]
        public TreeNodeCollection<T> Siblings
        {
            get { return _parent != null ? _parent.Nodes : null; }
        }

        [XmlArrayItem("node")]
        public TreeNodeCollection<T> Nodes
        {
            get { return _nodes ?? (_nodes = new TreeNodeCollection<T>(this)); }
        }

        /// <summary>
        /// The Root object this Node belongs to. never null
        /// </summary>
        [ScriptIgnore, XmlIgnore]
        public TreeRoot<T> Root { get; private set; }

        public void SetRootLink(TreeRoot<T> root)
        {
            if (Root != root) // assume sub trees are consistent
            {
                Root = root;
                if (HasChildren)
                {
                    foreach (var n in Nodes)
                    {
                        n.SetRootLink(root);
                    }
                }
            }
        }

        [ScriptIgnore, XmlIgnore]
        public bool HasChildren { get { return _nodes != null && _nodes.Count != 0; } }

        [ScriptIgnore, XmlIgnore]
        public bool IsRoot { get { return _parent == null; } }

        public bool IsAncestorOf(TreeNode<T> node)
        {
            if (node.Root != Root)
            {
                return false; // different trees
            }
            TreeNode<T> parent = node.Parent;
            while (parent != null && parent != this)
            {
                parent = parent.Parent;
            }
            return parent != null;
        }

        public bool IsChildOf(TreeNode<T> node)
        {
            return !IsAncestorOf(node);
        }

        [ScriptIgnore, XmlIgnore]
        public int Depth
        {
            get
            {
                int depth = 0;
                TreeNode<T> node = _parent;
                while (node != null)
                {
                    ++depth;
                    node = node._parent;
                }
                return depth;
            }
        }

        #endregion // Navigation

        #region Node Path

        public IList GetNodePath()
        {
            var list = new List<TreeNode<T>>();
            TreeNode<T> node = _parent;

            while (node != null)
            {
                list.Add(node);
                node = node.Parent;
            }
            list.Reverse();
            list.Add(this);

            return list;
        }

        public string GetNodePathAsString(char separator)
        {
            string s = "";
            TreeNode<T> node = this;

            while (node != null)
            {
                if (s.Length != 0)
                {
                    s = node.Key + separator + s;
                }
                else
                {
                    s = node.Key;
                }
                node = node.Parent;
            }
            return s;
        }

        #endregion // Node Path

        #region Modify
        /// <summary>
        /// Removes the current node and all child nodes recursively from it's parent.
        /// Throws an InvalidOperationException if this is a root node.
        /// </summary>
        public void Remove()
        {
            if (_parent == null)
            {
                throw new InvalidOperationException("cannot remove root node");
            }
            Detach();
        }

        /// <summary>
        /// Detaches this node from it's parent. 
        /// Postcondition: this is a root node.
        /// </summary>
        /// <returns> </returns>
        public TreeNode<T> Detach()
        {
            if (_parent != null)
            {
                Siblings.Remove(this);
            }
            return this;
        }
        #endregion // Modify

        #region Internal Helper

        internal void InternalDetach()
        {
            _parent = null;
            SetRootLink(new TreeRoot<T>(this));
        }

        internal void InternalSetParent(TreeNode<T> parent)
        {
            _parent = parent;
            if (_parent != null)
            {
                SetRootLink(parent.Root);
            }
        }

        #endregion // Internal Helper
    }
}