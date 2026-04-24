using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Users
{
    public class EditUserRoleVM
    {
        public int UserId { get; set; }

        public string? UserName { get; set; } = null!;

        public string? CurrentRole { get; set; } = null!;

        [Required(ErrorMessage = "Please select a role.")]
        public string NewRole { get; set; } = null!;

        public List<SelectListItem> Roles { get; set; } = [];
    }
}
