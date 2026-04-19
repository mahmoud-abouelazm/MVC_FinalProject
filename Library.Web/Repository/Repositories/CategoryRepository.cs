using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Category;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories
{
    public class CategoryRepository(ApplicationDbContext context) : Repository<Category>(context), ICategoryRepository
    {
        private readonly ApplicationDbContext _context = context;

        public Task DeleteAsync(Category? category)
        {
            _context.Categories.Remove(category!);
            return Task.CompletedTask;
        }

        public async Task<List<CategoryRowVM>> GetAllAsync()
        {
            var query = _context.Categories
                .Include(c => c.Books);

                var  data = await query
                            .Select(c => new CategoryRowVM
                             {
                              Id = c.Id,
                              Name = c.Name,
                              BookCount = c.Books.Count
                            })
                  .ToListAsync();

            return data;
        }

        public async Task<PagedResult<CategoryRowVM>> GetAllAsync(PaginationParams param)
        {
            var query = _context.Categories
                .Include(c => c.Books);

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(c => c.Name)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(c => new CategoryRowVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookCount = c.Books.Count
                })
                .ToListAsync();

            return new PagedResult<CategoryRowVM>
            {
                Data = data,
                TotalCount = total
            };
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
