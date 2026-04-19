using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Copies;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Web.Repository.Repositories
{
    public class CopyRepository(ApplicationDbContext context) : Repository<Copy>(context), ICopyRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<CopyRowVM>> GetAllRowsAsync(int page, int pageSize)
            => await BuildQuery()
                .OrderBy(c => c.Book.Title).ThenBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c=> new CopyRowVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookTitle = c.Book.Title,
                    BookId = c.BookId,
                    AllowToRental = c.AllowToRental,
                    HasRentals = c.CopyRentals.Any(),
                    RentalCount = c.CopyRentals.Count
                })
                .ToListAsync();

        public async Task<IEnumerable<CopyRowVM>> GetRowsByBookIdAsync(int bookId, int page, int pageSize)
            => await BuildQuery()
                .Where(c => c.BookId == bookId)
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c=> new CopyRowVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookTitle = c.Book.Title,
                    BookId = c.BookId,
                    AllowToRental = c.AllowToRental,
                    HasRentals = c.CopyRentals.Any(),
                    RentalCount = c.CopyRentals.Count
                })
                .ToListAsync();

        public async Task<int> CountAsync()
            => await _context.Copies.CountAsync();

        public async Task<int> CountByBookIdAsync(int bookId)
            => await _context.Copies.CountAsync(c => c.BookId == bookId);

        public async Task<Copy?> GetByIdWithRentalsAsync(int id)
            => await _context.Copies
                .Include(c => c.Book)
                .Include(c => c.CopyRentals)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> HasRentalHistoryAsync(int copyId)
            => await _context.CopyRentals.AnyAsync(cr => cr.CopyId == copyId);

        public Task DeleteAsync(Copy copy)
        {
            _context.Copies.Remove(copy);
            return _context.SaveChangesAsync();
        }

        private IQueryable<Copy> BuildQuery()
            => _context.Copies
                .Include(c => c.Book)
                .Include(c => c.CopyRentals);

        
    }
}