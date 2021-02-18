using Kaboom.Abstract;

namespace Kaboom.Model
{
    public interface ISetWindowBounds
    {
        void SetBoundsOfWindowWithIdentity(IWindowIdentity identity, Rectangle bounds);
    }
}
