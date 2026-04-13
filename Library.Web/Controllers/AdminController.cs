using Library.Web.Core.Models;
using Library.Web.Core.ViewModel;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Library.Web.Controllers
{
    public class AdminController(ApplicationDbContext context , IWebHostEnvironment env) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _env = env;

        private const int PageSize = 10;


        
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author);

            int total = await query.CountAsync();

            var books = await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(b => new BookRowVM
                {
                    Id = b.Id,
                    Title = b.Title,
                    Img = b.Img,
                    Price = b.Price,
                    IsDeleted = b.IsDeleted,
                    CategoryName = b.Category.Name,
                    AuthorNames = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.Name))
                })
                .ToListAsync();

            return View(new BookVM
            {
                Books = books,
                TotalBooks = total,
                CurrentPage = page,
                PageSize = PageSize
            });
        }

        
        public async Task<IActionResult> Create()
        {
           return View("Form", await BuildFormVm(new BookFormVM()));
        }


        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormVM vm, IFormFile? CoverImage)
        {
            if (!ModelState.IsValid)
                return View("Form", await BuildFormVm(vm));

            var book = new Book
            {
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                Img = await SaveImage(CoverImage)
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            await _context.BookAuthors.AddRangeAsync(vm.AuthorIds.Select(aid =>
                new BookAuthor { BookId = book.Id, AuthorId = aid }));
            await _context.SaveChangesAsync();

            TempData["Success"] = $"'{book.Title}' added to the archive.";
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

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

            return View("Form", await BuildFormVm(vm));
        }

       
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormVM vm, IFormFile? CoverImage)
        {
            if (!ModelState.IsValid)
                return View("Form", await BuildFormVm(vm));

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(b => b.Id == vm.Id);

            if (book == null) return NotFound();

            book.Title = vm.Title;
            book.Description = vm.Description;
            book.Price = vm.Price;
            book.CategoryId = vm.CategoryId;

            var newImg = await SaveImage(CoverImage);
            if (newImg != null) book.Img = newImg;

            _context.BookAuthors.RemoveRange(book.BookAuthors);
            await _context.BookAuthors.AddRangeAsync(vm.AuthorIds.Select(aid =>
                new BookAuthor { BookId = book.Id, AuthorId = aid }));

            await _context.SaveChangesAsync();

            TempData["Success"] = $"'{book.Title}' updated.";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.IsDeleted = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"'{book.Title}' removed from the active catalog.";
            return RedirectToAction(nameof(Index));
        }

       
        private async Task<BookFormVM> BuildFormVm(BookFormVM vm)
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

        private async Task<string?> SaveImage(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var dir = Path.Combine(env.WebRootPath, "uploads", "covers");
            Directory.CreateDirectory(dir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var stream = new FileStream(Path.Combine(dir, fileName), FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/covers/" + fileName;
        }

    }
}
