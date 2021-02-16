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
            Assert.ThrowsException<ValueIsNegativeOrZero>(() => {
                Rectangle bounds = new Rectangle(5, 5, -55, 5);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() => {
                Rectangle bounds = new Rectangle(5, 5, 0, 5);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() => {
                Rectangle bounds = new Rectangle(5, 5, 5, -55);
            });

            Assert.ThrowsException<ValueIsNegativeOrZero>(() => {
                Rectangle bounds = new Rectangle(5, 5, 5, 0);
            });

            Rectangle bounds = new Rectangle(5, 5, 5, 5);
            Rectangle bounds1 = new Rectangle(5, 5, 5, 5);
        }
    }
}
