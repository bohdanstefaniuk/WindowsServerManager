using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task DeleteApplicationAsync(DeleteApplicationDto dto)
        {
            var rootPath = "";
            var applicationPoolName = "";

            if (dto.DeleteStates.NeedDeleteApplication)
            {
                var siteManager = new SitesManager();
                switch (dto.SiteType)
                {
                    case IISSiteType.Site:
                        var site = siteManager.GetSiteByName(dto.Name);
                        var applicationRoot =
                            site.Applications.SingleOrDefault(a => a.Path == "/");
                        var virtualRoot =
                            applicationRoot?.VirtualDirectories.SingleOrDefault(v => v.Path == "/");
                        rootPath = virtualRoot?.PhysicalPath;
                        applicationPoolName = applicationRoot?.ApplicationPoolName;
                        siteManager.DeleteSite(dto.Name);
                        break;
                    case IISSiteType.Application:
                        var app = siteManager.GetApplicationByPath(dto.Name, dto.SiteName);
                        applicationPoolName = app.ApplicationPoolName;
                        rootPath = app.VirtualDirectories.SingleOrDefault(v => v.Path == "/")?.PhysicalPath;
                        siteManager.DeleteApplication(app.Path, dto.SiteName);
                        break;
                }
                
            }

            if (dto.DeleteStates.NeedDeleteApplicationPool)
            {
                _applicationPoolManager.Delete(applicationPoolName);
            }

            if (dto.DeleteStates.NeedDeleteDatabase && !string.IsNullOrEmpty(dto.Database))
            {
                using (var dbService = new DbService())
                {
                    try
                    {
                        await dbService.DropDatabase(dto.Database);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }

            if (dto.DeleteStates.NeedDeleteFiles)
            {
                var fileSystemService = new FileSystemService();
                fileSystemService.DeleteFolder(rootPath);
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