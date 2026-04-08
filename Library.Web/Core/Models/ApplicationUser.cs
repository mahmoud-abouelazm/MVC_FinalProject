using Microsoft.AspNetCore.Identity;

namespace Library.Web.Core.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    }
}
