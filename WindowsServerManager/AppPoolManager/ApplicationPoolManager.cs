using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.Administration;

namespace AppPoolManager
{
    public class ApplicationPoolManager : IDisposable
    {
        private readonly ServerManager _serverManager;
        public ApplicationPoolManager()
        {
            _serverManager = new ServerManager();
        }

        /// <summary>
        /// Check state of application pool for stop states
        /// </summary>
        /// <param name="state">state of application pool</param>
        /// <returns>true if application pool state is stopping or stopped</returns>
        private bool IsPoolStoppingOrStopped(ObjectState state)
        {
            return state == ObjectState.Stopping || state == ObjectState.Stopped;
        }

        /// <summary>
        /// Check state of application pool for start states
        /// </summary>
        /// <param name="state">state of application pool</param>
        /// <returns>true if application pool state is starting or started</returns>
        private bool IsPoolStartingOrStarted(ObjectState state)
        {
            return state == ObjectState.Starting || state == ObjectState.Started;
        }

        /// <summary>
        /// Return all application pools hosted on currenct server
        /// </summary>
        /// <returns>application pool collection</returns>
        public ApplicationPoolCollection GetApplicationPoolCollection()
        {
            return _serverManager.ApplicationPools;
        }

        /// <summary>
        /// Get application pool by name
        /// </summary>
        /// <param name="name">Name of application pool</param>
        /// <returns>Application pool</returns>
        public ApplicationPool GetApplicationPoolByName(string name)
        {
            var pools = _serverManager.ApplicationPools;
            return pools.SingleOrDefault(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Start application pool by name
        /// </summary>
        /// <param name="name">Name of application pool</param>
        /// <returns>true if application pool is starting or started</returns>
        public bool StartPoolByName(string name)
        {
            var pool = GetApplicationPoolByName(name);
            var poolState = pool.Start();
            return IsPoolStartingOrStarted(poolState);
        }

        /// <summary>
        /// Stop application pool by name
        /// </summary>
        /// <param name="name">Name of application pool</param>
        /// <returns>true if application pool is stopping or stopped</returns>
        public bool StopPoolByName(string name)
        {
            var pool = GetApplicationPoolByName(name);
            var poolState = pool.Stop();
            return IsPoolStoppingOrStopped(poolState);
            
        }

        public bool IsPoolStartingOrStarted(string poolName)
        {
            var pool = GetApplicationPoolByName(poolName);
            return IsPoolStartingOrStarted(pool.State);
        }

        public bool IsPoolStoppingOrStopped(string poolName)
        {
            var pool = GetApplicationPoolByName(poolName);
            return IsPoolStoppingOrStopped(pool.State);
        }

        /// <summary>
        /// TODO Implemetn this method in the next version (1.3)
        /// </summary>
        /// <returns></returns>
        public bool AddApplicationPool()
        {
            return default(bool);
        }

        public void Dispose()
        {
            _serverManager?.Dispose();
        }
    }
}
