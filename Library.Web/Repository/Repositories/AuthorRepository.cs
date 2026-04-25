using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Author;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Core.ViewModel.BookVMs;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Library.Web.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

    public async Task<PagedResult<AuthorRowVM>> GetAllAsync(PaginationParams param , string? sortBy="")
    {
        var query = _context.Authors
            .Include(a => a.BookAuthors)
                     .ThenInclude(ba => ba.Book)
        .AsQueryable();

        int total = await query.CountAsync();

        query = sortBy switch
        {
            "name_desc" => query.OrderByDescending(a => a.Name),
            "books" => query.OrderByDescending(a => a.BookAuthors.Count),
            "name_asc" => query.OrderBy(a => a.Name),
            _ => query.OrderBy(a => a.Name)
        };


        

        var data = await query
            
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(a => new AuthorRowVM
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                BookCount = a.BookAuthors.Count,


                BooksOfTheAuthor = a.BookAuthors
                    .Select(ba => new BookRowVM
                    {
                        Id = ba.Book.Id,
                        Title = ba.Book.Title,
                        Img = ba.Book.Img,
                        Price = ba.Book.Price,
                        IsDeleted = ba.Book.IsDeleted,
                        CategoryName = ba.Book.Category.Name,
                        AuthorNames = null 
                    })
                    .ToList()

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

    public async Task<bool> IsNameAvailableAsync(int id, string name)
    {
        return !await _context.Authors.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim() && a.Id != id);
    }
}