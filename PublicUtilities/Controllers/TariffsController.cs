using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class TariffsController : Controller
    {
        private readonly ITariffsRepository _tariffsRepository;

        public TariffsController(ITariffsRepository tariffsRepository)
        {
            _tariffsRepository = tariffsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new TariffsViewModel
            {
                WaterPrice = await _tariffsRepository.GetTariffPriceAsync("Вода"),
                GasPrice = await _tariffsRepository.GetTariffPriceAsync("Газ"),
                ElectricityPrice = await _tariffsRepository.GetTariffPriceAsync("Електроенергія")
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var model = new TariffsViewModel
            {
                WaterPrice = await _tariffsRepository.GetTariffPriceAsync("Вода"),
                GasPrice = await _tariffsRepository.GetTariffPriceAsync("Газ"),
                ElectricityPrice = await _tariffsRepository.GetTariffPriceAsync("Електроенергія")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TariffsViewModel model)
        {
            var oldPrice = await _tariffsRepository.GetTariffPriceAsync("Вода");

            if(model.WaterPrice != oldPrice) 
            {
                if (!_tariffsRepository.Add(model.WaterPrice, "Вода"))
                {
                    ModelState.AddModelError("", "Щось пішло не так при збереженні змін");
                    return View(model);
                }
            }

            oldPrice = await _tariffsRepository.GetTariffPriceAsync("Газ");

            if (model.GasPrice != oldPrice)
            {
                if (!_tariffsRepository.Add(model.GasPrice, "Газ"))
                {
                    ModelState.AddModelError("", "Щось пішло не так при збереженні змін");
                    return View(model);
                }
            }

            oldPrice = await _tariffsRepository.GetTariffPriceAsync("Електроенергія");

            if (model.ElectricityPrice != oldPrice)
            {
                if (!_tariffsRepository.Add(model.ElectricityPrice, "Електроенергія"))
                {
                    ModelState.AddModelError("", "Щось пішло не так при збереженні змін");
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
