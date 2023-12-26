using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class StatementsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
