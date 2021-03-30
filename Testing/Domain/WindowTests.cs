using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Domain.WindowTree.Window;
using Kaboom.Domain.WindowTree.Exceptions;
using Kaboom.Testing.Mocks;
using Kaboom.Abstraction;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class WindowTests
    {
        Window m_window;

        [TestInitialize]
        public void SetUp()
        {
            m_window = new Window(new Rectangle(0, 0, 100, 100));
        }

        [TestMethod]
        public void window_is_a_leaf()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(m_window.IsLeaf());
        }

        [TestMethod]
        public void window_cannot_insert_child()
        {
            //Arrange
            MockTreeNodeWithBounds child = new MockTreeNodeWithBounds();

            //Act & Assert
            Assert.ThrowsException<CannotInsertChild>(() => m_window.InsertAsFirst(child));
            Assert.ThrowsException<CannotInsertChild>(() => m_window.InsertAsLast(child));
        }

        [TestMethod]
        public void window_attempt_to_remove_child_returns_false()
        {
            //Arrange

            //Act & Assert
            Assert.ThrowsException<CannotRemoveChild>(() => m_window.Remove(new MockTreeNodeWithBounds()));
            Assert.ThrowsException<CannotRemoveChild>(() => m_window.Remove(null));
        }

        [TestMethod]
        public void windows_are_unique()
        {
            //Arrange
            Window otherWindow = new Window(new Kaboom.Abstraction.Rectangle(0, 0, 10, 10));

            //Act

            //Assert
            Assert.AreNotEqual(m_window, otherWindow);
            Assert.AreNotEqual(m_window.Identity, otherWindow.Identity);
        }

        [TestMethod]
        public void window_accepts_visitor()
        {
            //Arrange
            VisitedFlagMockVisitor visitor = new VisitedFlagMockVisitor();

            //Act
            m_window.Accept(visitor);

            //Assert
            Assert.IsTrue(visitor.HasBeenVisited);
        }


        [TestMethod]
        public void window_has_bounds()
        {
            //Arrange
            Rectangle newBounds = new Rectangle(5, 5, 13, 45);

            //Act
            m_window.Bounds = newBounds;

            //Assert
            Assert.AreEqual(newBounds, m_window.Bounds);
        }
    }
}
