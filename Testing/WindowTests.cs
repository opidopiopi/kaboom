using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Model;
using Testing.Mocks;
using Kaboom.Abstract;
using Kaboom.Model.Exceptions;
using System;

namespace Testing
{
    [TestClass]
    public class WindowTests
    {
        Window m_window;
        MockWindowBoundsSetter m_windowBoundsSetter;

        [TestInitialize]
        public void SetUp()
        {
            m_windowBoundsSetter = new MockWindowBoundsSetter();
            m_window = new Window(new MockWindowIdentity(1), new Rectangle(0, 0, 100, 100), m_windowBoundsSetter);
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
        public void window_can_set_parent()
        {
            //Arrange
            MockArrangement parent = new MockArrangement();

            //Act
            m_window.SetParent(parent);

            //Assert
            Assert.AreEqual(parent, m_window.GetParent());
        }

        [TestMethod]
        public void window_can_only_set_window_arrangement_as_parent()
        {
            //Arrange
            MockArrangement parent = new MockArrangement();
            MockTreeNodeWithBounds notAParent = new MockTreeNodeWithBounds();

            //Act & Assert
            m_window.SetParent(parent);
            Assert.ThrowsException<InvalidCastException>(() => m_window.SetParent(notAParent));
        }

        [TestMethod]
        public void window_cannot_insert_child()
        {
            //Arrange
            MockTreeNodeWithBounds child = new MockTreeNodeWithBounds();

            //Act & Assert
            Assert.ThrowsException<InvalidChildForThisNode>(() => m_window.Insert(child));
        }

        [TestMethod]
        public void window_attempt_to_remove_child_returns_false()
        {
            //Arrange

            //Act & Assert
            Assert.IsFalse(m_window.RemoveAndReturnSuccess(new MockTreeNodeWithBounds()));
            Assert.IsFalse(m_window.RemoveAndReturnSuccess(null));
        }

        [TestMethod]
        public void window_children_is_just_empty_list()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual(0, m_window.Children().Count);
        }

        [TestMethod]
        public void window_has_identity()
        {
            //Arrange
            MockWindowIdentity identity = new MockWindowIdentity(1);

            //Act

            //Assert
            Assert.AreEqual(identity, m_window.Identity());
        }
    }
}
