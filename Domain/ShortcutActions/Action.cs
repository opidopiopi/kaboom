namespace Kaboom.Domain.ShortcutActions
{
    public abstract class Action
    {
        public readonly Shortcut Shortcut;

        protected Action(Shortcut shortcut)
        {
            Shortcut = shortcut;
        }

        public abstract void Execute();
    }
}
