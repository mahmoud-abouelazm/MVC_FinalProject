using Library.Web.Core.Models;
using Library.Web.Core.ViewModel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await BuildViewModelAsync();
            if (vm is null) return Challenge();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(AdminProfileVM profile)
        {
            if (!ModelState.IsValid)
            {
                var invalidVm = await BuildViewModelAsync(profile: profile);
                if (invalidVm is null) return Challenge();
                return View("Index", invalidVm);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            user.FullName = profile.FullName.Trim();
            user.Email = profile.Email.Trim();
            user.PhoneNumber = string.IsNullOrWhiteSpace(profile.PhoneNumber) ? null : profile.PhoneNumber.Trim();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                var vmWithErrors = await BuildViewModelAsync(profile: profile);
                if (vmWithErrors is null) return Challenge();
                return View("Index", vmWithErrors);
            }

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AdminPasswordVM password)
        {
            if (!ModelState.IsValid)
            {
                var invalidVm = await BuildViewModelAsync(password: password);
                if (invalidVm is null) return Challenge();
                return View("Index", invalidVm);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            var result = await _userManager.ChangePasswordAsync(user, password.CurrentPassword, password.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                var vmWithErrors = await BuildViewModelAsync();
                if (vmWithErrors is null) return Challenge();
                return View("Index", vmWithErrors);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Success"] = "Password changed successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<AdminSettingsVM?> BuildViewModelAsync(AdminProfileVM? profile = null, AdminPasswordVM? password = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return null;

            return new AdminSettingsVM
            {
                Profile = profile ?? new AdminProfileVM
                {
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber
                },
                Password = password ?? new AdminPasswordVM()
            };
        }
    }
}
