using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class GarbageRemovalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
