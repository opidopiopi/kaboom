using Kaboom.Domain.WindowTree.Arrangements;
using Kaboom.Domain.WindowTree.Exceptions;
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
        public void arrangement_can_move_children()
        {
            //Arrange

            //Act

            //Assert
            Assert.Fail();
        }

    }
}
