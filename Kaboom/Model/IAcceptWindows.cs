using Kaboom.Abstract;

namespace Kaboom.Model
{
    public interface IAcceptWindows
    {
        void InsertWindow(IWindowIdentity identity, Rectangle bounds);
        void RemoveWindow(IWindowIdentity identity);
    }
}
