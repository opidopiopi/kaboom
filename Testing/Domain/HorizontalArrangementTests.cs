using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Arrangements;
using Kaboom.Domain.WindowTree.Exceptions;
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

        MockTreeNodeWithBounds m_first;
        MockTreeNodeWithBounds m_middle;
        MockTreeNodeWithBounds m_last;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new HorizontalArrangement();
        }

        private void SetUpChildrenAllLeafs()
        {
            m_first = new MockTreeNodeWithBounds();
            m_middle = new MockTreeNodeWithBounds();
            m_last = new MockTreeNodeWithBounds();

            m_arrangement.InsertAsFirst(m_first);
            m_arrangement.InsertAsLast(m_middle);
            m_arrangement.InsertAsLast(m_last);
        }

        private void SetUpChildrenMiddleLeaf()
        {
            m_first = new MockTreeNodeWithBounds(/*IsLeaf*/ false);
            m_middle = new MockTreeNodeWithBounds();
            m_last = new MockTreeNodeWithBounds(/*IsLeaf*/ false);

            m_arrangement.InsertAsFirst(m_first);
            m_arrangement.InsertAsLast(m_middle);
            m_arrangement.InsertAsLast(m_last);
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
            MockTreeNodeWithBounds first = new MockTreeNodeWithBounds();
            MockTreeNodeWithBounds middle = new MockTreeNodeWithBounds();
            MockTreeNodeWithBounds last = new MockTreeNodeWithBounds();

            m_arrangement.InsertAsFirst(first);
            m_arrangement.InsertAsLast(middle);
            m_arrangement.InsertAsLast(last);

            //Act
            //Assert
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, middle));
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, middle));

            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.POSITIVE, middle));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, middle));
        }

        [TestMethod]
        public void arrangement_node_can_only_be_moved_if_direct_child()
        {
            //Arrange
            MockTreeNodeWithBounds notAChild = new MockTreeNodeWithBounds();

            //Assert
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, notAChild));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, notAChild));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, notAChild));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, notAChild));

            //Arrange
            HorizontalArrangement childArrangement = new HorizontalArrangement();
            MockTreeNodeWithBounds childsChild = new MockTreeNodeWithBounds();
            childArrangement.InsertAsFirst(childsChild);
            m_arrangement.InsertAsFirst(childArrangement);

            //Assert
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, childsChild));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, childsChild));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, childsChild));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.Y, Direction.NEGATIVE, childsChild));
        }

        [TestMethod]
        public void arrangement_node_can_only_be_moved_inside_arrangement()
        {
            //Arrange
            MockTreeNodeWithBounds first = new MockTreeNodeWithBounds();
            MockTreeNodeWithBounds middle = new MockTreeNodeWithBounds();
            MockTreeNodeWithBounds last = new MockTreeNodeWithBounds();

            m_arrangement.InsertAsFirst(first);
            m_arrangement.InsertAsLast(middle);
            m_arrangement.InsertAsLast(last);

            //Act
            //Assert
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, first));
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, first));
            
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, middle));
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, middle));

            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, last));
            Assert.IsFalse(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, last));
        }


        [TestMethod]
        public void arrangement_can_move_children_swapping_positive()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            var expected = new List<ITreeNode> { m_middle, m_first, m_last };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_middle));
            m_arrangement.MoveChild(Axis.X, Direction.POSITIVE, m_middle);

            //Assert
            //Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
            //    $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
            //    $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
        }

        [TestMethod]
        public void arrangement_can_move_children_swapping_negative()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            var expected = new List<ITreeNode> { m_first, m_last, m_middle };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_middle));
            m_arrangement.MoveChild(Axis.X, Direction.NEGATIVE, m_middle);

            //Assert
            //Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
            //    $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
            //    $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
        }

        [TestMethod]
        public void arrangement_can_move_children_insert_at_neighbour_positive()
        {
            //Arrange
            SetUpChildrenMiddleLeaf();

            var expected = new List<ITreeNode> { m_first, m_last };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.POSITIVE, m_middle));
            m_arrangement.MoveChild(Axis.X, Direction.POSITIVE, m_middle);

            //Assert
            //Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
            //    $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
            //    $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
            //
            //Assert.AreEqual(m_middle, m_first.FirstChild());
        }

        [TestMethod]
        public void arrangement_can_move_children_insert_at_neighbour_negative()
        {
            //Arrange
            SetUpChildrenMiddleLeaf();

            var expected = new List<ITreeNode> { m_first, m_last };

            //Act
            Assert.IsTrue(m_arrangement.CanIMoveChild(Axis.X, Direction.NEGATIVE, m_middle));
            m_arrangement.MoveChild(Axis.X, Direction.NEGATIVE, m_middle);

            //Assert
            //Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
            //    $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
            //    $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
            //
            //Assert.AreEqual(m_middle, m_last.FirstChild());
        }
    }
}
