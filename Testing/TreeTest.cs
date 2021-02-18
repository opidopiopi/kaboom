using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Model;
using System.Collections.Generic;
using Testing.Mocks;

namespace Testing
{
    [TestClass]
    public class TreeTest
    {
        Screen m_screenA;
        Screen m_screenB;

        MockScreenProvider m_screenProvider;
        MockWindowProvider m_windowProvider;
        MockWindowBoundsSetter m_windowBoundsSetter;

        TilingWindowManager m_windowManager;

        [TestInitialize]
        public void SetUp()
        {
            m_screenA = new Screen(new Kaboom.Abstract.Rectangle(0, 0, 1920, 1080));
            m_screenB = new Screen(new Kaboom.Abstract.Rectangle(0, 1080, 1920, 1080));

            m_screenProvider = new MockScreenProvider(new List<Screen>(){ m_screenA, m_screenB });
            m_windowProvider = new MockWindowProvider();
            m_windowBoundsSetter = new MockWindowBoundsSetter();

            m_windowManager = new TilingWindowManager(m_windowProvider, m_screenProvider, m_windowBoundsSetter);
        }


        [TestMethod]
        public void tiling_window_manager_sets_up_workspace_with_screens()
        {
            Assert.AreEqual(m_windowManager.GetWorkspace(), m_screenA.GetParent());
            Assert.AreEqual(m_screenA.GetParent(), m_screenB.GetParent());
        }

        [TestMethod]
        public void new_window_will_be_inserted_under_correct_screen_root_arrangement()
        {
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);

            m_windowProvider.InsertWindow(windowScreenA, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Kaboom.Abstract.Rectangle(100, 2000, 400, 400));

            Assert.AreEqual(windowScreenA, ((Window) m_screenA.Children()[0].Children()[0]).Identity());
            Assert.AreEqual(windowScreenB, ((Window) m_screenB.Children()[0].Children()[0]).Identity());
        }

        [TestMethod]
        public void windows_can_be_removed()
        {
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);
            
            m_windowProvider.InsertWindow(windowScreenA, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Kaboom.Abstract.Rectangle(100, 2000, 400, 400));

            m_windowProvider.RemoveWindow(windowScreenA);
            Assert.AreEqual(0, m_screenA.Children()[0].Children().Count);
            Assert.AreEqual(1, m_screenB.Children()[0].Children().Count);

            m_windowProvider.RemoveWindow(windowScreenB);
            Assert.AreEqual(0, m_screenA.Children()[0].Children().Count);
            Assert.AreEqual(0, m_screenB.Children()[0].Children().Count);
        }

        [TestMethod]
        public void bounds_of_inserted_window_will_be_updated()
        {
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);
            MockWindowIdentity windowScreenB = new MockWindowIdentity(2);

            m_windowProvider.InsertWindow(windowScreenA, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            m_windowProvider.InsertWindow(windowScreenB, new Kaboom.Abstract.Rectangle(100, 2000, 400, 400));

            Assert.AreEqual(m_screenA.Bounds, ((Window)m_screenA.Children()[0].Children()[0]).Bounds);
            Assert.AreEqual(m_screenB.Bounds, ((Window)m_screenB.Children()[0].Children()[0]).Bounds);


            MockWindowIdentity windowScreenA_new = new MockWindowIdentity(3);
            Kaboom.Abstract.Rectangle screenA_LeftHalf = new Kaboom.Abstract.Rectangle(
                m_screenA.Bounds.X,
                m_screenA.Bounds.Y,
                m_screenA.Bounds.Width / 2,
                m_screenA.Bounds.Height);
            Kaboom.Abstract.Rectangle screenA_RightHalf = new Kaboom.Abstract.Rectangle(
                m_screenA.Bounds.X + m_screenA.Bounds.Width / 2,
                m_screenA.Bounds.Y,
                m_screenA.Bounds.Width / 2,
                m_screenA.Bounds.Height);

            m_windowProvider.InsertWindow(windowScreenA_new);

            Assert.AreEqual(screenA_LeftHalf, ((Window)m_screenA.Children()[0].Children()[0]).Bounds);
            Assert.AreEqual(screenA_RightHalf, ((Window)m_screenA.Children()[0].Children()[1]).Bounds);

            Assert.AreEqual(m_screenB.Bounds, ((Window)m_screenB.Children()[0].Children()[0]).Bounds);
        }

        [TestMethod]
        public void inserting_new_window_triggers_ISetApplicationPosition_on_bounds_update()
        {
            MockWindowIdentity windowScreenA = new MockWindowIdentity(1);

            m_windowProvider.InsertWindow(windowScreenA);

            Assert.AreEqual(m_windowBoundsSetter.TheIdentitiesAndBoundsIHaveSet[windowScreenA], m_screenA.Bounds);
        }
    }
}
