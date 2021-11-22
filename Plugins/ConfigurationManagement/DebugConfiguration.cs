using Kaboom.Application.Actions;
using Kaboom.Application.ConfigurationManagement;
using Kaboom.Domain.WindowTree;
using Plugins.Actions;
using Plugins.Shortcuts;

namespace Plugins.ConfigurationManagement
{
    public class DebugConfiguration : IConfiguration
    {
        private IConfiguration baseConfiguration;
        private ShortcutSetting debugPrintShortcut;

        public DebugConfiguration(IConfiguration baseConfiguration, IListenToShortcuts shortcutListener, IActionEventListener eventListener, IArrangementRepository arrangementRepository)
        {
            this.baseConfiguration = baseConfiguration;
            debugPrintShortcut = new ShortcutSetting("Shortcuts.DebugTreePrint", "Alt D", shortcutListener, eventListener, new DebugTreePrint(arrangementRepository));
        }

        public void ApplySettings()
        {
            baseConfiguration.ApplySettings();
            debugPrintShortcut.Apply();
        }

        public Setting GetSetting(string name)
        {
            return baseConfiguration.GetSetting(name);
        }

        public void LoadValuesForSettings()
        {
            baseConfiguration.LoadValuesForSettings();
        }

        public void SaveAllSettings()
        {
            baseConfiguration.SaveAllSettings();
        }
    }
}
