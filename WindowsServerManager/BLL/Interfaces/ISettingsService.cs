using System;
using System.Collections.Generic;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BLL.Interfaces
{
    public interface ISettingsService: IDisposable
    {
        List<Settings> GetSettings();
        Settings GetSettingsById(Guid id);
        Settings GetSettingByCode(string code);
        void CreateSettings(Settings settings);
        void UpdateSetting(Settings settings);
    }
}