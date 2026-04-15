using Library.Web.Core.Models;
namespace Library.Web.Core.ViewModel.BookVMs
{
    public class BookDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Img { get; set; }
        public decimal Price { get; set; }
        public int copiesNumber { get; set; }
        public string Category { get; set; }
        public List<Models.Author> Authors { get; set; }
    }
}
