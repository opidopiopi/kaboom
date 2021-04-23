using Kaboom.Application;
using Kaboom.Application.ConfigurationManagement;
using Kaboom.Application.WorkspaceActions;
using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Plugins.ConfigurationManagement
{
    public class SimpleConfiguration : Configuration
    {
        public SimpleConfiguration(IProvideSettings configurationSource, ActionService actionService, IWorkspace workspace)
            : base(configurationSource, Settings(actionService, workspace))
        {

        }

        private static Setting[] Settings(ActionService actionService, IWorkspace workspace)
        {
            return new Setting[]
            {
                //Move shortcuts
                new ShortcutSetting("Shortcuts.MoveLeft",       "Alt Left",     (shortcut) => actionService.AddAction(new MoveWindowAction(shortcut, workspace, Direction.Left))),
                new ShortcutSetting("Shortcuts.MoveRight",      "Alt Right",    (shortcut) => actionService.AddAction(new MoveWindowAction(shortcut, workspace, Direction.Right))),
                new ShortcutSetting("Shortcuts.MoveUp",         "Alt Up",       (shortcut) => actionService.AddAction(new MoveWindowAction(shortcut, workspace, Direction.Up))),
                new ShortcutSetting("Shortcuts.MoveDown",       "Alt Down",     (shortcut) => actionService.AddAction(new MoveWindowAction(shortcut, workspace, Direction.Down))),
                
                //Select shortcuts
                new ShortcutSetting("Shortcuts.SelectLeft",     "Ctrl Left",    (shortcut) => actionService.AddAction(new SelectWindowAction(shortcut, workspace, Direction.Left))),
                new ShortcutSetting("Shortcuts.SelectRight",    "Ctrl Right",   (shortcut) => actionService.AddAction(new SelectWindowAction(shortcut, workspace, Direction.Right))),
                new ShortcutSetting("Shortcuts.SelectUp",       "Ctrl Up",      (shortcut) => actionService.AddAction(new SelectWindowAction(shortcut, workspace, Direction.Up))),
                new ShortcutSetting("Shortcuts.SelectDown",     "Ctrl Down",    (shortcut) => actionService.AddAction(new SelectWindowAction(shortcut, workspace, Direction.Down))),

                //wrapping shortcuts
                new ShortcutSetting("Shortcuts.WrapVertical",   "Alt V",        (shortcut) => actionService.AddAction(new WrapWindowAction<VerticalArrangement>(shortcut, workspace))),
                new ShortcutSetting("Shortcuts.WrapHorizontal", "Alt H",        (shortcut) => actionService.AddAction(new WrapWindowAction<HorizontalArrangement>(shortcut, workspace))),
                new ShortcutSetting("Shortcuts.Unwrap",         "Alt U",        (shortcut) => actionService.AddAction(new UnWrapWindowAction(shortcut, workspace))),
            };
        }
    }
}