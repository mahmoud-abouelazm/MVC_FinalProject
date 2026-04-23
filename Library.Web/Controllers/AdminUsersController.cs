using Library.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class AdminUsersController(IUserService userService) : Controller
    {
        private const int PageSize = 15;
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
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> DeactiveEmail(int userId)
        {
            var result = await _userService.DeactiveEmailAsync(userId);
            if (!result) return NotFound();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userService.GetUserDetails(userId);
            if (user is null) return NotFound();
            return View("UserDetails" ,user);
        }


        }

}
