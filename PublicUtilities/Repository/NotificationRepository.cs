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

        public async Task<ICollection<NotificationViewModel>> GetGlobalNotification()
        {
            var model = await _context.Notifications
                .Where(n => n.Streets == null)
                .Select(n => new NotificationViewModel
                {
                    Text = n.Text,
                    Header = n.Header,
                    Date = n.Date.ToShortDateString(),
                })
                .ToListAsync();

            return model;
        }

        public async Task<ICollection<NotificationViewModel>> GetStreetNotifications(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<NotificationViewModel>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.Streets == item.Streets.Name && n.House == null && n.Apartment == null)
                    .Select(n => new NotificationViewModel
                    {
                        Street = $"Вул. {item.Streets.Name}",
                        Text = n.Text,
                        Header = n.Header,
                        Date = n.Date.ToShortDateString(),
                    })
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }

        public async Task<ICollection<NotificationViewModel>> GetHouseNotifications(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<NotificationViewModel>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.Streets == item.Streets.Name && n.House == item.House && n.Apartment == null)
                    .Select(n => new NotificationViewModel
                    {
                        Street = $"Вул. {item.Streets.Name}",
                        Building = $"буд. {item.House}",
                        Text = n.Text,
                        Header = n.Header,
                        Date = n.Date.ToShortDateString(),
                    })
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }

        public async Task<ICollection<NotificationViewModel>> GetDirectNotifications(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<NotificationViewModel>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.Streets == item.Streets.Name && n.House == item.House && n.Apartment == item.Apartment)
                    .Select(n => new NotificationViewModel
                    {
                        Street = $"Вул. {item.Streets.Name}",
                        Building = $"буд. {item.House}",
                        Apartment = $"кв. {item.Apartment}",
                        Text = n.Text,
                        Header = n.Header,
                        Date = n.Date.ToShortDateString(),
                    })
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }
    }
}
