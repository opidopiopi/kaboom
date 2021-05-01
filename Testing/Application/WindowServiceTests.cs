using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Application
{
    [TestClass]
    public class WindowServiceTests
    {
        private WindowService m_windowService;
        private MockArrangementRepository m_arrgangementRepo;
        private MockWindowRenderer m_renderer;

        private MockArrangement m_rootA;
        private MockArrangement m_rootB;
        private MockArrangement m_levelOneA;
        private MockArrangement m_levelOneB;
        private MockArrangement m_levelTwoA;
        private MockArrangement m_levelTwoB;
        private MockArrangement m_levelThree;

        private Window[] m_windows;

        [TestInitialize]
        public void SetUp()
        {
            m_renderer = new MockWindowRenderer();
            m_arrgangementRepo = new MockArrangementRepository();
            m_windowService = new WindowService(m_arrgangementRepo, m_renderer);


            /*  m_rootA
             *  |
             *  |--m_levelOneA
             *  |  |
             *  |  |--m_levelTwoA
             *  |  |  |
             *  |  |  |--m_levelThree
             *  |  |  |  |
             *  |  |  |  |--m_windows[5]
             *  |  |  |
             *  |  |  |--m_windows[3]
             *  |  |
             *  |  |--m_levelTwoB
             *  |  |  |
             *  |  |  |--m_windows[4]
             *  |  |
             *  |  |--m_windows[1]
             *  |
             *  |--m_levelOneB
             *  |  |
             *  |  |--m_windows[2]
             *  |
             *  |--m_windows[0]
             *  
             *  m_rootB
             */

            m_rootA = new MockArrangement("m_rootA", Axis.X, Axis.Y);
            m_rootB = new MockArrangement("m_rootB", Axis.X, Axis.Y);
            m_rootA.Bounds = new Bounds(0, 0, 100, 100);
            m_rootB.Bounds = new Bounds(100, 0, 100, 100);
            m_arrgangementRepo.InsertRoot(m_rootA);
            m_arrgangementRepo.InsertRoot(m_rootB);

            m_levelOneA = new MockArrangement("m_levelOneA", Axis.X, Axis.Y);
            m_levelOneB = new MockArrangement("m_levelOneB", Axis.X, Axis.Y);
            m_rootA.InsertAsLast(m_levelOneA);
            m_rootA.InsertAsLast(m_levelOneB);

            m_levelTwoA = new MockArrangement("m_levelTwoA", Axis.X, Axis.Y);
            m_levelTwoB = new MockArrangement("m_levelTwoB", Axis.X, Axis.Y);
            m_levelOneA.InsertAsLast(m_levelTwoA);
            m_levelOneA.InsertAsLast(m_levelTwoB);

            m_levelThree = new MockArrangement("m_levelThree", Axis.X, Axis.Y);
            m_levelTwoA.InsertAsLast(m_levelThree);

            m_windows = Enumerable.Range(0, 6).Select(i => new Window(new Bounds(1, 1, 1, 1), $"testWindow_{i}")).ToArray();

            m_rootA.InsertAsLast(m_windows[0]);
            m_levelOneA.InsertAsLast(m_windows[1]);
            m_levelOneB.InsertAsLast(m_windows[2]);
            m_levelTwoA.InsertAsLast(m_windows[3]);
            m_levelTwoB.InsertAsLast(m_windows[4]);
            m_levelThree.InsertAsLast(m_windows[5]);
        }

        private void AssertSequenceEqual(IEnumerable<IBoundedTreeNode> actual, IEnumerable<IBoundedTreeNode> expected)
        {
            Assert.IsTrue(Enumerable.SequenceEqual(actual, expected),
                $"expected: [\n    {expected.Select(node => node.ToString()).Aggregate((a, b) => a + ",\n    " + b)}\n]\n" +
                $"actual: [\n    {actual.Select(node => node.ToString()).Aggregate((a, b) => a + ",\n    " + b)}\n]\n");
        }


        [TestMethod]
        public void windowservice_can_insert_windows()
        {
            //Arrange
            Window newWindow = new Window(new Bounds(10, 10, 20, 20), "windowInRootA");
            Window anotherNewWindow = new Window(new Bounds(110, 10, 20, 20), "windowInRootB");
            Window onceAgainANewWindow = new Window(new Bounds(69, 420, 20, 20), "windowOutsideOfBothRootsBoundsSoItShouldLandInRootA");

            //Act
            m_windowService.InsertWindowIntoTree(newWindow);
            m_windowService.InsertWindowIntoTree(anotherNewWindow);
            m_windowService.InsertWindowIntoTree(onceAgainANewWindow);

            //Assert
            Assert.AreEqual(m_arrgangementRepo.FindParentOf(newWindow.ID), m_rootA);
            Assert.IsTrue(m_rootA.MyChildren.Contains(newWindow));

            Assert.AreEqual(m_arrgangementRepo.FindParentOf(anotherNewWindow.ID), m_rootB);
            Assert.IsTrue(m_rootB.MyChildren.Contains(anotherNewWindow));

            Assert.AreEqual(m_arrgangementRepo.FindParentOf(onceAgainANewWindow.ID), m_rootA);
            Assert.IsTrue(m_rootA.MyChildren.Contains(onceAgainANewWindow));
        }


        [TestMethod]
        public void windowservice_can_remove_windows()
        {
            //Arrange

            //Act
            Assert.AreEqual(m_arrgangementRepo.FindParentOf(m_windows[3].ID), m_levelTwoA);
            m_windowService.RemoveWindow(m_windows[3].ID);

            //Assert
            Assert.IsNull(m_arrgangementRepo.FindParentOf(m_windows[3].ID));
            Assert.IsFalse(m_levelTwoA.MyChildren.Contains(m_windows[3]));
        }


        [TestMethod]
        public void windowservice_rerenders_all_windows_downtree_after_insert()
        {
            //Arrange

            //Act
            var newWindow = new Window(new Bounds(10, 10, 10, 10), "wow");
            m_windowService.InsertWindowIntoTree(newWindow);

            //Assert
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(newWindow));
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(m_windows[0]));
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(m_windows[1]));
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(m_windows[2]));
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(m_windows[3]));
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(m_windows[4]));
            Assert.IsTrue(m_renderer.RenderedWindows.Contains(m_windows[5]));
        }

        [TestMethod]
        public void windowservice_rerenders_all_windows_in_tree_after_removing_a_window()
        {
            //Arrange

            //Act
            m_windowService.RemoveWindow(m_windows[1].ID);

            //Assert
            m_windows.ToList().ForEach(window => m_renderer.RenderedWindows.Contains(window));
        }

        [TestMethod]
        public void windowservice_updates_bounds_of_all_children_after_insert()
        {
            //Arrange
            m_rootA.Updated = false;
            m_levelOneA.Updated = false;
            m_levelOneB.Updated = false;
            m_levelTwoA.Updated = false;
            m_levelTwoB.Updated = false;
            m_levelThree.Updated = false;

            //Act
            var newWindow = new Window(new Bounds(10, 10, 10, 10), "wow");
            m_windowService.InsertWindowIntoTree(newWindow);

            //Assert
            Assert.IsTrue(m_rootA.Updated);
            Assert.IsTrue(m_levelOneA.Updated);
            Assert.IsTrue(m_levelOneB.Updated);
            Assert.IsTrue(m_levelTwoA.Updated);
            Assert.IsTrue(m_levelTwoB.Updated);
            Assert.IsTrue(m_levelThree.Updated);
        }

        [TestMethod]
        public void windowservice_updates_bounds_of_tree_after_removing_window()
        {
            //Arrange
            m_rootA.Updated = false;
            m_levelOneA.Updated = false;
            m_levelOneB.Updated = false;
            m_levelTwoA.Updated = false;
            m_levelTwoB.Updated = false;
            m_levelThree.Updated = false;

            //Act
            m_windowService.RemoveWindow(m_windows[1].ID);

            //Assert
            Assert.IsTrue(m_rootA.Updated);
            Assert.IsTrue(m_levelOneA.Updated);
            Assert.IsTrue(m_levelOneB.Updated);
            Assert.IsTrue(m_levelTwoA.Updated);
            Assert.IsTrue(m_levelTwoB.Updated);
            Assert.IsTrue(m_levelThree.Updated);
        }


        [TestMethod]
        public void windowservice_can_move_child_swapping()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "asdf");
            m_levelTwoA.InsertAsLast(otherWindow);

            var expected = new IBoundedTreeNode[] { m_levelThree, m_windows[3], otherWindow };
            CollectionAssert.AreEqual(expected, m_levelTwoA.MyChildren);

            //Act
            m_windowService.MoveWindow(otherWindow.ID, Direction.Up);
            expected = new IBoundedTreeNode[] { m_levelThree, otherWindow, m_windows[3] };

            //Assert
            CollectionAssert.AreEqual(expected, m_levelTwoA.MyChildren);

            //Act
            m_windowService.MoveWindow(otherWindow.ID, Direction.Down);
            expected = new IBoundedTreeNode[] { m_levelThree, m_windows[3], otherWindow };

            //Assert
            CollectionAssert.AreEqual(expected, m_levelTwoA.MyChildren);
        }

        [TestMethod]
        public void windowservice_can_move_child_inserting()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "asdf");
            m_levelTwoA.InsertAsFirst(otherWindow);

            var expected = new IBoundedTreeNode[] { otherWindow, m_levelThree, m_windows[3]};
            CollectionAssert.AreEqual(expected, m_levelTwoA.MyChildren);

            //Act
            m_windowService.MoveWindow(m_windows[3].ID, Direction.Up);
            expected = new IBoundedTreeNode[] { otherWindow, m_levelThree };

            //Assert
            CollectionAssert.AreEqual(expected, m_levelTwoA.MyChildren);

            //Act
            m_windowService.MoveWindow(otherWindow.ID, Direction.Down);
            expected = new IBoundedTreeNode[] { m_levelThree };

            //Assert
            CollectionAssert.AreEqual(expected, m_levelTwoA.MyChildren);
        }

        [TestMethod]
        public void windowservice_can_move_child_move_to_parent()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "otherWindow");
            var anotherWindow = new Window(new Bounds(10, 10, 10, 10), "anotherWindow");

            m_levelTwoB.InsertAsFirst(otherWindow);
            m_levelOneA.InsertAsFirst(anotherWindow);

            //Act
            m_windowService.MoveWindow(otherWindow.ID, Direction.Up);
            m_windowService.MoveWindow(m_windows[3].ID, Direction.Down);
            m_windowService.MoveWindow(m_windows[5].ID, Direction.Up);

            //Assert
            Assert.AreEqual(m_levelOneA, m_arrgangementRepo.FindParentOf(otherWindow.ID));
            Assert.AreEqual(m_levelOneA, m_arrgangementRepo.FindParentOf(m_windows[3].ID));
            Assert.AreEqual(m_levelTwoA, m_arrgangementRepo.FindParentOf(m_windows[5].ID));
            AssertSequenceEqual(m_levelOneA.MyChildren, new IBoundedTreeNode[] { anotherWindow, m_levelTwoA, m_windows[3], otherWindow, m_levelTwoB, m_windows[1] });
        }

        [TestMethod]
        public void windowservice_can_move_to_different_root_arrangement()
        {
            //Arrange

            //Act
            m_windowService.MoveWindow(m_windows[0].ID, Direction.Down);

            //Assert
            Assert.AreEqual(m_rootB, m_arrgangementRepo.FindParentOf(m_windows[0].ID));
            AssertSequenceEqual(m_rootB.MyChildren, new IBoundedTreeNode[] { m_windows[0] });

            //Act
            m_windowService.MoveWindow(m_windows[0].ID, Direction.Up);

            //Assert
            Assert.AreEqual(m_rootA, m_arrgangementRepo.FindParentOf(m_windows[0].ID));
            AssertSequenceEqual(m_rootA.MyChildren, new IBoundedTreeNode[] { m_levelOneA, m_levelOneB, m_windows[0] });
        }


        [TestMethod]
        public void windowservice_can_select_windows_on_same_level()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "otherWindow");
            var anotherWindow = new Window(new Bounds(10, 10, 10, 10), "anotherWindow");
            m_levelThree.InsertAsLast(otherWindow);
            m_levelThree.InsertAsLast(anotherWindow);

            //Act
            var selectUp = m_windowService.NextWindowInDirection(Direction.Up, otherWindow.ID);
            var selectLeft = m_windowService.NextWindowInDirection(Direction.Left, otherWindow.ID);
            var selectDown = m_windowService.NextWindowInDirection(Direction.Down, otherWindow.ID);
            var selectRight = m_windowService.NextWindowInDirection(Direction.Right, otherWindow.ID);

            //Assert
            Assert.AreEqual(m_windows[5].ID, selectUp);
            Assert.AreEqual(m_windows[5].ID, selectLeft);

            Assert.AreEqual(anotherWindow.ID, selectDown);
            Assert.AreEqual(anotherWindow.ID, selectRight);
        }

        [TestMethod]
        public void windowservice_can_select_windows_on_different_level()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "otherWindow");
            m_levelTwoA.InsertAsFirst(otherWindow);

            //Act
            var selectUp = m_windowService.NextWindowInDirection(Direction.Up, m_windows[5].ID);
            var selectLeft = m_windowService.NextWindowInDirection(Direction.Left, m_windows[5].ID);
            var selectDown = m_windowService.NextWindowInDirection(Direction.Down, m_windows[5].ID);
            var selectRight = m_windowService.NextWindowInDirection(Direction.Right, m_windows[5].ID);

            //Assert
            Assert.AreEqual(otherWindow.ID, selectUp);
            Assert.AreEqual(otherWindow.ID, selectLeft);

            Assert.AreEqual(m_windows[3].ID, selectDown);
            Assert.AreEqual(m_windows[3].ID, selectRight);
        }

        [TestMethod]
        public void windowservice_can_select_windows_on_lower_level_inside_other_arrangement()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "otherWindow");
            m_levelTwoA.InsertAsFirst(otherWindow);

            //Act
            var selectUp = m_windowService.NextWindowInDirection(Direction.Up, m_windows[3].ID);
            var selectLeft = m_windowService.NextWindowInDirection(Direction.Left, m_windows[3].ID);
            var selectDown = m_windowService.NextWindowInDirection(Direction.Down, otherWindow.ID);
            var selectRight = m_windowService.NextWindowInDirection(Direction.Right, otherWindow.ID);

            //Assert
            Assert.AreEqual(m_windows[5].ID, selectUp);
            Assert.AreEqual(m_windows[5].ID, selectLeft);

            Assert.AreEqual(m_windows[5].ID, selectDown);
            Assert.AreEqual(m_windows[5].ID, selectRight);
        }

        [TestMethod]
        public void windowservice_can_select_windows_on_same_level_inside_other_arrangement()
        {
            //Arrange

            //Act
            var selectDown = m_windowService.NextWindowInDirection(Direction.Down, m_windows[3].ID);
            var selectRight = m_windowService.NextWindowInDirection(Direction.Right, m_windows[3].ID);

            //Assert
            Assert.AreEqual(m_windows[4].ID, selectDown);
            Assert.AreEqual(m_windows[4].ID, selectRight);
        }

        [TestMethod]
        public void windowservice_can_select_windows_in_other_root_arrangement()
        {
            //Arrange
            var otherWindow = new Window(new Bounds(10, 10, 10, 10), "otherWindow");
            m_rootB.InsertAsFirst(otherWindow);

            //Act
            var selectUp = m_windowService.NextWindowInDirection(Direction.Up, otherWindow.ID);
            var selectLeft = m_windowService.NextWindowInDirection(Direction.Left, otherWindow.ID);
            var selectDown = m_windowService.NextWindowInDirection(Direction.Down, m_windows[0].ID);
            var selectRight = m_windowService.NextWindowInDirection(Direction.Right, m_windows[0].ID);

            //Assert
            Assert.AreEqual(m_windows[0].ID, selectUp);
            Assert.AreEqual(m_windows[0].ID, selectLeft);

            Assert.AreEqual(otherWindow.ID, selectDown);
            Assert.AreEqual(otherWindow.ID, selectRight);
        }
    }
}