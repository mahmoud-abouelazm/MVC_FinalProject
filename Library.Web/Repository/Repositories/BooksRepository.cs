using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Core.ViewModel.BookVMs;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Library.Web.Repository.Repositories
{
    public class BooksRepository(ApplicationDbContext context) : Repository<Book>(context) , IBookRepository
    {
                private const int PageSize = 10;
        private readonly ApplicationDbContext _context = context;



        public BookDetailsVM GetBookDetails(int id)
        {
            var sourceBook = context.Books
                .Include(x => x.Copies)
                .Include(x => x.Category)
                .Include(x => x.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.Id == id);

            if (sourceBook == null)
                return null;

            return new BookDetailsVM()
            {
                Id = sourceBook.Id,
                Category = sourceBook.Category.Name,
                copiesNumber = sourceBook.Copies.Count(c=>c.AllowToRental),
                Description = sourceBook.Description,
                Img = sourceBook.Img,
                Price = sourceBook.Price,
                Title = sourceBook.Title,
                Authors = sourceBook.BookAuthors.Select(ba => ba.Author).ToList()
            };
        }

        public List<Copy> GetNBookCopies(int Id ,int n)
        {
            return context.Copies
                .Include(c => c.Book)
                .Where(c => c.BookId == Id && c.AllowToRental)
                .OrderBy(c => c.Id)
                .Take(n)
                .ToList();
        }




        public async Task<PagedResult<BookRowVM>> GetAllBooksAsync(PaginationParams param , int? categoryId= null, string? search = null)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(q) ||
                    (b.Description != null && b.Description.ToLower().Contains(q)) ||
                    b.Category.Name.ToLower().Contains(q) ||
                    b.BookAuthors.Any(ba => ba.Author.Name.ToLower().Contains(q)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }


            int total = await query.CountAsync();

            var data = await query
                .OrderBy(b => b.Title)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(b => new BookRowVM
                {
                    Id = b.Id,
                    Title = b.Title,
                    Img = b.Img,
                    Price = b.Price,
                    IsDeleted = b.IsDeleted,
                    CategoryName = b.Category.Name,
                    AuthorNames = string.Join(", ", b.BookAuthors.Select(a => a.Author.Name))
                })
                .ToListAsync();

            return new PagedResult<BookRowVM>
            {
                Data = data,
                TotalCount = total
            };
        }

        public async Task<Book?> GetWithAuthorsAsync(int id)
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

     

        public async Task<List<Book>> GetNewBooks()
        {
            return await _context.Books
                .Include(b=>b.Category)
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Take(4)
                .ToListAsync(); ;
        }



        public async Task<bool> IsTitleAvailableAsync(string title , int id)
        {
            return !await _context.Books.AnyAsync(b => b.Id != id && b.Title.ToLower().Trim() == title.ToLower().Trim());
        }

        public async Task<PagedResult<BookRowVM>> GetBooksByAuthorAsync(PaginationParams param, int authorId, string? search = null)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authorId))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(q) ||
                    (b.Description != null && b.Description.ToLower().Contains(q)) ||
                    b.Category.Name.ToLower().Contains(q));
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(b => b.Title)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(b => new BookRowVM
                {
                    Id = b.Id,
                    Title = b.Title,
                    Img = b.Img,
                    Price = b.Price,
                    IsDeleted = b.IsDeleted,
                    CategoryName = b.Category.Name,
                    AuthorNames = string.Join(", ", b.BookAuthors.Select(a => a.Author.Name))
                })
                .ToListAsync();

            return new PagedResult<BookRowVM>
            {
                Data = data,
                TotalCount = total
            };
        }





















    }
}
