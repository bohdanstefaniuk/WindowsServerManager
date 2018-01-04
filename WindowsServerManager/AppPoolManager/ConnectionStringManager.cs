using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AppPoolManager.Tools;
using Microsoft.Web.Administration;

namespace AppPoolManager
{
    public class ConnectionStringManager: IDisposable
    {
        private readonly SitesManager _sitesManager;
        private readonly string _redisConnectionStringKey;
        private readonly string _appConfigFileName;

        public ConnectionStringManager(string redisConnectionStringKey, string appConfigFileName)
        {
            _redisConnectionStringKey = redisConnectionStringKey;
            _appConfigFileName = appConfigFileName;
            _sitesManager = new SitesManager();
        }

        /// <summary>
        /// Get connection strings for site or application
        /// </summary>
        /// <param name="siteName">Name of site or application path</param>
        /// <param name="isSite">True when siteName contains site, false when siteName is application path</param>
        /// <returns>connection strings section</returns>
        public ConnectionStringsSection GetSiteConnectionStrings(string siteName, bool isSite)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = GetConfigurationFilePath(siteName, isSite) };
            var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            return configFile.ConnectionStrings;
        }

        /// <summary>
        /// Get path to configuration file
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="isSite"></param>
        /// <returns></returns>
        public string GetConfigurationFilePath(string siteName, bool isSite)
        {
            Application rootApplication;
            if (isSite)
            {
                var site = _sitesManager.GetSiteByName(siteName);
                var siteApplications = site.Applications;
                rootApplication = siteApplications.SingleOrDefault(x => x.Path == "/");
            }
            else
            {
                rootApplication = _sitesManager.GetApplicationByPath(siteName);
            }
            
            var dictionary = rootApplication?.VirtualDirectories.FirstOrDefault();
            return $@"{dictionary?.PhysicalPath}\{_appConfigFileName}";
        }

        /// <summary>
        /// Get redis database number from connection string for site or application
        /// </summary>
        /// <param name="siteName">Name of site or application path</param>
        /// <param name="isSite">True when siteName contains site, false when siteName is application path</param>
        /// <returns>Redis db number or null when application/site doesn`t have redis</returns>
        public string GetRedisDb(string siteName, bool isSite)
        {
            var connectionStrings = GetSiteConnectionStrings(siteName, isSite);
            var redisConnectionString = connectionStrings.ConnectionStrings[_redisConnectionStringKey];

            if (redisConnectionString != null)
            {
                return ConnectionStringsTool.GetSectionFromString(
                    redisConnectionString.ConnectionString, "db");
            }

            return null;
        }

        /// <summary>
        /// Get mssql database name from connection string for site or application
        /// </summary>
        /// <param name="siteName">Name of site or application path</param>
        /// <param name="isSite">True when siteName contains site, false when siteName is application path</param>
        /// <returns>mssql database name or null when application/site doesn`t have mssql</returns>
        public string GetMssqlDb(string siteName, bool isSite)
        {
            var connectionStrings = GetSiteConnectionStrings(siteName, isSite);
            var redisConnectionString = connectionStrings.ConnectionStrings["db"];

            if (redisConnectionString != null)
            {
                return ConnectionStringsTool.GetSectionFromString(
                    redisConnectionString.ConnectionString, "Initial Catalog");
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _sitesManager.Dispose();
            }
            _disposed = true;
        }
    }
}