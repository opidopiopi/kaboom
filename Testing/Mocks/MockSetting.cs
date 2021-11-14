using Kaboom.Application.ConfigurationManagement;

namespace Kaboom.Testing.Mocks
{
    public class MockSetting : Setting
    {
        public string AppliedValue;

        public MockSetting(string name, string defaultValue) : base(name, defaultValue)
        {
        }

        protected override void Apply(string value) => AppliedValue = value;
    }
}
