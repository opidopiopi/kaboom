using Kaboom.Application;
using Kaboom.Application.WorkspaceActions;
using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Plugins
{
    class TilingWindowManager
    {
        private WindowsArrangementReposititory<HorizontalArrangement> m_arrangementRepository = new WindowsArrangementReposititory<HorizontalArrangement>();
        private WindowMapper m_windowMapper = new WindowMapper();
        private WindowsWindowRenderer m_windowRenderer;

        private WindowService m_windowService;
        private Workspace m_workspace;

        private WindowsShortcutListener m_shortcutListener = new WindowsShortcutListener();
        private ActionService m_actionService;

        private WindowCatcher m_windowCatcher;

        public TilingWindowManager()
        {
            m_windowRenderer = new WindowsWindowRenderer(m_windowMapper);
            m_windowService = new WindowService(m_arrangementRepository, m_windowRenderer);
            m_workspace = new Workspace(m_windowService, m_arrangementRepository);
            m_actionService = new ActionService(m_shortcutListener);

            m_windowCatcher = new WindowCatcher(m_windowMapper, m_workspace);
        }

        public void Start()
        {
            m_actionService.AddAction(new MoveWindowAction(new Shortcut(Modifier.ALT, 'j'), m_workspace, Direction.Left));
            m_actionService.AddAction(new MoveWindowAction(new Shortcut(Modifier.ALT, 'l'), m_workspace, Direction.Right));
            m_actionService.AddAction(new MoveWindowAction(new Shortcut(Modifier.ALT, 'i'), m_workspace, Direction.Up));
            m_actionService.AddAction(new MoveWindowAction(new Shortcut(Modifier.ALT, 'k'), m_workspace, Direction.Down));


            m_actionService.AddAction(new SelectWindowAction(new Shortcut(Modifier.ALT, 'a'), m_workspace, Direction.Left));
            m_actionService.AddAction(new SelectWindowAction(new Shortcut(Modifier.ALT, 'd'), m_workspace, Direction.Right));
            m_actionService.AddAction(new SelectWindowAction(new Shortcut(Modifier.ALT, 'w'), m_workspace, Direction.Up));
            m_actionService.AddAction(new SelectWindowAction(new Shortcut(Modifier.ALT, 's'), m_workspace, Direction.Down));

            m_actionService.AddAction(new WrapWindowAction<VerticalArrangement>(new Shortcut(Modifier.ALT, 'v'), m_workspace));
            m_actionService.AddAction(new WrapWindowAction<HorizontalArrangement>(new Shortcut(Modifier.ALT, 'h'), m_workspace));
            m_actionService.AddAction(new UnWrapWindowAction(new Shortcut(Modifier.ALT, 'u'), m_workspace));

            m_windowCatcher.RunUpdateLoop();
        }

        static void Main(string[] args)
        {
            TilingWindowManager tilingWindowManager = new TilingWindowManager();

            tilingWindowManager.Start();
        }
    }
}
