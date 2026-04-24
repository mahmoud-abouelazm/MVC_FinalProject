namespace Library.Web.Core.ViewModel.Copies
{
    public class CopyRowVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string BookTitle { get; set; } = null!;
        public int BookId { get; set; }
        public bool AllowToRental { get; set; }
        public bool HasRentals { get; set; }
        public int RentalCount { get; set; }

        public string StatusLabel => AllowToRental ? "Available" : "Restricted";
    }
}
