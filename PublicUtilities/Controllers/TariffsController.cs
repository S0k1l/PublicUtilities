using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class TariffsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
