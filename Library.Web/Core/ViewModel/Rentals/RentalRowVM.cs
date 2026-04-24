using Library.Web.Core.Constants;

namespace Library.Web.Core.ViewModel.Rentals
{
    public class RentalRowVM
    {
        public int Id { get; set; }
        public string MemberName { get; set; } = null!;
        public string MemberEmail { get; set; } = null!;
        public string CopyNames { get; set; } = "";  
        public string BookTitles { get; set; } = "";   
        public DateTime RentedAt { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public decimal Amount { get; set; }
        public RentalState Status { get; set; }
        public int CopyCount { get; set; }

        public bool IsOverdue => Status == RentalState.Active && DateTime.UtcNow > DueAt;
    }
}
