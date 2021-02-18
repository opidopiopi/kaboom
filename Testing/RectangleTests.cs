using Kaboom.Abstract;
using Kaboom.Abstract.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testing
{
    [TestClass]
    public class RectangleTests
    {
        [TestMethod]
        public void all_x_and_y_are_valid()
        {
            Rectangle bounds = new Rectangle(-55, -55, 5, 5);
            Rectangle bounds1 = new Rectangle(55, -55, 5, 5);
            Rectangle bounds2 = new Rectangle(-55, 55, 5, 5);
            Rectangle bounds3 = new Rectangle(55, 55, 5, 5);
            Rectangle bounds4 = new Rectangle(0, 0, 5, 5);
        }

        [TestMethod]
        public void only_positive_values_are_allowed_for_width_and_height()
        {
            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Rectangle bounds = new Rectangle(5, 5, -55, 5);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Rectangle bounds = new Rectangle(5, 5, 0, 5);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Rectangle bounds = new Rectangle(5, 5, 5, -55);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() =>
            {
                Rectangle bounds = new Rectangle(5, 5, 5, 0);
            });

            Rectangle bounds = new Rectangle(5, 5, 5, 5);
            Rectangle bounds1 = new Rectangle(5, 5, 5, 5);
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
                    Rectangle main = new Rectangle(x, y, w, h);

                    //if X and Y is outside the bounds return false
                    Rectangle tooFarLeft = new Rectangle(x - 50, y, 100, 100);
                    Rectangle tooFarRight = new Rectangle(x + w + 50, y, 100, 100);
                    Rectangle tooFarUp = new Rectangle(x, y - 50, 100, 100);
                    Rectangle tooFarDown = new Rectangle(x, y + h + 50, 100, 100);

                    //if X and Y are inside or on the border return true
                    Rectangle inside = new Rectangle(x + w / 2, y + h / 2, 100, 100);

                    Rectangle topLeftCorner = new Rectangle(x, y, 100, 100);
                    Rectangle topRightCorner = new Rectangle(x + w, y, 100, 100);
                    Rectangle lowerLeftCorner = new Rectangle(x, y + h, 100, 100);
                    Rectangle lowerRightCorner = new Rectangle(x + w, y + h, 100, 100);

                    Rectangle[] theseAreOutside = { tooFarLeft, tooFarRight, tooFarUp, tooFarDown };
                    Rectangle[] theseAreInside = { inside, topLeftCorner, topRightCorner, lowerLeftCorner, lowerRightCorner };

                    foreach (var isOutside in theseAreOutside)
                    {
                        Assert.IsFalse(main.IsOtherRectangleInside(isOutside), $"This rectangle: {isOutside} should be outside of this: {main}");
                    }

                    foreach (var isInside in theseAreInside)
                    {
                        Assert.IsTrue(main.IsOtherRectangleInside(isInside), $"This rectangle: {isInside} should be inside of this: {main}");
                    }
                }
            }
        }

        [TestMethod]
        public void rectangle_equals_test()
        {
            Rectangle rectangleA = new Rectangle(5, 5, 55, 55);
            Rectangle rectangleB = new Rectangle(5, 5, 55, 55);

            Assert.AreEqual(rectangleA, rectangleB);
        }
    }
}
