using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public partial class ArrangementTests
    {
        MockArrangement m_arrangement;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new MockArrangement();
        }

        private void AssertSequenceEqual(IEnumerable<IBoundedTreeNode> actual, IEnumerable<IBoundedTreeNode> expected)
        {
            Assert.IsTrue(Enumerable.SequenceEqual(actual, expected),
                $"expected: [\n    {expected.Select(node => node.ID.ToString()).Aggregate((a, b) => a + ",\n    " + b)}\n]\n" +
                $"actual: [\n    {actual.Select(node => node.ID.ToString()).Aggregate((a, b) => a + ",\n    " + b)}\n]\n");
        }

        [TestMethod]
        public void arrangement_is_not_a_leaf()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsFalse(m_arrangement.IsLeaf());
        }

        [TestMethod]
        public void arrangement_insert_as_first()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 5).Select(i => new MockTreeNode()).ToList();

            //Act
            expected.ForEach(child => m_arrangement.InsertAsFirst(child));
            expected.Reverse();

            //Assert
            AssertSequenceEqual(m_arrangement.MyChildren, expected);
        }

        [TestMethod]
        public void arrangement_insert_as_last()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 5).Select(i => new MockTreeNode()).ToList();

            //Act
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Assert
            AssertSequenceEqual(m_arrangement.MyChildren, expected);
        }


        [TestMethod]
        public void arrangement_children_can_be_removed()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 5).Select(i => new MockTreeNode()).ToList();
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Act
            m_arrangement.Remove(expected[2]);

            //Assert
            Assert.IsFalse(m_arrangement.MyChildren.Contains(expected[2]));

            expected.RemoveAt(2);
            AssertSequenceEqual(m_arrangement.MyChildren, expected);
        }


        [TestMethod]
        public void arrangement_can_swap_children()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 10).Select(i => new MockTreeNode()).ToList();
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Act
            m_arrangement.SwapChildren(expected[2].ID, expected[7].ID);
            {
                var temp = expected[7];
                expected[7] = expected[2];
                expected[2] = temp;
            }

            //Assert
            AssertSequenceEqual(m_arrangement.MyChildren, expected);

            //Act
            m_arrangement.SwapChildren(expected[5].ID, expected[1].ID);
            {
                var temp = expected[5];
                expected[5] = expected[1];
                expected[1] = temp;
            }

            //Assert
            AssertSequenceEqual(m_arrangement.MyChildren, expected);
        }


        [TestMethod]
        public void arrangement_can_remove_child_by_entity_id()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 10).Select(i => new MockTreeNode()).ToList();
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Act
            m_arrangement.RemoveChild(expected[5].ID);
            expected.RemoveAt(5);

            //Assert
            AssertSequenceEqual(m_arrangement.MyChildren, expected);
        }


        [TestMethod]
        public void arrangement_can_remove_window_and_return_object()
        {
            //Arrange
            List<IBoundedTreeNode> expected = Enumerable.Range(0, 10).Select(i => new MockTreeNode()).ToList<IBoundedTreeNode>();
            var window = new Window(new Bounds(1, 1, 1, 1), "test");

            expected.Insert(5, window);
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Act
            var removed = m_arrangement.RemoveAndReturnWindow(window.ID);
            expected.Remove(window);

            //Assert
            Assert.AreEqual(window, removed);
            AssertSequenceEqual(m_arrangement.MyChildren, expected);

            Assert.IsNull(m_arrangement.RemoveAndReturnWindow(expected.First().ID));
        }

        [TestMethod]
        public void arrangement_can_find_parent_of_child()
        {
            //Arrange
            var levelOneA = new MockArrangement(new Axis[] { });
            var levelOneB = new MockArrangement(new Axis[] { });
            m_arrangement.InsertAsLast(levelOneA);
            m_arrangement.InsertAsLast(levelOneB);

            var levelTwoA = new MockArrangement(new Axis[] { });
            var levelTwoB = new MockArrangement(new Axis[] { });
            levelOneA.InsertAsLast(levelTwoA);
            levelOneA.InsertAsLast(levelTwoB);

            var levelThree = new MockArrangement(new Axis[] { });
            levelTwoA.InsertAsLast(levelThree);

            Window[] windows = Enumerable.Range(0, 6).Select(i => new Window(new Bounds(1, 1, 1, 1), "test")).ToArray();

            m_arrangement.InsertAsLast(windows[0]);
            levelOneA.InsertAsLast(windows[1]);
            levelOneB.InsertAsLast(windows[2]);
            levelTwoA.InsertAsLast(windows[3]);
            levelTwoB.InsertAsLast(windows[4]);
            levelThree.InsertAsLast(windows[5]);

            //Act
            var parents = windows.Select(window => m_arrangement.FindParentOf(window.ID));
            var expected = new List<IBoundedTreeNode>() { m_arrangement, levelOneA, levelOneB, levelTwoA, levelTwoB, levelThree};

            //Assert
            AssertSequenceEqual(parents, expected);

            Assert.IsNull(m_arrangement.FindParentOf(new MockTreeLeaf().ID));
        }

        [TestMethod]
        public void arrangement_can_be_wrapped()
        {
            //Arrange
            var levelOneA = new MockArrangement();
            var levelOneB = new MockArrangement();
            m_arrangement.InsertAsLast(levelOneA);
            m_arrangement.InsertAsLast(levelOneB);

            var levelTwoA = new MockArrangement();
            var levelTwoB = new MockArrangement();
            levelOneA.InsertAsLast(levelTwoA);
            levelOneA.InsertAsLast(levelTwoB);

            var levelThree = new MockArrangement();
            levelTwoA.InsertAsLast(levelThree);

            Window[] windows = Enumerable.Range(0, 6).Select(i => new Window(new Bounds(1, 1, 1, 1), "test")).ToArray();

            m_arrangement.InsertAsLast(windows[0]);
            levelOneA.InsertAsLast(windows[1]);
            levelOneB.InsertAsLast(windows[2]);
            levelTwoA.InsertAsLast(windows[3]);
            levelTwoB.InsertAsLast(windows[4]);
            levelThree.InsertAsLast(windows[5]);

            //Act
            var wrapper = new MockArrangement();
            m_arrangement.WrapChildWithNode(levelOneA.ID, wrapper);

            //Assert
            Assert.IsTrue(m_arrangement.MyChildren.Contains(wrapper));
            Assert.IsFalse(m_arrangement.MyChildren.Contains(levelOneA));

            Assert.AreEqual(wrapper.FindParentOf(levelOneA.ID), wrapper);
            Assert.AreEqual(wrapper.FindParentOf(levelTwoA.ID), levelOneA);
            Assert.AreEqual(wrapper.FindParentOf(levelTwoB.ID), levelOneA);

            Assert.ThrowsException<System.Exception>(() => { m_arrangement.WrapChildWithNode(new MockTreeLeaf().ID, wrapper); });
        }

        [TestMethod]
        public void arrangement_can_be_unwrapped()
        {
            //Arrange
            var levelOneA = new MockArrangement();
            var levelOneB = new MockArrangement();
            m_arrangement.InsertAsLast(levelOneA);
            m_arrangement.InsertAsLast(levelOneB);

            var levelTwoA = new MockArrangement();
            var levelTwoB = new MockArrangement();
            levelOneA.InsertAsLast(levelTwoA);
            levelOneA.InsertAsLast(levelTwoB);

            var levelThree = new MockArrangement();
            levelTwoA.InsertAsLast(levelThree);

            Window[] windows = Enumerable.Range(0, 6).Select(i => new Window(new Bounds(1, 1, 1, 1), "test")).ToArray();

            m_arrangement.InsertAsLast(windows[0]);
            levelOneA.InsertAsLast(windows[1]);
            levelOneB.InsertAsLast(windows[2]);
            levelTwoA.InsertAsLast(windows[3]);
            levelTwoB.InsertAsLast(windows[4]);
            levelThree.InsertAsLast(windows[5]);

            //Act
            m_arrangement.UnWrapChildToSelf(levelOneA.ID);

            //Assert
            Assert.AreEqual(m_arrangement.FindParentOf(levelTwoA.ID), m_arrangement);
            Assert.AreEqual(m_arrangement.FindParentOf(levelTwoB.ID), m_arrangement);
            Assert.IsFalse(m_arrangement.MyChildren.Contains(levelOneA));

            Assert.ThrowsException<System.Exception>(() => { m_arrangement.UnWrapChildToSelf(new MockTreeLeaf().ID); });
        }
    }
}
