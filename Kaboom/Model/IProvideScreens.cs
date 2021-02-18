using Kaboom.Abstract;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public interface IProvideScreens
    {
        List<Rectangle> GetScreenBounds();
    }
}
