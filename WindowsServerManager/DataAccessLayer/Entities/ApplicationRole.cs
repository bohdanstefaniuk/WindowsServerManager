using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Entities
{
    public class ApplicationRole: IdentityRole
    {
        public string Description { get; set; }
    }
}