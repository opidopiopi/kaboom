using System.Collections.Generic;

namespace Kaboom.Domain.ShortcutActions
{
    public class ActionService
    {
        private List<Action> m_actions = new List<Action>();
        private IListenToShortcuts m_shortcutListener;

        public ActionService(IListenToShortcuts shortcutListener)
        {
            m_shortcutListener = shortcutListener;

            m_shortcutListener.ShortcutTriggered += (sender, args) =>
            {
                ExecuteActionForShortcut(args.Shortcut);
            };
        }

        private void ExecuteActionForShortcut(Shortcut shortcut)
        {
            var action = m_actions.Find(a => a.Shortcut.Equals(shortcut));

            if(action != null)
            {
                action.Execute();
            }
        }

        public void AddAction(Action action)
        {
            if(m_actions.Find(a => a.Shortcut.Equals(action.Shortcut)) != null){
                throw new ShortcutAlreadyInUse($"The Action: {action} could not be added because the shortcut: {action.Shortcut} is alread in use by another action!");
            }

            m_actions.Add(action);
            m_shortcutListener.RegisterShortcut(action.Shortcut);
        }

        public void RemoveAction(Action action)
        {
            m_actions.Remove(action);
            m_shortcutListener.UnRegisterShortcut(action.Shortcut);
        }
    }
}