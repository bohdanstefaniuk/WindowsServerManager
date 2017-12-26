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

        private bool IsSiteExists(string name)
        {
            return _serverManager.Sites.Any(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
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

        public bool DeleteSite(string name)
        {
            var site = GetSiteByName(name);
            _serverManager.Sites.Remove(site);
            _serverManager.CommitChanges();
            return !IsSiteExists(name);
        }

        public bool DeleteApplication(string name)
        {
            var application = GetApplicationByPath(name);
            foreach (var site in _serverManager.Sites)
            {
                if (site.Applications.Contains(application))
                {
                    site.Applications.Remove(application);
                }
            }
            _serverManager.CommitChanges();
            return !_serverManager.Sites.Any(x => x.Applications.Contains(application));
        }

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
