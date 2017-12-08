using System.Configuration;
using DataAccessLayer.SeedData;

namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.EF.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        internal static string AppAdminName => ConfigurationManager.AppSettings["AppAdminName"];

        internal static string AppAdminEmail => ConfigurationManager.AppSettings["AppAdminEmail"];

        internal static string AppAdminPassword => ConfigurationManager.AppSettings["AppAdminPassword"];

        protected override void Seed(DataAccessLayer.EF.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            ApplicationUserSeed.SeedApplicationUsers(context);
        }
    }
}
