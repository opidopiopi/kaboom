using Kaboom.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Testing.Mocks;

namespace Testing
{
    [TestClass]
    public class WorkspaceTests
    {
        Workspace m_workspace;
        MockWindowProvider m_windowProvider = new MockWindowProvider();

        [TestInitialize]
        public void SetUp()
        {
            MockScreenProvider screenProvider = new MockScreenProvider(new List<Kaboom.Abstract.Rectangle>() { new Kaboom.Abstract.Rectangle(0, 0, 1080, 1920) });
            MockWindowBoundsSetter windowBoundsSetter = new MockWindowBoundsSetter();
            m_workspace = new Workspace(screenProvider, windowBoundsSetter);

            m_windowProvider.SetWindowAcceptor(m_workspace);
        }

        [TestMethod]
        public void instantiating_workspace_creates_new_screen_with_root_arrangement()
        {
            Assert.AreEqual(typeof(Screen), m_workspace.Children()[0].GetType());
            Assert.AreEqual(typeof(WindowArrangement), m_workspace.Children()[0].Children()[0].GetType().BaseType);
        }

        [TestMethod]
        public void inserting_window_works()
        {
            //arrange
            MockWindowIdentity windowA = new MockWindowIdentity(1);
            MockWindowIdentity windowB = new MockWindowIdentity(2);

            //act
            m_windowProvider.InsertWindow(windowA);
            m_windowProvider.InsertWindow(windowB);

            //assert
            Assert.AreEqual(windowA, ((Window) m_workspace.Children()[0].Children()[0].Children()[0]).Identity());
            Assert.AreEqual(windowB, ((Window) m_workspace.Children()[0].Children()[0].Children()[1]).Identity());
        }

        [TestMethod]
        public void removing_windows_works()
        {
            //arrange
            MockWindowIdentity windowA = new MockWindowIdentity(1);
            MockWindowIdentity windowB = new MockWindowIdentity(2);

            //act
            m_windowProvider.InsertWindow(windowA);
            m_windowProvider.InsertWindow(windowB);
            m_windowProvider.RemoveWindow(windowA);

            //assert
            Assert.AreEqual(1, m_workspace.Children()[0].Children()[0].Children().Count);
            Assert.AreEqual(windowB, ((Window)m_workspace.Children()[0].Children()[0].Children()[0]).Identity());
        }

        [TestMethod]
        public void same_window_can_only_be_inserted_once()
        {
            //arrange
            MockWindowIdentity windowA = new MockWindowIdentity(1);
            MockWindowIdentity windowB = new MockWindowIdentity(2);

            //act
            m_windowProvider.InsertWindow(windowA);
            m_windowProvider.InsertWindow(windowB);
            m_windowProvider.InsertWindow(windowA);

            //assert
            Assert.AreEqual(2, m_workspace.Children()[0].Children()[0].Children().Count);
            Assert.AreEqual(windowA, ((Window)m_workspace.Children()[0].Children()[0].Children()[0]).Identity());
            Assert.AreEqual(windowB, ((Window)m_workspace.Children()[0].Children()[0].Children()[1]).Identity());
        }

        [TestMethod]
        public void first_inserted_window_is_set_as_currently_selected_window()
        {
            //arrange
            MockWindowIdentity windowA = new MockWindowIdentity(1);

            //act
            m_windowProvider.InsertWindow(windowA);

            //assert
            Assert.AreEqual(windowA, m_workspace.CurrentlySelectedWindow().Identity());
        }

        [TestMethod]
        public void currently_selected_window_stays_when_inserting_more()
        {
            //arrange
            MockWindowIdentity windowA = new MockWindowIdentity(1);
            MockWindowIdentity windowB = new MockWindowIdentity(2);

            //act
            m_windowProvider.InsertWindow(windowA);
            m_windowProvider.InsertWindow(windowB);

            //assert
            Assert.AreEqual(windowA, m_workspace.CurrentlySelectedWindow().Identity());
        }

        [TestMethod]
        public void when_removing_selected_window_the_next_is_selected()
        {
            //arrange
            MockWindowIdentity windowA = new MockWindowIdentity(1);
            MockWindowIdentity windowB = new MockWindowIdentity(2);

            //act
            m_windowProvider.InsertWindow(windowA);
            m_windowProvider.InsertWindow(windowB);
            m_windowProvider.RemoveWindow(windowA);

            //assert
            Assert.AreEqual(windowA, m_workspace.CurrentlySelectedWindow().Identity());
        }
    }
}
