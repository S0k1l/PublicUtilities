using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface INotificationRepository
    {
        Task<ICollection<PlacesOfResidence>> GetUserPlacesOfResidencesByUserName(string userName);
        Task<ICollection<NotificationViewModel>> GetStreetNotifications(ICollection<PlacesOfResidence> placesOfResidences);
        Task<ICollection<NotificationViewModel>> GetHouseNotifications(ICollection<PlacesOfResidence> placesOfResidences);
        Task<ICollection<NotificationViewModel>> GetDirectNotifications(ICollection<PlacesOfResidence> placesOfResidences);
        Task<ICollection<NotificationViewModel>> GetGlobalNotification();
    }
}
