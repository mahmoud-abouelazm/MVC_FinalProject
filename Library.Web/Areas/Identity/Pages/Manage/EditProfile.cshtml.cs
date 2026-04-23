using Library.Web.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Library.Web.Areas.Identity.Pages.Account.LoginModel;

namespace Library.Web.Areas.Identity.Pages.Manage
{
	[Authorize]
    public class EditProfileModel : PageModel
    {
		private readonly UserManager<ApplicationUser> userManager;

		public EditProfileModel(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[MaxLength(50, ErrorMessage = "Maximum Length is 50 characters")]
				[Required(ErrorMessage = "Full Name is required")]
				[Display(Name = "Full Name")]
			public required string FullName { get; set; }

			[Phone]
			[Required(ErrorMessage = "phone is required")]
			public required string PhoneNumber { get; set; }
		}


		public async Task OnGetAsync()
        {
			var user =await userManager.GetUserAsync(User);
			Input=new InputModel
			{
				FullName = user.FullName,
				PhoneNumber = user?.PhoneNumber?? "No phone number"
			};
		}

        public async Task<ActionResult> OnPostAsync()
        {
			if(!ModelState.IsValid) return Page();

			var user = await userManager.GetUserAsync(User);
			user?.FullName = Input.FullName;
			user?.PhoneNumber = Input.PhoneNumber;
			
			await userManager.UpdateAsync(user);

			return RedirectToPage("profile");

		}
    }
}
