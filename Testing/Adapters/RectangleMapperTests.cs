using Kaboom.Adapters;
using Kaboom.Domain.WindowTree.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace Kaboom.Testing.Adapters
{
    [TestClass]
    public class RectangleMapperTests
    {

        [TestMethod]
        public void rect_to_bounds()
        {
            //Arrange
            Rectangle rectangle = new Rectangle(5, 5, 69, 420);

            //Act
            var bounds = RectangleMapper.RectangleToBounds(rectangle);

            //Assert
            Assert.AreEqual(new Bounds(5, 5, 69, 420), bounds);
        }

        [TestMethod]
        public void bounds_to_rect()
        {
            //Arrange
            Bounds bounds = new Bounds(5, 5, 69, 420);

            //Act
            var rect = RectangleMapper.BoundsToRectangle(bounds);

            //Assert
            Assert.AreEqual(new Rectangle(5, 5, 69, 420), rect);
        }

        [TestMethod]
        public void fuzz_rect_to_bounds()
        {
            Random rnd = new Random();

            for(int i = 0; i < 100000; i++)
            {
                int x = rnd.Next() - (Int32.MaxValue / 2);
                int y = rnd.Next() - (Int32.MaxValue / 2);
                int width = rnd.Next();
                int height = rnd.Next();

                //Arrange
                Rectangle rectangle = new Rectangle(x, y, width, height);

                //Act
                var bounds = RectangleMapper.RectangleToBounds(rectangle);

                //Assert
                Assert.AreEqual(new Bounds(x, y, width, height), bounds);
            }
        }

        [TestMethod]
        public void fuzz_bounds_to_rect()
        {
            Random rnd = new Random();

            for (int i = 0; i < 100000; i++)
            {
                int x = rnd.Next() - (Int32.MaxValue / 2);
                int y = rnd.Next() - (Int32.MaxValue / 2);
                int width = rnd.Next();
                int height = rnd.Next();

                //Arrange
                Bounds bounds = new Bounds(x, y, width, height);

                //Act
                var rect = RectangleMapper.BoundsToRectangle(bounds);

                //Assert
                Assert.AreEqual(new Rectangle(x, y, width, height), rect);
            }
        }
    }
}