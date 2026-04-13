namespace Library.Web.Core.ViewModel.Category
{
    public class CategoryVM
    {
        public IEnumerable<CategoryRowVM> Categories { get; set; } = [];
        public int TotalCategories { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCategories / PageSize);

    }
}
