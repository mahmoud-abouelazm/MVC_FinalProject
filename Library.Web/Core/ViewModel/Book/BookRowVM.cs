namespace Library.Web.Core.ViewModel.Book
{
    public class BookRowVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Img { get; set; }
        public string AuthorNames { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
