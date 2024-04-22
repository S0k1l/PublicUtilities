using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IPhotoService _photoService;
        private const string DEFAULT_IMAGE = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg";

        public NewsController(INewsRepository newsRepository, IPhotoService photoService)
        {
            _newsRepository = newsRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _newsRepository.GetAllNews(page);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _newsRepository.GetNewsById(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var newNews = new News
            {
                Header = model.Header,
                Text = model.Text,
                Date = DateTime.Now,
            };

            if (model.Image != null)
            {
                var imageUrl = await _photoService.AddPhotoAsync(model.Image);
                newNews.ImageUrl = imageUrl.Url.ToString();
            }
            else
            {
                newNews.ImageUrl = DEFAULT_IMAGE;
            }

            if (_newsRepository.Add(newNews)) { return RedirectToAction("Details", new { id = newNews.Id }); }

            ModelState.AddModelError("", "Щось пішло не так при збережені");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsRepository.GetNewsById(id);
            var model = new EditNewsViewModel
            {
                Id = news.Id,
                Header = news.Header,
                Text = news.Text,
                Date = DateTime.Now,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNewsViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var news = await _newsRepository.GetNewsById(model.Id);

            news.Text = model.Text;
            news.Header = model.Header;

            if (model.Image != null)
            {
                if(news.ImageUrl != DEFAULT_IMAGE)
                {
                    var result = await _photoService.DeletePhotoAsync(news.ImageUrl);

                    if (result.Result != "ok") { return BadRequest(new { error = "Щось пішло не так при видаленні фото" }); }
                }

                var imageUrl = await _photoService.AddPhotoAsync(model.Image);
                news.ImageUrl = imageUrl.Url.ToString();
            }

            if (_newsRepository.Update(news)) { return RedirectToAction("Details", new { id = news.Id }); }

            ModelState.AddModelError("", "Щось пішло не так при збережені");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsRepository.GetNewsById(id);

            var result = await _photoService.DeletePhotoAsync(news.ImageUrl);

            if (result.Result != "ok") { return BadRequest(new { error = "Щось пішло не так при видаленні фото" }); }

            if (_newsRepository.Delete(news)) { return RedirectToAction("Index"); }
            
            return BadRequest(ModelState);
        }
    }
}
