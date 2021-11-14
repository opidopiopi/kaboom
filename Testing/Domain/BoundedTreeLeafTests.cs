using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Kaboom.Domain.WindowTree.Exceptions;
using Kaboom.Domain.WindowTree.Helpers;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class BoundedTreeLeafTests
    {


        [TestMethod]
        public void bounded_tree_leaf_cannot_insert_child()
        {
            //Arrange
            var treeLeafMock = new Mock<BoundedTreeLeaf>();
            var childMock = new Mock<IBoundedTreeNode>().Object;
            var referenceMock = new Mock<IBoundedTreeNode>().Object;

            //Act & Assert
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAsFirst(childMock));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAsFirst(null));

            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAsLast(childMock));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAsLast(null));

            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertBefore(childMock, referenceMock));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertBefore(childMock, null));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertBefore(null, referenceMock));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertBefore(null, null));

            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAfter(childMock, referenceMock));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAfter(childMock, null));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAfter(null, referenceMock));
            Assert.ThrowsException<CannotInsertChild>(() => treeLeafMock.Object.InsertAfter(null, null));
        }


        [TestMethod]
        public void bounded_tree_leaf_cannot_remove_child()
        {
            //Arrange
            var treeLeafMock = new Mock<BoundedTreeLeaf>();
            var childMock = new Mock<IBoundedTreeNode>().Object;

            //Act & Assert
            Assert.ThrowsException<CannotRemoveChild>(() => treeLeafMock.Object.Remove(childMock));
            Assert.ThrowsException<CannotRemoveChild>(() => treeLeafMock.Object.Remove(null));
        }
    }
}