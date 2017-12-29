using DataAccessLayer.Enums;

namespace WebUI.FullFramework.Models
{
    public class ChangeRoleModel
    {
        public string RedirectUrl { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}