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
        public async Task<IActionResult> Index(int placesOfResidenceId, string utilitiesName, string indicator, string consumed)
        {
            var utilities = await _indicatorsRepository.GetUtilities(utilitiesName);
            var price = utilities.Price * int.Parse(consumed);

            var newIndicartor = new Indicators
            {
                PlacesOfResidenceId = placesOfResidenceId,
                UtilitiesId = utilities.Id,
                Utilities = utilities,
                Indicator = indicator,
                Date = DateTime.Now,
                Price = price,
                isPaid = false,
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
