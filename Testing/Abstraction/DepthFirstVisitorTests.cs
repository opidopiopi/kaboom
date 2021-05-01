using Kaboom.Abstraction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Kaboom.Testing.Abstraction
{
    class MockVisitor : IVisitor<MockClass>
    {
        public string Sequence = "";

        public void Visit(MockClass mockClass)
        {
            Sequence += mockClass.ID;
        }
    }

    class MockClass : ICanBeVisited<MockClass>, IHaveChildrenThatCanBeVisited<MockClass>
    {
        private static int COUNTER = 0;

        public List<MockClass> Children = new List<MockClass>();
        public int ID = COUNTER++;

        public void Accept(IVisitor<MockClass> visitor)
        {
            visitor.Visit(this);
        }

        public void VisitAllChildren(IVisitor<MockClass> visitor)
        {
            Children.ForEach(child => child.Accept(visitor));
        }
    }

    [TestClass]
    public class DepthFirstVisitorTests
    {

        [TestMethod]
        public void visitor_visits_depth_first()
        {
            //Arrange
            var root = new MockClass();//0

            var levelOneA = new MockClass();//1
            var levelOneB = new MockClass();//2

            var levelTwoA = new MockClass();//3
            var levelTwoB = new MockClass();//4
            var levelTwoC = new MockClass();//5

            var levelThreeA = new MockClass();//6
            var levelThreeB = new MockClass();//7

            root.Children.Add(levelOneA);
            root.Children.Add(levelOneB);

            levelOneA.Children.Add(levelTwoA);
            levelOneA.Children.Add(levelTwoB);
            levelOneB.Children.Add(levelTwoC);

            levelTwoA.Children.Add(levelThreeA);
            levelTwoA.Children.Add(levelThreeB);

            var visitor = new MockVisitor();
            var depthFirst = new DepthFirstVisitor<MockClass>(visitor);

            //Act
            root.Accept(depthFirst);

            //Assert
            Assert.AreEqual("67341520", visitor.Sequence);
        }
    }
}