using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Library.Web.Services.HelperServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class AdminService(IBookRepository repo,
                       ApplicationDbContext context,
                       IImageService imageService) : IAdminService
    {
        private readonly IBookRepository _repo = repo;
        private readonly ApplicationDbContext _context = context;
        private readonly IImageService _imageService = imageService;

        public async Task<PagedResult<BookRowVM>> GetAllAsync(PaginationParams param)
        {
            return await _repo.GetAllBooksAsync(param, null, null);
        }

        public async Task CreateAsync(BookFormVM vm, IFormFile? file)
        {
            var img = await _imageService.SaveAsync(file);

            var book = new Book
            {
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                Img = img
            };

            object value = await _repo.AddAsync(book);

            var authors = vm.AuthorIds.Select(aid => new BookAuthor
            {
                BookId = book.Id,
                AuthorId = aid
            });

            await _context.BookAuthors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();
        }

        public async Task<BookFormVM> GetForEditAsync(int id)
        {
            var book = await _repo.GetWithAuthorsAsync(id);
            if (book == null) throw new Exception("Book not found");

            var vm = new BookFormVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Img = book.Img,
                Price = book.Price,
                CategoryId = book.CategoryId,
                AuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList()
            };

            return vm;
        }

        public async Task UpdateAsync(BookFormVM vm, IFormFile? file)
        {
            var book = await _repo.GetWithAuthorsAsync(vm.Id);

            if (book == null)
                throw new Exception("Book not found");

            book.Title = vm.Title;
            book.Description = vm.Description;
            book.Price = vm.Price;
            book.CategoryId = vm.CategoryId;

            var newImg = await _imageService.SaveAsync(file);
            if (newImg != null)
                book.Img = newImg;

            _context.BookAuthors.RemoveRange(book.BookAuthors);

            var authors = vm.AuthorIds.Select(aid => new BookAuthor
            {
                BookId = book.Id,
                AuthorId = aid
            });

            await _context.BookAuthors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);

            if (book == null)
                throw new Exception("Not found");

            book.IsDeleted = true;

            await _repo.UpdateAsync(book);
        }

        public async Task<BookFormVM> BuildFormAsync(BookFormVM vm)
        {
            vm.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync();

            vm.Authors = await _context.Authors
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
                .ToListAsync();

            return vm;
        }

        public async Task<bool> IsTitleAvailableAsync(string title , int id)
        {
            return await _repo.IsTitleAvailableAsync(title , id);
        }
    }
}
