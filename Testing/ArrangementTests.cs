using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Mocks;

namespace Testing
{
    [TestClass]
    public class ArrangementTests
    {
        MockArrangement m_arrangement;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new MockArrangement(new Kaboom.Abstract.Rectangle(0, 0, 500, 500));
        }

        [TestMethod]
        public void inserting_child_sets_childs_parent()
        {
            MockTreeNodeWithBounds node = new MockTreeNodeWithBounds();

            m_arrangement.Insert(node);

            Assert.IsTrue(m_arrangement.Children().Contains(node));
            Assert.AreEqual(m_arrangement, node.GetParent());
        }

        [TestMethod]
        public void inserting_child_updates_child_bounds()
        {
            MockTreeNodeWithBounds node = new MockTreeNodeWithBounds();

            m_arrangement.Insert(node);

            Assert.IsTrue(m_arrangement.Children().Contains(node));
            Assert.AreEqual(m_arrangement.Bounds, node.Bounds); 
        }

        [TestMethod]
        public void removing_direct_child_()
        {
            MockTreeNodeWithBounds node = new MockTreeNodeWithBounds();
            MockTreeNodeWithBounds anotherNode = new MockTreeNodeWithBounds();

            m_arrangement.Insert(node);
            Assert.IsTrue(m_arrangement.Children().Contains(node));

            Assert.IsFalse(m_arrangement.RemoveAndReturnSuccess(anotherNode));

            m_arrangement.Insert(anotherNode);

            Assert.IsTrue(m_arrangement.Children().Contains(anotherNode));
            Assert.IsTrue(m_arrangement.RemoveAndReturnSuccess(anotherNode));
            Assert.IsFalse(m_arrangement.Children().Contains(anotherNode));

            Assert.IsTrue(m_arrangement.Children().Contains(node));
        }

        [TestMethod]
        public void removing_child_of_child_()
        {
            MockArrangement childArrangement = new MockArrangement(new Kaboom.Abstract.Rectangle(5, 5, 55, 55));
            MockTreeNodeWithBounds childNode = new MockTreeNodeWithBounds();

            m_arrangement.Insert(childArrangement);

            //not here yet
            Assert.IsFalse(m_arrangement.RemoveAndReturnSuccess(childNode));

            childArrangement.Insert(childNode);
            Assert.IsTrue(m_arrangement.Children()[0].Children().Contains(childNode));

            //child of child should be removed
            Assert.IsTrue(m_arrangement.RemoveAndReturnSuccess(childNode));
            Assert.IsFalse(m_arrangement.Children()[0].Children().Contains(childNode));
        }
    }
}
