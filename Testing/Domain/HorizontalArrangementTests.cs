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
        public void arrangement_can_find_child_neighbour_in_supported_axis()
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
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(middle, Direction.Left));
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(middle, Direction.Right));

            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(middle, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(middle, Direction.Down));
        }

        [TestMethod]
        public void arrangement_can_find_neighbour_of_direct_child_only()
        {
            //Arrange
            MockTreeNodeWithBounds notAChild = new MockTreeNodeWithBounds();

            //Assert
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild, Direction.Left));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild, Direction.Right));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild, Direction.Down));

            //Arrange
            HorizontalArrangement childArrangement = new HorizontalArrangement();
            MockTreeNodeWithBounds childsChild = new MockTreeNodeWithBounds();
            childArrangement.InsertAsFirst(childsChild);
            m_arrangement.InsertAsFirst(childArrangement);

            //Assert
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild, Direction.Left));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild, Direction.Right));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild, Direction.Down));
        }


        [TestMethod]
        public void arrangement_can_move_children_swapping_left()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            var expected = new List<ITreeNode> { m_middle, m_first, m_last };

            //Act
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(m_middle, Direction.Left));
            m_arrangement.MoveChild(m_middle, Direction.Left);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
        }

        [TestMethod]
        public void arrangement_can_move_children_swapping_right()
        {
            //Arrange
            SetUpChildrenAllLeafs();

            var expected = new List<ITreeNode> { m_first, m_last, m_middle };

            //Act
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(m_middle, Direction.Right));
            m_arrangement.MoveChild(m_middle, Direction.Right);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
        }

        [TestMethod]
        public void arrangement_can_move_children_insert_at_neighbour_left()
        {
            //Arrange
            SetUpChildrenMiddleLeaf();

            var expected = new List<ITreeNode> { m_first, m_last };

            //Act
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(m_middle, Direction.Left));
            m_arrangement.MoveChild(m_middle, Direction.Left);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
            Assert.AreEqual(m_middle, m_first.LastChild());
        }

        [TestMethod]
        public void arrangement_can_move_children_insert_at_neighbour_right()
        {
            //Arrange
            SetUpChildrenMiddleLeaf();

            var expected = new List<ITreeNode> { m_first, m_last };

            //Act
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(m_middle, Direction.Right));
            m_arrangement.MoveChild(m_middle, Direction.Right);

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, expected),
                $"expected: [{expected.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n" +
                $"actual: [{m_arrangement.Children.Select(id => id.ToString()).Aggregate((a, b) => a + ", " + b)}]\n");
            Assert.AreEqual(m_middle, m_last.FirstChild());
        }
    }
}
