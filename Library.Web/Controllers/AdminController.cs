using Library.Web.Core.Constants;
using Library.Web.Core.Models;
using Library.Web.Core.ViewModel;
using Library.Web.Core.ViewModel.Book;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController(IAdminService adminService) : Controller
    {
        private readonly IAdminService _adminService = adminService;

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _adminService.GetAllAsync(new PaginationParams
            {
                Page = page,
                PageSize = 10
            });

            return View(new BookVM
            {
                Books = result.Data,
                TotalBooks = result.TotalCount,
                CurrentPage = page,
                PageSize = 10
            });
        }

        public async Task<IActionResult> Create()
        {
            return View("Form", await _adminService.BuildFormAsync(new BookFormVM()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookFormVM vm, IFormFile? CoverImage)
        {
            if (!ModelState.IsValid)
                return View("Form", await _adminService.BuildFormAsync(vm));

            await _adminService.CreateAsync(vm, CoverImage);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _adminService.GetForEditAsync(id);
            return View("Form", await _adminService.BuildFormAsync(vm));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookFormVM vm, IFormFile? CoverImage)
        {
            if (!ModelState.IsValid)
                return View("Form", await _adminService.BuildFormAsync(vm));

            await _adminService.UpdateAsync(vm, CoverImage);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _adminService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> IsTitleAvailable(string title , int id)
        {
            bool isAvailable = await _adminService.IsTitleAvailableAsync(title , id);
            return isAvailable
               ? Json(true)
               : Json("This title is already registered");
        }
    }
}