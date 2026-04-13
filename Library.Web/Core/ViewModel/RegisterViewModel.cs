using Library.Web.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Library.Web.Core.ViewModel
{
	public class RegisterViewModel
	{
		[MaxLength(50,ErrorMessage ="Maximum Length is 50 characters")]
		[Required(ErrorMessage = "Full Name is required")]
		public string FullName { get; set; } = null!;

		[EmailAddress]
		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; } = null!;
		

		[Required(ErrorMessage ="Password Is Required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
		public string Password { get; set; } = null!;

		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; } = null!;



		//Create a RegisterViewModel with FullName, Email, Password, ConfirmPassword.

		//Use[Required], [EmailAddress], [StringLength], and[Compare] data annotations for validation.

		//In AccountController.Register(POST), use UserManager<ApplicationUser>.CreateAsync.

		//If successful, assign the "User" role using UserManager.AddToRoleAsync.
	}
}
