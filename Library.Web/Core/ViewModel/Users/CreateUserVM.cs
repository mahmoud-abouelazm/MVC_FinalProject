using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Core.ViewModel.Users
{
    public class CreateUserVM
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Phone must be between 7 and 12 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Remote(
            action: "CheckEmail",
            controller: "AdminUsers",
            HttpMethod = "GET",
            ErrorMessage = "This email is already registered")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must have uppercase, lowercase, number and special character")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role")]
        public int RoleId { get; set; } = 2;

        public IEnumerable<SelectListItem> Roles { get; set; } = [];
    

    }
}
