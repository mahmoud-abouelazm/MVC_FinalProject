using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminRentalsController(IRentalService rentalService) : Controller
    {
        private const int PageSize = 10;
        private readonly IRentalService _rentalService = rentalService;

        public async Task<IActionResult> Index(string search = "", string statusFilter = "", int page = 1)
        {
            var vm = await _rentalService.GetIndexVmAsync(page, PageSize, search, statusFilter);
            return View(vm);
        }

        public async Task<IActionResult> ConfirmReturn(int id)
        {
            var vm = await _rentalService.GetReturnConfirmAsync(id);
            if (vm is null) return NotFound();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessReturn(int rentalId,string search = "", string statusFilter = "")
        {
            var result = await _rentalService.ProcessReturnAsync(rentalId);

            if (result.Success) TempData["Success"] = result.Message;
            else TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index),
                new { search, statusFilter });
        }
    }
}