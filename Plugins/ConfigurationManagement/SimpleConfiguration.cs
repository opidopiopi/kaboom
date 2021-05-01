using Kaboom.Application;
using Kaboom.Application.Actions;
using Kaboom.Application.Actions.WorkspaceActions;
using Kaboom.Application.ConfigurationManagement;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using Plugins.Shortcuts;

namespace Plugins.ConfigurationManagement
{
    public class SimpleConfiguration : Configuration
    {
        public SimpleConfiguration(IProvideSettings configurationSource, ISelection selection, IListenToShortcuts shortcutListener, IActionEventListener eventListener)
            : base(configurationSource, Settings(selection, shortcutListener, eventListener))
        {

        }

        private static Setting[] Settings(ISelection selection, IListenToShortcuts shortcutListener, IActionEventListener eventListener)
        {
            return new Setting[]
            {
                //Move shortcuts
                new ShortcutSetting("Shortcuts.MoveLeft",       "Alt Left",     shortcutListener, eventListener, new MoveWindowAction(selection, Direction.Left)),
                new ShortcutSetting("Shortcuts.MoveRight",      "Alt Right",    shortcutListener, eventListener, new MoveWindowAction(selection, Direction.Right)),
                new ShortcutSetting("Shortcuts.MoveUp",         "Alt Up",       shortcutListener, eventListener, new MoveWindowAction(selection, Direction.Up)),
                new ShortcutSetting("Shortcuts.MoveDown",       "Alt Down",     shortcutListener, eventListener, new MoveWindowAction(selection, Direction.Down)),
                
                //Select shortcuts
                new ShortcutSetting("Shortcuts.SelectLeft",     "Ctrl Left",    shortcutListener, eventListener, new SelectWindowAction(selection, Direction.Left)),
                new ShortcutSetting("Shortcuts.SelectRight",    "Ctrl Right",   shortcutListener, eventListener, new SelectWindowAction(selection, Direction.Right)),
                new ShortcutSetting("Shortcuts.SelectUp",       "Ctrl Up",      shortcutListener, eventListener, new SelectWindowAction(selection, Direction.Up)),
                new ShortcutSetting("Shortcuts.SelectDown",     "Ctrl Down",    shortcutListener, eventListener, new SelectWindowAction(selection, Direction.Down)),

                //wrapping shortcuts
                new ShortcutSetting("Shortcuts.WrapVertical",   "Alt V",        shortcutListener, eventListener, new WrapWindowAction<VerticalArrangement>(selection)),
                new ShortcutSetting("Shortcuts.WrapHorizontal", "Alt H",        shortcutListener, eventListener, new WrapWindowAction<HorizontalArrangement>(selection)),
                new ShortcutSetting("Shortcuts.Unwrap",         "Alt U",        shortcutListener, eventListener, new UnWrapWindowAction(selection)),
            };
        }
    }
}