using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kaboom.Model;
using System.Collections.Generic;

namespace Testing
{
    [TestClass]
    public class TreeTest
    {
        [TestMethod]
        public void tiling_window_manager_sets_up_workspace_with_screens()
        {
            Screen screenA = new Screen(new Kaboom.Abstract.Rectangle(0, 0, 1920, 1080));
            Screen screenB = new Screen(new Kaboom.Abstract.Rectangle(0, 1080, 1920, 1080));

            MockScreenProvider screenProvider = new MockScreenProvider(new List<Screen>(new Screen[]{screenA, screenB}));
            MockWindowProvider windowProvider = new MockWindowProvider();

            TilingWindowManager windowManager = new TilingWindowManager(windowProvider, screenProvider);

            Assert.AreEqual(windowManager.GetWorkspace(), screenA.GetParent());
            Assert.AreEqual(screenA.GetParent(), screenB.GetParent());
        }
    }
}
