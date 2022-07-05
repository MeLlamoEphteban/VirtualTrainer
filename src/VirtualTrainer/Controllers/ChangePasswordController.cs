using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VirtualTrainer.Authorization.Manage;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VirtualTrainer.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly SaliFitnessContext _context;

        public ChangePasswordController(SaliFitnessContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromServices]UserManager<IdentityUser> userManager, [FromServices]ILogger<ChangePasswordModel> logger)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            //user subscription data
            var userId = userManager.GetUserId(HttpContext.User);
            var userStart = await _context.Users.Where(u => u.UserAspNet == userId).ToArrayAsync();
            var userFinal = userStart[0].Iduser; //exact ID
            var userSubscriptionsData = await _context.UserSubscriptions.Where(u => u.Iduser == userFinal).ToArrayAsync();
            var subsID = userSubscriptionsData.Select(it => it.Idsubscription).ToArray();
            var subName = await _context.Subscriptions.Where(u => u.Idsubscription == subsID[0]).ToArrayAsync();

            //user actual password
            var hasPassword = await userManager.HasPasswordAsync(user);
            var cpModel = new ChangePasswordModel();
            cpModel._userManager = userManager;
            cpModel._logger = logger;
            cpModel.ExpirationDate = userSubscriptionsData[0].EndDate;
            cpModel.SubscriptionName = subName[0].SubName.ToString();

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
            //make the password change in the database
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
