using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class VerticalArrangementTests
    {
        VerticalArrangement m_arrangement;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new VerticalArrangement();
        }


        [TestMethod]
        public void supports_only_y_axis()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(m_arrangement.SupportsAxis(Axis.Y));
            Assert.IsFalse(m_arrangement.SupportsAxis(Axis.X));
        }


        [TestMethod]
        public void arrangement_can_find_child_neightbour_in_supported_axis()
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
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Left));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Right));

            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Up));
            Assert.IsNotNull(m_arrangement.NeighbourOfChildInDirection(middle.ID, Direction.Down));
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
            MockArrangement childArrangement = new MockArrangement();
            MockTreeNode childsChild = new MockTreeNode();
            childArrangement.InsertAsFirst(childsChild);
            m_arrangement.InsertAsFirst(childArrangement);

            //Assert
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Left));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Right));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Up));
            Assert.IsNull(m_arrangement.NeighbourOfChildInDirection(childsChild.ID, Direction.Down));
        }

        [TestMethod]
        public void arrangement_updates_bounds_of_children()
        {
            //Arrange
            MockTreeLeaf windowA = new MockTreeLeaf();
            MockTreeLeaf windowB = new MockTreeLeaf();
            MockTreeLeaf windowC = new MockTreeLeaf();
            MockTreeLeaf windowD = new MockTreeLeaf();
            m_arrangement.Bounds = new Bounds(-55, -5, 420, 400);

            //Act
            m_arrangement.InsertAsLast(windowA);
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(windowA.Bounds, new Bounds(-55, -5, 420, 400));

            //Act
            m_arrangement.InsertAsLast(windowB);
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(new Bounds(-55, -5, 420, 200), windowA.Bounds);
            Assert.AreEqual(new Bounds(-55, 195, 420, 200), windowB.Bounds);

            //Act
            m_arrangement.InsertAsLast(windowC);
            m_arrangement.InsertAsLast(windowD);
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(new Bounds(-55, - 5, 420, 100), windowA.Bounds);
            Assert.AreEqual(new Bounds(-55,  95, 420, 100), windowB.Bounds);
            Assert.AreEqual(new Bounds(-55, 195, 420, 100), windowC.Bounds);
            Assert.AreEqual(new Bounds(-55, 295, 420, 100), windowD.Bounds);
        }
    }
}
