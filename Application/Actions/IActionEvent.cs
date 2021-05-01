namespace Kaboom.Application.Actions
{
    public interface IActionEvent
    {
        bool Equals(IActionEvent actionEvent);
    }
}
