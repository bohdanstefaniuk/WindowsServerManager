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

		/// <summary>
		/// Get connection strings for site
		/// </summary>
		/// <param name="siteName">Site name or path</param>
		/// <param name="isSite">is site</param>
		/// <returns>Connection strings section</returns>
        public ConnectionStringsSection GetSiteConnectionStrings(string siteName, bool isSite)
        {
            return _connectionStringManager.GetSiteConnectionStrings(siteName, isSite);
        }

		/// <summary>
		/// Get redis db for site or app
		/// </summary>
		/// <param name="siteName">Site name or path</param>
		/// <param name="isSite">is site</param>
		/// <returns>Redis db number</returns>
		public string GetRedisDb(string siteName, bool isSite)
        {
            return _connectionStringManager.GetRedisDb(siteName, isSite);
        }

		/// <summary>
		/// Get ms sql database name for site
		/// </summary>
		/// <param name="siteName">Site name or path</param>
		/// <param name="isSite">is site</param>
		/// <returns>Return database name</returns>
		public string GetMssqlDb(string siteName, bool isSite)
        {
            return _connectionStringManager.GetMssqlDb(siteName, isSite);
        }

		/// <summary>
		/// Get path to configuration file by site
		/// </summary>
		/// <param name="siteName">Site name or path</param>
		/// <param name="siteType">is site</param>
		/// <returns>Path for config file</returns>
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