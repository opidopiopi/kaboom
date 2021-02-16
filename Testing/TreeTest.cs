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
    }
}
