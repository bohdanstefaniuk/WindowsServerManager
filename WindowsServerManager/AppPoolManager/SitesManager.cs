using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
                _serverManager.Dispose();
            }
            _disposed = true;
        }
    }
}
