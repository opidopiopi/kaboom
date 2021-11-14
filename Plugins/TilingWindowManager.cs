using Kaboom.Adapters;
using Kaboom.Application;
using Kaboom.Application.ConfigurationManagement;
using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using Plugins.ConfigurationManagement;
using Plugins.Overlay;
using Plugins.Shortcuts;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plugins
{
    [ExcludeFromCodeCoverage]
    class TilingWindowManager : IShutdownHandler
    {
        private WindowsArrangementReposititory<HorizontalArrangement> arrangementRepository = new WindowsArrangementReposititory<HorizontalArrangement>();
        private WindowMapper windowMapper = new WindowMapper();
        private Selection selection;

        private WindowsFormOverlay overlay = new WindowsFormOverlay();
        private WindowsRenderService renderService;
        private ActionService actionService = new ActionService();
        private WindowService windowService;

        private WindowsShortcutListener shortcutListener = new WindowsShortcutListener();

        private IConfiguration configuration;
        private SalarosConfigParser configParser;

        private WindowCatcher windowCatcher;

        public TilingWindowManager()
        {
            renderService = new WindowsRenderService(windowMapper, overlay);
            windowService = new WindowService(arrangementRepository, renderService);
            selection = new Selection(windowService, arrangementRepository);

            configParser = new SalarosConfigParser(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Kaboom",
                    "settings.conf"
                    ));

            actionService.AddActionEventSource(shortcutListener);

            configuration = new SimpleConfiguration(configParser, selection, shortcutListener, actionService, arrangementRepository, this);

#if DEBUG
            configuration = new DebugConfiguration(configuration, shortcutListener, actionService, arrangementRepository);
#endif
        }

        public void Start()
        {
            configuration.LoadValuesForSettings();
            configuration.ApplySettings();
            configuration.SaveAllSettings();
            configParser.Save();

            overlay.StartFormThread();

            using(windowCatcher = new WindowCatcher(windowMapper, selection, windowService, new DefaultCatchingRule()))
            {
                windowCatcher.RunUpdateLoop();
            }
        }

        public void Shutdown()
        {
            windowCatcher.StopUpdateLoop();
            shortcutListener.Dispose();
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            TilingWindowManager tilingWindowManager = new TilingWindowManager();

            tilingWindowManager.Start();
        }
    }
}
