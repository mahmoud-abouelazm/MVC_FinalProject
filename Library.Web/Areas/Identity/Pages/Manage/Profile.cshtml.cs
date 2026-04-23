using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Repository.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Library.Web.Areas.Identity.Pages.Manage
{
	[Authorize]
    public class ProfileModel : PageModel
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IRentalRepository rentalRepository;

		public List<Rental> ReturnedRentals { get; set; }=new List<Rental>();
		public List<Rental> ActiveRentals { get; set; }=new List<Rental>();
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public int NumOfActiveRentals { get; set; }
		public int NumOfOverDueRentals { get; set; }

		public ProfileModel(UserManager<ApplicationUser> userManager,IRentalRepository rentalRepository)
		{
			this.userManager = userManager;
			this.rentalRepository = rentalRepository;
		}



		public async Task OnGetAsync()
        {
			 var user =await userManager.GetUserAsync(User);
			//var id=User.FindFirst(ClaimTypes.NameIdentifier);
			if (user != null)
			{
				Name = user.FullName;
				Email = user.Email??"Not Exist";
				PhoneNumber = user.PhoneNumber??"No phone";

			var AllRental = await rentalRepository.GetAllRentalsForSpecificUserAsync(user.Id);
			ReturnedRentals = AllRental.Where(r => r.ReturnedAt != null).ToList();
			ActiveRentals = AllRental.Where(r => r.ReturnedAt == null).ToList();
			NumOfActiveRentals=ActiveRentals.Where(r => DateTime.UtcNow <= r.DueAt).Count();
			NumOfOverDueRentals=ActiveRentals.Where(r=> DateTime.UtcNow>r.DueAt).Count();			
			}


		}
    }
}
