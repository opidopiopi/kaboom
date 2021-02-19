using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Model;
using Testing.Mocks;
using Kaboom.Abstract;

namespace Testing
{
    [TestClass]
    public class IntegrationTests : IntegrationTestBase
    {
        [TestInitialize]
        public void SetUp()
        {
            Initialize();
        }


        [TestMethod]
        public void tiling_window_manager_sets_up_workspace_with_screens()
        {
            //arrange
            //SetUp()

            //act
            //SetUp()

            //assert
            Assert.AreEqual(m_windowManager.GetWorkspace(), m_screenA.GetParent());
            Assert.AreEqual(m_screenA.GetParent(), m_screenB.GetParent());
        }

        [TestMethod]
        public void new_window_will_be_inserted_under_correct_screen_root_arrangement()
        {
            //arrange
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);

            //act
            m_windowProvider.InsertWindow(windowScreenA, new Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Rectangle(100, 2000, 400, 400));

            //assert
            Assert.AreEqual(windowScreenA, ((Window) m_screenA.Children()[0].Children()[0]).Identity());
            Assert.AreEqual(windowScreenB, ((Window) m_screenB.Children()[0].Children()[0]).Identity());
        }

        [TestMethod]
        public void windows_can_be_removed()
        {
            //arrange
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);
            
            m_windowProvider.InsertWindow(windowScreenA, new Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Rectangle(100, 2000, 400, 400));

            //act
            m_windowProvider.RemoveWindow(windowScreenA);

            //assert
            Assert.AreEqual(0, m_screenA.Children()[0].Children().Count);
            Assert.AreEqual(1, m_screenB.Children()[0].Children().Count);

            //act
            m_windowProvider.RemoveWindow(windowScreenB);

            //assert
            Assert.AreEqual(0, m_screenA.Children()[0].Children().Count);
            Assert.AreEqual(0, m_screenB.Children()[0].Children().Count);
        }

        [TestMethod]
        public void bounds_of_inserted_window_will_be_updated()
        {
            //arrange
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);

            MockWindowIdentity windowScreenA_new = new MockWindowIdentity(3);
            Kaboom.Abstract.Rectangle screenA_LeftHalf = new Rectangle(
                m_screenA.Bounds.X,
                m_screenA.Bounds.Y,
                m_screenA.Bounds.Width / 2,
                m_screenA.Bounds.Height);
            Kaboom.Abstract.Rectangle screenA_RightHalf = new Rectangle(
                m_screenA.Bounds.X + m_screenA.Bounds.Width / 2,
                m_screenA.Bounds.Y,
                m_screenA.Bounds.Width / 2,
                m_screenA.Bounds.Height);


            //act
            m_windowProvider.InsertWindow(windowScreenA, new Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Rectangle(100, 2000, 400, 400));

            //assert
            Assert.AreEqual(m_screenA.Bounds, ((Window)m_screenA.Children()[0].Children()[0]).Bounds);
            Assert.AreEqual(m_screenB.Bounds, ((Window)m_screenB.Children()[0].Children()[0]).Bounds);

            //act
            m_windowProvider.InsertWindow(windowScreenA_new);

            //assert
            Assert.AreEqual(screenA_LeftHalf, ((Window)m_screenA.Children()[0].Children()[0]).Bounds);
            Assert.AreEqual(screenA_RightHalf, ((Window)m_screenA.Children()[0].Children()[1]).Bounds);

            Assert.AreEqual(m_screenB.Bounds, ((Window)m_screenB.Children()[0].Children()[0]).Bounds);
        }

        [TestMethod]
        public void inserting_new_window_triggers_ISetApplicationPosition_on_bounds_update()
        {
            //arrange
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);

            //act
            m_windowProvider.InsertWindow(windowScreenA);

            //assert
            Assert.AreEqual(m_windowBoundsSetter.TheIdentitiesAndBoundsIHaveSet[windowScreenA], m_screenA.Bounds);
        }
    }
}
