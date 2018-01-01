using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(WebUI.FullFramework.Startup))]
namespace WebUI.FullFramework
{
    public class Startup
    {
        readonly IServiceCreator _serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<IJsTreeMenuService>(CreateJsTreeMenuService);
            app.CreatePerOwinContext<IFeatureService>(CreateFeatureService);
            app.CreatePerOwinContext<ISettingsService>(CreateSettingsService);
            app.CreatePerOwinContext<IConnectionStringsService>(CreateConnectionStringsService);
            app.CreatePerOwinContext<IApplicationPoolService>(CreateApplciationPoolService);
            app.CreatePerOwinContext<IRedisService>(CreateRedisService);
            app.CreatePerOwinContext<IActionLogger>(CreateActionLogger);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return _serviceCreator.CreateUserService("DbConnection");
        }

        private ISettingsService CreateSettingsService()
        {
            return _serviceCreator.CreateSettingsService();
        }

        private IJsTreeMenuService CreateJsTreeMenuService()
        {
            return _serviceCreator.CreateJsTreeMenuService();
        }

        private IFeatureService CreateFeatureService()
        {
            return _serviceCreator.CreateFeatureService();
        }

        private IConnectionStringsService CreateConnectionStringsService()
        {
            return _serviceCreator.CreateConnectionStringsService();
        }

        private IApplicationPoolService CreateApplciationPoolService()
        {
            return _serviceCreator.CreateApplciationPoolService();
        }

        private IRedisService CreateRedisService()
        {
            return _serviceCreator.CreateRedisManager();
        }

        private IActionLogger CreateActionLogger()
        {
            return _serviceCreator.CreateActionLogger();
        }
    }
}