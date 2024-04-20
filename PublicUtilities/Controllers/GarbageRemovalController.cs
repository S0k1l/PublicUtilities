using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;
using PublicUtilities.Models;

namespace PublicUtilities.Controllers
{
    public class GarbageRemovalController : Controller
    {
        private readonly IGarbageRemovalRepository _garbageRemovalRepository;

        public GarbageRemovalController(IGarbageRemovalRepository garbageRemovalRepository)
        {
            _garbageRemovalRepository = garbageRemovalRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _garbageRemovalRepository.GetGarbageRemovalAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var model = await _garbageRemovalRepository.GetGarbageRemovalAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarbageRemoval model)
        {
            var oldModel = await _garbageRemovalRepository.GetGarbageRemovalAsync();
            oldModel.Text = model.Text;

            if(!_garbageRemovalRepository.Update(model)) 
            {
                ModelState.AddModelError("", "Щось пішло не так при збереженні змін");
                return View(model);
            }

            return View(model);
        }
    }
}
