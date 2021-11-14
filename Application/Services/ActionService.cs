using Kaboom.Application.Actions;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Application.Services
{
    public class ActionService : IActionEventListener
    {
        private List<(IActionEvent actionEvent, IAction action)> m_actions = new List<(IActionEvent, IAction)>();

        public void AddActionEventSource(IActionEventSource eventSource)
        {
            eventSource.AddActionEventListener(this);
        }

        public void OnActionEvent(IActionEvent actionEvent)
        {
            m_actions
                .Where(tuple => tuple.actionEvent.Equals(actionEvent))
                .ToList()
                .ForEach(tuple => tuple.action.Execute());
        }

        public void RegisterActionForEvent(IActionEvent actionEvent, IAction action)
        {
            m_actions.Add((actionEvent, action));
        }
    }
}
