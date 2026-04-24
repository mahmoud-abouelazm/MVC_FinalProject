using Library.Web.Core.ViewModel.Category;

namespace Library.Web.Core.ViewModel.Users
{
    public class UserVM
    {
        public IEnumerable<UserRowVM> Users { get; set; } = [];
        public int TotalUsers { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalUsers / PageSize);
        public string Search { get; set; } = "";

        public int ActiveUsers { get; set; }
        public int NotActiveUsers { get; set; }

        

    }
}
