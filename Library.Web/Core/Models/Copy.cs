namespace Library.Web.Core.Models
{
    public class Copy
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool AllowToRental { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public ICollection<CopyRental> CopyRentals { get; set; } = new List<CopyRental>();
    }
}
