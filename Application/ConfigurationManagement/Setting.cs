namespace Kaboom.Application.ConfigurationManagement
{
    public abstract class Setting
    {
        public readonly string Name;
        public readonly string DefaultValue;
        public string Value;

        public void Apply()
        {
            if (string.IsNullOrEmpty(Value))
            {
                Apply(DefaultValue);
            }
            else
            {
                Apply(Value);
            }
        }

        protected abstract void Apply(string value);

        protected Setting(string name, string defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }
    }
}