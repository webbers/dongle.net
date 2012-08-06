using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using WebUtils.System.Tree;

namespace WebUtils.Tests.System.Tree
{

    [TestClass]
    public class TreeNodeTest
    {
        public void TreeNodeConstructorTestHelper<T>()
        {
            
        }

        [TestMethod]
        public void TreeNodeConstructorTest()
        {
            const string key = "key";
            Foo value = default(Foo);
            var rootNode = new TreeNode<Foo>("root");
            var root = new TreeRoot<Foo>(rootNode);
            var node = new TreeNode<Foo>(key, value, rootNode);
            Assert.AreSame(root.RootNode, node.Root.RootNode);
            Assert.IsTrue(node.Key == key);
            Assert.AreEqual(value, node.Value);
        }

        public void TreeRootConstructorTestHelper<T>()
        {
            var rootNode = new TreeNode<T>("root");
            var root = new TreeRoot<T>(rootNode);
            Assert.IsTrue(root.RootNode == rootNode);
            Assert.IsTrue(root.RootNode.IsRoot);
        }

        [TestMethod]
        public void TreeRootConstructorTest()
        {
            TreeRootConstructorTestHelper<GenericParameterHelper>();
        }

        public void TreeNodeConstructorTest1Helper<T>()
        {
            const string key = "key";
            var value = default(T);
            var node = new TreeNode<T>(key, value);
            Assert.IsTrue(node.Key == key);
            Assert.AreEqual(node.Value, value);
        }

        [TestMethod]
        public void TreeNodeConstructorTest1()
        {
            TreeNodeConstructorTest1Helper<GenericParameterHelper>();
        }

        public void TreeNodeConstructorTest2Helper<T>()
        {
            const string key = "key";
            var node = new TreeNode<T>(key);
            Assert.IsTrue(node.Key == key);
        }

        [TestMethod]
        public void TreeNodeConstructorTest2()
        {
            TreeNodeConstructorTest2Helper<GenericParameterHelper>();
        }

        public void DetachTestHelper<T>()
        {
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            var node = new TreeNode<T>("key");

            root.RootNode.Nodes.Add(node);

            Assert.AreEqual(1, root.RootNode.Nodes.Count);

            node.Detach();

            Assert.AreEqual(0, root.RootNode.Nodes.Count);
            Assert.IsTrue(node.IsRoot);
        }

        [TestMethod]
        public void DetachTest()
        {
            DetachTestHelper<GenericParameterHelper>();
        }

        public void GetNodePathTestHelper<T>()
        {
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            root.RootNode.Nodes.Add(node1);

            var nodeTest = new List<TreeNode<T>> { root.RootNode, node1, node2, node3 };
            for (int i = 0; i < nodeTest.Count; i++)
            {
                Assert.AreSame(nodeTest[i], node3.GetNodePath()[i]);
            }
        }

        [TestMethod]
        public void GetNodePathTest()
        {
            GetNodePathTestHelper<GenericParameterHelper>();
        }

        public void GetNodePathAsStringTestHelper<T>()
        {
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            root.RootNode.Nodes.Add(node1);

            const string nodePathAsString = @"root/key1/key2/key3";

            Assert.AreEqual(nodePathAsString, node3.GetNodePathAsString('/'));
        }

        [TestMethod]
        public void GetNodePathAsStringTest()
        {
            GetNodePathAsStringTestHelper<GenericParameterHelper>();
        }

        public void IsAncestorOfTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            Assert.IsTrue(node1.IsAncestorOf(node3));
            Assert.IsTrue(node1.IsAncestorOf(node4));
            Assert.IsTrue(node1.IsAncestorOf(node2));
        }

        [TestMethod]
        public void IsAncestorOfTest()
        {
            IsAncestorOfTestHelper<GenericParameterHelper>();
        }
        public void IsChildOfTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            Assert.IsTrue(node4.IsChildOf(node1));
            Assert.IsTrue(node3.IsChildOf(node1));
            Assert.IsTrue(node2.IsChildOf(node1));
        }

        [TestMethod]
        public void IsChildOfTest()
        {
            IsChildOfTestHelper<GenericParameterHelper>();
        }

        public void RemoveTestHelper<T>()
        {
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            var value = default(T);
            var node1 = new TreeNode<T>("key1", value, root.RootNode);
            var node2 = new TreeNode<T>("key2");
            node1.Nodes.Add(node2);
            node2.Remove();
            Assert.IsNull(node2.Parent);
        }

        [TestMethod]
        public void RemoveTest()
        {
            RemoveTestHelper<GenericParameterHelper>();
        }

        public void AssertCannotRemoveRootHelper<T>()
        {
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            root.RootNode.Remove();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssertCannotRemoveRoot()
        {
            AssertCannotRemoveRootHelper<GenericParameterHelper>();
        }

        public void SetRootLinkTestHelper<T>()
        {
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            node1.SetRootLink(root);

            Assert.AreSame(node1.Root, root);
            Assert.AreSame(node2.Root, root);
            Assert.AreSame(node3.Root, root);
            Assert.AreSame(node4.Root, root);
        }

        [TestMethod]
        public void SetRootLinkTest()
        {
            SetRootLinkTestHelper<GenericParameterHelper>();
        }

        public void DepthTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            Assert.AreEqual(0, node1.Depth);
            Assert.AreEqual(1, node2.Depth);
            Assert.AreEqual(2, node3.Depth);
            Assert.AreEqual(2, node4.Depth);
        }

        [TestMethod]
        public void DepthTest()
        {
            DepthTestHelper<GenericParameterHelper>();
        }

        public void HasChildrenTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            Assert.IsTrue(node1.HasChildren);
            Assert.IsTrue(node2.HasChildren);
            Assert.IsFalse(node3.HasChildren);
            Assert.IsFalse(node4.HasChildren);
        }

        [TestMethod]
        public void HasChildrenTest()
        {
            HasChildrenTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for IsRoot
        ///</summary>
        public void IsRootTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            Assert.IsTrue(node1.IsRoot);
            Assert.IsFalse(node2.IsRoot);
            Assert.IsFalse(node3.IsRoot);
            Assert.IsFalse(node4.IsRoot);
        }

        [TestMethod]
        public void IsRootTest()
        {
            IsRootTestHelper<GenericParameterHelper>();
        }

        public void NodesTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            var node1ChildNodes = new List<TreeNode<T>> { node2 };
            var node2ChildNodes = new List<TreeNode<T>> { node3, node4 };
            foreach (var node in node1ChildNodes)
            {
                Assert.IsTrue(node1.Nodes.Contains(node));
            }
            foreach (var node in node2ChildNodes)
            {
                Assert.IsTrue(node2.Nodes.Contains(node));
            }
        }

        [TestMethod]
        public void NodesTest()
        {
            NodesTestHelper<GenericParameterHelper>();
        }

        public void AssertNodesDontContainChildOfChildTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);
            var node1ChildNodes = new List<TreeNode<T>> { node2 };
            var node2ChildNodes = new List<TreeNode<T>> { node3, node4 };
            foreach (var node in node1ChildNodes)
            {
                Assert.IsFalse(node2.Nodes.Contains(node));
            }
            foreach (var node in node2ChildNodes)
            {
                Assert.IsFalse(node1.Nodes.Contains(node));
            }
        }

        [TestMethod]
        public void AssertNodesDontContainChildOfChildTest()
        {
            AssertNodesDontContainChildOfChildTestHelper<GenericParameterHelper>();
        }

        public void ParentTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);

            Assert.IsNull(node1.Parent);
            Assert.AreSame(node1, node2.Parent);
            Assert.AreSame(node2, node3.Parent);
            Assert.AreSame(node2, node3.Parent);
        }

        [TestMethod]
        public void ParentTest()
        {
            ParentTestHelper<GenericParameterHelper>();
        }

        public void RootTestHelper<T>()
        {
            var value = default(T);
            var root = new TreeRoot<T>(new TreeNode<T>("root"));
            var node1 = new TreeNode<T>("key1", value, root.RootNode);
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);

            Assert.AreSame(root.RootNode, node1.Root.RootNode);
            Assert.AreSame(root.RootNode, node2.Root.RootNode);
            Assert.AreSame(root.RootNode, node3.Root.RootNode);
            Assert.AreSame(root.RootNode, node4.Root.RootNode);
        }

        [TestMethod]
        public void RootTest()
        {
            RootTestHelper<GenericParameterHelper>();
        }

        public void SiblingsTestHelper<T>()
        {
            var node1 = new TreeNode<T>("key1");
            var node2 = new TreeNode<T>("key2");
            var node3 = new TreeNode<T>("key3");
            var node4 = new TreeNode<T>("key4");
            node2.Nodes.Add(node3);
            node2.Nodes.Add(node4);
            node1.Nodes.Add(node2);

            var node2Siblings = new List<TreeNode<T>> { node2 };
            var node3Siblings = new List<TreeNode<T>> { node3, node4 };

            Assert.IsNull(node1.Siblings);

            foreach (var node in node2Siblings)
            {
                Assert.IsTrue(node2.Siblings.Contains(node));
            }
            foreach (var node in node3Siblings)
            {
                Assert.IsTrue(node3.Siblings.Contains(node));
            }
        }

        [TestMethod]
        public void SiblingsTest()
        {
            SiblingsTestHelper<GenericParameterHelper>();
        }

        [TestMethod]
        public void ValueTest()
        {
            var stringList = new List<string> { "test1", "test2" };
            var node1 = new TreeNode<List<string>>("key1", stringList);
            var node2 = new TreeNode<int>("key2", 0);
            var node3 = new TreeNode<double>("key3", 5);
            var node4 = new TreeNode<bool>("key4", true);
            Assert.IsTrue(node1.Value.GetType() == typeof(List<string>));
            foreach (string s in stringList)
            {
                Assert.IsTrue(node1.Value.Contains(s));
            }
            Assert.AreEqual(node2.Value, 0);
            Assert.AreEqual(node3.Value, 5);
            Assert.AreEqual(node4.Value, true);
        }
    }

    [TestClass]
    public class TreeNodeCollectionTest
    {
        [TestMethod]
        public void TreeNodeCollectionConstructorTest()
        {
            var owner = new TreeNode<Foo>("owner");
            var nodeCollection = new TreeNodeCollection<Foo>(owner);

            Assert.AreSame(owner, nodeCollection.Owner);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssertTreeNodeCollectionHasOwner()
        {
            var nodeCollection = new TreeNodeCollection<Foo>(null);
        }

        [TestMethod]
        public void TestEnumerator()
        {
            var node1 = new TreeNode<Foo>("node1");
            var node2 = new TreeNode<Foo>("node2");
            var node3 = new TreeNode<Foo>("node3");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Add(node1);
            nodeCollection.Add(node2);
            nodeCollection.Add(node3);

            var nodeCollectionEnumerator = nodeCollection.GetEnumerator();
            Assert.IsNull(nodeCollectionEnumerator.Current);
            nodeCollectionEnumerator.MoveNext();
            Assert.AreSame(node1, nodeCollectionEnumerator.Current);
            nodeCollectionEnumerator.MoveNext();
            Assert.AreSame(node2, nodeCollectionEnumerator.Current);
            nodeCollectionEnumerator.MoveNext();
            Assert.AreSame(node3, nodeCollectionEnumerator.Current);
    
        }
        [TestMethod]
        public void TestContains()
        {
            var node1 = new TreeNode<Foo>("node1");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            Assert.IsFalse(nodeCollection.Contains(node1));
            nodeCollection.Add(node1);
            Assert.IsTrue(nodeCollection.Contains(node1));
        }
        [TestMethod]
        public void TestInsert()
        {
            var node1 = new TreeNode<Foo>("node1");
            var node2 = new TreeNode<Foo>("node2");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Add(node2);
            Assert.AreSame(node2, nodeCollection[0]);
            nodeCollection.Insert(0, node1);
            Assert.AreSame(node1, nodeCollection[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertInsertCantExceedMaximumCollectionSize()
        {
            var node1 = new TreeNode<Foo>("node1");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Insert(1, node1);
        }
        [TestMethod]
        public void AssertNodeCollectionItemCanBeChangedByIndex()
        {
            var node1 = new TreeNode<Foo>("node1");
            var node2 = new TreeNode<Foo>("node2");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Add(node1);
            Assert.AreSame(node1, nodeCollection[0]);
            nodeCollection[0] = node2;
            Assert.AreSame(node2, nodeCollection[0]);
        }
        [TestMethod]
        public void AssertNewNodeCanBeCreatedOnAdd()
        {
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            Assert.AreEqual(0, nodeCollection.Count);
            nodeCollection.Add("key1", null);
            Assert.AreEqual(1, nodeCollection.Count);
            Assert.AreEqual("key1", nodeCollection[0].Key);
        }
        [TestMethod]
        public void AssertNodeIsAddedByAdd()
        {
            var node1 = new TreeNode<Foo>("node1");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            Assert.AreEqual(0, nodeCollection.Count);
            nodeCollection.Add(node1);
            Assert.AreEqual(1, nodeCollection.Count);
        }

        [TestMethod]
        public void AssertNodeIsAddedWithGivenValueAtSpecifiedIndex()
        {
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            Assert.AreEqual(0, nodeCollection.Count);
            nodeCollection.InsertAt(0, "key1", null);
            Assert.AreEqual(1, nodeCollection.Count);
            nodeCollection.InsertAt(100, "key2", null);
            Assert.AreEqual("key2", nodeCollection[1].Key);
        }

        [TestMethod]
        public void AssertNodeIsInsertedBefore()
        {
            var node2 = new TreeNode<Foo>("node2");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Add(node2);
            nodeCollection.InsertBefore(node2, "key1", null);
            Assert.AreEqual("key1", nodeCollection[0].Key);
        }

        [TestMethod]
        public void AssertNodeIsInsertedAfter()
        {
            var node1 = new TreeNode<Foo>("node1");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Add(node1);
            nodeCollection.InsertAfter(node1, "key2", null);
            Assert.AreEqual("key2", nodeCollection[1].Key);
        }

        [TestMethod]
        public void TestIndexOf()
        {
            var node1 = new TreeNode<Foo>("node1");
            var node2 = new TreeNode<Foo>("node2");
            var nodeCollection = new TreeNodeCollection<Foo>(new TreeNode<Foo>("owner"));
            nodeCollection.Add(node1);
            nodeCollection.Add(node2);
            Assert.AreEqual(0, nodeCollection.IndexOf(node1));
            Assert.AreEqual(1, nodeCollection.IndexOf(node2));
        }
    }

    public class Foo
    {
    }

    [TestClass]
    public class TreeNodeTestClass
    {
        [TestMethod]
        public void TestTreeNodeConstructor()
        {
            var node1 = new TreeNode<Foo>("root");
            var root = new TreeRoot<Foo>(node1);
            Assert.AreSame(node1, root.RootNode);
        }
    }
}
