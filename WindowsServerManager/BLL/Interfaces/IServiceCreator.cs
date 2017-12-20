using BLL.Services;

namespace BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        IJsTreeMenuService CreateJsTreeMenuService();
        IFeatureService CreateFeatureService();
        ISettingsService CreateSettingsService();
        IConnectionStringsService CreateConnectionStringsService();
        IApplicationPoolService CreateApplciationPoolService();
    }
}