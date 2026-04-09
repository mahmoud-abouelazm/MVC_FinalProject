using Library.Web.Core.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Web.Core.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime RentedAt { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public decimal Amount { get; set; }
        public RentalState Status { get; set; }  // e.g. "Active", "Returned", "Overdue"

        [ForeignKey(nameof(ApplicationUser))]
        public int ApplicationUserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public ICollection<CopyRental> CopyRentals { get; set; } = new List<CopyRental>();
    }
}
