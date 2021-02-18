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

        [TestInitialize]
        public void SetUp()
        {
            MockScreenProvider screenProvider = new MockScreenProvider(new List<Kaboom.Abstract.Rectangle>() { new Kaboom.Abstract.Rectangle(0, 0, 1080, 1920) });
            m_workspace = new Workspace(screenProvider);
        }

        [TestMethod]
        public void instantiating_workspace_creates_new_screen_with_root_arrangement()
        {
            Assert.AreEqual(typeof(Screen), m_workspace.Children()[0].GetType());
            Assert.AreEqual(typeof(WindowArrangement), m_workspace.Children()[0].Children()[0].GetType().BaseType);
        }
    }
}
