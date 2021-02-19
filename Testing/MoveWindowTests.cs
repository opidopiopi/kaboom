using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Model;
using Testing.Mocks;
using Kaboom.Abstract;

namespace Testing
{
    [TestClass]
    public class MoveWindowTests : IntegrationTestBase
    {
        [TestInitialize]
        public void SetUp()
        {
            Initialize();
        }


        [TestMethod]
        public void we_can_move_the_currently_selected_window_down()
        {
            //arrange
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);

            m_windowProvider.InsertWindow(windowScreenA, new Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Rectangle(100, 2000, 400, 400));

            //act
            //move windowScreenA to screenB
            m_windowManager.GetWorkspace().CurrentlySelectedWindow().MoveDown();

            //assert
            Assert.AreEqual(windowScreenA, ((Window)m_screenB.Children()[0].Children()[1]).Identity());
            Assert.AreEqual(windowScreenB, ((Window)m_screenB.Children()[0].Children()[0]).Identity());

            Assert.AreEqual(0, m_screenA.Children()[0].Children().Count);

            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X + m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height),
                ((Window)m_screenB.Children()[0].Children()[1]).Bounds);   //windowScreenA
            Assert.AreEqual(
                new Rectangle(
                    m_screenB.Bounds.X,
                    m_screenB.Bounds.Y,
                    m_screenB.Bounds.Width / 2,
                    m_screenB.Bounds.Height),
                ((Window)m_screenB.Children()[0].Children()[0]).Bounds);   //windowScreenB
        }

        [TestMethod]
        public void we_can_move_the_currently_selected_window_up()
        {
            //arrange
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);

            m_windowProvider.InsertWindow(windowScreenA, new Rectangle(100, 2000, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Rectangle(100, 100, 400, 400));

            //act
            //move windowScreenB to screenA
            m_windowManager.GetWorkspace().CurrentlySelectedWindow().MoveUp();

            //assert
            Assert.AreEqual(windowScreenA, ((Window)m_screenA.Children()[0].Children()[1]).Identity());
            Assert.AreEqual(windowScreenB, ((Window)m_screenA.Children()[0].Children()[0]).Identity());

            Assert.AreEqual(0, m_screenB.Children()[0].Children().Count);

            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X + m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Y,
                    m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Height),
                ((Window)m_screenA.Children()[0].Children()[1]).Bounds);   //windowScreenA
            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X,
                    m_screenA.Bounds.Y,
                    m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Height),
                ((Window)m_screenA.Children()[0].Children()[0]).Bounds);   //windowScreenB
        }

        [TestMethod]
        public void we_can_move_the_currently_selected_window_right()
        {
            //arrange
            MockWindowIdentity left = new MockWindowIdentity(1);
            MockWindowIdentity right = new MockWindowIdentity(2);

            m_windowProvider.InsertWindow(left, new Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(right, new Rectangle(100, 100, 400, 400));

            //act
            //move 'left' right
            m_windowManager.GetWorkspace().CurrentlySelectedWindow().MoveRight();

            //assert
            Assert.AreEqual(left, ((Window)m_screenA.Children()[0].Children()[1]).Identity());
            Assert.AreEqual(right, ((Window)m_screenA.Children()[0].Children()[0]).Identity());

            Assert.AreEqual(0, m_screenB.Children()[0].Children().Count);

            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X + m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Y,
                    m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Height),
                ((Window)m_screenA.Children()[0].Children()[1]).Bounds);   //left
            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X,
                    m_screenA.Bounds.Y,
                    m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Height),
                ((Window)m_screenA.Children()[0].Children()[0]).Bounds);   //right
        }

        [TestMethod]
        public void we_can_move_the_currently_selected_window_left()
        {
            //arrange
            MockWindowIdentity left = new MockWindowIdentity(1);
            MockWindowIdentity right = new MockWindowIdentity(2);

            m_windowProvider.InsertWindow(left, new Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(right, new Rectangle(100, 100, 400, 400));

            //act
            //move 'left' left to screenB
            m_windowManager.GetWorkspace().CurrentlySelectedWindow().MoveLeft();
            //move 'left' left to screenA
            m_windowManager.GetWorkspace().CurrentlySelectedWindow().MoveLeft();

            //assert
            Assert.AreEqual(left, ((Window)m_screenA.Children()[0].Children()[1]).Identity());
            Assert.AreEqual(right, ((Window)m_screenA.Children()[0].Children()[0]).Identity());

            Assert.AreEqual(0, m_screenB.Children()[0].Children().Count);

            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X + m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Y,
                    m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Height),
                ((Window)m_screenA.Children()[0].Children()[1]).Bounds);   //left
            Assert.AreEqual(
                new Rectangle(
                    m_screenA.Bounds.X,
                    m_screenA.Bounds.Y,
                    m_screenA.Bounds.Width / 2,
                    m_screenA.Bounds.Height),
                ((Window)m_screenA.Children()[0].Children()[0]).Bounds);   //right
        }

        /*                              //the test itself doesn't work yet, but I'm done for the day
        [TestMethod]
        public void move_window()
        {
            /* ScreenA
             * |
             * |----RootArrangement(Horizontal)
             * |    |
             * |    |---leftArrangement
             * |    |   |
             * |    |   |----left  >--------\
             * |    |                       | MoveRight()
             * |    |---rightArrangement    |
             * |    |   |          <--------/
             * |    |   |---right
             

            //arrange
            MockWindowIdentity left = new MockWindowIdentity(1);
            MockWindowIdentity right = new MockWindowIdentity(2);
            HorizontalArrangement leftArrangement = new HorizontalArrangement();
            HorizontalArrangement rightArrangement = new HorizontalArrangement();

            m_screenA.Insert((ITreeNode) leftArrangement);  //left becomes currentlySelected
            m_screenA.Insert((ITreeNode) rightArrangement);
            m_windowProvider.InsertWindow(left, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(right, new Kaboom.Abstract.Rectangle(rightArrangement.Bounds.X + 10, 100, 400, 400));

            //assert
            Assert.AreEqual(left, ((Window) m_screenA.Children()[0].Children()[0].Children()[0]).Identity());
            Assert.AreEqual(right, ((Window) m_screenA.Children()[0].Children()[1].Children()[0]).Identity());

            //act
            //move 'left' right to rightArrangement
            m_windowManager.GetWorkspace().CurrentlySelectedWindow().MoveRight();

            //assert
            Assert.AreEqual(left, ((Window)m_screenA.Children()[0].Children()[1].Children()[1]).Identity());
            Assert.AreEqual(right, ((Window)m_screenA.Children()[0].Children()[1].Children()[0]).Identity());

            Assert.AreEqual(0, m_screenA.Children()[0].Children()[0].Children().Count);
        }
        */
    }
}
