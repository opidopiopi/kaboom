using Kaboom.Domain.WindowTree.Arrangements;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class HorizontalArrangementTests
    {
        HorizontalArrangement m_arrangement;
        MockTreeNodeRepository m_treeNodeRepo;

        MockTreeNodeWithBounds m_first;
        MockTreeNodeWithBounds m_middle;
        MockTreeNodeWithBounds m_last;

        [TestInitialize]
        public void SetUp()
        {
            m_treeNodeRepo = new MockTreeNodeRepository();
            m_arrangement = new HorizontalArrangement(m_treeNodeRepo);
        }

        private void SetUpChildrenAllLeafs()
        {
            m_first = new MockTreeNodeWithBounds();
            m_middle = new MockTreeNodeWithBounds();
            m_last = new MockTreeNodeWithBounds();

            m_treeNodeRepo.AddTreeNode(m_first);
            m_treeNodeRepo.AddTreeNode(m_middle);
            m_treeNodeRepo.AddTreeNode(m_last);

            m_arrangement.InsertAsFirst(m_first.ID);
            m_arrangement.InsertAsLast(m_middle.ID);
            m_arrangement.InsertAsLast(m_last.ID);
        }

        private void SetUpChildrenMiddleLeaf()
        {
            m_first = new MockTreeNodeWithBounds(/*IsLeaf*/ false);
            m_middle = new MockTreeNodeWithBounds();
            m_last = new MockTreeNodeWithBounds(/*IsLeaf*/ false);

            m_treeNodeRepo.AddTreeNode(m_first);
            m_treeNodeRepo.AddTreeNode(m_middle);
            m_treeNodeRepo.AddTreeNode(m_last);

            m_arrangement.InsertAsFirst(m_first.ID);
            m_arrangement.InsertAsLast(m_middle.ID);
            m_arrangement.InsertAsLast(m_last.ID);
        }


        [TestMethod]
        public void can_only_move_in_X_axis()
        {
            //Arrange
            
            //Act
            
            //Assert
            Assert.IsTrue(m_arrangement.SupportsAxis(Axis.X));
            Assert.IsFalse(m_arrangement.SupportsAxis(Axis.Y));
        }

        [TestMethod]
        public void arrangement_children_can_only_be_moved_in_supported_axes()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            //Act
            //Assert
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_middle.ID));
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_middle.ID));

            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.POSITIVE, m_middle.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, m_middle.ID));
        }

        [TestMethod]
        public void arrangement_node_can_only_be_moved_if_direct_child()
        {
            //Arrange
            MockTreeNodeWithBounds notAChild = new MockTreeNodeWithBounds();

            //Assert
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, notAChild.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, notAChild.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, notAChild.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, notAChild.ID));

            //Arrange
            HorizontalArrangement childArrangement = new HorizontalArrangement(m_treeNodeRepo);
            MockTreeNodeWithBounds childsChild = new MockTreeNodeWithBounds();
            childArrangement.InsertAsFirst(childsChild.ID);
            m_arrangement.InsertAsFirst(childArrangement.ID);

            //Assert
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, childsChild.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, childsChild.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, childsChild.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, childsChild.ID));
        }

        [TestMethod]
        public void arrangement_node_can_only_be_moved_inside_arrangement()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            //Act
            //Assert
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_first.ID));
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_first.ID));
            
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_middle.ID));
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_middle.ID));

            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_last.ID));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_last.ID));
        }


        [TestMethod]
        public void arrangement_can_move_children_swapping_positive()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            var expected = new List<TreeNodeID> { m_middle.ID, m_first.ID, m_last.ID };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_middle.ID));
            m_arrangement.MoveChild(Axis.X, Direction.POSITIVE, m_middle.ID);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
        }

        [TestMethod]
        public void arrangement_can_move_children_swapping_negative()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            var expected = new List<TreeNodeID> { m_first.ID, m_last.ID, m_middle.ID };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_middle.ID));
            m_arrangement.MoveChild(Axis.X, Direction.NEGATIVE, m_middle.ID);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
        }

        [TestMethod]
        public void arrangement_can_move_children_insert_at_neighbour_positive()
        {
            //Arrange
            SetUpChildrenMiddleLeaf();

            var expected = new List<TreeNodeID> { m_first.ID, m_last.ID };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_middle.ID));
            m_arrangement.MoveChild(Axis.X, Direction.POSITIVE, m_middle.ID);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");

            Assert.AreEqual(m_middle.ID, m_first.FirstChild());
        }

        [TestMethod]
        public void arrangement_can_move_children_insert_at_neighbour_negative()
        {
            //Arrange
            SetUpChildrenMiddleLeaf();

            var expected = new List<TreeNodeID> { m_first.ID, m_last.ID };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_middle.ID));
            m_arrangement.MoveChild(Axis.X, Direction.NEGATIVE, m_middle.ID);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");

            Assert.AreEqual(m_middle.ID, m_last.FirstChild());
        }
    }
}
