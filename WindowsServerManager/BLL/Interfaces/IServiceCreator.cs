using BLL.Services;

namespace BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        IJsTreeMenuService CreaJsTreeMenuService();
    }
}