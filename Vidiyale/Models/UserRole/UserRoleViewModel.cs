using System.ComponentModel.DataAnnotations;

namespace Vidiyale.Models.UserRole
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
