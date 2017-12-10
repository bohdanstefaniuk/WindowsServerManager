using System;
using System.Threading.Tasks;
using DataAccessLayer.Identity;

namespace DataAccessLayer.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}