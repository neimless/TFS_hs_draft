using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;

namespace HSDraft_v2.API
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = (ClaimsIdentity)User.Identity;
            ViewBag.Name = user.Name;
            ViewBag.Admin = user.FindFirst("Admin") != null ? "true" : "false";
            return View();
        }
    }
}
