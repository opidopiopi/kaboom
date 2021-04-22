using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Domain.WindowTree.Exceptions;
using Kaboom.Testing.Mock;
using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class WindowTests
    {
        Window m_window;

        [TestInitialize]
        public void SetUp()
        {
            m_window = new Window(new Bounds(0, 0, 100, 100), "aTestWindow");
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
            MockTreeNode child = new MockTreeNode();

            //Act & Assert
            Assert.ThrowsException<CannotInsertChild>(() => m_window.InsertAsFirst(child));
            Assert.ThrowsException<CannotInsertChild>(() => m_window.InsertAsLast(child));
        }

        [TestMethod]
        public void window_attempt_to_remove_child_returns_false()
        {
            //Arrange

            //Act & Assert
            Assert.ThrowsException<CannotRemoveChild>(() => m_window.Remove(new MockTreeNode()));
            Assert.ThrowsException<CannotRemoveChild>(() => m_window.Remove(null));
        }

        [TestMethod]
        public void windows_are_unique()
        {
            //Arrange
            Window otherWindow = new Window(new Bounds(0, 0, 10, 10), "anotherTestWindow");

            //Act

            //Assert
            Assert.AreNotEqual(m_window, otherWindow);
            Assert.AreNotEqual(m_window.ID, otherWindow.ID);
        }

        [TestMethod]
        public void window_has_bounds()
        {
            //Arrange
            Bounds newBounds = new Bounds(5, 5, 13, 45);

            //Act
            m_window.Bounds = newBounds;

            //Assert
            Assert.AreEqual(newBounds, m_window.Bounds);
        }


        [TestMethod]
        public void window_has_title()
        {
            //Arrange
            Window otherWindow = new Window(new Bounds(0, 0, 10, 10), "anotherTestWindow");

            //Act

            //Assert
            Assert.AreEqual(otherWindow.Title, "anotherTestWindow");
        }
    }
}
