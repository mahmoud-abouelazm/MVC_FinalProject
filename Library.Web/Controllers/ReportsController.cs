using Library.Web.Core.Constants;
using Library.Web.Core.ViewModel.Reports;
using Library.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var year = DateTime.UtcNow.Year;
            var monthLabels = Enumerable.Range(1, 12)
                .Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m))
                .ToList();

            var totalUsers = await _context.Users.CountAsync();
            var totalBooks = await _context.Books.CountAsync(b => !b.IsDeleted);
            var totalAuthors = await _context.Authors.CountAsync();
            var totalRevenue = await _context.Rentals
                .Where(r => r.Status == RentalState.Returned)
                .SumAsync(r => (decimal?)r.Amount) ?? 0m;

            var usersByMonthRaw = await _context.Rentals
                .AsNoTracking()
                .Where(r => r.RentedAt.Year == year)
                .GroupBy(r => r.RentedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Select(r => r.ApplicationUserId).Distinct().Count()
                })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            var revenueByMonthRaw = await _context.Rentals
                .AsNoTracking()
                .Where(r => r.Status == RentalState.Returned && r.ReturnedAt.HasValue && r.ReturnedAt.Value.Year == year)
                .GroupBy(r => r.ReturnedAt!.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Amount = g.Sum(r => r.Amount)
                })
                .ToDictionaryAsync(x => x.Month, x => x.Amount);

            var rentalsByMonthRaw = await _context.Rentals
                .AsNoTracking()
                .Where(r => r.RentedAt.Year == year)
                .GroupBy(r => r.RentedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            var authorsByMonthRaw = await _context.CopyRentals
                .AsNoTracking()
                .Where(cr => cr.Rental.RentedAt.Year == year)
                .SelectMany(cr => cr.Copy.Book.BookAuthors.Select(ba => new
                {
                    Month = cr.Rental.RentedAt.Month,
                    ba.AuthorId
                }))
                .GroupBy(x => x.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Select(x => x.AuthorId).Distinct().Count()
                })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            var categoryData = await _context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    BooksCount = c.Books.Count(b => !b.IsDeleted)
                })
                .OrderByDescending(x => x.BooksCount)
                .ToListAsync();

            var usersByMonth = Enumerable.Range(1, 12).Select(m => usersByMonthRaw.GetValueOrDefault(m)).ToList();
            var revenueByMonth = Enumerable.Range(1, 12).Select(m => revenueByMonthRaw.GetValueOrDefault(m)).ToList();
            var rentalsByMonth = Enumerable.Range(1, 12).Select(m => rentalsByMonthRaw.GetValueOrDefault(m)).ToList();
            var authorsByMonth = Enumerable.Range(1, 12).Select(m => authorsByMonthRaw.GetValueOrDefault(m)).ToList();

            var vm = new ReportsVM
            {
                ReportYear = year,
                TotalUsers = totalUsers,
                TotalBooks = totalBooks,
                TotalAuthors = totalAuthors,
                TotalRevenue = totalRevenue,
                MonthLabels = monthLabels,
                UsersByMonth = usersByMonth,
                RevenueByMonth = revenueByMonth,
                RentalsByMonth = rentalsByMonth,
                AuthorsByMonth = authorsByMonth,
                CategoryLabels = categoryData.Select(x => x.Name).ToList(),
                CategoryBookCounts = categoryData.Select(x => x.BooksCount).ToList()
            };

            return View(vm);
        }
    }
}
