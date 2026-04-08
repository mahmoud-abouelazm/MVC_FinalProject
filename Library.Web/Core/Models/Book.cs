namespace Library.Web.Core.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Img { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<Copy> Copies { get; set; } = new List<Copy>();
    }
}
