using Kaboom.Application.ConfigurationManagement;

namespace Plugins.ConfigurationManagement
{
    public class StaticSetting : Setting
    {
        public StaticSetting(string name, string defaultValue)
            : base(name, defaultValue)
        {
        }

        protected override void Apply(string value)
        {

        }
    }
}
