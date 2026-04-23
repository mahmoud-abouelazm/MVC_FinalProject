using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Category;
using Library.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController(ApplicationDbContext context) : Controller
    {
        private const int PageSize = 10;
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index(int page = 1)
        {
            int total = await _context.Categories.CountAsync();

            var cats = await _context.Categories
                .OrderBy(c => c.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new CategoryRowVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookCount = c.Books.Count
                })
                .ToListAsync();

            return View(new CategoryVM
            {
                Categories = cats,
                TotalCategories = total,
                CurrentPage = page,
                PageSize = PageSize
            });
        }

       public IActionResult Create()
        {
            return View("Form", new CategoryFormVM());
        }
            

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormVM vm)
        {
            if (!ModelState.IsValid)
                return View("Form", vm);

            _context.Categories.Add(new Category { Name = vm.Name });
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Category '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            return View("Form", new CategoryFormVM { Id = cat.Id, Name = cat.Name });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryFormVM vm)
        {
            if (!ModelState.IsValid)
                return View("Form", vm);

            var cat = await _context.Categories.FindAsync(vm.Id);
            if (cat == null) return NotFound();

            cat.Name = vm.Name;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Category renamed to '{cat.Name}'.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Category '{cat.Name}' deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
