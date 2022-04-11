using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VirtualTrainer.Authorization.Manage;

namespace VirtualTrainer.Controllers
{
    public class ChangePasswordController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices]UserManager<IdentityUser> userManager, [FromServices]ILogger<ChangePasswordModel> logger)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var hasPassword = await userManager.HasPasswordAsync(user);
            var cpModel = new ChangePasswordModel();
            cpModel._userManager = userManager;
            cpModel._logger = logger;

            return View(cpModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromServices]UserManager<IdentityUser> userManager, [FromServices]SignInManager<IdentityUser> signInManager, [FromServices]ILogger<ChangePasswordModel> logger, ChangePasswordModel changePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, changePasswordModel.Input.OldPassword, changePasswordModel.Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await signInManager.RefreshSignInAsync(user);
            logger.LogInformation("User changed their password successfully.");

            return View(changePasswordModel);
        }
    }
}
