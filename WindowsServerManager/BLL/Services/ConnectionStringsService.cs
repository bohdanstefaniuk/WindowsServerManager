using System;
using System.Configuration;
using AppPoolManager;
using BLL.Enums;
using BLL.Interfaces;
using DataAccessLayer.Repositories;

namespace BLL.Services
{
    public class ConnectionStringsService : IConnectionStringsService
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public ConnectionStringsService()
        {
            var settingsManager = new SettingsService(new UnitOfWork());
            var redisConnectionKeys = settingsManager.GetSettingByCode("RedisConnectionKeys");
            var configurationFileName = settingsManager.GetSettingByCode("ConfigurationFileName");
            _connectionStringManager = new ConnectionStringManager(redisConnectionKeys.Value, configurationFileName.Value);
        }

        public ConnectionStringsSection GetSiteConnectionStrings(string siteName, bool isSite)
        {
            return _connectionStringManager.GetSiteConnectionStrings(siteName, isSite);
        }

        public string GetRedisDb(string siteName, bool isSite)
        {
            return _connectionStringManager.GetRedisDb(siteName, isSite);
        }

        public string GetMssqlDb(string siteName, bool isSite)
        {
            return _connectionStringManager.GetMssqlDb(siteName, isSite);
        }

        public string GetConfigurationFilePath(string siteName, IISSiteType siteType)
        {
            return _connectionStringManager.GetConfigurationFilePath(siteName, siteType == IISSiteType.Site);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}