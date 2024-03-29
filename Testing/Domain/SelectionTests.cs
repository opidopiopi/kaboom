﻿using Kaboom.Application;
using Kaboom.Domain;
using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class SelectionTests
    {
        private Selection m_selection;
        private Mock<IWindowService> m_windowService;
        private Mock<IArrangementRepository> m_arrangementRepo;

        [TestInitialize]
        public void SetUp()
        {
            m_windowService = new Mock<IWindowService>();
            m_arrangementRepo = new Mock<IArrangementRepository>();
            m_selection = new Selection(m_windowService.Object, m_arrangementRepo.Object);
        }


        [TestMethod]
        public void does_move_selected_window_if_not_null()
        {
            //Arrange
            Window window = new Window(new Bounds(1, 1, 1, 1), "window");
            m_selection.SelectWindow(window.ID);

            //Act
            m_selection.MoveSelectedWindow(Direction.Up);

            //Assert
            m_windowService.Verify(service => service.MoveWindow(window.ID, Direction.Up), Times.Once);
        }

        [TestMethod]
        public void does_not_move_selected_window_if_null()
        {
            //Arrange

            //Act
            m_selection.MoveSelectedWindow(Direction.Up);

            //Assert
            m_windowService.Verify(service => service.MoveWindow(It.IsAny<EntityID>(), Direction.Up), Times.Never);
        }

        [TestMethod]
        public void can_move_window_selection_if_selected_window_is_not_null()
        {
            //Arrange
            Window window = new Window(new Bounds(1, 1, 1, 1), "window");
            m_selection.SelectWindow(window.ID);

            EntityID neighbour = new EntityID();
            m_windowService.Setup(winService => winService.NextWindowInDirection(Direction.Up, window.ID)).Returns(neighbour);

            //Act
            m_selection.MoveSelection(Direction.Up);

            //Assert
            m_windowService.Verify(service => service.NextWindowInDirection(Direction.Up, window.ID), Times.Once);
            Assert.AreEqual(neighbour, m_selection.SelectedWindow);
        }

        [TestMethod]
        public void can_move_window_selection_if_selected_window_is_null()
        {
            //Arrange
            var window = new Window(new Bounds(1, 1, 1, 1), "window");
            var arrangement = new Mock<Arrangement>(new Axis[] { });
            arrangement.Object.InsertAsFirst(window);
            m_arrangementRepo.Setup(repo => repo.RootArrangements()).Returns((new Arrangement[] { arrangement.Object }).ToList());
            m_arrangementRepo.Setup(repo => repo.Find(arrangement.Object.ID)).Returns(arrangement.Object);

            //Act
            m_selection.MoveSelection(Direction.Up);

            //Assert
            Assert.AreEqual(window.ID, m_selection.SelectedWindow);
        }


        [TestMethod]
        public void does_wrap_selected_window_if_not_null()
        {
            //Arrange
            var window = new Window(new Bounds(1, 1, 1, 1), "window");
            var arrangement = new MockArrangement();
            arrangement.InsertAsFirst(window);
            m_arrangementRepo.Setup(repo => repo.FindParentOf(window.ID)).Returns(arrangement);
            m_selection.SelectWindow(window.ID);

            var wrapper = new Mock<Arrangement>(new Axis[] { });

            //Act
            m_selection.WrapSelectedWindow(wrapper.Object);

            //Assert
            Assert.IsTrue(arrangement.MyChildren.Contains(wrapper.Object));
        }


        [TestMethod]
        public void does_not_wrap_selected_window_if_null()
        {
            //Arrange
            var window = new Window(new Bounds(1, 1, 1, 1), "window");
            var arrangement = new MockArrangement();
            arrangement.InsertAsFirst(window);

            var wrapper = new Mock<Arrangement>(new Axis[] { });

            //Act
            m_selection.WrapSelectedWindow(wrapper.Object);

            //Assert
            Assert.IsFalse(arrangement.MyChildren.Contains(wrapper.Object));
        }

        [TestMethod]
        public void selection_can_unwrap_selected_window_if_not_null()
        {
            //Arrange
            var parent = new MockArrangement();
            var wrapper = new MockArrangement();
            var window = new Window(new Bounds(1, 1, 1, 1), "window");
            parent.InsertAsFirst(wrapper);
            wrapper.InsertAsFirst(window);

            m_arrangementRepo.Setup(repo => repo.FindParentOf(window.ID)).Returns(wrapper);
            m_arrangementRepo.Setup(repo => repo.FindParentOf(wrapper.ID)).Returns(parent);
            m_selection.SelectWindow(window.ID);

            m_windowService.Setup(service => service.UnWrapWindowParent(window.ID)).Callback(
                new InvocationAction(id =>
                {
                    parent.Remove(wrapper);
                    parent.InsertAsFirst(window);
                }));

            //Act
            m_selection.UnWrapSelectedWindow();

            //Assert
            Assert.IsTrue(parent.MyChildren.Contains(window));
            Assert.IsFalse(parent.MyChildren.Contains(wrapper));
            m_windowService.Verify(service => service.UnWrapWindowParent(window.ID), Times.Once);
        }


        [TestMethod]
        public void selection_can_not_unwrap_selected_window_if_null()
        {
            //Arrange
            var parent = new MockArrangement();
            var wrapper = new MockArrangement();
            var window = new Window(new Bounds(1, 1, 1, 1), "window");
            parent.InsertAsFirst(wrapper);
            wrapper.InsertAsFirst(window);

            //Act
            m_selection.UnWrapSelectedWindow();

            //Assert
            Assert.IsFalse(parent.MyChildren.Contains(window));
            Assert.IsTrue(parent.MyChildren.Contains(wrapper));
            m_windowService.Verify(service => service.UnWrapWindowParent(window.ID), Times.Never);
        }
    }
}