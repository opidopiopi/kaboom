using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Model;
using System.Collections.Generic;

namespace Testing
{
    [TestClass]
    public class TreeTest
    {
        Screen m_screenA;
        Screen m_screenB;

        MockScreenProvider m_screenProvider;
        MockWindowProvider m_windowProvider;

        TilingWindowManager m_windowManager;

        [TestInitialize]
        public void SetUp()
        {
            m_screenA = new Screen(new Kaboom.Abstract.Rectangle(0, 0, 1920, 1080));
            m_screenB = new Screen(new Kaboom.Abstract.Rectangle(0, 1080, 1920, 1080));

            m_screenProvider = new MockScreenProvider(new List<Screen>(){ m_screenA, m_screenB });
            m_windowProvider = new MockWindowProvider();

            m_windowManager = new TilingWindowManager(m_windowProvider, m_screenProvider);
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
            Window windowScreenA = new Window(null, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            Window windowScreenB = new Window(null, new Kaboom.Abstract.Rectangle(100, 2000, 400, 400));

            m_windowProvider.InsertWindow(windowScreenA);
            m_windowProvider.InsertWindow(windowScreenB);

            Assert.AreEqual(m_screenA, windowScreenA.GetParent().GetParent());
            Assert.AreEqual(m_screenB, windowScreenB.GetParent().GetParent());
        }

        [TestMethod]
        public void windows_can_be_removed()
        {
            Window windowScreenA = new Window(null, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            Window windowScreenB = new Window(null, new Kaboom.Abstract.Rectangle(100, 2000, 400, 400));

            m_windowProvider.InsertWindow(windowScreenA);
            m_windowProvider.InsertWindow(windowScreenB);

            m_windowProvider.RemoveWindow(windowScreenA);
            Assert.IsFalse(m_screenA.Children()[0].Children().Contains(windowScreenA));

            m_windowProvider.RemoveWindow(windowScreenB);
            Assert.IsFalse(m_screenB.Children()[0].Children().Contains(windowScreenB));
        }

        [TestMethod]
        public void bounds_of_inserted_window_will_be_updated()
        {
            Window windowScreenA = new Window(null, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
            Window windowScreenB = new Window(null, new Kaboom.Abstract.Rectangle(100, 2000, 400, 400));

            m_windowProvider.InsertWindow(windowScreenA);
            m_windowProvider.InsertWindow(windowScreenB);

            Assert.AreEqual(windowScreenA.Bounds, m_screenA.Bounds);
            Assert.AreEqual(windowScreenB.Bounds, m_screenB.Bounds);


            Window windowScreenA_new = new Window(null, new Kaboom.Abstract.Rectangle(100, 100, 400, 400));
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

            Assert.AreEqual(windowScreenA.Bounds, screenA_LeftHalf);
            Assert.AreEqual(windowScreenA_new.Bounds, screenA_RightHalf);

            Assert.AreEqual(windowScreenB.Bounds, m_screenB.Bounds);
        }
    }
}
