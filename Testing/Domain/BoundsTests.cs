using Kaboom.Abstraction.Exceptions;
using Kaboom.Domain.WindowTree.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaboom.Testing.Domain
{
    [TestClass]
    public class BoundsTests
    {
        [TestMethod]
        public void all_x_and_y_are_valid()
        {
            Bounds bounds = new Bounds(-55, -55, 5, 5);
            Bounds bounds1 = new Bounds(55, -55, 5, 5);
            Bounds bounds2 = new Bounds(-55, 55, 5, 5);
            Bounds bounds3 = new Bounds(55, 55, 5, 5);
            Bounds bounds4 = new Bounds(0, 0, 5, 5);
        }

        [TestMethod]
        public void only_positive_values_are_allowed_for_width_and_height()
        {
            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Bounds bounds = new Bounds(5, 5, -55, 5);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Bounds bounds = new Bounds(5, 5, 0, 5);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Bounds bounds = new Bounds(5, 5, 5, -55);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Bounds bounds = new Bounds(5, 5, 5, 0);
            });

            Bounds bounds = new Bounds(5, 5, 5, 5);
            Bounds bounds1 = new Bounds(5, 5, 5, 5);
        }

        [TestMethod]
        public void test_if_other_rectangle_top_left_corner_is_inside_rectangle()
        {
            for (int x = -100; x <= 100; x += 100)
            {
                for (int y = -100; y <= 100; y += 100)
                {
                    int w = 100;
                    int h = 100;
                    Bounds main = new Bounds(x, y, w, h);

                    //if X and Y is outside the bounds return false
                    Bounds tooFarLeft = new Bounds(x - 50, y, 100, 100);
                    Bounds tooFarRight = new Bounds(x + w + 50, y, 100, 100);
                    Bounds tooFarUp = new Bounds(x, y - 50, 100, 100);
                    Bounds tooFarDown = new Bounds(x, y + h + 50, 100, 100);

                    //if X and Y are inside or on the border return true
                    Bounds inside = new Bounds(x + w / 2, y + h / 2, 100, 100);

                    Bounds topLeftCorner = new Bounds(x, y, 100, 100);
                    Bounds topRightCorner = new Bounds(x + w, y, 100, 100);
                    Bounds lowerLeftCorner = new Bounds(x, y + h, 100, 100);
                    Bounds lowerRightCorner = new Bounds(x + w, y + h, 100, 100);

                    Bounds[] theseAreOutside = { tooFarLeft, tooFarRight, tooFarUp, tooFarDown, topRightCorner, lowerLeftCorner, lowerRightCorner };
                    Bounds[] theseAreInside = { inside, topLeftCorner };

                    foreach (var isOutside in theseAreOutside)
                    {
                        Assert.IsFalse(main.IsPointInside(isOutside.X, isOutside.Y), $"This rectangle: {isOutside} should be outside of this: {main}");
                    }

                    foreach (var isInside in theseAreInside)
                    {
                        Assert.IsTrue(main.IsPointInside(isInside.X, isInside.Y), $"This rectangle: {isInside} should be inside of this: {main}");
                    }
                }
            }
        }

        [TestMethod]
        public void rectangle_equals_test()
        {
            Bounds rectangleA = new Bounds(5, 5, 55, 55);
            Bounds rectangleB = new Bounds(5, 5, 55, 55);

            Assert.AreEqual(rectangleA, rectangleB);
        }
    }
}
