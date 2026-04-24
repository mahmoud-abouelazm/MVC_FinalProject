using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Users
{
    public class CreateUserVM
    {

        public int Id { get; set; } = 0;

        [Required ,StringLength(30)]
        public string FullName { get; set; }
        [Required]

        [StringLength(12)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public int  RoleId { get; set; } = 2;
        public IEnumerable<SelectListItem> Roles { get; set; } = [];

    }
}
