using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;

namespace PublicUtilities.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _newsRepository.GetAllNews(page);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _newsRepository.GetNewsById(id);
            return View(model);
        }
    }
}
