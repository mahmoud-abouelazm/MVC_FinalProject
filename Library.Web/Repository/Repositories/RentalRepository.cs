using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Rentals;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
        public class RentalRepository(ApplicationDbContext context): Repository<Rental>(context), IRentalRepository
        {
            private readonly ApplicationDbContext _context = context;
            public async Task<IEnumerable<Rental>> GetAllAsync()
		{
			return await dbcontext.Rentals.Include(r => r.CopyRentals).ThenInclude(cr => cr.Copy).ThenInclude(c => c.Book).ToListAsync();
		}

		public async Task<IEnumerable<Rental>> GetAllRentalsForSpecificUserAsync(int userId)
		{
			return await  dbcontext.Rentals.Where(r=>r.ApplicationUserId==userId).Include(r=>r.CopyRentals).ThenInclude(cr=>cr.Copy).ThenInclude(c=>c.Book).ToListAsync();
		}


            public async Task<(IEnumerable<RentalRowVM> Rows, int Total)> GetPagedAsync(int page, int pageSize, string search, string statusFilter)
            {

                var now = DateTime.UtcNow;
                var staleActive = await _context.Rentals
                    .Where(r => r.Status == RentalState.Active && r.DueAt < now)
                    .ToListAsync();

                foreach (var r in staleActive)
                    r.Status = RentalState.Overdue;

                if (staleActive.Any())
                    await _context.SaveChangesAsync();

                var query = _context.Rentals
                    .Include(r => r.ApplicationUser)
                    .Include(r => r.CopyRentals)
                        .ThenInclude(cr => cr.Copy)
                            .ThenInclude(c => c.Book)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var q = search.Trim().ToLower();
                    query = query.Where(r =>
                        r.ApplicationUser.FullName.ToLower().Contains(q) ||
                        r.ApplicationUser.Email!.ToLower().Contains(q) ||
                        r.CopyRentals.Any(cr =>
                            cr.Copy.Name.ToLower().Contains(q) ||
                            cr.Copy.Book.Title.ToLower().Contains(q)));
                }

                if (!string.IsNullOrEmpty(statusFilter) &&
                    Enum.TryParse<RentalState>(statusFilter, out var stateFilter))
                {
                    query = query.Where(r => r.Status == stateFilter);
                }

                int total = await query.CountAsync();

                var rows = await query
                    .OrderByDescending(r => r.RentedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => new RentalRowVM
                    {
                        Id = r.Id,
                        MemberName = r.ApplicationUser.FullName,
                        MemberEmail = r.ApplicationUser.Email!,
                        RentedAt = r.RentedAt,
                        DueAt = r.DueAt,
                        ReturnedAt = r.ReturnedAt,
                        Amount = r.Amount,
                        Status = r.Status,
                        CopyCount = r.CopyRentals.Count,
                        CopyNames = string.Join(", ", r.CopyRentals.Select(cr => cr.Copy.Name)),
                        BookTitles = string.Join(", ", r.CopyRentals.Select(cr => cr.Copy.Book.Title).Distinct())
                    })
                    .ToListAsync();

                return (rows, total);
            }

            public async Task<ReturnConfirmVM?> GetReturnConfirmVmAsync(int rentalId)
            {
                var rental = await _context.Rentals
                    .Include(r => r.ApplicationUser)
                    .Include(r => r.CopyRentals)
                        .ThenInclude(cr => cr.Copy)
                            .ThenInclude(c => c.Book)
                    .FirstOrDefaultAsync(r => r.Id == rentalId);

                if (rental is null) return null;

                var returnDate = DateTime.UtcNow;
                var daysRented = Math.Max(1, (int)Math.Ceiling((returnDate - rental.RentedAt).TotalDays));
                var totalPrice = rental.CopyRentals.Sum(cr => cr.Copy.Book.Price);
                var isLate = returnDate > rental.DueAt;
                var daysLate = isLate
                    ? (int)Math.Ceiling((returnDate - rental.DueAt).TotalDays)
                    : 0;

                var daysOnTime = daysRented - daysLate;
                var baseAmount = daysOnTime * totalPrice;
                var penalty = isLate ? daysLate * totalPrice * 2 : 0;
                var total = baseAmount + penalty;

                return new ReturnConfirmVM
                {
                    RentalId = rental.Id,
                    MemberName = rental.ApplicationUser.FullName,
                    MemberEmail = rental.ApplicationUser.Email!,
                    CopyNames = string.Join(", ", rental.CopyRentals.Select(cr => cr.Copy.Name)),
                    BookTitles = string.Join(", ", rental.CopyRentals.Select(cr => cr.Copy.Book.Title).Distinct()),
                    RentedAt = rental.RentedAt,
                    DueAt = rental.DueAt,
                    BookPrice = totalPrice,
                    DaysRented = daysRented,
                    IsLate = isLate,
                    DaysLate = daysLate,
                    BaseAmount = baseAmount,
                    PenaltyAmount = penalty,
                    TotalAmount = total
                };
            }

            public async Task<Rental?> GetByIdWithDetailsAsync(int rentalId)
                => await _context.Rentals
                    .Include(r => r.CopyRentals)
                        .ThenInclude(cr => cr.Copy)
                            .ThenInclude(c => c.Book)
                    .FirstOrDefaultAsync(r => r.Id == rentalId);

            public async Task<(int Active, int Overdue, int Returned, decimal Revenue)> GetSummaryAsync()
            {
                var active = await _context.Rentals.CountAsync(r => r.Status == RentalState.Active);
                var overdue = await _context.Rentals.CountAsync(r => r.Status == RentalState.Overdue);
                var returned = await _context.Rentals.CountAsync(r => r.Status == RentalState.Returned);
                var revenue = await _context.Rentals.Where(r => r.Status == RentalState.Returned).SumAsync(r => r.Amount);

                return (active, overdue, returned, revenue);
            }

            public async Task<Rental?> GetActiveRentalByCopyIdAsync(int copyId)
                => await _context.Rentals
                    .Include(r => r.CopyRentals)
                    .FirstOrDefaultAsync(r =>
                        r.CopyRentals.Any(cr => cr.CopyId == copyId) &&
                        r.Status != RentalState.Returned);
        }
    }
 }
