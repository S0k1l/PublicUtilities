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
                .Where(upor => upor.AppUser.UserName == userName)
                .Select(upor => upor.PlacesOfResidence)
                .ToListAsync();
        }

        public async Task<ICollection<NotificationViewModel>> GetUserNotificationByUserPlacesOfResidences(ICollection<PlacesOfResidence> placesOfResidences)
        {
            var model = new List<NotificationViewModel>();
            foreach (var item in placesOfResidences)
            {
                var notifications = await _context.Notifications
                    .Where(n => n.PlacesOfResidenceId == item.Id)
                    .Select(n => new NotificationViewModel
                    {
                        Text = n.Text,
                        Header = n.Header,
                        Date = n.Date.ToShortDateString(),
                        Street = $"Вул. {n.PlacesOfResidence.Streets.Name}",
                        Building = $", буд. {n.PlacesOfResidence.House}",
                        Apartment = $", кв. {n.PlacesOfResidence.Apartment}",
                    })
                    .ToListAsync();

                model.AddRange(notifications);
            }

            return model;
        }

        public async Task<ICollection<NotificationViewModel>> GetGlobalNotification()
        {
                var model = await _context.Notifications
                    .Where(n => n.PlacesOfResidence == null)
                    .Select(n => new NotificationViewModel
                    {
                        Text = n.Text,
                        Header = n.Header,
                        Date = n.Date.ToShortDateString(),
                    })
                    .ToListAsync();

            return model;
        }
    }
}
