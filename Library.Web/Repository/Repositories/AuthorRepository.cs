using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Author;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Library.Web.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Repository.Repositories;
    public class AuthorRepository(ApplicationDbContext context) : Repository<Author>(context), IAuthorRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task DeleteAsync(Author? author)
    {
        _context.Authors.Remove(author!);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public async Task<PagedResult<AuthorRowVM>> GetAllAsync(PaginationParams param)
    {
        var query = _context.Authors
            .Include(a => a.BookAuthors);

        int total = await query.CountAsync();

        var data = await query
            .OrderBy(a => a.Name)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(a => new AuthorRowVM
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                BookCount = a.BookAuthors.Count
            })
            .ToListAsync();

        return new PagedResult<AuthorRowVM>
        {
            Data = data,
            TotalCount = total
        };
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }
}