using Kaboom.Application;
using Kaboom.Application.WorkspaceActions;
using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.General;
using Kaboom.Testing.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Kaboom.Testing.Application
{
    [TestClass]
    public class ActionTests
    {

        [TestMethod]
        public void workspace_action_move_action_moves_window()
        {
            //Arrange
            var workspaceMock = new Mock<IWorkspace>();
            var action = new MoveWindowAction(new Shortcut(Modifier.CTRL, 'M'), workspaceMock.Object, Direction.Up);

            //Act
            action.Execute(new Mock<IActionTarget>().Object);

            //Assert
            workspaceMock.Verify(workspace => workspace.MoveSelectedWindow(Direction.Up), Times.Once());
        }

        [TestMethod]
        public void workspace_action_select_action_selects_next_window()
        {
            //Arrange
            var workspaceMock = new Mock<IWorkspace>();
            var action = new SelectWindowAction(new Shortcut(Modifier.CTRL, 'M'), workspaceMock.Object, Direction.Up);

            //Act
            action.Execute(new Mock<IActionTarget>().Object);

            //Assert
            workspaceMock.Verify(workspace => workspace.MoveSelection(Direction.Up), Times.Once());
        }

        [TestMethod]
        public void workspace_action_wrap_action_wraps_window()
        {
            //Arrange
            var workspaceMock = new Mock<IWorkspace>();
            var action = new WrapWindowAction<MockArrangement>(new Shortcut(Modifier.CTRL, 'M'), workspaceMock.Object);

            //Act
            action.Execute(new Mock<IActionTarget>().Object);

            //Assert
            workspaceMock.Verify(workspace => workspace.WrapSelectedWindow(It.IsAny<MockArrangement>()), Times.Once());
        }


        [TestMethod]
        public void workspace_action_unwrap_action_unwraps_arrangement()
        {
            //Arrange
            var workspaceMock = new Mock<IWorkspace>();
            var action = new UnWrapWindowAction(new Shortcut(Modifier.CTRL, 'M'), workspaceMock.Object);

            //Act
            action.Execute(new Mock<IActionTarget>().Object);

            //Assert
            workspaceMock.Verify(workspace => workspace.UnWrapSelectedWindow(), Times.Once());
        }
    }
}
