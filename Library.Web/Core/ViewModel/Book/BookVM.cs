namespace Library.Web.Core.ViewModel.Book
{
    public class BookVM
    {
        public IEnumerable<BookRowVM> Books { get; set; } = [];
        public int TotalBooks { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalBooks / PageSize);
    }
}
