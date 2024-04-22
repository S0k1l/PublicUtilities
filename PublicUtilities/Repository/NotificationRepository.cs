using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PlacesOfResidence>> GetUserPlacesOfResidencesByUserName(string userName)
        {
            return await _context.UsersPlacesOfResidence
                .Include(upor => upor.PlacesOfResidence.Streets)
                .Where(upor => upor.AppUser.UserName == userName)
                .Select(upor => upor.PlacesOfResidence)
                .ToListAsync();
        }

        public async Task<ICollection<Notifications>> GetGlobalNotification()
        {
            var model = await _context.Notifications
                .Where(n => n.Streets == null)
                .ToListAsync();

            return model;
        }

        public async Task<ICollection<Notifications>> GetStreetNotifications(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<Notifications>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.Streets == item.Streets.Name && n.House == null && n.Apartment == null)
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }

        public async Task<ICollection<Notifications>> GetHouseNotifications(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<Notifications>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.Streets == item.Streets.Name && n.House == item.House && n.Apartment == null)
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }

        public async Task<ICollection<Notifications>> GetDirectNotifications(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<Notifications>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.Streets == item.Streets.Name && n.House == item.House && n.Apartment == item.Apartment)
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }

        public bool Add(Notifications notifications)
        {
            _context.Add(notifications);
            return Save();
        }

        public bool Remove(Notifications notifications)
        {
            _context.Remove(notifications);
            return Save();
        }

        public bool Update(Notifications notifications)
        {
            _context.Update(notifications);
            return Save();
        }

        public bool Save()
        {
            var saved =_context.SaveChanges();
            return saved > 0 ? true: false;
        }

        public async Task<Notifications> GetNotification(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<ICollection<Notifications>> GetAllNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }
    }
}
