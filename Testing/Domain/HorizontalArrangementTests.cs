using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class HorizontalArrangementTests
    {
        HorizontalArrangement m_arrangement;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new HorizontalArrangement();
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


        [TestMethod]
        public void arrangement_updates_bounds_of_children()
        {
            //Arrange
            MockTreeLeaf windowA = new MockTreeLeaf();
            MockTreeLeaf windowB = new MockTreeLeaf();
            MockTreeLeaf windowC = new MockTreeLeaf();
            MockTreeLeaf windowD = new MockTreeLeaf();
            m_arrangement.Bounds = new Bounds(-5, -55, 400, 420);

            //Act
            m_arrangement.InsertAsLast(windowA);
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(windowA.Bounds, new Bounds(-5, -55, 400, 420));

            //Act
            m_arrangement.InsertAsLast(windowB);
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(new Bounds(-5, -55, 200, 420), windowA.Bounds);
            Assert.AreEqual(new Bounds(195, -55, 200, 420), windowB.Bounds);

            //Act
            m_arrangement.InsertAsLast(windowC);
            m_arrangement.InsertAsLast(windowD);
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(new Bounds(-5, -55, 100, 420), windowA.Bounds);
            Assert.AreEqual(new Bounds(95, -55, 100, 420), windowB.Bounds);
            Assert.AreEqual(new Bounds(195, -55, 100, 420), windowC.Bounds);
            Assert.AreEqual(new Bounds(295, -55, 100, 420), windowD.Bounds);
        }


        [TestMethod]
        public void arrangement_updates_bounds_triggers_update_for_lower_arrangements()
        {
            /*  m_arrangement
             *  |
             *  |--anotherOne
             *  |  |
             *  |  |--bitesTheDust
             *  |  |  |
             *  |  |  |--windowC
             *  |  |
             *  |  |--winowB
             *  |
             *  |--windowA
             */

            //Arrange
            HorizontalArrangement anotherOne = new HorizontalArrangement();
            MockTreeLeaf windowA = new MockTreeLeaf();
            m_arrangement.InsertAsLast(anotherOne);
            m_arrangement.InsertAsLast(windowA);
            m_arrangement.Bounds = new Bounds(123, 456, 200, 69);

            HorizontalArrangement bitesTheDust = new HorizontalArrangement();
            MockTreeLeaf windowB = new MockTreeLeaf();
            anotherOne.InsertAsLast(bitesTheDust);
            anotherOne.InsertAsLast(windowB);

            MockTreeLeaf windowC = new MockTreeLeaf();
            bitesTheDust.InsertAsLast(windowC);

            //Act
            m_arrangement.UpdateBoundsOfChildren();

            //Assert
            Assert.AreEqual(new Bounds(223, 456, 100, 69), windowA.Bounds);
            Assert.AreEqual(new Bounds(173, 456, 50, 69), windowB.Bounds);
            Assert.AreEqual(new Bounds(123, 456, 50, 69), windowC.Bounds);
        }
    }
}
