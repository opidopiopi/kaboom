using Kaboom.Domain.ShortcutActions;
using Kaboom.Testing.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaboom.Testing.Domain.Actions
{
    [TestClass]
    public class ActionsServiceTests
    {
        private ActionService m_actionService;
        private MockActionTarget m_actionTarget;
        private MockShortcutListener m_shortcutListener;

        [TestInitialize]
        public void SetUp()
        {
            m_actionTarget = new MockActionTarget();
            m_shortcutListener = new MockShortcutListener();

            m_actionService = new ActionService(m_shortcutListener, m_actionTarget);
        }


        [TestMethod]
        public void actionservice_adding_action_registers_shortcut()
        {
            //Arrange
            var action = new MockAction(new Shortcut(Modifier.WINDOWS, 'M'));

            //Act
            m_actionService.AddAction(action);

            //Assert
            Assert.IsTrue(m_shortcutListener.Shortcuts.Contains(action.Shortcut));
        }

        [TestMethod]
        public void actionservice_removing_action_unregisters_shortcut()
        {
            //Arrange
            var action = new MockAction(new Shortcut(Modifier.WINDOWS, 'M'));
            m_actionService.AddAction(action);

            //Act
            m_actionService.RemoveAction(action);

            //Assert
            Assert.IsFalse(m_shortcutListener.Shortcuts.Contains(action.Shortcut));
        }

        [TestMethod]
        public void actionservice_adding_action_when_shortcut_already_used_throws_exception()
        {
            //Arrange
            var action = new MockAction(new Shortcut(Modifier.WINDOWS, 'M'));
            var anotherAction = new MockAction(new Shortcut(Modifier.WINDOWS, 'M'));
            m_actionService.AddAction(action);

            //Act

            //Assert
            Assert.ThrowsException<ShortcutAlreadyInUse>(() => { m_actionService.AddAction(anotherAction); }, $"Action added: {anotherAction}");
        }

        [TestMethod]
        public void actionservice_triggering_shortcut_executes_action()
        {
            //Arrange
            var action = new MockAction(new Shortcut(Modifier.WINDOWS, 'M'));
            var anotherAction = new MockAction(new Shortcut(Modifier.WINDOWS, 'W'));
            m_actionService.AddAction(action);
            m_actionService.AddAction(anotherAction);

            //Act & Assert
            m_shortcutListener.TriggerShortcut(new Shortcut(Modifier.WINDOWS, 'M'));
            Assert.AreEqual(1, action.TriggerCount);
            Assert.AreEqual(0, anotherAction.TriggerCount);

            m_shortcutListener.TriggerShortcut(new Shortcut(Modifier.WINDOWS, 'M'));
            Assert.AreEqual(2, action.TriggerCount);
            Assert.AreEqual(0, anotherAction.TriggerCount);

            m_shortcutListener.TriggerShortcut(new Shortcut(Modifier.WINDOWS, 'W'));
            Assert.AreEqual(2, action.TriggerCount);
            Assert.AreEqual(1, anotherAction.TriggerCount);

            m_shortcutListener.TriggerShortcut(new Shortcut(Modifier.WINDOWS, 'M'));
            Assert.AreEqual(3, action.TriggerCount);
            Assert.AreEqual(1, anotherAction.TriggerCount);

            m_shortcutListener.TriggerShortcut(new Shortcut(Modifier.WINDOWS, 'W'));
            Assert.AreEqual(3, action.TriggerCount);
            Assert.AreEqual(2, anotherAction.TriggerCount);

            m_shortcutListener.TriggerShortcut(new Shortcut(Modifier.WINDOWS, 'W'));
            Assert.AreEqual(3, action.TriggerCount);
            Assert.AreEqual(3, anotherAction.TriggerCount);

        }
    }
}
