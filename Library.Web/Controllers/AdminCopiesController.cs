using Library.Web.Core.ViewModel.Copies;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCopiesController(ICopyService copyService) : Controller
    {
        private const int PageSize = 15;
        private readonly ICopyService _copyService = copyService;

        public async Task<IActionResult> Index(int? bookId, string search = "", int page = 1)
        {
            var vm = bookId.HasValue
                ? await _copyService.GetIndexVmByBookAsync(bookId.Value, page, PageSize, search)
                : await _copyService.GetIndexVmAsync(page, PageSize, search);
            return View(vm);
        }

        public async Task<IActionResult> Create(int? bookId)
            => View("Form", await _copyService.GetCreateVmAsync(bookId));


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CopyFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Books = (await _copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            var result = await _copyService.CreateAsync(vm);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                vm.Books = (await _copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index), new { bookId = vm.BookId });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _copyService.GetEditVmAsync(id);
            if (vm is null) return NotFound();
            return View("Form", vm);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CopyFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Books = (await _copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            var result = await _copyService.UpdateAsync(vm);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                vm.Books = (await _copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index), new { bookId = vm.BookId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? bookId)
        {
            var result = await _copyService.DeleteAsync(id);

            if (result.Success) TempData["Success"] = result.Message;
            else TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index), new { bookId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRental(int id, int? bookId)
        {
            var result = await _copyService.ToggleRentalAsync(id);

            if (result.Success) TempData["Success"] = result.Message;
            else TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index), new { bookId });
        }

        public async Task<IActionResult> IsNameAvaliableWithThisBook(int bookId, string name, int? copyId)
        {
            {
                var isAvailable = await _copyService.IsNameAvaliableWithThisBookAsync(bookId, name, copyId);
                return isAvailable ?
                    Json(isAvailable) :
                    Json($"A copy with the name '{name}' already exists for this book.")
                    ;
            }
        }
    }
}
