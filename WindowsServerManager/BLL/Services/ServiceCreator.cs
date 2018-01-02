using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Infrastructure;
using BLL.Interfaces;
using DataAccessLayer.Repositories;

namespace BLL.Services
{
    //TODO Change to DI
    //Rename for some kind of factory
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        public ISettingsService CreateSettingsService()
        {
            return new SettingsService(new UnitOfWork());
        }
        
        public IJsTreeMenuService CreateJsTreeMenuService()
        {
            return new JsTreeMenuService();
        }

        public IFeatureService CreateFeatureService()
        {
            return new FeatureService();
        }

        public IConnectionStringsService CreateConnectionStringsService()
        {
            return new ConnectionStringsService();
        }

        public IApplicationPoolService CreateApplciationPoolService()
        {
            return new ApplicationPoolService();
        }

        public IActionLogsService CreateActionLogger()
        {
            return new ActionLogsService(new UnitOfWork());
        }

        public IRedisService CreateRedisManager()
        {
            return new RedisService();
        }
    }
}
