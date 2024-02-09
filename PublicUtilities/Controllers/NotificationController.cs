using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;

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
            var userPlacesOfResidence = await _notificationRepository.GetUserPlacesOfResidencesByUserName(User.Identity.Name);
            var model = await _notificationRepository.GetUserNotificationByUserPlacesOfResidences(userPlacesOfResidence);
            return View(model);
        }
    }
}
