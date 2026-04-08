namespace Library.Web.Core.Models
{
    public class CopyRental
    {
        public int CopyId { get; set; }
        public Copy Copy { get; set; } = null!;

        public int RentalId { get; set; }
        public Rental Rental { get; set; } = null!;
    }
}
