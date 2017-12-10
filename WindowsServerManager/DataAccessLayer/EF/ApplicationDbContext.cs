using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.EF
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string conectionString) : base(conectionString) { }

        public ApplicationDbContext(): base("DbConnection") {}

        public DbSet<Settings> Settings { get; set; }
    }
}
