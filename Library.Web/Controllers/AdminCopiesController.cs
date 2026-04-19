using Library.Web.Core.ViewModel.Copies;
using Library.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class AdminCopiesController(ICopyService copyService) : Controller
    {
        private const int PageSize = 15;

        
        public async Task<IActionResult> Index(int? bookId, int page = 1)
        {
            var vm = bookId.HasValue
                ? await copyService.GetIndexVmByBookAsync(bookId.Value, page, PageSize)
                : await copyService.GetIndexVmAsync(page, PageSize);

            return View(vm);
        }

        public async Task<IActionResult> Create(int? bookId)
            => View("Form", await copyService.GetCreateVmAsync(bookId));


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CopyFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Books = (await copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            var result = await copyService.CreateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                vm.Books = (await copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index), new { bookId = vm.BookId });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var vm = await copyService.GetEditVmAsync(id);
            if (vm is null) return NotFound();
            return View("Form", vm);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CopyFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Books = (await copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            var result = await copyService.UpdateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                vm.Books = (await copyService.GetCreateVmAsync(vm.BookId)).Books;
                return View("Form", vm);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index), new { bookId = vm.BookId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? bookId)
        {
            var result = await copyService.DeleteAsync(id);

            if (result.Success) TempData["Success"] = result.Message;
            else TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index), new { bookId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRental(int id, int? bookId)
        {
            var result = await copyService.ToggleRentalAsync(id);

            if (result.Success) TempData["Success"] = result.Message;
            else TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index), new { bookId });
        }
    }
}
