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

        public TilingWindowManager()
        {
            m_windowRenderer = new WindowsWindowRenderer(m_windowMapper);
            m_windowService = new WindowService(m_arrangementRepository, m_windowRenderer);
            m_workspace = new Workspace(m_windowService, m_arrangementRepository);
            m_actionService = new ActionService(m_shortcutListener);
        }

        public void Start()
        {
            m_actionService.AddAction(new MoveWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Left, KeyModifiers.Alt), m_workspace, Direction.Left));
            m_actionService.AddAction(new MoveWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Right, KeyModifiers.Alt), m_workspace, Direction.Right));
            m_actionService.AddAction(new MoveWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Up, KeyModifiers.Alt), m_workspace, Direction.Up));
            m_actionService.AddAction(new MoveWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Down, KeyModifiers.Alt), m_workspace, Direction.Down));


            m_actionService.AddAction(new SelectWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Left, KeyModifiers.Control), m_workspace, Direction.Left));
            m_actionService.AddAction(new SelectWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Right, KeyModifiers.Control), m_workspace, Direction.Right));
            m_actionService.AddAction(new SelectWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Up, KeyModifiers.Control), m_workspace, Direction.Up));
            m_actionService.AddAction(new SelectWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.Down, KeyModifiers.Control), m_workspace, Direction.Down));

            m_actionService.AddAction(new WrapWindowAction<VerticalArrangement>(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.V, KeyModifiers.Alt), m_workspace));
            m_actionService.AddAction(new WrapWindowAction<HorizontalArrangement>(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.H, KeyModifiers.Alt), m_workspace));
            m_actionService.AddAction(new UnWrapWindowAction(ShortcutMapper.MapToShortcut(System.Windows.Forms.Keys.U, KeyModifiers.Alt), m_workspace));

            using(var windowCatcher = new WindowCatcher(m_windowMapper, m_workspace))
            {
                windowCatcher.RunUpdateLoop();
            }
        }

        static void Main(string[] args)
        {
            TilingWindowManager tilingWindowManager = new TilingWindowManager();

            tilingWindowManager.Start();
        }
    }
}
