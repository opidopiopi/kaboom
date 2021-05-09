namespace Kaboom.Application.Actions
{
    public interface IActionEventListener
    {
        void OnActionEvent(IActionEvent actionEvent);
        void AddActionEventSource(IActionEventSource eventSource);
        void RegisterActionForEvent(IActionEvent actionEvent, IAction action);
    }
}