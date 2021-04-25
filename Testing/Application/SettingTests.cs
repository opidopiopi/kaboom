using Kaboom.Application.ConfigurationManagement;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Kaboom.Testing.Application
{

    [TestClass]
    public class SettingTests
    {

        [TestMethod]
        public void has_name_and_default_value()
        {
            //Arrange
            string name = "AWonderfulName";
            string defaultValue = "AMAZING";

            Mock<Setting> setting = new Mock<Setting>(name, defaultValue);

            //Act

            //Assert
            Assert.AreEqual(name, setting.Object.Name);
            Assert.AreEqual(defaultValue, setting.Object.DefaultValue);
        }


        [TestMethod]
        public void if_value_is_null_or_empty_default_value_is_applied()
        {
            //Arrange
            string name = "AWonderfulName";
            string defaultValue = "AMAZING";

            var setting = new MockSetting(name, defaultValue);

            //Act
            setting.Apply();

            //Assert
            Assert.AreEqual(defaultValue, setting.AppliedValue);

            //Act
            setting.Value = "";
            setting.Apply();

            //Assert
            Assert.AreEqual(defaultValue, setting.AppliedValue);
        }


        [TestMethod]
        public void if_value_is_not_null_or_empty_value_is_applied()
        {
            //Arrange
            string name = "AWonderfulName";
            string defaultValue = "AMAZING";
            string newValue = "Some other wonderful value";

            var setting = new MockSetting(name, defaultValue);

            //Act
            setting.Value = newValue;
            setting.Apply();

            //Assert
            Assert.AreEqual(newValue, setting.AppliedValue);
        }
    }
}