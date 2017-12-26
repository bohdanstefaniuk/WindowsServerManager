using System;
using System.Dynamic;
using System.Linq;
using AppPoolManager;
using BLL.Dto;
using BLL.Enums;
using BLL.Interfaces;

namespace BLL.Services
{
    public class ApplicationPoolService : IApplicationPoolService
    {
        private readonly ApplicationPoolManager _applicationPoolManager;

        public ApplicationPoolService()
        {
            _applicationPoolManager = new ApplicationPoolManager();
        }

        /// <summary>
        /// Start application pool by name
        /// </summary>
        /// <param name="name">Name of application pool</param>
        /// <returns>true if application pool starting or started</returns>
        public bool StartPoolByName(string name)
        {
           return _applicationPoolManager.StartPoolByName(name);
        }

        /// <summary>
        /// Stop application pool by name
        /// </summary>
        /// <param name="name">Name of application pool</param>
        /// <returns>true if application pool stopping or stopped</returns>
        public bool StopPoolByName(string name)
        {
            return _applicationPoolManager.StopPoolByName(name);
        }

        public bool RecyclePoolByName(string name)
        {
            return _applicationPoolManager.RecyclePoolByName(name);
        }

        // TODO Refactor Need to exlude facade method
        public void DeleteApplication(string name, ApplicationDeleteDepth deleteDepth, IISSiteType siteType)
        {
            var rootPath = "";
            var applicationPoolName = "";

            if ((deleteDepth & ApplicationDeleteDepth.ApplicationOrSite) != 0)
            {
                var siteManager = new SitesManager();
                switch (siteType)
                {
                    case IISSiteType.Site:
                        var site = siteManager.GetSiteByName(name);
                        var applicationRoot =
                            site.Applications.SingleOrDefault(a => a.Path == "/");
                        var virtualRoot =
                            applicationRoot?.VirtualDirectories.SingleOrDefault(v => v.Path == "/");
                        rootPath = virtualRoot?.PhysicalPath;
                        applicationPoolName = applicationRoot?.ApplicationPoolName;
                        siteManager.DeleteSite(name);
                        break;
                    case IISSiteType.Application:
                        var app = siteManager.GetApplicationByPath(name);
                        applicationPoolName = app.ApplicationPoolName;
                        rootPath = app.VirtualDirectories.SingleOrDefault(v => v.Path == "/")?.PhysicalPath;
                        siteManager.DeleteApplication(app.Path);
                        break;
                }
                
            }

            if ((deleteDepth & ApplicationDeleteDepth.FileSystem) != 0 && !string.IsNullOrEmpty(rootPath))
            {
                var fileSystemService = new FileSystemService();
                fileSystemService.DeleteFolder(rootPath);
            }

            if ((deleteDepth & ApplicationDeleteDepth.ApplicationPool) != 0)
            {
                _applicationPoolManager.Delete(applicationPoolName);
            }
        }

        public bool IsPoolStartingOrStarted(string poolName)
        {
            return _applicationPoolManager.IsPoolStartingOrStarted(poolName);
        }

        public bool IsPoolStoppingOrStopped(string poolName)
        {
            return _applicationPoolManager.IsPoolStoppingOrStopped(poolName);
        }

        public void Dispose()
        {
            _applicationPoolManager?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}