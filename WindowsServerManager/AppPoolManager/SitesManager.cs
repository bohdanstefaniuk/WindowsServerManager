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
        /// Get site exists
        /// </summary>
        /// <param name="name">Site name</param>
        /// <returns></returns>
        private bool IsSiteExists(string name)
        {
            return _serverManager.Sites.Any(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
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

        /// <summary>
        /// Get application by path
        /// </summary>
        /// <param name="appPath">Application path</param>
        /// <returns>Application</returns>
        public Application GetApplicationByPath(string appPath)
        {
            var sites = _serverManager.Sites;
            var applications = sites.Select(x => x.Applications);

            Application application = null;

            foreach (var app in applications)
            {
                application =
                    app.FirstOrDefault(x => x.Path.IndexOf(appPath, StringComparison.OrdinalIgnoreCase) >= 0);

                if (application != null)
                {
                    break;
                }
            }

            return application;
        }

        /// <summary>
        /// Get application by name for concrete site
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public Application GetApplicationByPath(string appPath, string siteName)
        {
            var site = _serverManager.Sites[siteName];
            return site.Applications.SingleOrDefault(x =>
                string.Equals(x.Path, appPath, StringComparison.CurrentCultureIgnoreCase));
        }


        /// <summary>
        /// Get all aplications by pool
        /// </summary>
        /// <param name="poolName">Application pool name</param>
        /// <returns>Collection of applications</returns>
        public IEnumerable<Application> GetApplicationsByPool(string poolName)
        {
            var apps = (from site in _serverManager.Sites
                from app in site.Applications
                where app.ApplicationPoolName.Equals(poolName)
                select app);

            return apps;
        }

        /// <summary>
        /// Delete site by name
        /// </summary>
        /// <param name="name">Site name</param>
        /// <returns>true when site doesn`t exists</returns>
        public bool DeleteSite(string name)
        {
            var site = GetSiteByName(name);
            _serverManager.Sites.Remove(site);
            _serverManager.CommitChanges();
            return !IsSiteExists(name);
        }

        /// <summary>
        /// Delete application by name for concrete site
        /// </summary>
        /// <param name="name"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public bool DeleteApplication(string name, string siteName)
        {
            var application = GetApplicationByPath(name, siteName);
            var sites = _serverManager.Sites[siteName];
            sites.Applications.Remove(application);
            _serverManager.CommitChanges();
            return !_serverManager.Sites.Any(x => x.Applications.Contains(application));
        }

        /// <summary>
        /// TODO GetSiteConnectionStrings()
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public List<string> GetSiteConnectionStrings(string siteName)
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
