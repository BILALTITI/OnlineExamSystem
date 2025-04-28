using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.ViewModel;

namespace OnlineExamSystem.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AccountController : Controller // Removed class-level [Authorize]
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

     
        public IActionResult Login()
        {

            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
           
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index2", "AdminBase", new { area = "Admins" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Exam", new { area = "" });
                    }
                }

                TempData["Error"] = "Invalid login attempt.";
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            }
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" }); // Redirect to public area
        }

        // Admin-only actions below
        [Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user == null ? NotFound() : View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded
                ? RedirectToAction("Users")
                : View("Error");
        }
    }
}