using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Application.ConfigurationManagement
{
    public class ConfigurationBase : IConfiguration
    {
        private List<Setting> m_settings;
        private IProvideSettings m_settingProvider;

        public ConfigurationBase(IProvideSettings configurationSource, params Setting[] settings)
        {
            m_settings = settings.ToList();
            m_settingProvider = configurationSource;
        }

        public void LoadValuesForSettings()
        {
            m_settings.ForEach(setting => m_settingProvider.LoadSetting(setting));
        }

        public void SaveAllSettings()
        {
            m_settings.ForEach(setting => m_settingProvider.SaveSetting(setting));
        }

        public void ApplySettings()
        {
            m_settings.ForEach(setting => setting.Apply());
        }

        public Setting GetSetting(string name)
        {
            return m_settings.Find(setting => setting.Name.Equals(name));
        }
    }
}