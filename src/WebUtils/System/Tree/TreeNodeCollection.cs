using System;
using System.Collections;
using System.Collections.Generic;

namespace WebUtils.System.Tree
{
    /// <summary>
    /// Implements a collection of Tree Nodes (Node<T>)
    /// Implementation Note: The root of a data tree is always a Node<T>. You cannot
    /// create a standalone NodeList<T>.
    /// </summary>
    /// <typeparam name="T">typeof the data value of each node</typeparam>
    public class TreeNodeCollection<T> : CollectionBase, IEnumerable<TreeNode<T>>
    {
        #region CTORs

        public TreeNodeCollection(TreeNode<T> owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            Owner = owner;
        }
        #endregion

        #region Additional public interface

        /// <summary>
        /// The Node to which this collection belongs (this==Owner.Childs). 
        /// Never null.
        /// </summary>
        public TreeNode<T> Owner { get; private set; }

        #endregion // public interface

        #region Collection implementation (indexer, add, remove)

        public new IEnumerator<TreeNode<T>> GetEnumerator()
        {
            foreach (TreeNode<T> node in InnerList)
            {
                yield return node;
            }
        }

        public void Insert(int index, TreeNode<T> node)
        {
            List.Insert(index, node);
        }

        public bool Contains(TreeNode<T> node)
        {
            return List.Contains(node);
        }


        /// <summary>
        /// Indexer accessing the index'th Node.
        /// If the owning node belongs to a tree, Setting the node fires a NodeChanged event
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TreeNode<T> this[int index]
        {
            get { return ((TreeNode<T>)List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Appends a new node with the specified value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value for the new node</param>
        /// <returns>the node that was created</returns>
        public TreeNode<T> Add(string key, T value)
        {
            var n = new TreeNode<T>(key, value);
            List.Add(n);

            return n;
        }

        // required for XML Serializer, not to bad to have...
        public void Add(TreeNode<T> node)
        {
            List.Add(node);
        }

        /// <summary>
        /// Adds a new node with the given value at the specified index.
        /// </summary>
        /// <param name="index">Position where to insert the item.
        /// All values are accepted, if index is out of range, the new item is inserted as first or 
        /// last item</param>
        /// <param name="key"> </param>
        /// <param name="value">value for the new node</param>
        /// <returns></returns>
        public TreeNode<T> InsertAt(int index, string key, T value)
        {
            var n = new TreeNode<T>(key, value, Owner);

            // "tolerant insert"
            if (index < 0)
            {
                index = 0;
            }

            if (index >= Count)
            {
                index = Count;
                List.Add(n);
            }
            else
            {
                List.Insert(index, n);
            }
            return n;
        }

        /// <summary>
        /// Inserts a new node before the specified node.
        /// </summary>
        /// <param name="insertPos">Existing node in front of which the new node is inserted</param>
        /// <param name="key"> </param>
        /// <param name="value">value for the new node</param>
        /// <returns>The newly created node</returns>
        public TreeNode<T> InsertBefore(TreeNode<T> insertPos, string key, T value)
        {
            int index = IndexOf(insertPos);
            return InsertAt(index, key, value);
        }

        /// <summary>
        /// Inserts a new node after the specified node
        /// </summary>
        /// <param name="insertPos">Existing node after which the new node is inserted</param>
        /// <param name="key"> </param>
        /// <param name="value">value for the new node</param>
        /// <returns>The newly created node</returns>
        public TreeNode<T> InsertAfter(TreeNode<T> insertPos, string key, T value)
        {
            int index = IndexOf(insertPos) + 1;
            if (index == 0)
            {
                index = Count;
            }
            return InsertAt(index, key, value);
        }

        public int IndexOf(TreeNode<T> node)
        {
            return (List.IndexOf(node));
        }

        public void Remove(TreeNode<T> node)
        {
            int index = IndexOf(node);
            if (index < 0)
            {
                throw new ArgumentException("the node to remove is not a in this collection");
            }
            RemoveAt(index);
        }

        #endregion

        #region CollectionBase overrides (action handler)

        protected override void OnValidate(object value)
        {
            // Verify: value.Parent must be null or this.mOwner)
            base.OnValidate(value);
            TreeNode<T> parent = ((TreeNode<T>)value).Parent;
            if (parent != null && parent != Owner)
            {
                throw new ArgumentException("Cannot add a node referenced in another node collection");
            }
        }

        protected override void OnInsert(int index, Object value)
        {
            // set parent note to this.mOwner
            ((TreeNode<T>)value).InternalSetParent(Owner);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ((TreeNode<T>)value).InternalDetach();

            base.OnRemoveComplete(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                ((TreeNode<T>)oldValue).InternalDetach();
                ((TreeNode<T>)newValue).InternalSetParent(Owner);

            }
            base.OnSet(index, oldValue, newValue);
        }

        protected override void OnClear()
        {
            // set parent to null for all elements
            foreach (TreeNode<T> node in InnerList)
            {
                node.InternalDetach();
            }
            base.OnClear();
        }

        #endregion // CollectionBase overrides
    }
}
