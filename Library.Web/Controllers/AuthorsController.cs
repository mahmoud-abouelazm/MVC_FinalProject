using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Author;
using Library.Web.Data;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController (IAuthorService authorService) : Controller
    {
        private readonly IAuthorService _authorService = authorService;

        
        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _authorService.GetAllAsync(new PaginationParams
            {
                Page = page,
                PageSize = 10
            } ,"");

            return View(new AuthorVM
            {
                Authors = result.Data,
                TotalAuthors = result.TotalCount,
                CurrentPage = page,
                PageSize = 10
            });
        }

        public IActionResult Create()
        {
            return View("Form", new AuthorFormVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorFormVM vm)
        {
            if (!ModelState.IsValid)
                return View("Form", vm);

            await _authorService.CreateAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _authorService.GetVmForEditAsync(id);

            if (vm == null)
                return NotFound();

            return View("Form", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorFormVM vm)
        {
            if (!ModelState.IsValid)
                return View("Form", vm);

            await _authorService.UpdateAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> IsNameAvailable(int id, string name)
        {
            var isAvailable = await _authorService.IsNameAvailableAsync(id, name);
            return Json(isAvailable);

        }

    }
}
