using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface INotificationRepository
    {
        Task<ICollection<PlacesOfResidence>> GetUserPlacesOfResidencesByUserName(string userName);
        Task<ICollection<NotificationViewModel>> GetUserNotificationByUserPlacesOfResidences(ICollection<PlacesOfResidence> placesOfResidences);
    }
}
