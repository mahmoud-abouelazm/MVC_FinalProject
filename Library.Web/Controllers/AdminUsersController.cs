using Library.Web.Core.ViewModel.Users;
using Library.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController(IUserService userService) : Controller
    {
        private const int PageSize = 5;
        private readonly IUserService _userService = userService;

        public async Task<IActionResult> Index(string search = "", int page = 1)
        {
            var model = await _userService.GetIndexVmAsync(page, PageSize, search);
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var result = await _userService.ConfirmEmailAsync(userId);
            if (!result) return NotFound();
            TempData["Success"] = "Activated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeactiveEmail(int userId)
        {
            var result = await _userService.DeactiveEmailAsync(userId);
            if (!result) return NotFound();

            TempData["Success"] = "Deactivated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userService.GetUserDetails(userId);
            if (user is null) return NotFound();
            return View("UserDetails", user);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewUser()
        {
            var vm = await _userService.GetCreateUserVMFormAsync();
            return View("CreateUserForm", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewUser(CreateUserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                userVM = await _userService.GetCreateUserVMFormAsync();
                return View("CreateUserForm", userVM);
            }

            var result = await _userService.AddNewUserAsync(userVM);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                userVM = await _userService.GetCreateUserVMFormAsync();
                return View("CreateUserForm", userVM);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditRoleOfUser(int userId)
        {
            var user = await _userService.GetUserDetails(userId);
            if (user is null) return NotFound();

           

            var vm = await _userService.GetEditUserRoleVMAsync(userId);
            if (vm is null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoleOfUser(EditUserRoleVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await _userService.GetEditUserRoleVMAsync(vm.UserId);
                return View(vm);
            }

            var result = await _userService.ChangeUserRoleAsync(vm.UserId, vm.NewRole);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(EditRoleOfUser), new { userId = vm.UserId });
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var existing = await _userService.CheckIfEmailExistsAsync(email);
            return existing is false
                ? Json(true)
                : Json("This email is already registered");
        }
    }
}
