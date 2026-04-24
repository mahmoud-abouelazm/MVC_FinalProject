namespace Library.Web.Core.ViewModel.Reports
{
    public class ReportsVM
    {
        public int ReportYear { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBooks { get; set; }
        public int TotalAuthors { get; set; }
        public decimal TotalRevenue { get; set; }

        public IReadOnlyList<string> MonthLabels { get; set; } = [];
        public IReadOnlyList<int> UsersByMonth { get; set; } = [];
        public IReadOnlyList<decimal> RevenueByMonth { get; set; } = [];
        public IReadOnlyList<int> RentalsByMonth { get; set; } = [];
        public IReadOnlyList<int> AuthorsByMonth { get; set; } = [];

        public IReadOnlyList<string> CategoryLabels { get; set; } = [];
        public IReadOnlyList<int> CategoryBookCounts { get; set; } = [];
    }
}
