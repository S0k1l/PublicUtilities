using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
    }
}
