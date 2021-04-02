using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class HorizontalArrangementTests
    {
        HorizontalArrangement m_arrangement;

        IBoundedTreeNode m_first;
        IBoundedTreeNode m_middle;
        IBoundedTreeNode m_last;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new HorizontalArrangement();
        }

        private void SetUpChildrenAllLeafs()
        {
            m_first = new MockTreeLeaf();
            m_middle = new MockTreeLeaf();
            m_last = new MockTreeLeaf();

            m_arrangement.InsertAsFirst(m_first);
            m_arrangement.InsertAsLast(m_middle);
            m_arrangement.InsertAsLast(m_last);
        }

        private void SetUpChildrenMiddleLeaf()
        {
            m_first = new MockTreeNode();
            m_middle = new MockTreeLeaf();
            m_last = new MockTreeNode();

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
            MockTreeNode first = new MockTreeNode();
            MockTreeNode middle = new MockTreeNode();
            MockTreeNode last = new MockTreeNode();

            m_arrangement.InsertAsFirst(first);
            m_arrangement.InsertAsLast(middle);
            m_arrangement.InsertAsLast(last);

            //Act
            //Assert
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Left));
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Right));

            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Down));
        }

        [TestMethod]
        public void arrangement_can_find_neighbour_of_direct_child_only()
        {
            //Arrange
            MockTreeNode notAChild = new MockTreeNode();

            //Assert
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild.ID, Direction.Left));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild.ID, Direction.Right));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild.ID, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(notAChild.ID, Direction.Down));

            //Arrange
            HorizontalArrangement childArrangement = new HorizontalArrangement();
            MockTreeNode childsChild = new MockTreeNode();
            childArrangement.InsertAsFirst(childsChild);
            m_arrangement.InsertAsFirst(childArrangement);

            //Assert
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Left));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Right));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Down));
        }
    }
}
