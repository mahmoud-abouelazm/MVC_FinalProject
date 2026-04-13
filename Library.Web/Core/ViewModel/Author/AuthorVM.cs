namespace Library.Web.Core.ViewModel.Author
{
    public class AuthorVM
    {
        public IEnumerable<AuthorRowVM> Authors { get; set; } = [];
        public int TotalAuthors { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalAuthors / PageSize);
    }
}
