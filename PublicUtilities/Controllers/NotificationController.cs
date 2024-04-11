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

            var notification = await _notificationRepository.GetUserNotificationByUserPlacesOfResidences(userPlacesOfResidence);
            model.AddRange(notification);
            
            var globalNotification = await _notificationRepository.GetGlobalNotification();
            model.AddRange(globalNotification);

            return View(model.OrderByDescending(m => m.Date).ToList());
        }
    }
}
