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
            m_arrangement = new MockArrangement(new Axis[] { });
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
            children.ForEach(child => m_arrangement.InsertAsFirst(child));
            children.Reverse();

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children(), children));
        }

        [TestMethod]
        public void arrangement_insert_as_last()
        {
            //Arrange
            List<MockTreeNodeWithBounds> children = Enumerable.Range(0, 5).Select(i => new MockTreeNodeWithBounds()).ToList();

            //Act
            children.ForEach(child => m_arrangement.InsertAsLast(child));

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children(), children));
        }


        [TestMethod]
        public void arrangement_children_can_be_removed()
        {
            //Arrange
            List<MockTreeNodeWithBounds> children = Enumerable.Range(0, 5).Select(i => new MockTreeNodeWithBounds()).ToList();
            children.ForEach(child => m_arrangement.InsertAsLast(child));

            //Act
            m_arrangement.Remove(children[2]);

            //Assert
            Assert.IsFalse(m_arrangement.Children().Contains(children[2]));

            children.RemoveAt(2);
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.Children(), children));
        }
    }
}
