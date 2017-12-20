using System;
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

        public void Dispose()
        {
            _applicationPoolManager?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}