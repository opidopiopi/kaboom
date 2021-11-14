using Kaboom.Application.Actions;
using Kaboom.Application.Services;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Kaboom.Testing.Application
{
    [TestClass]
    public class ActionServiceTests
    {
        private ActionService m_actionService;
        private Mock<IActionEventSource> m_eventSource = new Mock<IActionEventSource>();


        [TestInitialize]
        public void SetUp()
        {
            m_actionService = new ActionService();
        }


        [TestMethod]
        public void adding_event_source_registers_itself_as_listener()
        {
            //Arrange
            m_eventSource.Setup(source => source.AddActionEventListener(m_actionService));

            //Act
            m_actionService.AddActionEventSource(m_eventSource.Object);

            //Assert
            m_eventSource.Verify(source => source.AddActionEventListener(m_actionService), Times.Once);
        }


        [TestMethod]
        public void can_register_action_for_event()
        {
            //Arrange

            //Act
            m_actionService.RegisterActionForEvent(new MockActionEvent(), new MockAction());
            m_actionService.RegisterActionForEvent(null, new MockAction());
            m_actionService.RegisterActionForEvent(new MockActionEvent(), null);
            m_actionService.RegisterActionForEvent(null, null);

            //Assert

        }


        [TestMethod]
        public void on_action_event_triggers_corresponding_action()
        {
            //Arrange
            var actionA = new MockAction();
            var actionB = new MockAction();

            var eventA = new MockActionEvent();
            var eventB = new MockActionEvent();

            m_actionService.RegisterActionForEvent(eventA, actionA);
            m_actionService.RegisterActionForEvent(eventB, actionB);

            //Act
            m_actionService.OnActionEvent(eventA);

            //Assert
            Assert.AreEqual(1, actionA.TriggerCount);
            Assert.AreEqual(0, actionB.TriggerCount);


            //Act
            m_actionService.OnActionEvent(eventB);

            //Assert
            Assert.AreEqual(1, actionA.TriggerCount);
            Assert.AreEqual(1, actionB.TriggerCount);
        }
    }
}