using Kaboom.Application;
using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Plugins.ConfigurationManagement;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plugins
{
    [ExcludeFromCodeCoverage]
    class TilingWindowManager
    {
        private WindowsArrangementReposititory<HorizontalArrangement> m_arrangementRepository = new WindowsArrangementReposititory<HorizontalArrangement>();
        private WindowMapper m_windowMapper = new WindowMapper();
        private WindowsWindowRenderer m_windowRenderer;

        private WindowService m_windowService;
        private Workspace m_workspace;

        private WindowsShortcutListener m_shortcutListener = new WindowsShortcutListener();
        private ActionService m_actionService;

        private SimpleConfiguration m_configuration;
        private SalarosConfigParser m_configParser;

        public TilingWindowManager()
        {
            m_windowRenderer = new WindowsWindowRenderer(m_windowMapper);
            m_windowService = new WindowService(m_arrangementRepository, m_windowRenderer);
            m_workspace = new Workspace(m_windowService, m_arrangementRepository);
            m_actionService = new ActionService(m_shortcutListener);

            m_configParser = new SalarosConfigParser(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Kaboom",
                    "settings.conf"
                    ));
            m_configuration = new SimpleConfiguration(m_configParser, m_actionService, m_workspace);
        }

        public void Start()
        {
            m_configuration.LoadValuesForSettings();
            m_configuration.ApplySettings();
            m_configuration.SaveAllSettings();
            m_configParser.Save();

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
