using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class IndicatorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
