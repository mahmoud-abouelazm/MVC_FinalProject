namespace Library.Web.Core.ViewModel.Rentals
{
    public class RentalVM
    {
        public IEnumerable<RentalRowVM> Rentals { get; set; } = [];
        public int TotalRentals { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalPages => (int)Math.Ceiling((double)TotalRentals / PageSize);
        public string Search { get; set; } = "";
        public string StatusFilter { get; set; } = "";   // "Active" | "Returned" | "Overdue" | ""

       
        public int ActiveCount { get; set; }
        public int OverdueCount { get; set; }
        public int ReturnedCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
