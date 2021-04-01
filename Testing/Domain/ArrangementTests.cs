using Kaboom.Domain.WindowTree.General;
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
        public void arrangement_insert_as_first()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 5).Select(i => new MockTreeNode()).ToList();

            //Act
            expected.ForEach(child => m_arrangement.InsertAsFirst(child));
            expected.Reverse();

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.MyChildren, expected));
        }

        [TestMethod]
        public void arrangement_insert_as_last()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 5).Select(i => new MockTreeNode()).ToList();

            //Act
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Assert
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.MyChildren, expected));
        }


        [TestMethod]
        public void arrangement_children_can_be_removed()
        {
            //Arrange
            List<MockTreeNode> expected = Enumerable.Range(0, 5).Select(i => new MockTreeNode()).ToList();
            expected.ForEach(child => m_arrangement.InsertAsLast(child));

            //Act
            m_arrangement.Remove(expected[2]);

            //Assert
            Assert.IsFalse(m_arrangement.MyChildren.Contains(expected[2]));

            expected.RemoveAt(2);
            Assert.IsTrue(Enumerable.SequenceEqual(m_arrangement.MyChildren, expected));
        }
    }
}
