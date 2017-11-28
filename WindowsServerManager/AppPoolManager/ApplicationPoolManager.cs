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

        public ApplicationPoolCollection GetApplicationPoolCollection()
        {
            return _serverManager.ApplicationPools;
        }

        public ApplicationPool GetApplicationPoolByName(string name)
        {
            var pools = _serverManager.ApplicationPools;
            return pools.SingleOrDefault(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
        }

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
