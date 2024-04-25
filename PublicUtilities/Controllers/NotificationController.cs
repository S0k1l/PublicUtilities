using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
  
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(UserRoles.Admin))
            {
                var adminModel = await _notificationRepository.GetAllNotifications();
                return View(adminModel.OrderByDescending(m => m.Date).ToList());
            }

            var model = new List<Notifications>();

            var userPlacesOfResidence = await _notificationRepository.GetUserPlacesOfResidences(User.Identity.Name);

            var streetNotifications = await _notificationRepository.GetStreetNotifications(userPlacesOfResidence);
            model.AddRange(streetNotifications);

            var houeseNotifications = await _notificationRepository.GetHouseNotifications(userPlacesOfResidence);
            model.AddRange(houeseNotifications);

            var directNotifications = await _notificationRepository.GetDirectNotifications(userPlacesOfResidence);
            model.AddRange(directNotifications);

            var globalNotifications = await _notificationRepository.GetGlobalNotification();
            model.AddRange(globalNotifications);

            return View(model.OrderByDescending(m => m.Date).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int placesOfResidenceId, string header, string text)
        {
            if (placesOfResidenceId == 0) { return View(); }
            
            var por = await _notificationRepository.GetUserPlaceOfResidences(placesOfResidenceId);

            var model = new CreateNotificationViewModel
            {
                Apartment = por.Apartment,
                Building = por.House,
                Street = por.Streets.Name,
                Header = header,
                Text = text,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateNotificationViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var notification = new Notifications
            {
                Apartment = model.Apartment,
                House = model.Building,
                Streets = model.Street,
                Header = model.Header,
                Text = model.Text,
                Date = DateTime.Now,
            };

            if (_notificationRepository.Add(notification)) { return RedirectToAction("Edit", new { id = notification.Id }); }

            ModelState.AddModelError("", "Щось пішло не так при збережені");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var notifiction = await _notificationRepository.GetNotification(id);
            var model = new EditNotificationsViewModel
            {
                Id = id,
                Street = notifiction.Streets,
                Building = notifiction.House,
                Apartment = notifiction.Apartment,
                Header = notifiction.Header,
                Text = notifiction.Text,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNotificationsViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var notification = await _notificationRepository.GetNotification(model.Id);

            notification.Streets = model.Street;
            notification.House = model.Building;
            notification.Apartment = model.Apartment;
            notification.Header = model.Header;
            notification.Text = model.Text;

            if (_notificationRepository.Update(notification)) { return RedirectToAction("Edit", new { id = notification.Id }); }

            ModelState.AddModelError("", "Щось пішло не так при збережені");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var notification = await _notificationRepository.GetNotification(id);

            if (_notificationRepository.Remove(notification)) { return RedirectToAction("Index"); }

            ModelState.AddModelError("", "Щось пішло не так при збережені");
            return RedirectToAction("Edit", new { id = notification.Id });
        }
    }
}
