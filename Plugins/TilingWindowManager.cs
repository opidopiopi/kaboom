using Kaboom.Application;
using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Plugins.ConfigurationManagement;
using Plugins.Shortcuts;
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

        private ActionService m_actionService = new ActionService();
        private WindowService m_windowService;
        private Selection m_selection;

        private WindowsShortcutListener m_shortcutListener = new WindowsShortcutListener();

        private SimpleConfiguration m_configuration;
        private SalarosConfigParser m_configParser;

        public TilingWindowManager()
        {
            m_windowRenderer = new WindowsWindowRenderer(m_windowMapper);
            m_windowService = new WindowService(m_arrangementRepository, m_windowRenderer);
            m_selection = new Selection(m_windowService, m_arrangementRepository);

            m_configParser = new SalarosConfigParser(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Kaboom",
                    "settings.conf"
                    ));

            m_actionService.AddActionEventSource(m_shortcutListener);
            m_configuration = new SimpleConfiguration(m_configParser, m_selection, m_shortcutListener, m_actionService);
        }

        public void Start()
        {
            m_configuration.LoadValuesForSettings();
            m_configuration.ApplySettings();
            m_configuration.SaveAllSettings();
            m_configParser.Save();

            using(var windowCatcher = new WindowCatcher(m_windowMapper, m_selection))
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
