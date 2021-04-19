using Kaboom.Abstraction;
using Kaboom.Application;
using Kaboom.Application.WorkspaceActions;
using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using Kaboom.Testing.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        private MockArrangementRepository m_arrangementRepository;
        private Mock<IWindowRenderer> m_windowRendererMock;
        private WindowService m_windowService;
        private Workspace m_workspace;

        private MockShortcutListener m_shortcutListener;
        private ActionService m_actionService;

        private VerticalArrangement m_screenA;
        private HorizontalArrangement m_screenB;
        private HorizontalArrangement m_screenC;

        private Arrangement[] m_childArrangements;
        private Window[] m_windows;

        private readonly Shortcut MoveUp = new Shortcut(Modifier.WINDOWS, '0');
        private readonly Shortcut MoveDown = new Shortcut(Modifier.WINDOWS, '1');
        private readonly Shortcut MoveLeft = new Shortcut(Modifier.WINDOWS, '2');
        private readonly Shortcut MoveRight = new Shortcut(Modifier.WINDOWS, '3');

        private readonly Shortcut SelectUp = new Shortcut(Modifier.ALT, '0');
        private readonly Shortcut SelectDown = new Shortcut(Modifier.ALT, '1');
        private readonly Shortcut SelectLeft = new Shortcut(Modifier.ALT, '2');
        private readonly Shortcut SelectRight = new Shortcut(Modifier.ALT, '3');

        private readonly Shortcut WrapHorizontal = new Shortcut(Modifier.CTRL, '0');
        private readonly Shortcut WrapVertical = new Shortcut(Modifier.CTRL, '1');
        private readonly Shortcut UnWrap = new Shortcut(Modifier.CTRL, '2');

        [TestInitialize]
        public void SetUp()
        {
            m_arrangementRepository = new MockArrangementRepository();
            m_windowRendererMock = new Mock<IWindowRenderer>();

            m_windowService = new WindowService(m_arrangementRepository, m_windowRendererMock.Object);
            m_workspace = new Workspace(m_windowService, m_arrangementRepository);

            m_shortcutListener = new MockShortcutListener();
            m_actionService = new ActionService(m_shortcutListener);
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
            m_screenA.Bounds = new Rectangle(-1080, 0, 1080, 1920);
            m_screenB = new HorizontalArrangement();
            m_screenB.Bounds = new Rectangle(0, 0, 1920, 1080);
            m_screenC = new HorizontalArrangement();
            m_screenC.Bounds = new Rectangle(0, 1080, 1280, 720);

            m_arrangementRepository.InsertRoot(m_screenA);
            m_arrangementRepository.InsertRoot(m_screenB);
            m_arrangementRepository.InsertRoot(m_screenC);
        }

        private void AddActions()
        {
            m_actionService.AddAction(new MoveWindowAction(MoveUp, m_workspace, Direction.Up));
            m_actionService.AddAction(new MoveWindowAction(MoveDown, m_workspace, Direction.Down));
            m_actionService.AddAction(new MoveWindowAction(MoveLeft, m_workspace, Direction.Left));
            m_actionService.AddAction(new MoveWindowAction(MoveRight, m_workspace, Direction.Right));


            m_actionService.AddAction(new SelectWindowAction(SelectUp, m_workspace, Direction.Up));
            m_actionService.AddAction(new SelectWindowAction(SelectDown, m_workspace, Direction.Down));
            m_actionService.AddAction(new SelectWindowAction(SelectLeft, m_workspace, Direction.Left));
            m_actionService.AddAction(new SelectWindowAction(SelectRight, m_workspace, Direction.Right));

            m_actionService.AddAction(new WrapWindowAction<HorizontalArrangement>(WrapHorizontal, m_workspace));
            m_actionService.AddAction(new WrapWindowAction<VerticalArrangement>(WrapVertical, m_workspace));
            m_actionService.AddAction(new UnWrapWindowAction(UnWrap, m_workspace));
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
                new Window(new Rectangle(0, 0, 1, 1), "window0"),
                new Window(new Rectangle(0, 0, 1, 1), "window1"),
                new Window(new Rectangle(0, 0, 1, 1), "window2"),
                new Window(new Rectangle(0, 0, 1, 1), "window3"),
                new Window(new Rectangle(0, 0, 1, 1), "window4"),
                new Window(new Rectangle(0, 0, 1, 1), "window5"),
                new Window(new Rectangle(0, 0, 1, 1), "window6"),
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

            var asdf = new (Shortcut shortcut, EntityID windowID)[]{
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
                m_shortcutListener.TriggerShortcut(wow.shortcut);

                //Assert
                Assert.AreEqual(wow.windowID, m_workspace.SelectedWindow, $"\nactual window: {m_arrangementRepository.FindParentOf(m_workspace.SelectedWindow).FindChild(m_workspace.SelectedWindow)}");
            });
        }


        [TestMethod]
        public void integration_move_selected_window()
        {
            //Arrange
            ScenarioOne();

            new Shortcut[]{
                SelectLeft,
                MoveRight,
                MoveDown,
            }.ToList().ForEach(shortcut => m_shortcutListener.TriggerShortcut(shortcut));

            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X,
                    m_screenA.Bounds.Y + m_screenA.Bounds.Height / 3,
                    m_screenA.Bounds.Width,
                    m_screenA.Bounds.Height / 3
                ),
                m_windows[0].Bounds
            );

            new Shortcut[]{
                SelectDown,
                MoveRight,
                MoveRight,
                MoveRight,
                MoveRight,
            }.ToList().ForEach(shortcut => m_shortcutListener.TriggerShortcut(shortcut));

            Assert.AreEqual(
                new Rectangle(
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
                new Window(new Rectangle(1, 1, 1, 1), "window0"),
                new Window(new Rectangle(1, 1, 1, 1), "window1"),
                new Window(new Rectangle(1, 1, 1, 1), "window2"),
            };
            m_windows.Reverse<Window>().ToList().ForEach(window => m_workspace.InsertWindow(window));

            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[0].Bounds
            );

            Assert.AreEqual(m_windows[2].ID, m_workspace.SelectedWindow);

            //Act
            new Shortcut[]{
                SelectLeft,
                SelectLeft,
                WrapVertical,
                SelectRight,
                MoveLeft,
            }.ToList().ForEach(shortcut => m_shortcutListener.TriggerShortcut(shortcut));

            //Assert
            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height / 2
                ),
                m_windows[0].Bounds
            );

            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y + m_screenB.Bounds.Height / 2,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height / 2
                ),
                m_windows[1].Bounds
            );

            Assert.AreEqual(
                new Rectangle(
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
                new Window(new Rectangle(1, 1, 1, 1), "window0"),
                new Window(new Rectangle(1, 1, 1, 1), "window1"),
                new Window(new Rectangle(1, 1, 1, 1), "window2"),
            };
            m_windows.Reverse<Window>().ToList().ForEach(window => m_workspace.InsertWindow(window));

            //Act
            new Shortcut[]{
                SelectLeft,
                SelectLeft,
                WrapVertical,
                SelectRight,
                MoveLeft,
                UnWrap,
            }.ToList().ForEach(shortcut => m_shortcutListener.TriggerShortcut(shortcut));

            //Assert
            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[0].Bounds
            );

            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X + m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 3,
                    m_screenB.Bounds.Height
                ),
                m_windows[1].Bounds
            );

            Assert.AreEqual(
                new Rectangle(
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