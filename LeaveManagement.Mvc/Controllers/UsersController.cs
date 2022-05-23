using LeaveManagement.Mvc.Contracts;
using LeaveManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                returnUrl ??= Url.Content("~/");
                var isLoggedIn = await _authenticationService.Login(login.Email, login.Password);
                if (isLoggedIn)
                    return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError("", "Log in Attempt Failed. Please try again.");
            return View(login);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM registration, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                returnUrl ??= Url.Content("~/");
                var isCreated = await _authenticationService.Register(registration);
                if (isCreated)
                    return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
            return View(registration);
        }

        [HttpPost]
        public async Task<ActionResult> Logout(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            await _authenticationService.Logout();
            return LocalRedirect(returnUrl);
        }
    }
}
