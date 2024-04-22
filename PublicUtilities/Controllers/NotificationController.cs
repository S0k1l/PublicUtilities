using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;
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

        //TODO: Rework way to get notifications  
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new List<NotificationViewModel>();

            var userPlacesOfResidence = await _notificationRepository.GetUserPlacesOfResidencesByUserName(User.Identity.Name);

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
    }
}
