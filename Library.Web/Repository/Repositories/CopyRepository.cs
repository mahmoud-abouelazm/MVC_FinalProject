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

       public async Task<IEnumerable<CopyRowVM>> GetAllRowsAsync(int page, int pageSize, string search)
            => await ApplySearch(BuildQuery(), search)
                .OrderBy(c => c.Book.Title).ThenBy(c => c.Name)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(c => new CopyRowVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookTitle = c.Book.Title,
                    BookId = c.BookId,
                    AllowToRental = c.AllowToRental,
                    HasRentals = c.CopyRentals.Any(),
                    RentalCount = c.CopyRentals.Count
                }).ToListAsync();

        

        public async Task<IEnumerable<CopyRowVM>> GetRowsByBookIdAsync(
            int bookId, int page, int pageSize, string search)
            => await ApplySearch(BuildQuery().Where(c => c.BookId == bookId), search)
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(c => new CopyRowVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookTitle = c.Book.Title,
                    BookId = c.BookId,
                    AllowToRental = c.AllowToRental,
                    HasRentals = c.CopyRentals.Any(),
                    RentalCount = c.CopyRentals.Count
                }).ToListAsync();




        public async Task<int> CountAsync(string search)
            => await ApplySearch(BuildQuery(), search).CountAsync();

        public async Task<int> CountByBookIdAsync(int bookId, string search)
            => await ApplySearch(BuildQuery().Where(c => c.BookId == bookId), search).CountAsync();

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


        private static IQueryable<Copy> ApplySearch(IQueryable<Copy> query, string search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;
            var q = search.Trim().ToLower();
            return query.Where(c =>
                c.Name.ToLower().Contains(q) ||
                c.Book.Title.ToLower().Contains(q));
        }
    }
}