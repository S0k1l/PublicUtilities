using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;

namespace PublicUtilities.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IStatisticRepository _statisticRepository;

        public StatisticController(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Water()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }

            var model = await _statisticRepository.GetUserStatisticByUtilitiesType(User.Identity.Name, "Вода");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Gas()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }

            var model = await _statisticRepository.GetUserStatisticByUtilitiesType(User.Identity.Name, "Газ");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Electricity()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }

            var model = await _statisticRepository.GetUserStatisticByUtilitiesType(User.Identity.Name, "Електроенергія");
            return View(model);
        }
    }
}
