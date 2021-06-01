using Kaboom.Application.ConfigurationManagement;
using Kaboom.Testing.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Application
{
    [TestClass]
    public class ConfigurationTests
    {
        private IConfiguration m_configuration;
        private Mock<IProvideSettings> m_settingProvider;
        private MockSetting[] m_settings;

        [TestInitialize]
        public void SetUp()
        {
            m_settingProvider = new Mock<IProvideSettings>();
            m_settings = Enumerable.Range(0, 5).Select(index => new MockSetting(index.ToString(), index.ToString())).ToArray();

            m_configuration = new ConfigurationBase(m_settingProvider.Object, m_settings);
        }

        [TestMethod]
        public void can_load_all_settings()
        {
            //Arrange
            List<Setting> loadedSettings = new List<Setting>();
            m_settingProvider
                .Setup(provider => provider.LoadSetting(It.IsAny<Setting>()))
                .Callback<Setting>(setting => loadedSettings.Add(setting));

            //Act
            m_configuration.LoadValuesForSettings();

            //Assert
            CollectionAssert.AreEquivalent(m_settings, loadedSettings);
        }


        [TestMethod]
        public void can_save_all_settings()
        {
            //Arrange
            List<Setting> savedSettings = new List<Setting>();
            m_settingProvider
                .Setup(provider => provider.SaveSetting(It.IsAny<Setting>()))
                .Callback<Setting>(setting => savedSettings.Add(setting));

            //Act
            m_configuration.SaveAllSettings();

            //Assert
            CollectionAssert.AreEquivalent(m_settings, savedSettings);
        }

        [TestMethod]
        public void can_apply_all_settings()
        {
            //Arrange
            var newValue = "AMAZING VALUE";
            m_settings.ToList().ForEach(setting => {
                setting.Value = newValue;
                Assert.IsTrue(string.IsNullOrEmpty(setting.AppliedValue));
            });


            //Act
            m_configuration.ApplySettings();

            //Assert
            m_settings.ToList().ForEach(setting => Assert.AreEqual(newValue, setting.AppliedValue));
        }
    }
}