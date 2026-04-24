namespace Library.Web.Core.ViewModel.Rentals
{
    public class RentalCreateVM
    {
        public int BookId { get; set; }
        public int CopyId { get; set; }
        public int UserId { get; set; }
        public DateTime DueAt { get; set; } = DateTime.Today.AddDays(14);
    }
}
