using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VirtualTrainer.Account;

namespace VirtualTrainer.Controllers
{
    [AllowAnonymous]
    public class Identity : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login([FromServices]SignInManager<IdentityUser> signInManager, [FromServices]ILogger<LoginModel> logger, string returnUrl = null)
        {
            //return Content("Asta e pagina de login");
            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ModelState.AddModelError(string.Empty, ErrorMessage);
            //}

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var loginModel = new LoginModel();
            loginModel._signInManager= signInManager;
            loginModel._logger= logger;
            loginModel.ReturnUrl = returnUrl;
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromServices] SignInManager<IdentityUser> signInManager, [FromServices] ILogger<LoginModel> logger, LoginModel loginModel)
        {
            loginModel._signInManager = signInManager;
            loginModel._logger = logger;
            //return Content("Am primit" + loginModel.Input.Email);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            loginModel.ReturnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(loginModel.Input.Email, loginModel.Input.Password, loginModel.Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    return LocalRedirect(loginModel.ReturnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");
                    return RedirectToAction("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginModel);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(loginModel);
        }
    }
}
