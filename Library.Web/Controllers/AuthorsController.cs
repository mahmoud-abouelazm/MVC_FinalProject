using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Author;
using Library.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    public class AuthorsController (ApplicationDbContext context): Controller
    {
        private const int PageSize = 10;
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index(int page = 1)
        {
            int total = await _context.Authors.CountAsync();

            var authors = await _context.Authors
                .OrderBy(a => a.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(a => new AuthorRowVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio,
                    BookCount = a.BookAuthors.Count
                })
                .ToListAsync();

            return View(new AuthorVM
            {
                Authors = authors,
                TotalAuthors = total,
                CurrentPage = page,
                PageSize = PageSize
            });
        }

        public IActionResult Create()
        {
          return  View("Form", new AuthorFormVM());
        }
            

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorFormVM vm)
        {
            if (!ModelState.IsValid)
                return View("Form", vm);

            await _context.Authors.AddAsync(new Author { Name = vm.Name, Bio = vm.Bio });
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Author '{vm.Name}' registered.";
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            return View("Form", new AuthorFormVM
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio
            });
        }



        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorFormVM vm)
        {
            if (!ModelState.IsValid)
                return View("Form", vm);

            var author = await _context.Authors.FindAsync(vm.Id);
            if (author == null) return NotFound();

            author.Name = vm.Name;
            author.Bio = vm.Bio;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Author '{author.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Author '{author.Name}' removed.";
            return RedirectToAction(nameof(Index));
        }
    }
}
