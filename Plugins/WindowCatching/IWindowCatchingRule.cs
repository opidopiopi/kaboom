using Kaboom.Adapters;

namespace Plugins
{
    public interface IWindowCatchingRule
    {
        bool DoWeWantToCatchThisWindow(IWindow window);
    }
}