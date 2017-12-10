using System;
using System.Threading.Tasks;
using DataAccessLayer.EF;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity;
using DataAccessLayer.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Repositories
{
    public class IdentityUnitOfWork: IIdentityUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        public IdentityUnitOfWork(string connectionString)
        {
            _db = new ApplicationDbContext(connectionString);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
        }

        public ApplicationUserManager UserManager => _userManager;

        public ApplicationRoleManager RoleManager => _roleManager;

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _userManager.Dispose();
                _roleManager.Dispose();
            }
            _disposed = true;
        }
    }
}