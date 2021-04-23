using Kaboom.Application.ConfigurationManagement;
using Salaros.Configuration;

namespace Plugins.ConfigurationManagement
{
    class SalarosConfigParser : IProvideSettings
    {
        private ConfigParser m_configParser;

        public SalarosConfigParser(string filePath)
        {
            m_configParser = new ConfigParser(
                filePath,
                new ConfigParserSettings
                {
                    MultiLineValues = MultiLineValues.Simple | MultiLineValues.QuoteDelimitedValues
                }
            );
        }

        public void Save()
        {
            m_configParser.Save();
        }

        public void LoadSetting(Setting setting)
        {
            (string sectionName, string valueName) = GetSectionAndName(setting.Name);

            setting.Value = m_configParser.GetValue(sectionName, valueName, setting.DefaultValue);
        }

        public void SaveSetting(Setting setting)
        {
            (string sectionName, string valueName) = GetSectionAndName(setting.Name);

            m_configParser.SetValue(sectionName, valueName, setting.Value);
        }

        private (string sectionName, string valueName) GetSectionAndName(string name)
        {
            if (name.Contains("."))
            {
                var splitted = name.Split('.');

                return (splitted[0], splitted[1]);
            }
            else
            {
                return ("General", name);
            }
        }
    }
}