namespace Library.Web.Core.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
