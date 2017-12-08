using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.EF;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.SeedData
{
    internal static class ApplicationUserSeed
    {
        internal static void SeedApplicationUsers(ApplicationDbContext dbContext)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                roleManager.Create(new ApplicationRole
                {
                    Name = role.ToString(),
                    Description = GetRoleDescriptions()[role]
                });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));

            // Seed admin user
            var admin = new ApplicationUser
            {
                Email = Configuration.AppAdminEmail,
                EmailConfirmed = true,
                UserName = Configuration.AppAdminEmail
            };

            var result = userManager.Create(admin, Configuration.AppAdminPassword);
            if (result.Succeeded)
            {
                var adminRole = roleManager.Roles.SingleOrDefault(x => x.Name == Role.Admin.ToString());
                if (adminRole != null)
                {
                    userManager.AddToRole(admin.Id, adminRole.Name);
                }
            }
        }

        private static IDictionary<Role, string> GetRoleDescriptions()
        {
            return new Dictionary<Role, string>
            {
                { Role.Admin, "Администратор системы, имеет полный доступ на все операции" },
                { Role.BusinessAnalytics, "Бизнес аналитик, в него есть возможность управлять состоянием фич для приложения и удалять" },
                { Role.Developers, "Разработчик, имеет доступ ко всем операциям, кроме администрирования системы" }
            };
        }
    }
}