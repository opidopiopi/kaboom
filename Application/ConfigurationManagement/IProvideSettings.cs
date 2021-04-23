namespace Kaboom.Application.ConfigurationManagement
{
    public interface IProvideSettings
    {
        void LoadSetting(Setting setting);
        void SaveSetting(Setting setting);
    }
}