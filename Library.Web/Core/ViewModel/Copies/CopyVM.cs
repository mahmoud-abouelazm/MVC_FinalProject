namespace Library.Web.Core.ViewModel.Copies
{
    public class CopyVM
    {
        public IEnumerable<CopyRowVM> Copies { get; set; } = [];

        public int? FilterBookId { get; set; }
        public string FilterBookTitle { get; set; } = "";
        public string Search { get; set; } = "";

        public int TotalCopies { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalPages => (int)Math.Ceiling((double)TotalCopies / PageSize);
    }
}
