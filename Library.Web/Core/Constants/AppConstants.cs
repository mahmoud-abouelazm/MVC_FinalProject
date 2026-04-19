using Library.Web.Core.Models;

namespace Library.Web.Core.Constants
{
    public enum RentalState
    {
        Active,
        Returned,
        Overdue
    }
    public class PaginationParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
    public class PagedResult<T> where T : class
    {
        public IEnumerable<T>? Data { get; set; } 
        public int TotalCount { get; set; }
    }
    public class AppConstants
    {
    }
}
