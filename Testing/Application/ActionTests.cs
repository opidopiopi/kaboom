using Kaboom.Application;
using Kaboom.Application.Actions.WorkspaceActions;
using Kaboom.Domain.WindowTree.ValueObjects;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Kaboom.Testing.Application
{
    [TestClass]
    public class ActionTests
    {

        [TestMethod]
        public void selection_action_move_action_moves_window()
        {
            //Arrange
            var selectionMock = new Mock<ISelection>();
            var action = new MoveWindowAction(selectionMock.Object, Direction.Up);

            //Act
            action.Execute();

            //Assert
            selectionMock.Verify(selection => selection.MoveSelectedWindow(Direction.Up), Times.Once());
        }

        [TestMethod]
        public void selection_action_select_action_selects_next_window()
        {
            //Arrange
            var selectionMock = new Mock<ISelection>();
            var action = new SelectWindowAction(selectionMock.Object, Direction.Up);

            //Act
            action.Execute();

            //Assert
            selectionMock.Verify(selection => selection.MoveSelection(Direction.Up), Times.Once());
        }

        [TestMethod]
        public void selection_action_wrap_action_wraps_window()
        {
            //Arrange
            var selectionMock = new Mock<ISelection>();
            var action = new WrapWindowAction<MockArrangement>(selectionMock.Object);

            //Act
            action.Execute();

            //Assert
            selectionMock.Verify(selection => selection.WrapSelectedWindow(It.IsAny<MockArrangement>()), Times.Once());
        }


        [TestMethod]
        public void selection_action_unwrap_action_unwraps_arrangement()
        {
            //Arrange
            var selectionMock = new Mock<ISelection>();
            var action = new UnWrapWindowAction(selectionMock.Object);

            //Act
            action.Execute();

            //Assert
            selectionMock.Verify(selection => selection.UnWrapSelectedWindow(), Times.Once());
        }
    }
}
