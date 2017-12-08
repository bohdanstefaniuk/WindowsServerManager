using System;
using System.Configuration;
using System.Linq;
using AppPoolManager.Tools;

namespace AppPoolManager
{
    public class ConnectionStringManager: IDisposable
    {
        private readonly SitesManager _sitesManager;
        public ConnectionStringManager()
        {
            _sitesManager = new SitesManager();
        }

        public ConnectionStringsSection GetSiteConnectionStrings(string siteName)
        {
            var site = _sitesManager.GetSiteByName(siteName);
            var siteApplications = site.Applications;

            var connectionStringFileUrl = "";
            var rootApplication = siteApplications.SingleOrDefault(x => x.Path == "/");
            var dictionary = rootApplication.VirtualDirectories.FirstOrDefault();
            connectionStringFileUrl += $@"{dictionary.PhysicalPath}\{AppPoolManagerConfiguration.AppConfigFileName}";

            var map = new ExeConfigurationFileMap { ExeConfigFilename = connectionStringFileUrl };
            var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            return configFile.ConnectionStrings;
        }

        public string GetRedisDb(string siteName)
        {
            var connectionStrings = GetSiteConnectionStrings(siteName);
            var redisConnectionString = connectionStrings.ConnectionStrings[AppPoolManagerConfiguration.RedisConnectionStringKey];

            if (redisConnectionString != null)
            {
                return ConnectionStringsTool.GetSectionFromString(
                    redisConnectionString.ConnectionString, "db");
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