using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface INotificationRepository
    {
        Task<ICollection<PlacesOfResidence>> GetUserPlacesOfResidencesByUserName(string userName);
        Task<ICollection<Notifications>> GetStreetNotifications(ICollection<PlacesOfResidence> placesOfResidences);
        Task<ICollection<Notifications>> GetHouseNotifications(ICollection<PlacesOfResidence> placesOfResidences);
        Task<ICollection<Notifications>> GetDirectNotifications(ICollection<PlacesOfResidence> placesOfResidences);
        Task<ICollection<Notifications>> GetGlobalNotification();
        Task<ICollection<Notifications>> GetAllNotifications();
        Task<Notifications> GetNotification(int id);
        bool Add(Notifications notifications);
        bool Remove(Notifications notifications);
        bool Update(Notifications notifications);
        bool Save();
        
    }
}
