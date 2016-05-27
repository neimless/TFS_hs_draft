using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HSDraft_v2.Models;
using Microsoft.AspNet.Identity;

namespace HSDraft_v2.API
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            var signInStatus = await signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);
            if (signInStatus == SignInResult.Success)
            {
                return Redirect("/home");
            }
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }


        public IActionResult SignOut()
        {
            signInManager.SignOut();
            return Redirect("/home");
        }
    }
}
