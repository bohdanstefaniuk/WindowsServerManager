using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}