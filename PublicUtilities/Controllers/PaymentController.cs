using Microsoft.AspNetCore.Mvc;

namespace PublicUtilities.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Water()
        {
            return View();
        }
        public IActionResult Gas()
        {
            return View();
        }
        public IActionResult Electricity()
        {
            return View();
        }
    }
}
