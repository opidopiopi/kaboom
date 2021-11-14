namespace Kaboom.Application.Actions
{
    public interface IActionEventSource
    {
        void AddActionEventListener(IActionEventListener actionEventListener);
    }
}
