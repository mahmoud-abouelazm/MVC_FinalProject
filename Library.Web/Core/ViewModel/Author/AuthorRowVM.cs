namespace Library.Web.Core.ViewModel.Author
{
    public class AuthorRowVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
        public int BookCount { get; set; }
    }
}
