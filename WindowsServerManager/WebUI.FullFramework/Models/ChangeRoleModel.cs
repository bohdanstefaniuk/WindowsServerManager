using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Enums;

namespace WebUI.FullFramework.Models
{
    public class ChangeRoleModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}