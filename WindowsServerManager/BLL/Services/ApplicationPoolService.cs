using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AppPoolManager;
using BLL.Dto;
using BLL.Enums;
using BLL.Interfaces;
using Microsoft.Web.Administration;

namespace BLL.Services
{
    public class ApplicationPoolService : IApplicationPoolService
    {
        private readonly ApplicationPoolManager _applicationPoolManager;

        #region Constructors: Public

        public ApplicationPoolService()
        {
            _applicationPoolManager = new ApplicationPoolManager();
        }

        #endregion

        #region Methods: Privat

        private bool DeleteApplicationPool(string applicationPoolName)
        {
            try
            {
                _applicationPoolManager.Delete(applicationPoolName);
                return _applicationPoolManager.GetApplicationPoolByName(applicationPoolName) == null;
            }
            catch
            {
                // ignored
            }

            return false;
        }

        private async Task<bool> DeleteDatabase(string databaseName)
        {
            using (var dbService = new DbService())
            {
                try
                {
                    await dbService.DropDatabase(databaseName);
                    return await dbService.IsDatabaseExists(databaseName);
                }
                catch
                {
                    // ignored
                }
            }

            return false;
        }

        private void DeleteFolder(string path)
        {
            var fileSystemService = new FileSystemService();
            fileSystemService.DeleteFolder(path);
        }

        #endregion

        #region Methods: Public

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

        /// <summary>
        /// Recycle application pool by name
        /// </summary>
        /// <param name="name">Application pool name</param>
        /// <returns>True when pool starting or started</returns>
        public bool RecyclePoolByName(string name)
        {
            return _applicationPoolManager.RecyclePoolByName(name);
        }

        /// <summary>
        /// Delete application and related files/site/database.
        /// </summary>
        /// <param name="dto">Delete params</param>
        /// <returns>async task</returns>
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
                siteManager.Dispose();

            }

            if (dto.DeleteStates.NeedDeleteApplicationPool)
            {
                DeleteApplicationPool(applicationPoolName);
            }

            if (dto.DeleteStates.NeedDeleteDatabase && !string.IsNullOrEmpty(dto.Database))
            {
                await DeleteDatabase(dto.Database);
            }

            if (dto.DeleteStates.NeedDeleteFiles)
            {
                DeleteFolder(rootPath);
            }

            if (dto.DeleteStates.NeedDeleteChildApplications)
            {
                
            }
        }

        /// <summary>
        /// Get status of application pool
        /// </summary>
        /// <param name="poolName">application pool name</param>
        /// <returns>true when pool started or starting</returns>
        public bool IsPoolStartingOrStarted(string poolName)
        {
            return _applicationPoolManager.IsPoolStartingOrStarted(poolName);
        }

        /// <summary>
        /// Get status of application pool
        /// </summary>
        /// <param name="poolName">application pool name</param>
        /// <returns>true when pool stoped or stoping</returns>
        public bool IsPoolStoppingOrStopped(string poolName)
        {
            return _applicationPoolManager.IsPoolStoppingOrStopped(poolName);
        }

        /// <summary>
        /// Get list of application for pool
        /// </summary>
        /// <param name="poolName">Applciation pool name</param>
        /// <returns></returns>
        public IEnumerable<Application> GetApplicationsByPool(string poolName)
        {
            using (var siteManager = new SitesManager())
            {
                return siteManager.GetApplicationsByPool(poolName);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _applicationPoolManager?.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion


    }
}