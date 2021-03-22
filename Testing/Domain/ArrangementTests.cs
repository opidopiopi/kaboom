using Kaboom.Domain.WindowTree.Arrangements;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public partial class ArrangementTests
    {
        MockArrangement m_arrangement;

        [TestInitialize]
        public void SetUp()
        {
            m_arrangement = new MockArrangement(new MockTreeNodeRepository(), new Axis[] { });
        }

        [TestMethod]
        public void arrangement_is_not_a_leaf()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsFalse(m_arrangement.IsLeaf());
        }

        [TestMethod]
        public void arrangement_accepts_visitor()
        {
            //Arrange
            VisitedFlagMockVisitor visitor = new VisitedFlagMockVisitor();

            //Act
            m_arrangement.Accept(visitor);

            //Assert
            Assert.IsTrue(visitor.HasBeenVisited);
        }

        [TestMethod]
        public void arrangement_insert_as_first()
        {
            //Arrange
            List<MockTreeNodeWithBounds> children = Enumerable.Range(0, 5).Select(i => new MockTreeNodeWithBounds()).ToList();

            //Act
            children.ForEach(child => m_arrangement.InsertAsFirst(child.ID));
            children.Reverse();

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, children.Select(child => child.ID)));
        }

        [TestMethod]
        public void arrangement_insert_as_last()
        {
            //Arrange
            List<MockTreeNodeWithBounds> children = Enumerable.Range(0, 5).Select(i => new MockTreeNodeWithBounds()).ToList();

            //Act
            children.ForEach(child => m_arrangement.InsertAsLast(child.ID));

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, children.Select(child => child.ID)));
        }


        [TestMethod]
        public void arrangement_children_can_be_removed()
        {
            //Arrange
            List<MockTreeNodeWithBounds> children = Enumerable.Range(0, 5).Select(i => new MockTreeNodeWithBounds()).ToList();
            children.ForEach(child => m_arrangement.InsertAsLast(child.ID));

            //Act
            m_arrangement.Remove(children[2].ID);

            //Assert
            Assert.IsFalse(m_arrangement.Children.Contains(children[2].ID));

            children.RemoveAt(2);
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children, children.Select(child => child.ID)));
        }

        [TestMethod]
        public void arrangement_can_get_first_and_last_child()
        {
            //Arrange
            int num = 5;
            List<MockTreeNodeWithBounds> children = Enumerable.Range(0, num).Select(i => new MockTreeNodeWithBounds()).ToList();
            children.ForEach(child => m_arrangement.InsertAsLast(child.ID));

            //Act

            //Assert
            Assert.AreEqual(children[0].ID, m_arrangement.FirstChild());
            Assert.AreEqual(children[num -1].ID, m_arrangement.LastChild());
        }
    }
}
