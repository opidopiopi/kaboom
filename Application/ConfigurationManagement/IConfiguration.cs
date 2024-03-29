﻿namespace Kaboom.Application.ConfigurationManagement
{
    public interface IConfiguration
    {
        void ApplySettings();
        void LoadValuesForSettings();
        void SaveAllSettings();
    }
}