using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
