using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PublicUtilities.Interface;
using PublicUtilities.Models;

namespace PublicUtilities.Controllers
{
    public class IndicatorsController : Controller
    {
        private readonly IIndicatorsRepository _indicatorsRepository;

        public IndicatorsController(IIndicatorsRepository indicatorsRepository)
        {
            _indicatorsRepository = indicatorsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string alert = null)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }

            var model =await _indicatorsRepository.GetIndeicatorsByUserName(User.Identity.Name);

            if (!alert.IsNullOrEmpty())
            {
                ViewBag.AlertMessage = alert;
                alert = null;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int placesOfResidenceId, int utilitiesId, string indicator, string consumed)
        {
            var utilitiesPrice = await _indicatorsRepository.GetUtilitiesPriceById(utilitiesId);
            var price = utilitiesPrice * int.Parse(indicator);

            var newIndicartor = new Indicators
            {
                PlacesOfResidenceId = placesOfResidenceId,
                UtilitiesId = utilitiesId,
                Indicator = indicator,
                Date = DateTime.Now,
                Price = price,
                Paid = false,
                Consumed = consumed,
            };

            if (_indicatorsRepository.AddIndicator(newIndicartor))
            {
                return RedirectToAction("Index", new { alert = "Your alert message here" });
            }

            return RedirectToAction("Index", new { alert = "Показники успішно поданні!" });
        }
    }
}
