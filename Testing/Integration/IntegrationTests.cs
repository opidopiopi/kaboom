using Kaboom.Application;
using Kaboom.Application.Actions.SelectionActions;
using Kaboom.Application.Services;
using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Kaboom.Testing.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        private MockArrangementRepository m_arrangementRepository;
        private Mock<IRenderService> m_windowRendererMock;
        private WindowService m_windowService;
        private Selection m_selection;

        private ActionService m_actionService;

        private VerticalArrangement m_screenA;
        private HorizontalArrangement m_screenB;
        private HorizontalArrangement m_screenC;

        private Arrangement[] m_childArrangements;
        private Window[] m_windows;

        private readonly MockActionEvent MoveUp = new MockActionEvent();
        private readonly MockActionEvent MoveDown = new MockActionEvent();
        private readonly MockActionEvent MoveLeft = new MockActionEvent();
        private readonly MockActionEvent MoveRight = new MockActionEvent();

        private readonly MockActionEvent SelectUp = new MockActionEvent();
        private readonly MockActionEvent SelectDown = new MockActionEvent();
        private readonly MockActionEvent SelectLeft = new MockActionEvent();
        private readonly MockActionEvent SelectRight = new MockActionEvent();

        private readonly MockActionEvent WrapHorizontal = new MockActionEvent();
        private readonly MockActionEvent WrapVertical = new MockActionEvent();
        private readonly MockActionEvent UnWrap = new MockActionEvent();

        [TestInitialize]
        public void SetUp()
        {
            m_arrangementRepository = new MockArrangementRepository();
            m_windowRendererMock = new Mock<IRenderService>();

            m_windowService = new WindowService(m_arrangementRepository, m_windowRendererMock.Object);
            m_selection = new Selection(m_windowService, m_arrangementRepository);

            m_actionService = new ActionService();
            AddActions();

            //somewhat like this
            /*  |------||--------------|
             *  | A    || screenB      |
             *  |      ||              |
             *  |      ||--------------|
             *  |      ||---------|
             *  |------|| screenC |
             *          |---------|
             */
            m_screenA = new VerticalArrangement();
            m_screenA.Bounds = new Bounds(-1080, 0, 1080, 1920);
            m_screenB = new HorizontalArrangement();
            m_screenB.Bounds = new Bounds(0, 0, 1920, 1080);
            m_screenC = new HorizontalArrangement();
            m_screenC.Bounds = new Bounds(0, 1080, 1280, 720);

            m_arrangementRepository.InsertRoot(m_screenA);
            m_arrangementRepository.InsertRoot(m_screenB);
            m_arrangementRepository.InsertRoot(m_screenC);
        }

        private void AddActions()
        {
            m_actionService.RegisterActionForEvent(MoveUp, new MoveWindowAction(m_selection, Direction.Up));
            m_actionService.RegisterActionForEvent(MoveDown, new MoveWindowAction(m_selection, Direction.Down));
            m_actionService.RegisterActionForEvent(MoveLeft, new MoveWindowAction(m_selection, Direction.Left));
            m_actionService.RegisterActionForEvent(MoveRight, new MoveWindowAction(m_selection, Direction.Right));

            m_actionService.RegisterActionForEvent(SelectUp, new SelectWindowAction(m_selection, Direction.Up));
            m_actionService.RegisterActionForEvent(SelectDown, new SelectWindowAction(m_selection, Direction.Down));
            m_actionService.RegisterActionForEvent(SelectLeft, new SelectWindowAction(m_selection, Direction.Left));
            m_actionService.RegisterActionForEvent(SelectRight, new SelectWindowAction(m_selection, Direction.Right));

            m_actionService.RegisterActionForEvent(WrapHorizontal, new WrapWindowAction<HorizontalArrangement>(m_selection));
            m_actionService.RegisterActionForEvent(WrapVertical, new WrapWindowAction<VerticalArrangement>(m_selection));
            m_actionService.RegisterActionForEvent(UnWrap, new UnWrapWindowAction(m_selection));
        }

        private void ScenarioOne()
        {
            m_childArrangements = new Arrangement[]
            {
                new HorizontalArrangement(),
                new HorizontalArrangement(),
                new HorizontalArrangement(),
                new VerticalArrangement(),
                new VerticalArrangement(),
                new VerticalArrangement(),
            };

            m_windows = new Window[]
            {
                new Window(new Bounds(0, 0, 1, 1), "window0"),
                new Window(new Bounds(0, 0, 1, 1), "window1"),
                new Window(new Bounds(0, 0, 1, 1), "window2"),
                new Window(new Bounds(0, 0, 1, 1), "window3"),
                new Window(new Bounds(0, 0, 1, 1), "window4"),
                new Window(new Bounds(0, 0, 1, 1), "window5"),
                new Window(new Bounds(0, 0, 1, 1), "window6"),
            };

            m_screenA.InsertAsLast(m_childArrangements[0]);
            m_screenA.InsertAsLast(m_childArrangements[1]);
            m_screenB.InsertAsLast(m_childArrangements[2]);
            m_screenB.InsertAsLast(m_childArrangements[3]);
            m_screenC.InsertAsLast(m_childArrangements[4]);
            m_screenC.InsertAsLast(m_childArrangements[5]);

            m_childArrangements[0].InsertAsLast(m_windows[0]);
            m_childArrangements[0].InsertAsLast(m_windows[1]);

            m_childArrangements[1].InsertAsLast(m_windows[2]);

            m_childArrangements[2].InsertAsLast(m_windows[3]);

            m_childArrangements[3].InsertAsLast(m_windows[4]);

            m_childArrangements[4].InsertAsLast(m_windows[5]);

            m_childArrangements[5].InsertAsLast(m_windows[6]);

            m_screenA.UpdateBoundsOfChildren();
            m_screenB.UpdateBoundsOfChildren();
            m_screenC.UpdateBoundsOfChildren();
        }


        [TestMethod]
        public void integration_move_selection()
        {
            //Arrange
            ScenarioOne();

            var asdf = new (MockActionEvent actionEvent, EntityID windowID)[]{
                (SelectLeft, m_windows[0].ID),
                (SelectRight, m_windows[1].ID),
                (SelectDown, m_windows[2].ID),
                (SelectRight, m_windows[3].ID),
                (SelectRight, m_windows[4].ID),
                (SelectDown, m_windows[5].ID),
                (SelectRight, m_windows[6].ID),
                (SelectLeft, m_windows[5].ID),
                (SelectUp, m_windows[4].ID),
                (SelectLeft, m_windows[3].ID),
                (SelectLeft, m_windows[2].ID),
                (SelectUp, m_windows[1].ID),
                (SelectLeft, m_windows[0].ID),
            };

            asdf.ToList().ForEach(wow => {
                //Act
                m_actionService.OnActionEvent(wow.actionEvent);

                //Assert
                Assert.AreEqual(wow.windowID, m_selection.SelectedWindow, $"\nactual window: {m_arrangementRepository.FindParentOf(m_selection.SelectedWindow).FindChild(m_selection.SelectedWindow)}");
            });
        }


        [TestMethod]
        public void integration_move_selected_window()
        {
            //Arrange
            ScenarioOne();

            new MockActionEvent[]{
                SelectLeft,
                MoveRight,
                MoveDown,
            }.ToList().ForEach(actionEvent => m_actionService.OnActionEvent(actionEvent));

            Assert.AreEqual(
                new Bounds(
                    m_screenA.Bounds.X,
                    m_screenA.Bounds.Y + m_screenA.Bounds.Height / 3,
                    m_screenA.Bounds.Width,
                    m_screenA.Bounds.Height / 3
                ),
                m_windows[0].Bounds
            );

            new MockActionEvent[]{
                SelectDown,
                MoveRight,
                MoveRight,
                MoveRight,
                MoveRight,
            }.ToList().ForEach(actionEvent => m_actionService.OnActionEvent(actionEvent));

            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X + m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[2].Bounds
            );
        }


        [TestMethod]
        public void integration_wrap_window()
        {
            //Arrange
            m_windows = new Window[] {
                new Window(new Bounds(1, 1, 1, 1), "window0"),
                new Window(new Bounds(1, 1, 1, 1), "window1"),
                new Window(new Bounds(1, 1, 1, 1), "window2"),
            };
            m_windows.Reverse<Window>().ToList().ForEach(window => m_windowService.InsertWindowIntoTree(window, m_selection));

            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[0].Bounds
            );

            Assert.AreEqual(m_windows[2].ID, m_selection.SelectedWindow);

            //Act
            new MockActionEvent[]{
                SelectLeft,
                SelectLeft,
                WrapVertical,
                SelectRight,
                MoveLeft,
            }.ToList().ForEach(actionEvent => m_actionService.OnActionEvent(actionEvent));

            //Assert
            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height / 2
                ),
                m_windows[0].Bounds
            );

            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y + m_screenB.Bounds.Height / 2,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height / 2
                ),
                m_windows[1].Bounds
            );

            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X + m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height
                ),
                m_windows[2].Bounds
            );
        }

        [TestMethod]
        public void integration_unwrap_window()
        {
            //Arrange
            m_windows = new Window[] {
                new Window(new Bounds(1, 1, 1, 1), "window0"),
                new Window(new Bounds(1, 1, 1, 1), "window1"),
                new Window(new Bounds(1, 1, 1, 1), "window2"),
            };
            m_windows.Reverse<Window>().ToList().ForEach(window => m_windowService.InsertWindowIntoTree(window, m_selection));

            //Act
            new MockActionEvent[]{
                SelectLeft,
                SelectLeft,
                WrapVertical,
                SelectRight,
                MoveLeft,
                UnWrap,
            }.ToList().ForEach(actionEvent => m_actionService.OnActionEvent(actionEvent));

            //Assert
            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[0].Bounds
            );

            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X + m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[1].Bounds
            );

            Assert.AreEqual(
                new Bounds(
                    m_screenB.Bounds.X + (m_screenB.Bounds.Width / 3) * 2,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[2].Bounds
            );
        }
    }
}