using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using AppPoolManager.Tools;

namespace AppPoolManager
{
    public class SitesManager : IDisposable
    {
        private readonly ServerManager _serverManager;
        public SitesManager()
        {
            _serverManager = new ServerManager();
        }

        /// <summary>
        /// Get sites hosted in IIS
        /// </summary>
        /// <returns>SiteCollection</returns>
        public SiteCollection GetSiteCollection()
        {
            return _serverManager.Sites;
        }

        /// <summary>
        /// Get site hosted in IIS by name
        /// </summary>
        /// <param name="name">Site name</param>
        /// <returns>Site</returns>
        public Site GetSiteByName(string name)
        {
            var sites = _serverManager.Sites;
            return sites.SingleOrDefault(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void GetApplications()
        {
            var sites = _serverManager.Sites;
            foreach (var site in sites)
            {
                var applications = site.Applications;
                Console.WriteLine($"Site: {site.Name}");
                foreach (var application in applications)
                {
                    Console.WriteLine($"\tApplication: {application.ToString()}, FullPath: {application.Path}");
                    var smth = application.Attributes;
                    foreach (var applicationAttribute in application.Attributes)
                    {
                        Console.WriteLine($"\t\tAttribute: {applicationAttribute.Name}, value: {applicationAttribute.Value}");
                    }
                }
            }
        }

        /// <summary>
        /// Get all aplications in Site
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns>ApplicationCollection</returns>
        public ApplicationCollection GetSiteApplications(string siteName)
        {
            var site = GetSiteByName(siteName);
            return site.Applications;
        }

        public ApplicationCollection GetAppsIncludeInApplication(Application application)
        {
            var collection = application.GetCollection(typeof(ApplicationCollection));
            return null;
        }

        public List<string> GetSiteConnectionStrings(Site site)
        {
            return default(List<string>);
        }

        public ConnectionStringsSection GetSiteConnectionStrings(string siteName)
        {
            var site = GetSiteByName(siteName);
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
            _serverManager?.Dispose();
        }
    }
}
